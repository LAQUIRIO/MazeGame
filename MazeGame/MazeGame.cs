using Maze;
using MazeRecursion;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLog;

namespace MazeGame;

delegate void LoadMazeFunc(IMapProvider mapProvider, int? width, int? height);
public class MazeGame : Game
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    private readonly GraphicsDeviceManager _graphics;
    private MenuScreen _menuScreen;
    private MazeScreen _mazeScreen;
    private IMapProvider _mapProvider;
    public MazeGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Exiting += (sender, args) => logger.Info($"Game finished: game window is closed");
        _menuScreen = new MenuScreen(this, _graphics, LoadMaze);
        Components.Add(_menuScreen);
        base.Initialize();
    }
    protected override void LoadContent()
    {
        base.LoadContent();
    }
    protected void LoadMaze(IMapProvider mapProvider, int? width, int? height)
    {
        logger.Info($"left Main Menu");
        _mapProvider = new MazeGenerator(null, null);
        _mazeScreen = new MazeScreen(this, _graphics, _mapProvider, Back, height, width);
        Components.Remove(_menuScreen);
        _menuScreen.Dispose();
        Components.Add(_mazeScreen);
    }
    protected void Back()
    {
        _mazeScreen.Dispose();
        Components.Remove(_mazeScreen);
        _menuScreen = new MenuScreen(this, _graphics, LoadMaze);
        Components.Add(_menuScreen);
    }
}


