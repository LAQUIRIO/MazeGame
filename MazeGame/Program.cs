
using NLog;
using System;
using System.Drawing.Text;

internal class Program
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    [STAThreadAttribute]
    public static void Main()
    {
        try//try catch is to log any exception
        {
            using var game = new MazeGame.MazeGame();
            game.Run();
        }
        catch (Exception e)
        {
            logger.Debug("program encountered an exception:"+e);
            throw;
        }

    }
}