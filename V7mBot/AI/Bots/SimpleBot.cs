using System;
using System.Collections.Generic;
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
            Fighting
        };
        State _state = State.Drinking;

        int START_DRINKING_HEALTH = 21;
        int START_FIGHTING_HEALTH = 80;

        public SimpleBot(Knowledge knowledge) : base(knowledge)
        {
            _knowledge = knowledge;
        }

        public override Move Act()
        {
            int hp = _knowledge.HeroLife;
            
            //update state machine
            if (_state == State.Drinking && hp >= START_FIGHTING_HEALTH)
                _state = State.Fighting;
            else if (_state == State.Fighting && hp < START_DRINKING_HEALTH)
                _state = State.Drinking;

            //act upon current state
            if(_state == State.Drinking)
                return _knowledge.Taverns.GetMove(_knowledge.HeroPosition);
            else
                return  _knowledge.Mines.GetMove(_knowledge.HeroPosition);
        }
    }
}
