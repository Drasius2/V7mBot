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
        //--> put that kinda specifics in derived classes
        const float ZERO_THREAT_DISTANCE = 8;
        const float MINING_THREAT_TO_COST = 8;
        const float TAVERN_THREAT_TO_COST = 50;

        private GameResponse _rawData;
        private int _heroID;
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

        public float HeroMineRatio
        {
            get
            {
                int maxMines = _rawData.game.heroes.Max(hero => hero.mineCount);
                if (maxMines == 0)
                    return 0;
                return _hero.mineCount / (float)maxMines;
            }
        }

        public float HeroGoldRatio
        {
            get
            {
                int maxGold = _rawData.game.heroes.Max(hero => hero.gold);
                if (maxGold == 0)
                    return 0;
                return _hero.gold / (float)maxGold;
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

        public float GetNormalizedThreat(int x, int y, float zeroThreatDistance)
        {
            if (zeroThreatDistance == 0)
                return 0;
            return Math.Max(0, zeroThreatDistance - _threat[x, y].PathCost) / zeroThreatDistance;
        }

        public void Update(GameResponse rawData)
        {
            _map.Parse(rawData.game.board.tiles);
            _rawData = rawData;
            _heroID = rawData.hero.id;
            _hero = rawData.hero;
            
            //--> put that kinda specifics in derived classes
            Chart(_threat, OpponentFilter(_heroID));
            Chart(_mines, OpponentMineFilter(_heroID), ComputeMiningCostByThreat);
            Chart(_taverns, TavernFilter(), ComputeTavernCostByThreat);
        }

        delegate float CostSource(int x, int y);

        private void Chart(NavGrid nav, Predicate<TileMap.Tile> match, CostSource modifier)
        {
            nav.Reset();
            nav.SetNodeCost((x, y) => IsPassable(_map[x, y]) ? ComputeMiningCostByThreat(x, y) : -1);
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
            if (tile.Type == TileMap.TileType.Hero && tile.Owner == _heroID)
                return true;
            return false;
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

        private float ComputeMiningCostByThreat(int x, int y)
        {
            return 1 + GetNormalizedThreat(x, y, ZERO_THREAT_DISTANCE) * MINING_THREAT_TO_COST;
        }

        private float ComputeTavernCostByThreat(int x, int y)
        {
            return 1 + GetNormalizedThreat(x, y, ZERO_THREAT_DISTANCE) * TAVERN_THREAT_TO_COST;
        }
    }
}
