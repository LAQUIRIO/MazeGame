using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace MazeGame
{
    public delegate void Action();
    public delegate void HasPLayerMoved(bool hasMoved);
    public sealed class InputManager
    {
        private List<(Keys, Action)> keys = new List<(Keys, Action)>();
        private static InputManager instance = null;
        private KeyboardState previousState;
        private readonly Logger logging = LogManager.GetCurrentClassLogger();
        public Boolean playerMoved = false;
        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputManager();
                    instance.playerMoved = true;
                }
                return instance;
            }
        }
        public void Update(HasPLayerMoved setPLayerMoved)
        {
            KeyboardState state = Keyboard.GetState();
            if (instance != null) {
                foreach (var key in keys)
                {
                    if (state.IsKeyDown(key.Item1)&&previousState.IsKeyUp(key.Item1))
                    {
                        logging.Info($" {key.Item1} Key pressed");
                        key.Item2();
                        setPLayerMoved(true);
                    }
                }
            }
            previousState = state;
        }
        public void AddKeyHandler(Keys key,Action action)
        {
            if (instance != null) { 
                keys.Add((key,action));
            }
        }
    }
}
