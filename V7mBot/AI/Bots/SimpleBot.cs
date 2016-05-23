using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.AI.Bots
{
    public class SimpleBot : Bot
    {
        public SimpleBot(Knowledge knowledge) : base(knowledge)
        {
            _knowledge = knowledge;
        }

        public override Move Act()
        {
            Move action = _knowledge.Goals.GetMove(_knowledge.Hero);
            return action;
        }
    }
}
