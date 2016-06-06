using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.AI.Bots
{
    public class RaiderBot : ActionBot
    {

        abstract class RaiderAction : Action
        {
            public const float NULL_RATING = -1;

            public float Weight = 1.0f;


            public RaiderBot Raider { get; }

            public RaiderAction(RaiderBot owner)
            {
                Raider = owner;
            }

            public override float ComputeRating()
            {
                return Weight;
            }
            protected float Step(float edge, float v)
            {
                return (v >= edge) ? 1 : 0;
            }

            protected float Linstep(float edge0, float edge1, float v)
            {
                return Clamp((v - edge0) / (edge1 - edge0), 0, 1);
            }

            protected float Clamp(float x, float min, float max)
            {
                return (float)Math.Max(min, Math.Min(max, x));
            }

            protected float Sigmoid(float value)
            {
                return (float)(1.0 / (1.0 + Math.Exp(-value)));
            }
        }

        class DrinkingAction : RaiderAction
        {
            const int MAX_HP = 100;
            const int HP_PER_BEER = 50;

            public DrinkingAction(RaiderBot owner) : base(owner) { }

            public override float ComputeRating()
            {
                if (Raider.Self.Gold < 2)
                    return NULL_RATING;

                var self = Raider.Self;
                float distanceToTavern = Raider.DistanceToNextTavern();
                float distanceToMine = Raider.DistanceToNextMine();
                float distanceMod = Sigmoid(distanceToMine - distanceToTavern);
                float income = 0.5f + 0.5f * self.MineRatio;
                float scared = 0.5f + 0.5f * Raider.GetThreat(5);
                //Range 0..1
                float drinkValue = income * scared * distanceMod * Linstep(MAX_HP, MAX_HP - HP_PER_BEER, self.Life);
                return base.ComputeRating() * drinkValue;
            }

            public override Move Act()
            {
                return Raider.World["taverns"].GetMove(Raider.Self.Position);
            }
        }

        class MiningAction : RaiderAction
        {
            const int MINING_MIN_HP = 21;

            public MiningAction(RaiderBot owner) : base(owner) { }

            public override float ComputeRating()
            {
                var self = Raider.Self;
                float distanceToNext = Raider.DistanceToNextMine();
                float hpAtNextMine = self.Life - distanceToNext;
                float threat = Raider.GetThreat(5);
                float needMines = 1.0f - 0.5f * self.MineRatio;

                //Range 0..1
                float mineValue = needMines * (1 - threat) * Step(MINING_MIN_HP, hpAtNextMine);
                return base.ComputeRating() * mineValue;
            }

            public override Move Act()
            {
                return Raider.World["mines"].GetMove(Raider.Self.Position);
            }
        }

        class CombatAction : RaiderAction
        {
            const float DMG_PER_HIT = 20;
            const int MEDIAN_DISTANCE = 4;

            public CombatAction(RaiderBot owner) : base(owner) { }

            public override float ComputeRating()
            {
                var self = Raider.Self;
                int selfHits = (int)Math.Ceiling(self.Life / DMG_PER_HIT);
                var victim = Raider.GetClosestEnemy();
                int victimHits = (int)Math.Ceiling(victim.Life / DMG_PER_HIT);
                float distanceToVictim = Raider.DistanceToNextEnemy();
                if (distanceToVictim <= 2 && selfHits < victimHits)
                    return NULL_RATING;
                if (distanceToVictim > 2 && selfHits <= victimHits)
                    return NULL_RATING;

                bool wontHeal = distanceToVictim < Raider.DistanceToTavernFrom(victim.Position);
                return base.ComputeRating() * (wontHeal ? 1 : 0.5f) * Sigmoid(MEDIAN_DISTANCE - distanceToVictim);
            }

            public override Move Act()
            {
                return Raider.World["threat"].GetMove(Raider.Self.Position);
            }
        }

        class StayAction : RaiderAction
        {
            public StayAction(RaiderBot owner) : base(owner) { }

            public override Move Act()
            {
                return Move.Stay;
            }
        }

        public RaiderBot(Knowledge knowledge) : base(knowledge)
        {
            World.Chart("threat", World.TypeFilter(TileMap.TileType.Hero, World.Hero.ID), World.DefaultCost);
            World.Chart("mines", World.TypeFilter(TileMap.TileType.GoldMine, World.Hero.ID), World.CostByThreat(50));
            World.Chart("taverns", World.TypeFilter(TileMap.TileType.Tavern), World.CostByThreat(50));
            World.Chart("beer_dist", World.TypeFilter(TileMap.TileType.Tavern), World.AnyHero);

            Add(new StayAction(this) { Weight = 0f });
            Add(new DrinkingAction(this) { Weight = 2.0f });
            Add(new MiningAction(this));
            Add(new CombatAction(this));
        }

        private float DistanceToNextMine()
        {
            var pos = Self.Position;
            return World["mines"][pos.X, pos.Y].PathCost;
        }

        private float DistanceToNextTavern()
        {
            var pos = Self.Position;
            return World["taverns"][pos.X, pos.Y].PathCost;
        }
        private float DistanceToNextEnemy()
        {
            var pos = Self.Position;
            return World["threat"][pos.X, pos.Y].PathCost;
        }
        public float DistanceToTavernFrom(Position pos)
        {
            return World["beer_dist"][pos.X, pos.Y].PathCost;
        }

        private bool IsWinning()
        {
            return Self.MineRatio * Math.Min(1, 1.2 * Self.GoldRatio) > 1;
        }

        private float GetThreat(float threatDistance)
        {
            var pos = Self.Position;
            return World.GetNormalizedThreat(pos.X, pos.Y, threatDistance);
        }

        private HeroInfo GetClosestEnemy()
        {
            NavGrid grid = World["threat"];
            int idx = grid.IndexOf(Self.Position);
            while (grid[idx].PathCost > 0)
                idx = grid[idx].Previous;

            Position pos = grid.PositionOf(idx);
            HeroInfo hero = World.Heroes.Where(h => h.Position == pos).FirstOrDefault();
            return hero;
        }

    }
}