using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace MazeGame
{
    public sealed class InputManager
    {
        private Dictionary<Keys, Action> keys = new Dictionary<Keys, Action>();
        private static InputManager instance = null;
        private KeyboardState previousState;
        private readonly Logger logging = LogManager.GetCurrentClassLogger();
        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputManager();
                }
                return instance;
            }
        }
        public void Update()
        {
            KeyboardState state = Keyboard.GetState();
            if (instance != null) {
                foreach (var key in keys)
                {
                    if (state.IsKeyDown(key.Key)&&previousState.IsKeyUp(key.Key))
                    {
                        logging.Info($" {key.Key} Key pressed");
                        key.Value();
                    }
                }
            }
            previousState = state;
        }
        public void AddKeyHandler(Keys key,Action action)
        {
            if (instance != null) {
                if (keys.ContainsKey(key))
                {
                    keys[key] += action;
                }
                else
                {
                    keys.Add(key, action);
                }
            }
        }
    }
}
