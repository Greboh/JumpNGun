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
            switch (_type)
            {
                case ButtonType.Start:
                    _position = new Vector2(GameWorld.Instance.ScreenSize.X / 2.5f, 365);
                    break;
                case ButtonType.Settings:
                    _position = new Vector2(GameWorld.Instance.ScreenSize.X / 2.5f, 437);
                    break;
                case ButtonType.Highscores:
                    _position = new Vector2(GameWorld.Instance.ScreenSize.X / 2.5f, 525);
                    break;
                case ButtonType.Quit:
                    _position = new Vector2(GameWorld.Instance.ScreenSize.X / 2.5f, 590);
                    break;
            }
        }

        public override void Start()
        {
            GameObject.Transform.Position = _position;
            _sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            _sr.SetOrigin(Vector2.Zero);
            _buttonRect = new Rectangle((int) _position.X, (int) _position.Y, _sr.Sprite.Width, _sr.Sprite.Height);

            switch (_type)
            {
                case ButtonType.Start:
                    break;
                case ButtonType.Settings:
                    break;
                case ButtonType.Highscores:
                    break;
                case ButtonType.Quit:
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            _mouseRect = new Rectangle((int) GameWorld.Instance.myMouse.X, (int) GameWorld.Instance.myMouse.Y, 50, 50);

            if (_mouseRect.Intersects(_buttonRect))
            {
                _sr.SetColor(Color.Aqua);

                switch (_type)
                {
                    case ButtonType.Start:
                        Console.WriteLine($"Intersects with {_type}");
                        
                        if(GameWorld.Instance.myMouse.LeftButton == ButtonState.Pressed && _canIntersect)
                        {
                            GameWorld.Instance.ChangeState(new MainGameState());
                            
                            
                            _canIntersect = false;
                        }
                        else if(GameWorld.Instance.myMouse.LeftButton == ButtonState.Released && !_canIntersect)
                        {
                            _canIntersect = true;
                        }
                            
                        
                        
                        break;
                    case ButtonType.Settings:

                        break;
                    case ButtonType.Highscores:

                        break;
                    case ButtonType.Quit:

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