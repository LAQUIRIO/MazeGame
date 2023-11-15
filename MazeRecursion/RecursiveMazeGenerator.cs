namespace MazeRecursion;
using Maze;

internal class RecursiveMazeGenerator : IMapProvider
{
    private Direction[,]? _directions;
    private readonly List<MapVector> _path;
    private readonly Random _rand;
    public RecursiveMazeGenerator(int? randomSeed)
    {
        _path = new List<MapVector>();
        if (randomSeed.HasValue)
        {
            _rand = new Random(randomSeed.Value);
        }
        else
        {
            _rand = new Random();
        }
    }

    private void Walk(MapVector providedVect)
    {
        if (_directions!.Length <= _path.Count)
        {
            return;
        }
        _path.Add(providedVect);
        List<Direction> dirs = ShuffleDirections(providedVect);

        foreach (var dir in dirs)
        {
            if (DirectionISValid(providedVect + dir))
            {
                _directions[providedVect.Y, providedVect.X] |= dir;
                MapVector newVect = providedVect + dir;
                _directions[newVect.Y, newVect.X] |= GetOppositeDir(dir);

                Walk(newVect);
            }
        }
    }

    private List<Direction> ShuffleDirections(MapVector providedVect)
    {
        List<Direction> dirs = new List<Direction>((Direction[])Enum.GetValues(typeof(Direction)));
        List<Direction> result = new List<Direction>();
        while (dirs.Count > 0)
        {
            int index = _rand.Next(0, dirs.Count);
            result.Add(dirs[index]);
            dirs.RemoveAt(index);
        }
        return result;
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

    private bool DirectionISValid(MapVector newVect)
    {
        return newVect.InsideBoundary(_directions!.GetLength(1), _directions.GetLength(0)) && !_path.Contains(newVect);
    }

    public Direction[,] CreateMap(int width, int height)
    {
        if (width < 3 || height < 3)
        {
            throw new ArgumentException("Width and height must be greater than 2");
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
