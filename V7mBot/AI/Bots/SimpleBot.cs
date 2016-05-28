using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.AI.Bots
{
    public class SimpleBot : Bot
    {
        enum State
        {
            Drinking,
            Mining
        };
        State _state = State.Drinking;
        int _doneDrinkingTime = 0;

        int START_MINING_HEALTH = 75;
        int START_DRINKING_HEALTH = 20;

        public SimpleBot(Knowledge knowledge) : base(knowledge)
        {
        }

        private float DistanceToNextMine()
        {
            var pos = Self.Position;
            return World.Mines[pos.X, pos.Y].PathCost;
        }

        private float DistanceToNextTavern()
        {
            var pos = Self.Position;
            return World.Taverns[pos.X, pos.Y].PathCost;
        }

        private bool IsWinning()
        {
            return Self.MineRatio * Math.Min(1, 1.2 * Self.GoldRatio) > 1;
        }

        private bool IsThreatened(float threatDistance)
        {
            var pos = Self.Position;
            return World.GetNormalizedThreat(pos.X, pos.Y, threatDistance) > 0;
        }

        private void SwitchState(State next)
        {
            _doneDrinkingTime = 0;
            _state = next;
        }

        public override Move Act()
        {
            int hp = Self.Life;
            
            //update state machine
            if (_state == State.Drinking)
            {
                if(Self.Gold < 2)
                    SwitchState(State.Mining);

                if (hp >= START_MINING_HEALTH)
                {
                    if(IsWinning())
                    {
                        if (!IsThreatened(5))
                            SwitchState(State.Mining);
                    }
                    else
                    {
                        if(!IsThreatened(3))
                            SwitchState(State.Mining);
                        else if(_doneDrinkingTime > 3)
                            SwitchState(State.Mining);
                    }
                }
            }
            else if (_state == State.Mining)
            {                
                float hpAtNextMine = hp - DistanceToNextMine();
                if (Self.Gold > 2)
                {
                    if (IsWinning() && IsThreatened(5))
                        SwitchState(State.Drinking);
                    else if (IsThreatened(3))
                        SwitchState(State.Drinking);
                    else if (Self.MineRatio > 0 && hpAtNextMine <= START_DRINKING_HEALTH)
                        SwitchState(State.Drinking);
                }
            }

            //act upon current state
            if (_state == State.Drinking)
            {
                if (hp < START_MINING_HEALTH || DistanceToNextTavern() > 1)
                    return World.Taverns.GetMove(Self.Position);

                _doneDrinkingTime++;
                return Move.Stay;
            }
            else
                return World.Mines.GetMove(Self.Position);
        }
    }
}
