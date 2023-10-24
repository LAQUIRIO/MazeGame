using Maze;
using MazeRecursion;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using System.Windows.Forms;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Color = Microsoft.Xna.Framework.Color;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace MazeGame;

public class MazeGame : Game
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private IMap _map;
    private MediaState gameState;
    private PlayerSprite _playerSprite;
    private Texture2D _wall;
    private Texture2D _floor;
    private SpriteFont _font;
    private Texture2D _goal;
    private readonly int _texturesSize = 32;
    private bool interfaceDrawn = false;
    private int _menuIndex = 0;
    private readonly string[] _menuItems = { "Start", "select Map Size", "Maze" };
    public MazeGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        IMapProvider mapProvider = new MazeGenerator(new MapVector(0,0), 1);
        _map = new Map(mapProvider);
        _map.CreateMap(4, 4);

        base.Exiting += (sender, args) => logger.Info($"Game finished: game window is closed");
        logger.Info($"Player's starting position: x={_map.Player.StartX}, y={_map.Player.StartY}");
        logger.Info($"Goal's position: x={_map.Goal.X}, y={_map.Goal.Y}");


        _playerSprite = new PlayerSprite(this, _map.Player);
        this.Components.Add(_playerSprite);
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
        this.
        _goal = Content.Load<Texture2D>("Tree");
        _wall = Content.Load<Texture2D>("wall");
        _floor = Content.Load<Texture2D>("path");
        _font = Content.Load<SpriteFont>("font");
        //DrawMap();
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        { 
            logger.Info("Game finished: player has quit");
            Exit();
        }

        base.Update(gameTime);
    }


