using Maze;

namespace MazeRecursion
{
    public static class RecursiveMazeFactory
    {
        public static IMapProvider Create()
        {
            return new RecursiveMazeGenerator(null);
        }
    }
}
