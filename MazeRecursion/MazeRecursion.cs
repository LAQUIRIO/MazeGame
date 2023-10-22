namespace MazeRecursion;
using Maze;
public class MazeRecursion
{
    Direction[,] directions;
    List<Direction> path;
    public MazeRecursion(int width, int height)
    {
        directions = new Direction[width, height];
        path = new List<Direction>();
    }
    void Walk(MapVector providedVect)
    {

    }
}
