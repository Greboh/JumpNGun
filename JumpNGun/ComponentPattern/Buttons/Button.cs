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
        /*
            [Discription]
            This class handles buttons position as well as mouse and button sprite interactions.
            The class is otherwise used for changing GameStates corresponding to clicked the button context
        */


        #region Fields
        private Vector2 _position;

        //static button positions based on photoshop design mockup
        private Vector2 _startButtonPosition = new Vector2(566, 365);
        private Vector2 _settingButtonPosition = new Vector2(517, 437);
        private Vector2 _highscoreButtonPosition = new Vector2(470, 505);
        private Vector2 _quitButtonPosition = new Vector2(577, 575);
        private Vector2 _audioButtonPosition = new Vector2(563, 405);
        private Vector2 _controlsButtonPosition = new Vector2(493, 486);
        private Vector2 _musicButtonPosition = new Vector2(553, 375);
        private Vector2 _sfxButtonPosition = new Vector2(614, 443);
        private Vector2 _backButtonPosition = new Vector2(583,639);

        private bool fireOnce = true; // bool used for insuring sound doesn't fire more than once when hovering

        private float _mouseCooldown; // mouse click cooldown used for avoiding unwanted menu button navigation

        private ButtonType _type;

        private Rectangle _mouseRect;
        private Rectangle _buttonRect;

        private SpriteRenderer _sr;

        // TODO REMOVE THIS
        private bool _canIntersect = true;
        #endregion

        #region Constructor
        public Button(ButtonType type)
        {
            _type = type;
        }
        #endregion

        #region Methods

        public override void Awake()
        {
            
            switch (_type) // Sets position for the button sprite
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
                    _position = _audioButtonPosition;
                    break;
                case ButtonType.Controls:
                    _position = _controlsButtonPosition;
                    break;
                case ButtonType.Music:
                    _position = _musicButtonPosition;
                    break;
                case ButtonType.Sfx:
                    _position = _sfxButtonPosition;
                    break;
                case ButtonType.Back:
                    _position = _backButtonPosition;
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
            _mouseCooldown += (float)gameTime.ElapsedGameTime.TotalSeconds; // negates double input on buttons
            
            // used for checking mouse rect & sprite rect intersection
            if (_mouseRect.Intersects(_buttonRect))
            {
                _sr.SetColor(Color.LightGray); // sets color on hover
                if (fireOnce)
                {
                    SoundManager.Instance.PlayRandomClick();
                    fireOnce = false;
                }
                
                

                //TODO: Refactor this mess
                switch (_type)
                {
                    case ButtonType.Start:
                        //Console.WriteLine($"Intersects with {_type}");

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
                        //Console.WriteLine($"Intersects with {_type}");

                        if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Pressed && _canIntersect)
                        {
                            GameWorld.Instance.ChangeState(new SettingsMenuState());

                            _mouseCooldown += (float)gameTime.ElapsedGameTime.TotalSeconds;
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
                        //Console.WriteLine($"Intersects with {_type}");

                        if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Pressed && _canIntersect && _mouseCooldown > 0.5f)
                        {
                            GameWorld.Instance.ChangeState(new AudioSettingsState());

                            _mouseCooldown += 0;
                            _canIntersect = false;
                        }
                        else if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Released && !_canIntersect )
                        {
                            _canIntersect = true;
                        }
                        break;
                    case ButtonType.Controls:
                        //Console.WriteLine($"Intersects with {_type}");

                        if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Pressed && _canIntersect && _mouseCooldown > 0.5f)
                        {
                            GameWorld.Instance.ChangeState(new ControlSettingsState());

                            _mouseCooldown += 0;
                            

                            _canIntersect = false;
                        }
                        else if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Released && !_canIntersect)
                        {
                            _canIntersect = true;
                        }
                        break;
                    case ButtonType.Music:
                        Console.WriteLine($"Intersects with {_type}");

                        if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Pressed && _canIntersect && _mouseCooldown > 0.5f)
                        {
                            _mouseCooldown += 0;
                            _canIntersect = false;
                            if (SoundManager.Instance._musicDisabled == true)
                            {
                                SoundManager.Instance._musicDisabled = false;
                                SoundManager.Instance.toggleSoundtrackOn();

                            }
                            else
                            {
                                SoundManager.Instance._musicDisabled = true;
                                SoundManager.Instance.toggleSoundtrackOff();


                            }
                        }
                        else if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Released && !_canIntersect)
                        {
                            SoundManager.Instance.PlayClip("menu_click");

                            _canIntersect = true;
                        }
                        break;
                    case ButtonType.Sfx:
                        Console.WriteLine($"Intersects with {_type}");

                        if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Pressed && _canIntersect && _mouseCooldown > 0.5f)
                        {
                            


                            _mouseCooldown += 0;
                            _canIntersect = false;
                            if (SoundManager.Instance._sfxDisabled == true)
                            {
                                SoundManager.Instance._sfxDisabled = false;
                                SoundManager.Instance.toggleSFXOn();
                            }
                            else
                            {
                                SoundManager.Instance._sfxDisabled = true;
                                SoundManager.Instance.toggleSFXOff();

                            }

                        }
                        else if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Released && !_canIntersect)
                        {
                            SoundManager.Instance.PlayClip("menu_click");
                            
                            _canIntersect = true;
                        }
                        break;
                    case ButtonType.Back:
                        //Console.WriteLine($"Intersects with {_type}");

                        if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Pressed && _canIntersect && _mouseCooldown > 0.5f)
                        {



                            //if (GameWorld.Instance._currentState == Convert.ChangeType(GameWorld.Instance._currentState, typeof(AudioSettingsState)))
                            //{
                            //    GameWorld.Instance.ChangeState(new SettingsMenuState());

                            //}
                            if (GameWorld.Instance._currentState is SettingsMenuState) // return to Main menu from settings
                            {
                                GameWorld.Instance.ChangeState(new MainMenuState());

                            }
                            else if (GameWorld.Instance._currentState is AudioSettingsState) // return to main settings menu from audio settings
                            {
                                GameWorld.Instance.ChangeState(new SettingsMenuState());

                            }
                            else if (GameWorld.Instance._currentState is ControlSettingsState) // return to main settings menu from control settings
                            {
                                GameWorld.Instance.ChangeState(new SettingsMenuState());

                            }


                            //if (GameWorld.Instance._currentState == Convert.ChangeType(GameWorld.Instance._currentState, typeof(SettingsMenuState)))
                            //{
                            //    GameWorld.Instance.ChangeState(new MainMenuState());
                            //}
                            //else if (GameWorld.Instance._currentState == Convert.ChangeType(GameWorld.Instance._currentState, typeof(AudioSettingsState)))
                            //{
                            //    GameWorld.Instance.ChangeState(new SettingsMenuState());

                            //}




                            _mouseCooldown += 0;


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
                _sr.SetColor(Color.White); // resets hover color
                fireOnce = true;

            }
        }
        #endregion
    }
}