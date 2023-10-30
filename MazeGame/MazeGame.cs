using Maze;
using Microsoft.Xna.Framework;
using NLog;
using System;

namespace MazeGame;

delegate void LoadMazeFunc(IMapProvider mapProvider, int? width, int? height);
public class MazeGame : Game
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    private readonly GraphicsDeviceManager _graphics;
    private MenuScreen _menuScreen;
    private MazeScreen _mazeScreen;
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
        try
        {
            _mazeScreen = new MazeScreen(this, _graphics, mapProvider, Back, width, height);
            Components.Remove(_menuScreen);
            _menuScreen.Dispose();
            Components.Add(_mazeScreen);
        }catch (Exception e)
        {
            logger.Error(e.Message);
            _mazeScreen.Dispose();
            Components.Remove(_mazeScreen);
            _menuScreen = new MenuScreen(this, _graphics, LoadMaze);
            _menuScreen.errorMessage = "Maze Map invalid";
            Components.Add(_menuScreen);
        }
    }
    protected void Back()
    {
        _mazeScreen.Dispose();
        Components.Remove(_mazeScreen);
        _menuScreen = new MenuScreen(this, _graphics, LoadMaze);
        Components.Add(_menuScreen);
    }
}


