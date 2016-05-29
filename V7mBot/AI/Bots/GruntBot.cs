using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.AI.Bots
{
    public class GruntBot : Bot
    {
        Random _rng = new Random();

        public GruntBot(Knowledge knowledge) : base(knowledge)
        {
            World.Chart("threat", World.TypeFilter(TileMap.TileType.Hero, World.Hero.ID), World.DefaultCost);
        }

        public override Move Act()
        {
            //just approach closest enemy
            //return Move.Stay;
            //return RandomEnumValue<Move>();
            return World["threat"].GetMove(Self.Position);
        }
    }
}
