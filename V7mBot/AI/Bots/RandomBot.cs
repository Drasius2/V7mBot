using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.AI.Bots
{
    public class RandomBot : Bot
    {
        Random _rng = new Random();

        public RandomBot(Knowledge knowledge) : base(knowledge)
        {
        }

        public override Move Act()
        {
            return RandomEnumValue<Move>();
        }

        T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(_rng.Next(v.Length));
        }

        override public IEnumerable<VisualizationRequest> Visualizaton
        {
            get
            {
                return Enumerable.Empty<VisualizationRequest>();
            }
        }
    }
}
