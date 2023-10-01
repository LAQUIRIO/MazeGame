using Maze;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MazeFromFile;
using System.Xml.Linq;

namespace MazeGame;

public class MazeGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private IMap map;
    private PlayerSprite _playerSprite;
    private Texture2D _wall;
    private Texture2D _floor;
    private Texture2D _player;
    private Texture2D _goal;
    private readonly int _textireSize = 32;
    private Game _game;
    public MazeGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        IMapProvider mapProvider = new MazeFromFile.MazeFromFile("C:\\Users\\laqui\\OneDrive\\c#\\2\\Assignment 1\\map9x7.txt");
        Direction[,] dir = mapProvider.CreateMap();
        map = new Map(mapProvider);
        map.CreateMap();
        _graphics.PreferredBackBufferWidth = map.Width * _textireSize;
        _graphics.PreferredBackBufferHeight = map.Height * _textireSize;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _playerSprite = new PlayerSprite(this, map.Player);
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
        if (map.IsGameFinished)
        {
            Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();
        Block[,] grid = map.MapGrid;
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                if (grid[y, x] == Block.Solid)
                {
                    _spriteBatch.Draw(_wall, new Vector2(x*_wall.Width, y* _wall.Height), Color.White);
                }
                else
                {
                    _spriteBatch.Draw(_floor, new Vector2(x*_floor.Width, y* _floor.Height), Color.White);
                }
            }
        }
        _spriteBatch.Draw(_goal, new Vector2(map.Goal.X * _textireSize, map.Goal.Y * _textireSize), Color.White);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
