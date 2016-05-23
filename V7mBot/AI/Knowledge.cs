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
        private Hero _hero;
        private TileMap _map;
        private NavGrid _threat;
        private NavGrid _mines;
        private NavGrid _taverns;

        public TileMap Map
        {
            get
            {
                return _map;
            }
        }

        public NavGrid Threat
        {
            get
            {
                return _threat;
            }
        }

        public NavGrid Mines
        {
            get
            {
                return _mines;
            }
        }

        public NavGrid Taverns
        {
            get
            {
                return _taverns;
            }
        }

        public Position HeroPosition
        {
            get
            {
                //json has x & y swapped
                return new Position(_hero.pos.y, _hero.pos.x);
            }
        }

        public int HeroLife
        {
            get
            {
                return _hero.life;
            }
        }

        public Knowledge(GameResponse rawData)
        {
            int mapSize = rawData.game.board.size;
            _map = new TileMap(mapSize);
            _map.Parse(rawData.game.board.tiles);
            _threat = CreateNavGrid(_map);
            _mines = CreateNavGrid(_map);
            _taverns = CreateNavGrid(_map);
        }

        public void Update(GameResponse rawData)
        {
            _map.Parse(rawData.game.board.tiles);
            int heroID = rawData.hero.id;
            _hero = rawData.game.heroes.First(hero => hero.id == heroID);
            Chart(_threat, _map.Find(tile => tile.Type == TileMap.TileType.Hero && tile.Owner != heroID));
            Chart(_mines, _map.Find(tile => tile.Type == TileMap.TileType.GoldMine && tile.Owner != heroID));
            Chart(_taverns, _map.Find(tile => tile.Type == TileMap.TileType.Tavern));
        }

        private void Chart(NavGrid nav, IEnumerable<Position> positions)
        {
            nav.Reset();
            foreach (var pos in positions)
                nav.Seed(pos, 0);
            nav.Flood();
        }

        private NavGrid CreateNavGrid(TileMap map)
        {
            var grid = new NavGrid(map.Width, map.Height);
            grid.SetNodeCost((x, y) =>
            {
                bool passable = _map[x, y].Type == TileMap.TileType.Free || _map[x, y].Type == TileMap.TileType.Hero;
                return passable ? 1 : -1;
            });
            return grid;
        }
    }
}
