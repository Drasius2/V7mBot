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
        //TODO: stuff like this needs to be bot specific
        //const float ZERO_THREAT_DISTANCE = 10;
        const float MINING_THREAT_TO_COST = 50;
        const float TAVERN_THREAT_TO_COST = 50;

        private GameResponse _rawData;
        float _zeroThreatDistance;
        private HeroInfo _hero;
        private TileMap _map;
        private NavGrid _threat;
        private NavGrid _mines;
        private NavGrid _taverns;

        public HeroInfo Hero
        {
            get
            {
                return _hero;
            }
        }

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

        public GameResponse RawData
        {
            get
            {
                return _rawData;
            }
        }

        public Knowledge(GameResponse rawData)
        {
            _rawData = rawData;

            int mapSize = _rawData.game.board.size;
            _map = new TileMap(mapSize);
            _map.Parse(_rawData.game.board.tiles);

            _zeroThreatDistance = 1 + (mapSize / 4);
            _threat = CreateNavGrid(_map);

            _mines = CreateNavGrid(_map);

            _taverns = CreateNavGrid(_map);

            var heroData = _rawData.game.heroes.First(h => h.id == _rawData.hero.id);
            int index = _rawData.game.heroes.IndexOf(heroData);
            _hero = new HeroInfo(this, index);

        }

        public float GetNormalizedThreat(int x, int y, float zeroThreatDistance)
        {
            if (zeroThreatDistance == 0)
                return 0;
            return Math.Max(0, zeroThreatDistance - _threat[x, y].PathCost) / zeroThreatDistance;
        }

        public void Update(GameResponse rawData)
        {
            _rawData = rawData;
            _map.Parse(rawData.game.board.tiles);
            //TODO: --> stuff like this needs to be bot specific
            Chart(_threat, OpponentFilter(_hero.ID));
            Chart(_mines, OpponentMineFilter(_hero.ID), (x, y) => ComputeCostByThreat(x, y, MINING_THREAT_TO_COST));
            Chart(_taverns, TavernFilter(), (x, y) => ComputeCostByThreat(x, y, TAVERN_THREAT_TO_COST));
        }

        private NavGrid CreateNavGrid(TileMap map)
        {
            var grid = new NavGrid(map.Width, map.Height);
            grid.SetNodeCost((x, y) => IsPassable(map[x, y]) ? 1 : -1);
            return grid;
        }

        private bool IsPassable(TileMap.Tile tile)
        {
            if (tile.Type == TileMap.TileType.Free)
                return true;
            if (tile.Type == TileMap.TileType.Hero)
                return true;
            return false;
        }

        private void Chart(NavGrid nav, Predicate<TileMap.Tile> match, NavGrid.CostQuery source)
        {
            nav.Reset();
            nav.SetNodeCost(source);
            foreach (var pos in _map.Find(tile => match(tile)))
                nav.Seed(pos, 0);
            nav.Flood();
        }

        private void Chart(NavGrid nav, Predicate<TileMap.Tile> match)
        {
            nav.Reset();
            foreach (var pos in _map.Find(tile => match(tile)))
                nav.Seed(pos, 0);
            nav.Flood();
        }

        //PREDICATES

        private Predicate<TileMap.Tile> TavernFilter()
        {
            return tile => tile.Type == TileMap.TileType.Tavern;
        }

        private Predicate<TileMap.Tile> OpponentMineFilter(int heroID)
        {
            return tile => tile.Type == TileMap.TileType.GoldMine && tile.Owner != heroID;
        }

        private Predicate<TileMap.Tile> OpponentFilter(int heroID)
        {
            return tile => tile.Type == TileMap.TileType.Hero && tile.Owner != heroID;
        }

        //COST MODIFIER
        
        private float ComputeCostByThreat(int x, int y, float scale)
        {
            var node = _map[x, y];
            int id = _hero.ID;
            if (node.Type == TileMap.TileType.Free || (node.Type == TileMap.TileType.Hero && node.Owner == id))
                return GetNormalizedThreat(x, y, _zeroThreatDistance) * scale;
            else
                return -1;
        }
    }
}
