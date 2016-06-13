using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.AI.Bots
{
    public class GruntBot : StatefulBot<GruntBot.StateIDs>
    {
        public enum StateIDs
        {
            Drinking,
            Mining,
            Combat
        }

        abstract class GruntState : State
        {
            const float START_COMBAT_MINERATIO = 0.2f;
            const float DMG_PER_HIT = 20;

            public GruntBot Grunt { get; }

            public GruntState(GruntBot owner)
            {
                Grunt = owner;
            }

            protected bool IsCombatViable()
            {
                int selfHits = (int)Math.Ceiling(Grunt.Self.Life / DMG_PER_HIT);
                var victim = Grunt.GetClosestEnemy();
                int victimHits = (int)Math.Ceiling(victim.Life / DMG_PER_HIT);
                float distanceToVictim = Grunt.DistanceToNextEnemy();
                bool isVulnerable = (distanceToVictim <= 2) ? selfHits >= victimHits : selfHits > victimHits;
                bool wontHeal = distanceToVictim < Grunt.DistanceToTavernFrom(victim.Position);
                bool worthy = victim.MineRatio >= START_COMBAT_MINERATIO;
                return isVulnerable && wontHeal && worthy;
            }
        }

        class DrinkingState : GruntState
        {
            const int START_MINING_HEALTH = 75;

            public DrinkingState(GruntBot owner) : base(owner) { }


            public override StateIDs Update()
            {
                //FIGHTING?
                if(IsCombatViable())
                    return StateIDs.Combat;

                if (Grunt.Self.Gold < 2)
                    return StateIDs.Mining;

                if (Grunt.Self.MineRatio == 0) //nothing to lose
                    return StateIDs.Mining;

                if (Grunt.Self.Life < START_MINING_HEALTH)
                    return StateIDs.Drinking;

                if (!Grunt.IsThreatened(5))
                    return StateIDs.Mining;

                return StateIDs.Drinking;
            }

            public override Move Act()
            {
                if (Grunt.Self.Life < START_MINING_HEALTH || Grunt.DistanceToNextTavern() > 1)
                    return Grunt.World["taverns"].GetMove(Grunt.Self.Position);

                return Move.Stay;
            }
        }

        class MiningState : GruntState
        {
            int START_DRINKING_HEALTH = 20;

            public MiningState(GruntBot owner) : base(owner) { }

            public override StateIDs Update()
            {
                //FIGHTING?
                if (IsCombatViable())
                    return StateIDs.Combat;

                //DRINKING?
                if (Grunt.Self.Gold > 2 && Grunt.Self.MineRatio > 0) //somethign to lose & can afford beer?
                {
                    float hpAtNextMine = Grunt.Self.Life - Grunt.DistanceToNextMine();
                    if ((Grunt.IsWinning() && Grunt.IsThreatened(5)) || Grunt.IsThreatened(3))
                        return StateIDs.Drinking;

                    if (Grunt.Self.MineRatio > 0 && hpAtNextMine <= START_DRINKING_HEALTH)
                        return StateIDs.Drinking;
                }
                return StateIDs.Mining;
            }

            public override Move Act()
            {
                return Grunt.World["mines"].GetMove(Grunt.Self.Position);
            }
        }

        class CombatState : GruntState
        {
            public CombatState(GruntBot owner) : base(owner) { }

            public override StateIDs Update()
            {
                if (IsCombatViable())
                    return StateIDs.Combat;
                
                return StateIDs.Mining;
            }

            public override Move Act()
            {
                return Grunt.World["threat"].GetMove(Grunt.Self.Position);
            }
        }

        public GruntBot(Knowledge knowledge) : base(knowledge)
        {
            float zeroThreatDistance = 1 + (World.Map.Width / 4);
            World.Chart("threat", World.TypeFilter(TileMap.TileType.Hero, World.Hero.ID), World.DefaultCost);
            World.Chart("mines", World.TypeFilter(TileMap.TileType.GoldMine, World.Hero.ID), World.CostByChart("threat", zeroThreatDistance, 50));
            World.Chart("taverns", World.TypeFilter(TileMap.TileType.Tavern), World.CostByChart("threat", zeroThreatDistance, 50));
            TileMap.TileType heroOrFree = TileMap.TileType.Hero | TileMap.TileType.Free;
            World.Chart("beer_dist", World.TypeFilter(TileMap.TileType.Tavern), node => (node.Type & heroOrFree) > 0 ? 1 : -1);
            Register(StateIDs.Drinking, new DrinkingState(this));
            Register(StateIDs.Mining, new MiningState(this));
            Register(StateIDs.Combat, new CombatState(this));
            Enter(StateIDs.Mining);
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

        private bool IsThreatened(float threatDistance)
        {
            var pos = Self.Position;
            return World.SampleChartNormalized(pos.X, pos.Y, "threat", threatDistance) > 0;
        }

        private HeroInfo GetClosestEnemy()
        {
            NavGrid grid = World["threat"];
            int idx = grid.IndexOf(Self.Position);
            while(grid[idx].PathCost > 0)
                idx = grid[idx].Previous;

            Position pos = grid.PositionOf(idx);
            HeroInfo hero = World.Heroes.Where(h => h.Position == pos).FirstOrDefault();
            return hero;
        }

        override public IEnumerable<VisualizationRequest> Visualizaton
        {
            get
            {
                yield return new VisualizationRequest("threat", "Threat");
                yield return new VisualizationRequest("mines", "Mines");
                yield return new VisualizationRequest("beer_dist", "Beer");
            }
        }
    }
}
