using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.AI
{
    public class KnowledgeBase
    {
        private TileMap _mapState;
        private NavGrid _heroDistance;

        public TileMap MapState
        {
            get
            {
                return _mapState;
            }
        }

        public NavGrid HeroDistance
        {
            get
            {
                return _heroDistance;
            }
        }

        public KnowledgeBase(GameResponse rawData)
        {
            int mapSize = rawData.game.board.size;
            _mapState = new TileMap(mapSize);
            _mapState.Parse(rawData.game.board.tiles);

            _heroDistance = new NavGrid(mapSize, mapSize);
            _heroDistance.SetNodeCost((x, y) =>
            {
                return (_mapState[x, y].Type == TileMap.TileType.Free) ? 1 : -1;
            });
        }

        public void Update(GameResponse rawData)
        {
            _mapState.Parse(rawData.game.board.tiles);
            //update hero-distance
            int heroID = rawData.hero.id;
            int heroX, heroY;
            if(_mapState.Find(out heroX, out heroY, tile => tile.Type == TileMap.TileType.Hero && tile.Owner == heroID))
            {
                _heroDistance.Reset();
                _heroDistance.Seed(heroX, heroY, 0);
                _heroDistance.Flood();
            }
        }
    }
}
