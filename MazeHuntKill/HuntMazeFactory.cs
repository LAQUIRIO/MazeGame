using Maze;

namespace MazeHuntKill
{
    public static class HuntMazeFactory
    {
        public static IMapProvider Create()
        {
            return new MazeHuntKillGenerator(null);
        }
    }
}
