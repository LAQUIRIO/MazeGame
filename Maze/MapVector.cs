using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class MapVector : IMapVector
    {
    

        public MapVector()
        {
        }

        public MapVector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set;  }
        public int Y { get; private set; }

        public bool IsValid
        {
            get
            {
                if (X >= 0 && Y >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        
        public bool InsideBoundary(int width, int height)
        {
            if( width > X && height > Y && IsValid)
            {
                return true;
            }else {    
                return false;
            }
        }
        public double Magnitude()
        {
            return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
        }

        public override bool Equals(object? obj)
        {
            return obj is MapVector vector &&
                   X == vector.X &&
                   Y == vector.Y;
        }

        public static implicit operator MapVector(Direction Direction)
        {
            switch (Direction)
            {
                case Direction.N:
                    return new MapVector { X = 0, Y = -1 };
                case Direction.E:
                    return new MapVector { X = 1, Y = 0 };
                case Direction.S:
                    return new MapVector { X = 0, Y = 1 };
                case Direction.W:
                    return new MapVector { X = -1, Y = 0 };
                default:
                    break;
            }
            return new MapVector { X = 0, Y = 0 };
        }

        public static MapVector operator *(MapVector vector, int scalar)
        {
            return new MapVector { X = vector.X * scalar, Y = vector.Y * scalar };
        }

        public static MapVector operator +(MapVector vector1, MapVector vector2)
        {
            return new MapVector { X = vector1.X + vector2.X, Y = vector1.Y + vector2.Y };
        }

        public static MapVector operator -(MapVector vector1, MapVector vector2)
        {
            return new MapVector { X = vector1.X - vector2.X, Y = vector1.Y - vector2.Y };
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
