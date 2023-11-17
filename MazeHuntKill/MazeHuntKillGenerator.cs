using Maze;

namespace MazeHuntKill;
delegate bool IsValid(MapVector vector);
internal class MazeHuntKillGenerator : IMapProvider
{
    private readonly Random _rand;
    private Direction[,]? _map;

    internal MazeHuntKillGenerator(int? seed)
    {
        if (seed.HasValue)
        {
            _rand = new Random(seed.Value);
        }
        else
        {
            _rand = new Random();
        }
    }

    public Direction[,] CreateMap(int width, int height)
    {
        if (width < 2 || height < 2 || width > 25 || height > 25)
        {
            throw new Exception("Invalid width or height");
        }
        _map = new Direction[height, width];
        MapVector? currentVector  = new MapVector(_rand.Next(0, width - 1), _rand.Next(0, height - 1));
        try {
            while (currentVector != null)
            {
                while (GetPossiblePosition(currentVector, WalkDirectionIsValid).Length > 0)
                {
                    currentVector = Walking(currentVector);
                }
                currentVector = Hunt();
            }

        }
        catch (Exception e)
        {
            throw new Exception("Maze Map invalid");
        }
            return _map;
    }

    public Direction[,] CreateMap()
    {
        return CreateMap(5, 5);
    }
    private MapVector Walking(MapVector currentVector)
    {
        Direction[] possibleDirections = GetPossiblePosition(currentVector, WalkDirectionIsValid);
        Direction direction = possibleDirections[_rand.Next(0, possibleDirections.Length-1)];
        _map![currentVector.Y, currentVector.X] |= direction;
        MapVector newVector = currentVector + direction;
        _map[newVector.Y, newVector.X] |= GetOppositeDirection(direction);
        return newVector;
    }
    private MapVector? Hunt()
    {
        //List<MapVector> huntVectors = new List<MapVector>();
        for (int y = 0; y < _map!.GetLength(0); y++)
        {
            for (int x = 0; x < _map.GetLength(1); x++)
            {
                MapVector vector = new MapVector(x, y);
                bool isHuntVectorValid = GetPossiblePosition(vector, HuntDirectionIsValid).Length > 0;
                if (_map[y, x] == Direction.None && isHuntVectorValid)
                {
                    Direction[] possibleDirections = GetPossiblePosition(vector, HuntDirectionIsValid);
                    Direction direction = possibleDirections[_rand.Next(0, possibleDirections.Length - 1)];
                    _map[vector.Y, vector.X] |= direction;
                    MapVector newVector = vector + direction;
                    _map[newVector.Y, newVector.X] |= GetOppositeDirection(direction);
                    return newVector;
                }
            }
        }
        return null;
    }
    private static Direction GetOppositeDirection(Direction dir)
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

    private Direction[] GetPossiblePosition(MapVector vector, IsValid isDirectionValid)
    {
        List<Direction> possibleDirections = new List<Direction>();
        Direction[] directions = { Direction.N, Direction.S, Direction.E, Direction.W };

        foreach (Direction dir in directions)
        {
            MapVector tempVector = vector + dir;
            if (tempVector.InsideBoundary(_map!.GetLength(1), _map.GetLength(0)) && isDirectionValid(tempVector))
            {
                possibleDirections.Add(dir);
            }
        }
        return possibleDirections.OrderBy(item => _rand.Next()).ToArray();
    }

    private bool HuntDirectionIsValid(MapVector vector)
    {
        return _map![vector.Y, vector.X] != Direction.None;
    }
    private bool WalkDirectionIsValid(MapVector vector)
    {
        return _map![vector.Y, vector.X] == Direction.None;
    }
}
