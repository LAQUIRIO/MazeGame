using Maze;
using Microsoft.Xna.Framework;
using System;
using NLog;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeGame
{
    internal class PlayerSprite : DrawableGameComponent
    {
        private readonly Logger logging = LogManager.GetCurrentClassLogger();
        private IPlayer _player;
        private Texture2D _texture;
        private Texture2D _floor;
        private Vector2 _oldPosition;
        private SpriteBatch _spriteBatch;
        private readonly Game _game;
        private InputManager _inputManager;
        private bool _playerMoved;

        public PlayerSprite(Game game, IPlayer player) : base(game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }
            _game = game;
            _player = player;
        }
        public override void Initialize()
        {
            _playerMoved = true;
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);

            _inputManager = InputManager.Instance;
            _inputManager.AddKeyHandler(Keys.Up, _player.MoveForward);
            _inputManager.AddKeyHandler(Keys.Down, _player.MoveBackward);
            _inputManager.AddKeyHandler(Keys.Left, _player.TurnLeft);
            _inputManager.AddKeyHandler(Keys.Right, _player.TurnRight);

            _oldPosition = new Vector2(_player.Position.X, _player.Position.Y);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _floor = _game.Content.Load<Texture2D>("path");
            _texture = _game.Content.Load<Texture2D>("LadyBug");
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            _inputManager.Update(value=> _playerMoved = value);
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            if (_playerMoved)
            {
                DrawPlayer();
                logging.Debug($"Draw player at {_player.Position.X},{_player.Position.Y} looking {_player.Facing}");
                _oldPosition.X = _player.Position.X;
                _oldPosition.Y = _player.Position.Y;
                _playerMoved = false;
            }

        }
        private void DrawPlayer()
        {
            _spriteBatch.Begin();
            logging.Debug($"Draw path at {_oldPosition.X},{_oldPosition.Y}");
            //draw floor on old position
            _spriteBatch.Draw(_floor, new Vector2(_oldPosition.X * _floor.Width, _oldPosition.Y * _floor.Height), Color.White);
            //set player position to center of the block
            Vector2 vector2 = new Vector2(getStartingPoint(_player.Position.X, _texture.Width), getStartingPoint(_player.Position.Y, _texture.Height));
            //set rotation point to center of the image
            Vector2 center = new Vector2(_texture.Width / 2, _texture.Height / 2);
            //draw player
            _spriteBatch.Draw(_texture, vector2, null, Color.White, _player.GetRotation(), center, 1, SpriteEffects.None, 1);
            _spriteBatch.End();
        }
        private float getStartingPoint(int position, int imgSize)
        {
            return (position) * imgSize + imgSize / 2;
        }

    }
}
