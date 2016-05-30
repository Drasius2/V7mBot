using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V7mBot.AI
{
    public struct Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X;
        public int Y;

        public static Position Invalid = new Position(-1, -1);

        public override bool Equals(Object obj)
        {
            return obj is Position && this == (Position)obj;
        }
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
        public static bool operator ==(Position a, Position b)
        {
            return a.X == b.X && a.Y == b.Y;
        }
        public static bool operator !=(Position a, Position b)
        {
            return !(a == b);
        }
    };
}
