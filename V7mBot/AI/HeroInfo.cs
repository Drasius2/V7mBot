using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.AI
{
    public class HeroInfo
    {
        int _index;
        Knowledge _knowledge;

        public HeroInfo(Knowledge knowlede, int index)
        {
            _knowledge = knowlede;
            _index = index;
        }

        public int ID
        {
            get { return RawHero.id; }
        }

        public Hero RawHero
        {
            get { return _knowledge.RawData.game.heroes[_index]; }
        }

        public Position Position
        {
            get
            {
                //json has x & y swapped
                return new Position(RawHero.pos.y, RawHero.pos.x);
            }
        }

        public int Life
        {
            get
            {
                return RawHero.life;
            }
        }

        public int Mines
        {
            get { return RawHero.mineCount; }
        }

        public float MineRatio
        {
            get
            {
                int maxMines = _knowledge.RawData.game.heroes.Max(hero => hero.mineCount);
                if (maxMines == 0)
                    return 0;
                return RawHero.mineCount / (float)maxMines;
            }
        }

        public float GoldRatio
        {
            get
            {
                int maxGold = _knowledge.RawData.game.heroes.Max(hero => hero.gold);
                if (maxGold == 0)
                    return 0;
                return RawHero.gold / (float)maxGold;
            }
        }

        public int Gold
        {
            get { return RawHero.gold; }
        }
    }
}
