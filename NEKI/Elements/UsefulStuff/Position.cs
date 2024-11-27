using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEKI.Elements.UsefulStuff
{
    public class Position : IEquatable<Position>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int y, int x)
        {
            X = x;
            Y = y;
        }

        public Position(Position other)
        {
            X = other.X;
            Y = other.Y;
        }
        public bool Equals(Position other)
        {
            if (other == null)
                return false;

            // Compare only A and B properties
            return this.X == other.X && this.Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Position other)
            {
                return X == other.X && Y == other.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}