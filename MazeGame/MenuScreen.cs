using Microsoft.Xna.Framework;
using MazeRecursion;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using NLog;
using Maze;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MazeGame
{
    enum SizeMenuText
    {
        Width,
        Height,
        Generate_Maze,
        Exit
    }
    enum MainMenuText
    {
        Recursive_Generator,
        Select_Maze_From_File,
        Exit
    }
    internal class MenuScreen : DrawableGameComponent
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly LoadMazeFunc _loadMaze;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Game _game;
        private Enum? _selectedText;
        private Enum? _previouslySelectedText;
        private SpriteBatch _spriteBatch;
        private InputManager _inputManager;
        private SpriteFont _font;
        private int? _width, _height;
        
        private Type _screenState = typeof(MainMenuText);

        public static object logger { get; private set; }

        public MenuScreen(Game game, GraphicsDeviceManager graphicsDevice, LoadMazeFunc loadMaze) : base(game)
        {
            _game = game;
            _graphics = graphicsDevice;
            _loadMaze = loadMaze;
        }
        public override void Initialize()
        {
            _inputManager = InputManager.Instance;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _selectedText = MainMenuText.Recursive_Generator;
            _previouslySelectedText = null;
            
            _inputManager.AddKeyHandler(Keys.Up, SelectAbove);
            _inputManager.AddKeyHandler(Keys.Down, SelectBelow);
            _inputManager.AddKeyHandler(Keys.Enter, Select);

            _graphics.PreferredBackBufferWidth =  900;
            _graphics.PreferredBackBufferHeight = 650;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        public void SelectAbove()
        {
            ChangeSelectedText(-1);
        }
        public void SelectBelow()
        {
            ChangeSelectedText(1);
        }
        public void Select()
        {
            switch (_selectedText)
            {
                case MainMenuText.Recursive_Generator:
                    _state = MenuState.SizeMenu;
                    break;
                case MainMenuText.Select_Maze_From_File:
                    _loadMaze(SelectMap(), null, null);
                    break;
                case MainMenuText.Exit:
                    _game.Exit();
                    break;
            }
        }
        public void ChangeSelectedText(int moveDirection)
        {
            int selectedText = (int)_selectedText;
            int numberOfTexts = Enum.GetNames(typeof(MainMenuText)).Length;
            if (numberOfTexts > selectedText + moveDirection && selectedText + moveDirection >= 0)
            {
                _selectedText = (MainMenuText)(selectedText + moveDirection);
            }
        }
        protected override void LoadContent()
        {
            _font = _game.Content.Load<SpriteFont>("font");
            base.LoadContent();
        }
        internal void Reset()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {

            _inputManager.Update();
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            if (_selectedText != _previouslySelectedText)
            {
                GraphicsDevice.Clear(Color.Black);
                _spriteBatch.Begin();
                DrawText("Maze Game", new Vector2(100, 50), Color.White);   
                for (int i = 0; i < Enum.GetNames(typeof(MainMenuText)).Length; i++)
                {
                    Vector2 textPlacement = new Vector2(100, 110 + 60 * i);
                    if (_selectedText == (MainMenuText)i)
                    {
                        DrawText(Enum.GetName(typeof(MainMenuText), i), textPlacement, Color.Red);
                    }
                    else
                    {
                        DrawText(Enum.GetName(typeof(MainMenuText), i), textPlacement, Color.White);
                    }
                }
                _spriteBatch.End();
                _previouslySelectedText = _selectedText;
            }


            base.Draw(gameTime);
        }
        private void DrawText(string text, Vector2 vector, Color color)
        {
            _spriteBatch.DrawString(_font, text.Replace('_',' '), vector, color);
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

                    _logger.Info($"Selected map: {path}");
                }
            }
            return new MazeFromFile.MazeFromFile(path);
        }
    }
}
