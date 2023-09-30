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

    }
}
