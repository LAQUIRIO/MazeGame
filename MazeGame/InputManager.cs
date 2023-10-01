using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    public delegate void Action();
    public sealed class InputManager
    {
        private List<(Keys, Action)> keys = new List<(Keys, Action)>();
        private static InputManager instance = null;
        private KeyboardState previousState;
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
                    if (state.IsKeyDown(key.Item1)&&previousState.IsKeyUp(key.Item1))
                    {
                        key.Item2();
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
