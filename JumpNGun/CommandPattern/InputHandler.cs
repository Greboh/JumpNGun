using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JumpNGun
{
    public class InputHandler
    {
        private static InputHandler _instance;

        public static InputHandler Intance
        {
            get { return _instance ??= new InputHandler(); }
        }

        // Dictionary that contains TKeys Keys and TValues ICommands
        private Dictionary<Keys, ICommand> _keybindings = new Dictionary<Keys, ICommand>();


        #region MoveDirections

        private Vector2 _up = new Vector2(0, -20);
        private Vector2 _left = new Vector2(-1, 0);
        private Vector2 _right = new Vector2(1, 0);
        private Vector2 _down = new Vector2(0, 1);

        #endregion

        private InputHandler()
        {
            AddBindings();
        }

        /// <summary>
        /// Adds all bindings used
        /// </summary>
        private void AddBindings()
        {
            _keybindings.Add(Keys.W, new MoveCommand(_up));
            _keybindings.Add(Keys.A, new MoveCommand(_left));
            _keybindings.Add(Keys.D, new MoveCommand(_right));
            _keybindings.Add(Keys.S, new MoveCommand(_down));
        }

        public void Execute(Player player)
        {
            KeyboardState keyState = Keyboard.GetState();

            foreach (var key in _keybindings.Keys.Where(key => keyState.IsKeyDown(key)))
            {
                _keybindings[key].Execute(player);
            }
        }
    }
}