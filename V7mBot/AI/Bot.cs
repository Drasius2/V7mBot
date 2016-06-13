using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.AI
{
    public abstract class Bot
    {
        private Knowledge _knowledge;

        public HeroInfo Self
        {
            get { return _knowledge.Hero; }
        }

        public Knowledge World
        {
            get { return _knowledge; }
        }

        public Bot(Knowledge knowledge)
        {
            _knowledge = knowledge;
        }

        abstract public Move Act();

        public struct VisualizationRequest
        {
            public VisualizationRequest(string chart, string desc)
            {
                ChartName = chart;
                Description = desc;
            }
            public string ChartName;
            public string Description;
        }
        abstract public IEnumerable<VisualizationRequest> Visualizaton
        {
            get;
        }
    }
}
