using Maze;
using MazeRecursion;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using System.Windows.Forms;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace MazeGame;

delegate void LoadMazeFunc(IMapProvider mapProvider, int? width, int? heigth);
public class MazeGame : Game
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
   /* private readonly string[] _stateList = { "Menu", "Maze" };
    private int _previousGameState = 0;
    private int _gameState = 1;*/
    private MenuScreen _menuScreen;
    private MazeScreen _mazeScreen;
    private IMapProvider _mapProvider;
    private int? _width, _heigth;
    public MazeGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Exiting += (sender, args) => logger.Info($"Game finished: game window is closed");
        //_menuScreen = new MenuScreen(this, _graphics, LoadMaze);
        //Components.Add(_menuScreen);
        LoadMaze(_mapProvider, _width, _heigth);
        Components.Remove(_mazeScreen);
        Components.Add(_mazeScreen);
        base.Initialize();
    }
    private static IMapProvider SelectMap()
    {
        string path = "";
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                path = openFileDialog.FileName;

                logger.Info($"Selected map: {path}");
            }
        }
        return new MazeFromFile.MazeFromFile(path);
    }
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("font");
        base.LoadContent();
    }

   /*protected override void Update(GameTime gameTime)
    {
        if (_previousGameState != _gameState)
        {
            if (_stateList[_gameState] == "Menu")
            {
                _previousGameState = _gameState;
                *//*_menuScreen.Reset();
                if (_mazeScreen != null)
                {
                    Components.Remove(_mazeScreen);
                    _mazeScreen.Dispose();
                }
                Components.Add(_menuScreen);*//*
                LoadMaze(new MazeGenerator(null,null), 6, 8);
            }
            else if (_stateList[_gameState] == "Maze" && _mapProvider != null)
            {
            }

        }
        base.Update(gameTime);
    }*/
    protected void LoadMaze(IMapProvider mapProvider, int? width, int? heigth)
    {
        logger.Info($"left Main Menu");
        _mapProvider = new MazeGenerator(null, null);
        _mazeScreen = new MazeScreen(this, _graphics, _mapProvider, Back, _width, _heigth);
        Components.Add(_mazeScreen);
    }
    protected void Back()
    {
        _mazeScreen.Dispose();
        _menuScreen = null;

        if (_mazeScreen != null)
        _mapProvider = new MazeGenerator(null, null);
        _mazeScreen = new MazeScreen(this, _graphics, _mapProvider, Back, _width, _heigth);
        Components.Add(_mazeScreen);
    }
}


