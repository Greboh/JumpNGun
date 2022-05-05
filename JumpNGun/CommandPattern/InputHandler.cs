using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JumpNGun
{
    public enum ButtonState
    {
        Up,
        Down
    }

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

        private Vector2 _up = new Vector2(0, -1);
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
            _keybindings.Add(Keys.W, new JumpCommand());
            _keybindings.Add(Keys.A, new MoveCommand(_left));
            _keybindings.Add(Keys.D, new MoveCommand(_right));
        }

        public void Execute(Player player)
        {
            KeyboardState keyState = Keyboard.GetState();

            foreach (Keys key in _keybindings.Keys)
            {
                if(keyState.IsKeyDown(key))
                {
                    _keybindings[key].Execute(player);
                }
      
                if(key == Keys.W)
                {
                    if(keyState.IsKeyDown(key))
                    {
                        EventManager.Instance.TriggerEvent("OnJump", new Dictionary<string, object>()
                            {
                                {"buttonState", ButtonState.Down}
                            }
                        );
                    }
                    if(keyState.IsKeyUp(key))
                    {
                        EventManager.Instance.TriggerEvent("OnJump", new Dictionary<string, object>()
                            {
                                {"buttonState", ButtonState.Up}
                            }
                        );
                    }

                }
                
                
            }
  
        }
    }
}