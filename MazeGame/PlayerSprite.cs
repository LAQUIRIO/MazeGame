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
        private Game _game;
        private Texture2D _PlayerTexture;

        public PlayerSprite(Game game) : base(game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }
            _game = game;
        }
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_texture, new Vector2(_player.StartX, _player.StartY), Color.White;
            _spriteBatch.End();
            base.Draw(gameTime);

        }

    }
}
