
using System;
using System.Drawing.Text;

internal class Program
{
    [STAThreadAttribute]
    public static void Main()
    {
        using var game = new MazeGame.MazeGame();
        game.Run();

    }
}