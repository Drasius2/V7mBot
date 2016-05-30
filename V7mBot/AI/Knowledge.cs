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
        public float MiningThreatToCost = 50;
        public float TavernThreatToCost = 50;
        
        private class NavQuery
        {
            public NavGrid Grid;
            public Predicate<TileMap.Tile> Filter;
            public NavGrid.CostQuery CostFunction;
        }

        private Dictionary<string, NavQuery> _charts = new Dictionary<string, NavQuery>();       
        private GameResponse _rawData;
        float _zeroThreatDistance;
        private HeroInfo _hero;
        private List<HeroInfo> _heroes;
        private TileMap _map;

        public HeroInfo Hero
        {
            get
            {
                return _hero;
            }
        }

        public IEnumerable<HeroInfo> Heroes
        {
            get
            {
                return _heroes;
            }
        }

        public TileMap Map
        {
            get
            {
                return _map;
            }
        }
        public GameResponse RawData
        {
            get
            {
                return _rawData;
            }
        }

        public NavGrid this[string chartName]
        {
            get { return _charts[chartName].Grid; }
        }

        public Knowledge(GameResponse rawData)
        {
            _rawData = rawData;

            int mapSize = _rawData.game.board.size;
            _zeroThreatDistance = 1 + (mapSize / 4);

            _map = new TileMap(mapSize);
            _map.Parse(_rawData.game.board.tiles);

            int index = 0;
            _heroes = _rawData.game.heroes.Select(data => new HeroInfo(this, index++)).ToList();
            _hero = _heroes.First(h => h.ID == _rawData.hero.id);
        }

        public void Update(GameResponse rawData)
        {
            _rawData = rawData;
            _map.Parse(rawData.game.board.tiles);
            //TODO: --> stuff like this needs to be bot specific
            foreach(var q in _charts.Values)
            {
                UpdateChart(q.Grid, q.Filter, q.CostFunction);
            }
        }
        
        private bool IsPassable(TileMap.Tile tile)
        {
            if (tile.Type == TileMap.TileType.Free)
                return true;
            if (tile.Type == TileMap.TileType.Hero)
                return true;
            return false;
        }
        
        public void Chart(string name, Predicate<TileMap.Tile> match, NavGrid.CostQuery costFunc)
        {
            _charts[name] = new NavQuery()
            {
                Grid = new NavGrid(Map.Width, Map.Height),
                CostFunction = costFunc,
                Filter = match
            };
        }

        private void UpdateChart(NavGrid nav, Predicate<TileMap.Tile> match, NavGrid.CostQuery source)
        {
            nav.Reset();
            if(source != null)
                nav.SetNodeCost(source);
            foreach (var pos in _map.Find(tile => match(tile)))
                nav.Seed(pos, 0);
            nav.Flood();
        }
        
        //PREDICATES

        public Predicate<TileMap.Tile> TypeFilter(TileMap.TileType type)
        {
            return tile => tile.Type == type;
        }

        public Predicate<TileMap.Tile> TypeFilter(TileMap.TileType type, int heroID)
        {
            return tile => tile.Type == type && tile.Owner != heroID;
        }


        //COST MODIFIER

        public NavGrid.CostQuery CostByThreat(float threatToCost)
        {
            return (x, y) => ComputeCostByThreat(x, y, threatToCost);
        }

        public float GetNormalizedThreat(int x, int y, float zeroThreatDistance)
        {
            if (zeroThreatDistance == 0)
                return 0;
            return Math.Max(0, zeroThreatDistance - this["threat"][x, y].PathCost) / zeroThreatDistance;
        }

        private float ComputeCostByThreat(int x, int y, float scale)
        {
            var node = _map[x, y];
            int id = _hero.ID;
            if (node.Type == TileMap.TileType.Free || (node.Type == TileMap.TileType.Hero && node.Owner == id))
                return 1 + GetNormalizedThreat(x, y, _zeroThreatDistance) * scale;
            else
                return -1;
        }

        public float DefaultCost(int x, int y)
        {
            var node = _map[x, y];
            int id = _hero.ID;
            if (node.Type == TileMap.TileType.Free || (node.Type == TileMap.TileType.Hero && node.Owner == id))
                return 1;
            else
                return -1;
        }

        public float AnyHero(int x, int y)
        {
            var node = _map[x, y];
            if (node.Type == TileMap.TileType.Free || node.Type == TileMap.TileType.Hero)
                return 1;
            else
                return -1;
        }
    }
}
