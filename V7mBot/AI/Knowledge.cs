using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.AI
{
    public class Knowledge
    {
        private Position _heroPosition;
        private TileMap _mapState;
        private NavGrid _threat;
        private NavGrid _goals;

        public TileMap MapState
        {
            get
            {
                return _mapState;
            }
        }

        public NavGrid Threat
        {
            get
            {
                return _threat;
            }
        }

        public NavGrid Goals
        {
            get
            {
                return _goals;
            }
        }


        public Position Hero
        {
            get
            {
                return _heroPosition;
            }
        }

        public Knowledge(GameResponse rawData)
        {
            int mapSize = rawData.game.board.size;
            _mapState = new TileMap(mapSize);
            _mapState.Parse(rawData.game.board.tiles);

            _threat = new NavGrid(mapSize, mapSize);
            _threat.SetNodeCost((x, y) =>
            {
                return (_mapState[x, y].Type == TileMap.TileType.Free || _mapState[x, y].Type == TileMap.TileType.Hero) ? 1 : -1;
            });

            _goals = new NavGrid(mapSize, mapSize);
            _goals.SetNodeCost((x, y) =>
            {
                return (_mapState[x, y].Type == TileMap.TileType.Free || _mapState[x, y].Type == TileMap.TileType.Hero) ? 1 : -1;
            });
        }

        public void Update(GameResponse rawData)
        {
            _mapState.Parse(rawData.game.board.tiles);
            //update hero-distance
            int heroID = rawData.hero.id;
            _heroPosition = _mapState.Find(tile => tile.Type == TileMap.TileType.Hero && tile.Owner == heroID).First();
            _threat.Reset();
            foreach (var pos in _mapState.Find(tile => tile.Type == TileMap.TileType.Hero && tile.Owner != heroID))
                _threat.Seed(pos, 0);
            _threat.Flood();

            //update goals - seed with all uncontrolled mines
            _goals.Reset();
            foreach (var pos in _mapState.Find(tile => tile.Type == TileMap.TileType.GoldMine && tile.Owner != heroID))
                _goals.Seed(pos, 0);
            _goals.Flood();
        }
    }
}
