using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.AI.Bots
{
    public class PeonBot : StatefulBot<PeonBot.StateIDs>
    {
        public enum StateIDs
        {
            Drinking,
            Mining
        }

        class DrinkingState : State
        {
            const int START_MINING_HEALTH = 75;
            int _drinkingTime = 0;

            public PeonBot Peon { get; }

            public DrinkingState(PeonBot owner)
            {
                Peon = owner;
            }

            public override StateIDs Update()
            {
                if (Peon.Self.Gold < 2)
                    return StateIDs.Mining;

                if (Peon.Self.Life < START_MINING_HEALTH)
                    return StateIDs.Drinking;

                if (Peon.IsWinning())
                {
                    if (!Peon.IsThreatened(5))
                        return StateIDs.Mining;
                }
                else
                {
                    if (!Peon.IsThreatened(3) || _drinkingTime > 3)
                        return StateIDs.Mining;
                }
                return StateIDs.Drinking;
            }

            public override Move Act()
            {
                _drinkingTime++;
                if (Peon.Self.Life < START_MINING_HEALTH || Peon.DistanceToNextTavern() > 1)
                    return Peon.World["taverns"].GetMove(Peon.Self.Position);

                return Move.Stay;
            }

            public override void Enter(State previous)
            {
                _drinkingTime = 0;
            }
        }
        
        class MiningState : State
        {
            int START_DRINKING_HEALTH = 20;

            public PeonBot Peon { get; }
  
            public MiningState(PeonBot owner)
            {
                Peon = owner;
            }

            public override StateIDs Update()
            {
                float hpAtNextMine = Peon.Self.Life - Peon.DistanceToNextMine();
                if (Peon.Self.Gold > 2) //can afford beer?
                {
                    if ((Peon.IsWinning() && Peon.IsThreatened(5)) || Peon.IsThreatened(3))
                        return StateIDs.Drinking;

                    if (Peon.Self.MineRatio > 0 && hpAtNextMine <= START_DRINKING_HEALTH)
                        return StateIDs.Drinking;
                }
                return StateIDs.Mining;
            }

            public override Move Act()
            {
                return Peon.World["mines"].GetMove(Peon.Self.Position);
            }
        }

        public PeonBot(Knowledge knowledge) : base(knowledge)
        {            
            float zeroThreatDistance = 1 + (World.Map.Width / 4);
            World.Chart("threat", World.TypeFilter(TileMap.TileType.Hero, World.Hero.ID), World.DefaultCost);
            World.Chart("mines", World.TypeFilter(TileMap.TileType.GoldMine, World.Hero.ID), World.CostByChart("threat", zeroThreatDistance, 50));
            World.Chart("taverns", World.TypeFilter(TileMap.TileType.Tavern), World.CostByChart("threat", zeroThreatDistance, 50));

            Register(StateIDs.Drinking, new DrinkingState(this));
            Register(StateIDs.Mining, new MiningState(this));
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

        private bool IsWinning()
        {
            return Self.MineRatio * Math.Min(1, 1.2 * Self.GoldRatio) > 1;
        }

        private bool IsThreatened(float threatDistance)
        {
            var pos = Self.Position;
            return World.SampleChartNormalized(pos.X, pos.Y, "threat", threatDistance) > 0;
        }

        override public IEnumerable<VisualizationRequest> Visualizaton
        {
            get
            {
                yield return new VisualizationRequest("threat", "Threat");
                yield return new VisualizationRequest("mines", "Mines");
                yield return new VisualizationRequest("taverns", "Taverns");
            }
        }
    }
}
