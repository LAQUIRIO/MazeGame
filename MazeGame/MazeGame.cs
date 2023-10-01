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
    IMap map;
    private Texture2D _wall;
    private Texture2D _floor;
    private Texture2D _player;
    private Texture2D _goal;
    private Game _game;
    public MazeGame()
    {
        IMapProvider mapProvider = new MazeFromFile.MazeFromFile("C:\\Users\\laqui\\OneDrive\\c#\\2\\Assignment 1\\map9x13.txt");
        Direction[,] dir = mapProvider.CreateMap();
        map = new Map(mapProvider);
        map.CreateMap();
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = map.Width * 32;
        _graphics.PreferredBackBufferHeight = map.Height * 32;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        this.BeginDraw();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _goal = Content.Load<Texture2D>("Tree");
        _wall = Content.Load<Texture2D>("wall");
        _floor = Content.Load<Texture2D>("path");
        // TODO: use this.Content to load your game content here
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

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
                    _spriteBatch.Draw(_wall, new Vector2(x*32, y*32), Color.White);
                }
                else
                {
                    _spriteBatch.Draw(_floor, new Vector2(x*32, y*32), Color.White);
                }
            }
        }
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
