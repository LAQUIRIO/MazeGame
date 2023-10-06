using Maze;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using System.Drawing;
using System.IO;
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
    private PlayerSprite _playerSprite;
    private Texture2D _wall;
    private Texture2D _floor;
    private Texture2D _goal;
    private readonly int _texturesSize = 32;
    public MazeGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        IMapProvider mapProvider = SelectMap();
        _map = new Map(mapProvider);
        _map.CreateMap();

        base.Exiting += (sender, args) => logger.Info($"Game finished: game window is closed");
        logger.Info($"Player's starting position: x={_map.Player.StartX}, y={_map.Player.StartY}");
        logger.Info($"Goal's position: x={_map.Goal.X}, y={_map.Goal.Y}");

        _graphics.PreferredBackBufferWidth = _map.Width * _texturesSize;
        _graphics.PreferredBackBufferHeight = _map.Height * _texturesSize;
        _graphics.ApplyChanges();

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
        _goal = Content.Load<Texture2D>("Tree");
        _wall = Content.Load<Texture2D>("wall");
        _floor = Content.Load<Texture2D>("path");
        DrawMap();
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        { 
            logger.Info("Game finished: player has quit");
            Exit();
        }
        if (_map.IsGameFinished)
        {
            logger.Info("Game finished: player has reached the goal");
            Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
    private void DrawMap()
    {
        Block[,] grid = _map.MapGrid;
        _spriteBatch.Begin();
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                if (grid[y, x] == Block.Solid)
                {
                    logger.Debug($"Draw wall at {x},{y}");
                    _spriteBatch.Draw(_wall, new Vector2(x * _wall.Width, y * _wall.Height), Color.White);
                }
                else
                {
                    logger.Debug($"Draw path at {x},{y}");
                    _spriteBatch.Draw(_floor, new Vector2(x * _floor.Width, y * _floor.Height), Color.White);
                }
            }
        }
        _spriteBatch.Draw(_goal, new Vector2(_map.Goal.X * _goal.Width, _map.Goal.Y * _goal.Height), Color.White);
        _spriteBatch.End();
    }   
}
