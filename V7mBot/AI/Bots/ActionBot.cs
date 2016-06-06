using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.AI.Bots
{
    public abstract class ActionBot : Bot
    {
        protected abstract class Action
        {
            abstract public float ComputeRating();

            virtual public Move Act()
            {
                return Move.Stay;
            }

            virtual public void Start(Action previous)
            {
            }
        }

        protected Action _action = null;
        List<Action> _actions = new List<Action>();


        public ActionBot(Knowledge knowledge) : base(knowledge) { }

        protected Action SelectBest()
        {
            var prev = _action;
            Action best = null;
            float bestRating = float.MinValue;
            string log = "";
            foreach (var action in _actions)
            {
                float rating = action.ComputeRating();
                log += "[" + action.GetType().Name + ": " + rating + "] ";
                if (rating > bestRating)
                {
                    best = action;
                    bestRating = rating;
                }
            }
            Console.WriteLine(log);
            if (prev != best)
            {
                _action = best;
                _action.Start(prev);
            }
            return _action;
        }

        protected void Add(Action action)
        {
            _actions.Add(action);
        }

        public override Move Act()
        {
            return SelectBest().Act();
        }
    }
}
