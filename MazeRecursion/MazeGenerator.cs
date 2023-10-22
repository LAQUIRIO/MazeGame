namespace MazeRecursion;
using Maze;

public class MazeGenerator : IMapProvider
{
    private Direction[,]? _directions;
    private List<MapVector> _path;
    private readonly Random _rand = new Random();
    private MapVector? _startingVector;
    public MazeGenerator(MapVector? startingVector)
    {
        _path = new List<MapVector>();
        _startingVector = startingVector;
    }
     
    private void Walk(MapVector providedVect)
    {
        if (_directions == null ||_directions.Length <= _path.Count)
        {
            return;
        }
            List<Direction> dirs = GetPossibleDirections(providedVect);

        while (dirs.Count>0)//avoiding trapping itself in a corner and ending the gen early
        {   
            Direction dir = dirs[_rand.Next(0, dirs.Count)];
            _directions[providedVect.X, providedVect.Y] |= dir;
            MapVector newVect = providedVect + dir;
            _directions[newVect.X, newVect.Y] |= GetOppositeDir(dir);
            Walk(newVect);

            dirs = GetPossibleDirections(providedVect);
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
        Direction[] directions = (Direction[])Enum.GetValues(typeof(Direction));
        List<Direction> result = new List<Direction>();
        foreach (var dir in directions)
        {
            if (_path.Contains(providedVect + dir))
            {
                result.Add(dir);
            }
        }
        return result;
    }

    public Direction[,] CreateMap(int width, int height)
    {
        _directions = new Direction[width, height];
        if (_startingVector == null)
        {
            int x = _rand.Next(0, width);
            int y = _rand.Next(0, height);
            _startingVector = new MapVector(x, y);
        }
        Walk(_startingVector);
        return _directions;
    }

    public Direction[,] CreateMap()
    {
        int width = 5;
        int height = 5;
        _directions = new Direction[width, height];

        if (_startingVector == null)
        {
            int x = _rand.Next(0, _directions.GetLength(0));
            int y = _rand.Next(0, _directions.GetLength(1));
            _startingVector = new MapVector(x, y);
        }
        Walk(_startingVector);
        return _directions;
    }
}
