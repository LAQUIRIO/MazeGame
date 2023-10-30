using Microsoft.Xna.Framework;
using MazeRecursion;
using System;
using Microsoft.Xna.Framework.Graphics;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using NLog;
using Maze;
using System.Windows.Forms;

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
        private Enum _selectedText;
        private Enum _previouslySelectedText;
        private SpriteBatch _spriteBatch;
        private InputManager _inputManager;
        private SpriteFont _font;
        private int _width, _height;
        private bool _dimensionChange;
        private Type _screenState = typeof(MainMenuText);
        private readonly int _minDimension = 5;
        private readonly int _maxDimension = 25;

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
            _height = _minDimension;
            _width = _minDimension;
            
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
                    _screenState = typeof(SizeMenuText);
                    SetSizeMenuKeys();
                    _selectedText = SizeMenuText.Width;
                    break;
                case MainMenuText.Select_Maze_From_File:
                    try
                    {
                        _loadMaze(SelectMap(), null, null);
                    }catch(Exception e)
                    { 
                    _logger.Debug(e.Message);
                    }
                        break;
                case SizeMenuText.Generate_Maze:
                    _loadMaze(new MazeGenerator(null, null), (_width-1)/2, (_height-1)/2);
                    break;
                case SizeMenuText.Exit:
                    _screenState = typeof(MainMenuText);
                    RemoveSizeMenuKeys();
                    _selectedText = MainMenuText.Recursive_Generator;
                    break;
                case MainMenuText.Exit:
                    _game.Exit();
                    break;
            }
        }

        public void SetSizeMenuKeys()
        {
            _inputManager.AddKeyHandler(Keys.Left, LowerDimension);
            _inputManager.AddKeyHandler(Keys.Right, RiseDimension);
        }
        private void RemoveSizeMenuKeys()
        {
            _inputManager.RemoveKeyHandler(Keys.Left, LowerDimension);
            _inputManager.RemoveKeyHandler(Keys.Right, RiseDimension);
        }
        public void LowerDimension()
        {
            UpdateDimension(-2);
        } 
        public void RiseDimension()
        {
            UpdateDimension(+2);
        }
        private void UpdateDimension(int direction)
        {
            if (_selectedText.Equals(SizeMenuText.Width))
            {
                if (validSize(_width+direction))
                {
                    _width += direction;
                }
            }
            else if (_selectedText.Equals(SizeMenuText.Height))
            {
                if (validSize(_height+direction))
                {
                    _height += direction;
                }
            }
            _dimensionChange = true;
        }

        private bool validSize(int size)
        {
            return size >= _minDimension && size <= _maxDimension;
        }

        public void ChangeSelectedText(int moveDirection)
        {
            int selectedText = (int)Enum.ToObject(_screenState, _selectedText);
            int numberOfTexts = Enum.GetNames(_screenState).Length;
            if (numberOfTexts > selectedText + moveDirection && selectedText + moveDirection >= 0)
            {
                Array enumValues = Enum.GetValues(_screenState);
                _selectedText = (Enum)enumValues.GetValue(selectedText + moveDirection);
            }
        }
        protected override void LoadContent()
        {
            _font = _game.Content.Load<SpriteFont>("font");
            base.LoadContent();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _inputManager.RemoveKeyHandler(Keys.Up, SelectAbove);
                _inputManager.RemoveKeyHandler(Keys.Down, SelectBelow);
                _inputManager.RemoveKeyHandler(Keys.Enter, Select);
                RemoveSizeMenuKeys();
            }
            base.Dispose(disposing);
        }
        public override void Update(GameTime gameTime)
        {

            _inputManager.Update();
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            if (_selectedText != _previouslySelectedText || _dimensionChange)
            {
                GraphicsDevice.Clear(Color.Black);
                _spriteBatch.Begin();
                DrawText("Maze Game", new Vector2(100, 100), Color.White);
                foreach (var enumValue in Enum.GetValues(_selectedText.GetType()))
                {
                    Vector2 textPlacement = new Vector2(100, 200 + 100 * (int)enumValue + 1);
                    string text = Enum.GetName(_screenState, enumValue);
                    if (enumValue.Equals(SizeMenuText.Width))
                    {
                        text += $" {_width}";
                    }
                    else if (enumValue.Equals(SizeMenuText.Height))
                    {
                        text += $" {_height}";
                    }
                    if (enumValue.Equals(_selectedText))
                    {
                        DrawText(text, textPlacement, Color.Red);
                    }
                    else
                    {
                        DrawText(text, textPlacement, Color.White);
                    }
                }
                _spriteBatch.End();
                _previouslySelectedText = _selectedText;
                _dimensionChange = false;
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
