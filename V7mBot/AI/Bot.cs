using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.AI
{
    public abstract class Bot
    {
        protected Knowledge _knowledge;

        public Bot(Knowledge knowledge)
        {
            _knowledge = knowledge;
        }

        abstract public Move Act();
    }
}
