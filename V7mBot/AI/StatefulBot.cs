using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.AI
{
    public abstract class StatefulBot<T> : Bot
    {
        protected abstract class State //could make this class fully abstract but it helps readability to not implement something like Act() if you don't need it
        {
            abstract public T Update();

            virtual public Move Act()
            {
                return Move.Stay;
            }

            virtual public void Enter(State previous)
            {
            }
        }

        protected State _state = null;
        Dictionary<T, State> _states = new Dictionary<T, State>();


        public StatefulBot(Knowledge knowledge) : base(knowledge) { }

        protected void Enter(T key)
        {
            var prev = _state;
            _state = _states[key];
            if(prev != _state)
                _state.Enter(prev);
        }

        protected void Register(T key, State state)
        {
            _states[key] = state;
        }

        public override Move Act()
        {
            Enter(_state.Update());
            return _state.Act();
        }
    }
}
