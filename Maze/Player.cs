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
            float pi = (float)Math.PI;
            switch (Facing)
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
            }
        }

        private bool IsMoveValid(MapVector position)
        {
            return _MapGrid[position.Y,position.X] == Block.Empty;
            
        }
        public void MoveBackward()
        {
            if (IsMoveValid(Position - Facing)){
                Position -= Facing;
            }
        }

        public void MoveForward()
        {
            if (IsMoveValid(Position + Facing))
            {
                Position += Facing;
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
