using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.AI
{
    public class NavGrid
    {
        //TODO: consider caching the neighbours per node if Neighbours() becomes a bottle-neck
        public const float NullCost = float.MaxValue;

        public class Node
        {
            public float NodeCost;
            public float PathCost = NullCost;
            public int Previous = -1;
            public bool Open = false;
        }

        Queue<int> _open;
        List<Node> _grid;
        int _width;
        int _height;
        int _maxPathCost;

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public Node this[Position pos]
        {
            get { return _grid[IndexOf(pos)]; }
        }

        public Node this[int x, int y]
        {
            get { return _grid[IndexOf(x, y)]; }
        }

        public Node this[int i]
        {
            get { return _grid[i]; }
        }

        public float MaxPathCost
        {
            get { return _maxPathCost; }
        }

        public NavGrid(int width, int height)
        {
            _width = width;
            _height = height;
            _open = new Queue<int>();
            _grid = new List<Node>(width * height);
            for (int i = 0; i < width * height; i++)
                _grid.Add(new Node());
        }
                
        public delegate float CostQuery(int x, int y);
        public void SetNodeCost(CostQuery query)
        {
            int i = 0;
            for (int y = 0; y < _height; y++)
                for (int x = 0; x < _width; x++, i++)
                    _grid[i].NodeCost = query(x, y);
        }

        public void Reset()
        {
            _maxPathCost = -1;
            _open.Clear();
            int i = 0;
            for (int y = 0; y < _height; y++)
                for (int x = 0; x < _width; x++, i++)
                {
                    _grid[i].PathCost = NullCost;
                    _grid[i].Previous = -1;
                    _grid[i].Open = false;
                }
        }

        internal void Seed(Position pos, int cost)
        {
            Seed(pos.X, pos.Y, cost);
        }

        public void Seed(int x, int y, float cost)
        {
            int i = IndexOf(x, y);
            _grid[i].PathCost = cost;
            _grid[i].Open = true;
            _open.Enqueue(i);
        }        

        public void Flood()
        {
            while (_open.Count > 0)
            {
                int i = _open.Dequeue();
                Node source = _grid[i];
                source.Open = false;
                //expand to neighbours
                foreach(int j in Neighbours(i))
                {
                    Node target = _grid[j];
                    if (target.NodeCost < 0)
                        continue; //not passable
                    float cost = source.PathCost + target.NodeCost;
                    if (target.PathCost <= cost)
                        continue;

                    target.PathCost = cost;
                    target.Previous = i;

                    if (target.Open)
                        continue;

                    target.Open = true;
                    _open.Enqueue(j);
                }
            }
            //that bunch of ??? prevent a crash when Where() is empty (that's why we hide it behind a property, right?)
            _maxPathCost = _grid.Where(n => n.PathCost < NullCost).Select(n => (int?)n.PathCost).Max() ?? 0;
        }

        public Position PositionOf(int index)
        {
            int y = index / _width;
            return new Position(index - y * _width, y);
        }

        public int IndexOf(Position pos)
        {
            return pos.Y * _width + pos.X;
        }

        public int IndexOf(int x, int y)
        {
            return y * _width + x;
        }

        public Move GetMove(Position pos)
        {
            return GetMove(IndexOf(pos));
        }

        public Move GetMove(int x, int y)
        {
            return GetMove(IndexOf(x, y));
        }

        public Move GetMove(int current)
        {
            int prev = _grid[current].Previous;
            if (prev == -1)
                return Move.Stay;

            int step = prev - current;
            if(step == 1)
                return Move.East;
            else if (step == -1)
                return Move.West;
            else if (step == _width)
                return Move.South;
            else if (step == -_width)
                return Move.North;

            return Move.Stay;
        }

        private IEnumerable<int> Neighbours(int i)
        {
            int x = i % _width;
            int y = i / _width;
            if (y > 0) //UP
                yield return i - _width;
            if (y < _height - 1) //DOWN
                yield return i + _width;
            if (x > 0) //LEFT
                yield return i - 1;
            if (x < _width - 1) //RIGHT
                yield return i + 1;
        }
    }
}
