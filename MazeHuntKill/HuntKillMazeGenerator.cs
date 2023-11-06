using Maze;
using System.Linq;
namespace MazeHuntKill;
public class HuntKillMazeGenerator : IMapProvider
{
    private readonly Random _rand;
    private MapVector? _startingVector;
    private Direction[,] _map;

    public HuntKillMazeGenerator(MapVector? startingVector, int? seed)
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
        Direction[] possibleDirections = getPossiblePosition(currentVector);
        Direction direction = possibleDirections[_rand.Next(0, possibleDirections.Length)];
        _map[currentVector.X, currentVector.Y] |= direction;
        MapVector newVector = currentVector + direction;
        _map[newVector.X, newVector.Y] |= GetOppositeDirection(direction);
        return newVector;
    }

    private Direction GetOppositeDirection(Direction dir)
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

    private Direction[] getPossiblePosition(MapVector vector)
    {
        List<Direction> possibleDirections = new List<Direction>();

        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            if (DirectionIsValid(vector + dir))
            {
                possibleDirections.Add(dir);
            }
        }
        return possibleDirections.ToArray();
    }

    private bool DirectionIsValid(MapVector vector)
    {
        return _map[vector.Y, vector.X] == Direction.None
            && vector.InsideBoundary(_map.GetLength(1), _map.GetLength(0));
    }

    public Direction[,] CreateMap(int width, int height)
    {
        throw new NotImplementedException();
    }

    public Direction[,] CreateMap()
    {
        throw new NotImplementedException();
    }
}
