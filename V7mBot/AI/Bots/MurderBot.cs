using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.AI.Bots
{
    public class MurderBot : Bot
    {
        Random _rng = new Random();

        public MurderBot(Knowledge knowledge) : base(knowledge)
        {
            _knowledge = knowledge;
        }

        public override Move Act()
        {
            //just approach closest enemy
            //return Move.Stay;
            //return RandomEnumValue<Move>();
            return _knowledge.Threat.GetMove(_knowledge.HeroPosition);
        }

        T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(_rng.Next(v.Length));
        }
    }
}
