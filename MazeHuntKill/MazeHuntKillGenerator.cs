using Maze;
using System.Linq;
using System.Numerics;

namespace MazeHuntKill;
delegate bool IsValid(MapVector vector);
public class MazeHuntKillGenerator : IMapProvider
{
    private readonly Random _rand;
    private MapVector? _startingVector;
    private Direction[,] _map;

    public MazeHuntKillGenerator(MapVector? startingVector, int? seed)
    {
        _startingVector = startingVector;
        if (seed.HasValue)
        {
            _rand = new Random(seed.Value);
        }
        else
        {
            _rand = new Random();
        }
    }
    private MapVector Walking(MapVector currentVector)
    {
        Direction[] possibleDirections = GetPossiblePosition(currentVector, DirectionIsValidForWalk);
        Direction direction = possibleDirections[_rand.Next(0, possibleDirections.Length)];
        _map[currentVector.X, currentVector.Y] |= direction;
        MapVector newVector = currentVector + direction;
        _map[newVector.X, newVector.Y] |= GetOppositeDirection(direction);
        return newVector;
    }
    private MapVector? Hunt()
    {
        for (int y = 0; y < _map.GetLength(1); y++)
        {
            for (int x = 0; x < _map.GetLength(0); x++)
            {
                MapVector vector = new MapVector(x, y);
                if (_map[y, x] == Direction.None)
                {
                    Direction[] possibleDirections = GetPossiblePosition(vector, DirectionIsValidForHunt);
                    Direction direction = possibleDirections[_rand.Next(0, possibleDirections.Length)];
                    _map[y, x] |= direction;
                    MapVector newVector = vector + direction;
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

    private Direction[] GetPossiblePosition(MapVector vector, IsValid isValid)
    {
        List<Direction> possibleDirections = new List<Direction>();

        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            if (vector.X > 0 && vector.Y > 0 && vector.X < _map.GetLength(1)-1 && vector.Y < _map.GetLength(0) && isValid(vector + dir))
            {
                possibleDirections.Add(dir);
            }
        }
        return possibleDirections.ToArray();
    }
    private bool DirectionIsValidForHunt(MapVector vector)
    {
        return _map[vector.Y, vector.X] != Direction.None;
    }
    private bool DirectionIsValidForWalk(MapVector vector)
    {
        return _map[vector.Y, vector.X] == Direction.None;
    }

    public Direction[,] CreateMap(int width, int height)
    {
        _map = new Direction[width, height];
        if (_startingVector == null)
        {
            _startingVector = new MapVector(_rand.Next(0, width), _rand.Next(0, height));
        }
        MapVector? currentVector = _startingVector!;
        while (currentVector != null)
        {
            while (GetPossiblePosition(currentVector, DirectionIsValidForWalk).Length > 0)
            {
                currentVector = Walking(currentVector);
            }
            currentVector = Hunt();
        }
        return _map;
    }

    public Direction[,] CreateMap()
    {
        throw new NotImplementedException();
    }
}
