using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.Knowledge
{
    public class KnowledgeBase
    {
        private TileMap _mapState;

        public TileMap MapState
        {
            get
            {
                return _mapState;
            }
        }

        public KnowledgeBase(GameResponse rawData)
        {
            int mapSize = rawData.game.board.size;
            _mapState = new TileMap(mapSize);
            _mapState.Parse(rawData.game.board.tiles);
        }

        public void Update(GameResponse rawData)
        {
            _mapState.Parse(rawData.game.board.tiles);
        }
    }
}
