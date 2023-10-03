using Maze;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using System;

namespace MazeGame;

public class MazeGame : Game
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private readonly IMap _map;
    private PlayerSprite _playerSprite;
    private Texture2D _wall;
    private Texture2D _floor;
    private Texture2D _goal;
    private readonly int _texturesSize = 32;
    private bool _drawn = false;
    public MazeGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        IMapProvider mapProvider = new MazeFromFile.MazeFromFile("C:\\Users\\laqui\\OneDrive\\c#\\2\\Assignment 1\\map9x13.txt");
        _map = new Map(mapProvider);
        _map.CreateMap();
        logger.Info($"Player's starting position: x={_map.Player.StartX}, y={_map.Player.StartY}");
        logger.Info($"Goal's position: x={_map.Goal.X}, y={_map.Goal.Y}");
        _graphics.PreferredBackBufferWidth = _map.Width * _texturesSize;
        _graphics.PreferredBackBufferHeight = _map.Height * _texturesSize;
        _graphics.PreferMultiSampling = true;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Window.AllowUserResizing = true;
        _playerSprite = new PlayerSprite(this, _map.Player);
        this.Components.Add(_playerSprite);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _goal = Content.Load<Texture2D>("Tree");
        _wall = Content.Load<Texture2D>("wall");
        _floor = Content.Load<Texture2D>("path");
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        if (_map.IsGameFinished)
        {
            logger.Info("Game finished: player has reached the goal");
            Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        if (!_drawn)
        {
        }
        _playerSprite.Draw(gameTime);
    }
}
