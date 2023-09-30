using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class Player : IPlayer
    {
        public Player(Block[,]? mapGrid = null, Direction facing=Direction.N, int startX = 1, int startY=1)
        {
            if (mapGrid == null)
            {
                mapGrid = new Block[0,0];
            }
            _MapGrid = mapGrid;
            Facing = facing;
            StartX = startX;
            StartY = startY;
            Position = new MapVector(startX, startY);
        }
        private readonly Block[,] _MapGrid;
        public Direction Facing { get; private set; }

        public MapVector Position {get; private set;}

        public int StartX { get; private set; }

        public int StartY { get; private set; }

        public float GetRotation()
        {
            return (float)(Math.Log2((Double)Facing) * Math.PI / 2);
            /*switch (Facing)
            {
                case Direction.N:
                    return 0;
                case Direction.E:
                    return pi / 2;
                case Direction.S:
                    return pi;
                case Direction.W:
                    return 3 * pi / 2;
                default:
                    return 0;
            }*/
        }

        private bool IsMoveValid(MapVector position)
        {
            if (_MapGrid == null) return false;
            if (_MapGrid[position.Y,position.X] == Block.Empty) return true;
            else return false;
        }
        public void MoveBackward()
        {
            if (IsMoveValid(Position - (MapVector)Facing)){
                Position -= (MapVector)Facing;
            }
        }

        public void MoveForward()
        {
            if (IsMoveValid(Position + (MapVector)Facing))
            {
                Position += (MapVector)Facing;
            }
        }

        public void TurnLeft()
        {
            switch (Facing)
            {
                case Direction.N:
                    Facing = Direction.W;
                    break;
                case Direction.E:
                    Facing = Direction.N;
                    break;
                case Direction.S:
                    Facing = Direction.E;
                    break;
                case Direction.W:
                    Facing = Direction.S;
                    break;
                default:
                    break;
            }
        }

        public void TurnRight()
        {
            switch (Facing)
            {
                case Direction.N:
                    Facing = Direction.E;
                    break;
                case Direction.E:
                    Facing = Direction.S;
                    break;
                case Direction.S:
                    Facing = Direction.W;
                    break;
                case Direction.W:
                    Facing = Direction.N;
                    break;
                default:
                    break;
            }
        }
    }
}
