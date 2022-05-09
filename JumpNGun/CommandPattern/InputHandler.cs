using System.Collections.Generic;
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
        // private Dictionary<Keys, ICommand> _keybindings = new Dictionary<Keys, ICommand>();
        private Dictionary<KeyInfo, ICommand> _keybindings = new Dictionary<KeyInfo, ICommand>();


        #region MoveDirections

        private Vector2 _left = new Vector2(-1, 0);
        private Vector2 _right = new Vector2(1, 0);

        #endregion

        private InputHandler()
        {
            InitializeInput();
        }

        /// <summary>
        /// Adds all bindings used
        /// </summary>
        private void InitializeInput()
        {
            // _keybindings.Add(Keys.W, new JumpCommand());
            // _keybindings.Add(Keys.A, new MoveCommand(_left));
            // _keybindings.Add(Keys.D, new MoveCommand(_right));
            // _keybindings.Add(Keys.Space, new ShootCommand());            

            _keybindings.Add(new KeyInfo(Keys.A), new MoveCommand(_left));
            _keybindings.Add(new KeyInfo(Keys.D), new MoveCommand(_right));
            _keybindings.Add(new KeyInfo(Keys.W), new JumpCommand());
            _keybindings.Add(new KeyInfo(Keys.LeftAlt), new DashCommand());
            _keybindings.Add(new KeyInfo(Keys.Space), new ShootCommand());

        }

        public void Execute(Player player)
        {
            KeyboardState keyState = Keyboard.GetState();

            foreach (KeyInfo keyInfo in _keybindings.Keys)
            {
                if (keyState.IsKeyDown(keyInfo.Key))
                {
                    _keybindings[keyInfo].Execute(player);
                    keyInfo.IsDown = true;

                    EventManager.Instance.TriggerEvent("OnKeyPress", new Dictionary<string, object>()
                        {
                            {"key", keyInfo.Key},
                            {"isKeyDown", keyInfo.IsDown}
                        }
                    );

                }

                if (!keyState.IsKeyDown(keyInfo.Key) && keyInfo.IsDown == true)
                {
                    keyInfo.IsDown = false;

                    EventManager.Instance.TriggerEvent("OnKeyPress", new Dictionary<string, object>()
                        {
                            {"key", keyInfo.Key},
                            {"isKeyDown", keyInfo.IsDown}
                        }
                    );
                }
            }

        }

        public class KeyInfo
        {
            public bool IsDown { get; set; }

            public Keys Key { get; set; }

            public KeyInfo(Keys key)
            {
                this.Key = key;
            }
        }
    }
}