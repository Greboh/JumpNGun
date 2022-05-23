using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using JumpNGun.StatePattern.GameStates;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;

namespace JumpNGun
{
    class Button : Component
    {
        private Vector2 _position;

        //hardcoded button positions
        private Vector2 _startButtonPosition = new Vector2(566, 365);
        private Vector2 _settingButtonPosition = new Vector2(517, 437);
        private Vector2 _highscoreButtonPosition = new Vector2(470, 505);
        private Vector2 _quitButtonPosition = new Vector2(577, 575);

        private ButtonType _type;

        private Rectangle _mouseRect;
        private Rectangle _buttonRect;

        private SpriteRenderer _sr;

        // TODO REMOVE THIS
        private bool _canIntersect = true;

        public Button(ButtonType type)
        {
            _type = type;
        }

        public override void Awake()
        {

            // Sets position for the button sprite
            switch (_type)
            {
                case ButtonType.Start:
                    _position = _startButtonPosition;
                    break;
                case ButtonType.Settings:
                    _position = _settingButtonPosition;
                    break;
                case ButtonType.Highscores:
                    _position = _highscoreButtonPosition;
                    break;
                case ButtonType.Quit:
                    _position = _quitButtonPosition;
                    break;
                case ButtonType.Audio:
                    _position = _highscoreButtonPosition;
                    break;
                case ButtonType.Controls:
                    _position = _quitButtonPosition;
                    break;
            }
        }

        public override void Start()
        {
            GameObject.Transform.Position = _position;
            _sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            _sr.SetOrigin(Vector2.Zero); // sets origin to top left
            _buttonRect = new Rectangle((int) _position.X, (int) _position.Y, _sr.Sprite.Width, _sr.Sprite.Height); // construcs rectangle from position and sprite width/height
        }

        public override void Update(GameTime gameTime)
        {
            _mouseRect = new Rectangle((int) GameWorld.Instance.myMouse.X, (int) GameWorld.Instance.myMouse.Y, 10, 10); // mouse rectangle

            // used for checking mouse rect, sprite rect intersection
            if (_mouseRect.Intersects(_buttonRect))
            {
                _sr.SetColor(Color.LightGray);

                
                switch (_type)
                {
                    case ButtonType.Start:
                        Console.WriteLine($"Intersects with {_type}");

                        if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Pressed && _canIntersect)
                        {
                            GameWorld.Instance.ChangeState(new MainGameState());


                            _canIntersect = false;
                        }
                        else if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Released && !_canIntersect)
                        {
                            _canIntersect = true;
                        }
                        break;
                    case ButtonType.Settings:
                        Console.WriteLine($"Intersects with {_type}");

                        if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Pressed && _canIntersect)
                        {
                            GameWorld.Instance.ChangeState(new SettingsMenuState());


                            _canIntersect = false;
                        }
                        else if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Released && !_canIntersect)
                        {
                            _canIntersect = true;
                        }
                        break;
                    case ButtonType.Highscores:

                        break;
                    case ButtonType.Quit:
                        if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Pressed && _canIntersect)
                        {

                            GameWorld.Instance.Exit();

                            _canIntersect = false;
                        }
                        else if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Released && !_canIntersect)
                        {
                            _canIntersect = true;
                        }
                        break;
                    case ButtonType.Audio:
                        Console.WriteLine($"Intersects with {_type}");

                        if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Pressed && _canIntersect)
                        {
                            GameWorld.Instance.ChangeState(new AudioSettingsState());


                            _canIntersect = false;
                        }
                        else if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Released && !_canIntersect)
                        {
                            _canIntersect = true;
                        }
                        break;
                    case ButtonType.Controls:
                        Console.WriteLine($"Intersects with {_type}");

                        if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Pressed && _canIntersect)
                        {
                            GameWorld.Instance.ChangeState(new ControlSettingsState());


                            _canIntersect = false;
                        }
                        else if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Released && !_canIntersect)
                        {
                            _canIntersect = true;
                        }
                        break;
                }
            }
            else
            {
                _sr.SetColor(Color.White);
            }
        }
    }
}