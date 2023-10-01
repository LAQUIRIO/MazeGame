using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public delegate void Action();

namespace MazeGame
{
    public sealed class InputManager
    {
        private static List<(Keys, Action)> keys = new List<(Keys, Action)>();
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
        public static void Update()
        {
            if (instance != null) { 
                keys.ForEach((key) =>
                {
                    if (Console.KeyAvailable)
                    {
                        /*ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        if (keyInfo.KeyChar == key.Item1)
                        {
                            key.Item2();
                        }*/
                    }
                }); 
            }
        }
        public static void AddKeyHandler(Keys key,Action action)
        {
            if (instance != null) { 
                keys.Add((key,action));
            }
        }
    }
}
