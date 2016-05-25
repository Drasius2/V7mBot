using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.AI.Bots
{
    public class ThreeStateBot : Bot
    {
        enum State
        {
            Drinking,
            Mining,
            Fighting
        };
        State _state = State.Drinking;

        int START_MINING_HEALTH = 80;

        public ThreeStateBot(Knowledge knowledge) : base(knowledge)
        {
            _knowledge = knowledge;
        }

        private float DistanceToNextMine()
        {
            return 0;
        }

        private bool IsWeakerOpponentNear()
        {
            return false;
        }

        public override Move Act()
        {
            int hp = _knowledge.HeroLife;

            //update state machine
            if (_state == State.Fighting)
            {

            }
            if (_state == State.Drinking)
            {
                if (IsWeakerOpponentNear())
                    _state = State.Fighting;
                if (hp >= START_MINING_HEALTH)
                    _state = State.Mining;
            }
            else if (_state == State.Mining)
            {
                float hpAtNextMine = hp - DistanceToNextMine();
                if (hpAtNextMine <= 20)
                    _state = State.Drinking;
            }

            //act upon current state
            if (_state == State.Drinking)
                return _knowledge.Taverns.GetMove(_knowledge.HeroPosition);
            else
                return _knowledge.Mines.GetMove(_knowledge.HeroPosition);
        }
    }
}
