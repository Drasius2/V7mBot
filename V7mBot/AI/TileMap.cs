using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.AI
{
    public class TileMap
    {
        public enum TileType
        {
            Impassable,
            Free,
            Hero,
            Tavern,
            GoldMine
        }

        public struct Tile
        {
            public TileType Type;
            public int Owner;
        }

        int _size;
        Tile[,] _tiles;

        public int Width
        {
            get { return _size; }
        }

        public int Height
        {
            get { return _size; }
        }

        public Tile this[int x, int y]
        {
            get { return _tiles[x, y]; }
        }

        public TileMap(int mapSize)
        {
            _size = mapSize;
            _tiles = new Tile[_size, _size];
        }

        public void Parse(string tiles)
        {
            char[] charData = tiles.ToCharArray();
            for (int x = 0; x < _size; x++)
                for (int y = 0; y < _size; y++)
                {
                    //index of the first char that describes this tile
                    int i = 2 * (y * _size + x);
                    switch (charData[i])
                    {
                        case '#':
                            _tiles[x, y].Type = TileType.Impassable;
                            break;
                        case ' ':
                            _tiles[x, y].Type = TileType.Free;
                            break;
                        case '@':
                            _tiles[x, y].Type = TileType.Hero;
                            _tiles[x, y].Owner = (int)char.GetNumericValue(charData[i + 1]);
                            break;
                        case '[':
                            _tiles[x, y].Type = TileType.Tavern;
                            break;
                        case '$':
                            _tiles[x, y].Type = TileType.GoldMine;
                            _tiles[x, y].Owner = (int)char.GetNumericValue(charData[i + 1]);
                            break;
                    }
                }
        }
        
        public IEnumerable<Position> Find(Predicate<Tile> condition)
        {
            for (int x = 0; x < _size; x++)
                for (int y = 0; y < _size; y++)
                    if (condition(_tiles[x, y]))
                        yield return new Position(x, y);
        }
    }
}
