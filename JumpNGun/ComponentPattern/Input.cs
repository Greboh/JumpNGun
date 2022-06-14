using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace JumpNGun
{
    
    /// <summary>
    /// Mouse Buttons used in keybinding
    /// </summary>
    public enum MouseButton
    {
        None,
        Mouse0, // Left Click
        Mouse1, // Right Click
    }

    public struct KeyCode
    {
        // The keyboardBinding
        public Keys KeyboardBinding { get; set; }
        
        // The mouseBinding 
        public MouseButton MouseBinding { get; set; }

        // Name of the action that the binding is on
        public string ActionName { get; set; }
        
        // Whether to use keyboard or mouse on this keybind
        public bool IsKeyboardBinding { get; private set; }
        
        #region Constructors

        /// <summary>
        /// Used if the binding is a keyboard one
        /// </summary>
        /// <param name="key">Which keyboard key to assign</param>
        /// <param name="actionName">What name to assign to the binding</param>
        public KeyCode(Keys key, string actionName)
        {
            KeyboardBinding = key;
            MouseBinding = MouseButton.None;
            ActionName = actionName;

            IsKeyboardBinding = true;
        }

        /// <summary>
        /// Used if the binding is a keyboard one
        /// </summary>
        /// <param name="mouse">Which mouse key to assign</param>
        /// <param name="actionName">What name to assign to the binding</param>
        public KeyCode(MouseButton mouse, string actionName)
        {
            KeyboardBinding = Keys.None;
            MouseBinding = mouse;
            ActionName = actionName;

            IsKeyboardBinding = false;
        }

        #endregion
    }
    
    /// <summary>
    /// Klassen er lavet af Nichlas Hoberg
    /// </summary>
    public class Input : Component
    {
        private KeyboardState _currentKeyState; // Reference our KeyboardState
        private MouseState _currentMouseState; // Reference our MouseState
        private Dictionary<KeyCode, ICommand> _keybindings = new Dictionary<KeyCode, ICommand>(); // Initialize Dictionary
        
        #region KeyCodes

        // Used when rebinding the key and used in Player.cs to handle logic according to the key
        public KeyCode MoveLeft { get; private set; } = new KeyCode(Keys.A, "move_left");
        public KeyCode MoveRight { get; private set; } = new KeyCode(Keys.D, "move_right");
        public KeyCode Jump { get; private set; } = new KeyCode(Keys.W, "jump");
        public KeyCode Dash { get; private set; } = new KeyCode(Keys.LeftAlt, "dash");
        public KeyCode Shoot { get; private set; } = new KeyCode(MouseButton.Mouse0, "shoot");

        #endregion

        public override void Start()
        {

            // Binding keys
            BindKey(MoveLeft,new MoveCommand(new Vector2(-1, 0)));
            BindKey(MoveRight, new MoveCommand(new Vector2(1, 0)));
            BindKey(Jump, new JumpCommand());
            BindKey(Shoot, new ShootCommand());
            BindKey(Dash, new DashCommand());
            
            // Example on Rebinding a key
            RebindKey(new KeyCode(Keys.Space, "shoot"), new ShootCommand());
        }


        public override void Update(GameTime gameTime)
        {
            // Update Keyboard & Mouse states
            _currentKeyState = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();
        }

        public void Execute(Player player)
        {
            foreach (KeyCode keyCode in _keybindings.Keys)
            {
                // Check if the keyCode is a mouseBinding
                if (keyCode.IsKeyboardBinding)
                {
                    // Reference to our current pressed key
                    Keys currentKey = keyCode.KeyboardBinding;

                    // If that key is pressed
                    if (_currentKeyState.IsKeyDown(currentKey) && player.CanMove)
                    {
                        _keybindings[keyCode].Execute(player);
                        
                        // Trigger event sending the key and that it's pressed down
                        EventHandler.Instance.TriggerEvent("OnKeyPress", new Dictionary<string, object>()
                        {
                            {"key", currentKey},
                            {"isKeyDown", true}
                        });
                        
                    }
                    // Check if currentKey is no longer pressed down
                    else if (_currentKeyState.IsKeyUp(currentKey) && currentKey != Keys.None)
                    {
                        // Trigger event sending the pressed key and that it's no longer pressed down
                        EventHandler.Instance.TriggerEvent("OnKeyPress", new Dictionary<string, object>()
                        {
                            {"key", currentKey},
                            {"isKeyDown", false}
                        });
                    }
                }
                else
                {
                    if (keyCode.MouseBinding == GetMouseButtons())
                    {
                        _keybindings[keyCode].Execute(player);
                    }
                }
            }
        }


        #region Binding & Rebinding

        /// <summary>
        /// Binds a key
        /// </summary>
        /// <param name="keyCode">The keyCode to assign the command to</param>
        /// <param name="command">The command to assign the keyCode to</param>
        private void BindKey(KeyCode keyCode, ICommand command)
        {
            // If the dictionary doesnt contain the keycode
            if (!_keybindings.ContainsKey(keyCode))
            {
                _keybindings.Add(keyCode, command);
            }
            else Console.Error.WriteLine($"An error occured when binding: An item with the same key has already been added => {keyCode.ActionName.ToLower()}");
        }

        /// <summary>
        /// Rebind an existing keyCode
        /// </summary>
        /// <param name="newKey">The new keyCode</param>
        /// <param name="command">The Command</param>
        private void RebindKey( KeyCode newKey, ICommand command)
        {
            foreach (KeyValuePair<KeyCode, ICommand> keybinding in _keybindings)
            {
                // Check if the Tkey's ActionName is equal to our newKeys ActionName
                if(keybinding.Key.ActionName == newKey.ActionName)
                {
                    // Remove the dictionary TKey
                    _keybindings.Remove(keybinding.Key);

                    // Change our KeyCode variables so they are updated to the new key
                    switch (keybinding.Key.ActionName)
                    {
                        case "move_left":
                            MoveLeft = newKey;
                            break;
                        case "move_right":
                            MoveRight = newKey;
                            break;
                        case "jump":
                            Jump = newKey;
                            break;
                        case "dash":
                            Dash = newKey;
                            break;
                        case "shoot":
                            Shoot = newKey;
                            break;
                    }
                }
            }

            // Add the new keybinding
            _keybindings.Add(newKey, command);
        }

        #endregion
        
        /// <summary>
        /// Gets if a mouseButton is pressed down
        /// </summary>
        /// <returns>A MouseButton</returns>
        private MouseButton GetMouseButtons()
        {
            // Create new mouseButton
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
        
    }
}