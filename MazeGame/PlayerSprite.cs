using Maze;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeGame
{
    internal class PlayerSprite : DrawableGameComponent
    {
        private IPlayer _player;
        private Texture2D _texture;
        private SpriteBatch _spriteBatch;
        private readonly Game _game;
        private InputManager _inputManager;

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
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            _inputManager = InputManager.Instance;
            _inputManager.AddKeyHandler(Keys.Up, _player.MoveForward);
            _inputManager.AddKeyHandler(Keys.Down, _player.MoveBackward);
            _inputManager.AddKeyHandler(Keys.Left, _player.TurnLeft);
            _inputManager.AddKeyHandler(Keys.Right, _player.TurnRight);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _texture = _game.Content.Load<Texture2D>("LadyBug");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _inputManager.Update();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            Vector2 vector2 = new Vector2(getStartingPoint(_player.StartX, _texture.Width), getStartingPoint(_player.StartY,_texture.Height));
            Vector2 center = new Vector2(_texture.Width / 2, _texture.Height / 2);
            _spriteBatch.Draw(_texture, vector2, null,Color.White,_player.GetRotation(), center, 1,SpriteEffects.None,1);
            _spriteBatch.End();
            base.Draw(gameTime);

        }
        private float getStartingPoint(int position, int imgSize)
        {
            return (position) * imgSize + imgSize / 2;
        }

    }
}
