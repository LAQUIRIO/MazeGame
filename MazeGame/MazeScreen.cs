using Maze;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLog;
using System;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace MazeGame
{
    internal class MazeScreen : DrawableGameComponent
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private GraphicsDeviceManager _graphics;
        private readonly IMapProvider _mapProvider;
        private readonly int? _width, _heigth;
        private readonly int _texturesSize = 32;
        private readonly Action _back;
        private SpriteBatch _spriteBatch;
        private Game game;
        private IMap _map;
        private PlayerSprite _playerSprite;
        private Texture2D _wall;
        private Texture2D _floor;
        private Texture2D _goal;

        public MazeScreen(Game game, GraphicsDeviceManager graphicsDevice, IMapProvider mapProvider, Action back, int? width, int? heigth) : base(game)
        {
            this.game = game;
            _graphics = graphicsDevice;
            _mapProvider = mapProvider;
            _back = back;
            _width = width;
            _heigth = heigth;
        }


        public override void Initialize()
        {
            _map = new Map(_mapProvider);
            if (_width.HasValue && _heigth.HasValue)
            {
                _map.CreateMap(_width.Value, _heigth.Value);
            }
            else
            {
                _map.CreateMap();
            }
            logger.Info($"Player's starting position: x={_map.Player.StartX}, y={_map.Player.StartY}");
            logger.Info($"Goal's position: x={_map.Goal.X}, y={_map.Goal.Y}");
            _graphics.PreferredBackBufferWidth = _map.Width * _texturesSize;
            _graphics.PreferredBackBufferHeight = _map.Height * _texturesSize;
            _graphics.ApplyChanges();

            _playerSprite = new PlayerSprite(game, _map.Player);
            game.Components.Add(_playerSprite);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
            _goal = game.Content.Load<Texture2D>("Tree");
            _wall = game.Content.Load<Texture2D>("wall");
            _floor = game.Content.Load<Texture2D>("path");
            DrawMap();
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            _playerSprite.Update(gameTime);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                logger.Info("Game finished: player has quit");
                _back();
            }

            else if (_map.IsGameFinished)
            {
                logger.Info("Game finished: player has reached the goal");
                _back();
            }
        }

        private void DrawMap()
        {
            //draw map from _map's block grid
            Block[,] grid = _map.MapGrid;
            _spriteBatch.Begin();
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (grid[y, x] == Block.Solid)
                    {
                        //walls
                        logger.Debug($"Draw wall at {x},{y}");
                        _spriteBatch.Draw(_wall, new Vector2(x * _wall.Width, y * _wall.Height), Color.White);
                    }
                    else
                    {
                        //floor
                        logger.Debug($"Draw path at {x},{y}");
                        _spriteBatch.Draw(_floor, new Vector2(x * _floor.Width, y * _floor.Height), Color.White);
                    }
                }
            }
            _spriteBatch.Draw(_goal, new Vector2(_map.Goal.X * _goal.Width, _map.Goal.Y * _goal.Height), Color.White);
            _spriteBatch.End();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _map = null;
                game.Components.Remove(this);
                game.Components.Remove(_playerSprite);
                if (_playerSprite != null)
                {
                    _playerSprite.Dispose();
                }
                if (_spriteBatch != null) 
                {
                    _spriteBatch.Dispose(); 
                }

            }
            base.Dispose(disposing);
        }
    }
}
