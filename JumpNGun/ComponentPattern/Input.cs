using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace JumpNGun
{
    public enum MouseButton
    {
        None,
        Mouse0, // Left Click
        Mouse1, // Right Click
    }

    public struct KeyCode
    {
        public Keys KeyboardBinding { get; set; }
        public MouseButton MouseBinding { get; set; }

        public string ActionName { get; set; }

        /// <summary>
        /// Whether to use keyboard or mouse on this keybind
        /// </summary>
        public bool PreferKeyboard { get; set; }
        
        #region Constructors

        public KeyCode(Keys key, string actionName)
        {
            KeyboardBinding = key;
            MouseBinding = MouseButton.None;
            ActionName = actionName;

            PreferKeyboard = true;
        }


        public KeyCode(MouseButton mouse, string actionName)
        {
            KeyboardBinding = Keys.None;
            MouseBinding = mouse;
            ActionName = actionName;

            PreferKeyboard = false;
        }

        #endregion
    }

    public class Input : Component
    {
        private KeyboardState _currentKeyState;
        private MouseState _currentMouseState;
        private Dictionary<KeyCode, ICommand> _keybindings = new Dictionary<KeyCode, ICommand>();

        public override void Start()
        {

            BindKey(new KeyCode(Keys.W, "jump"), new JumpCommand());
            BindKey(new KeyCode(Keys.A,"move_left"),new MoveCommand(new Vector2(-1, 0)));
            BindKey(new KeyCode(Keys.D, "move_right"), new MoveCommand(new Vector2(1, 0)));
            BindKey(new KeyCode(Keys.LeftAlt, "Dash"), new DashCommand());
            
            
            BindKey(new KeyCode(MouseButton.Mouse0,"shoot"), new ShootCommand());
            
            PrintAllKeybindings();

            RebindKey(new KeyCode(MouseButton.Mouse1, "shoot"), new ShootCommand());
            
            
            // PrintKeybindings();
        }


        public override void Update(GameTime gameTime)
        {
            _currentKeyState = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();
        }

        public void Execute(Player player)
        {
            foreach (KeyCode keyCode in _keybindings.Keys)
            {
                if (keyCode.PreferKeyboard)
                {
                    if (_currentKeyState.IsKeyDown(keyCode.KeyboardBinding))
                    {
                        // Console.WriteLine(keyCode.KeyboardBinding);
                        _keybindings[keyCode].Execute(player);
                    }
                }
                else
                {
                    if (keyCode.MouseBinding == GetMouseButtons())
                    {
                        // Console.WriteLine(keyCode.MouseBinding);
                        _keybindings[keyCode].Execute(player);
                    }
                }
            }
        }


        #region Binding & Rebinding

        private void BindKey(KeyCode keyCode, ICommand command)
        {
            if (!_keybindings.ContainsKey(keyCode))
            {
                _keybindings.Add(keyCode, command);
            }
            else Console.Error.WriteLine($"An error occured when binding: An item with the same key has already been added => {keyCode.ActionName.ToLower()}");
        }

        private void RebindKey( KeyCode newKey, ICommand command)
        {
            foreach (KeyValuePair<KeyCode, ICommand> keybinding in _keybindings)
            {
                if(keybinding.Key.ActionName == newKey.ActionName)
                {
                    Console.WriteLine("\nRebinding");
                    
                    PrintKeybinding(keybinding.Key);
                    
                    _keybindings.Remove(keybinding.Key);
                }
            }

            _keybindings.Add(newKey, command);
            
            Console.WriteLine($"\nRebinding was successful!");
            PrintKeybinding(newKey);
        }

        #endregion
        
        private MouseButton GetMouseButtons()
        {
            MouseButton mouse = new MouseButton();

            //Check the button
            if (_currentMouseState.LeftButton == ButtonState.Pressed)
            {
                mouse = MouseButton.Mouse0;
            }
            else if (_currentMouseState.RightButton == ButtonState.Pressed)
            {
                mouse = MouseButton.Mouse1;
            }

            return mouse;
        }

        #region Prints

        private void PrintAllKeybindings()
        {
            Console.WriteLine("All Keybindings: ");
            foreach (KeyValuePair<KeyCode, ICommand> keyCode in _keybindings)
            {
                Console.WriteLine("_____________________________________________");
                Console.WriteLine($"1. ActionName: {keyCode.Key.ActionName}\n" +
                                  $"2. Keyboard-binding: {keyCode.Key.KeyboardBinding}\n" +
                                  $"3. Mouse-Binding: {keyCode.Key.MouseBinding}\n" +
                                  $"4. Command: {keyCode.Value.GetType().Name}");
                Console.WriteLine("_____________________________________________");
            }
        }

        private void PrintKeybinding(KeyCode key)
        {
            foreach (var keybinding in _keybindings)
            {
                if (key.ActionName == keybinding.Key.ActionName)
                {
                    Console.WriteLine("_____________________________________________");
                    Console.WriteLine($"1. ActionName: {key.ActionName}\n" +
                                      $"2. Keyboard-binding: {key.KeyboardBinding}\n" +
                                      $"3. Mouse-Binding: {key.MouseBinding}\n" +
                                      $"4. Command: {keybinding.Value.GetType().Name}");
                    Console.WriteLine("_____________________________________________");
                }
            }
        }

        #endregion
    }
}