namespace MazeRecursion;
using Maze;
public class MazeRecursion
{
    private Direction[,] _directions;
    private List<Direction> _path;
    private Random _rand = new Random();
    private MapVector _startingVector;
    public MazeRecursion(int width, int height, MapVector currentVector)
    {
        _directions = new Direction[width, height];
        _path = new List<Direction>();
        if (currentVector == null)
        {
            int x = _rand.Next(0, width);
            int y = _rand.Next(0, height);
            currentVector = new MapVector(x, y);
        }
        _startingVector = currentVector;
    }
     
        public void Walk(MapVector providedVect)
    {
        if (_directions.Length <= _path.Count)
        {
            return;
        }
        while (true)//avoiding trapping itself in a corner and ending the gen early
        { 
            List<Direction> dirs = GetPossibleDirections(providedVect);
            if (dirs.Count == 0)
            {
                return;
            }
            Direction dir = dirs[_rand.Next(0, dirs.Count)];
            _directions[providedVect.X, providedVect.Y] |= dir;
            MapVector newVect = providedVect + dir;
            _directions[newVect.X, newVect.Y] |= GetOppositeDir(dir);
        }

    }

    private static Direction GetOppositeDir(Direction dir)
    {
        return dir switch
        {
            Direction.N => Direction.S,
            Direction.E => Direction.W,
            Direction.S => Direction.N,
            Direction.W => Direction.E,
            Direction.None => Direction.None,
            _ => throw new Exception("Invalid Direction"),
        };
    }

    private List<Direction> GetPossibleDirections(MapVector providedVect)
    {
        throw new NotImplementedException();
    }
}
