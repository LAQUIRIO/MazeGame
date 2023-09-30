using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public delegate void Action();

namespace MazeGame
{
    public sealed class InputManager
    {
        private static List<(char,Action)> values = new List<(char, Action)>();
        private static InputManager instance = null;
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
        public void AddKeyHandler(char key,Action action)
        {
            if (instance != null) { 
                values.Add((key,action));
            }
        }
    }
}
