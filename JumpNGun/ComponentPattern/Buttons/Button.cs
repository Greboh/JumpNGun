using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using JumpNGun.StatePattern.GameStates;
using Microsoft.Xna.Framework.Input;

namespace JumpNGun
{
    public class Button : Component
    {
        /*
            [Description]
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
        private Vector2 _backButtonPosition = new Vector2(583, 639);
        private Vector2 _quitToMainButtonPosition = new Vector2(487, 700);
        private Vector2 _ResumeButtonPosition = new Vector2(556, 631);
        private Vector2 _sfxPauseButtonPosition = new Vector2(1168, 88);
        private Vector2 _musicPauseButtonPosition = new Vector2(1109, 20);
        private Vector2 _submitButtonPosition = new Vector2(571, 491);
        private Vector2 _inputFieldPosition = new Vector2(476, 427);
        private Vector2 _character1Position = new Vector2(448, 365);
        private Vector2 _character2Position = new Vector2(686, 365);


        private bool fireOnce = true; // bool used for insuring sound doesn't fire more than once when hovering

        private float _mouseCooldown; // mouse click cooldown used for avoiding unwanted menu button navigation

        private ButtonType _type;

        private Rectangle _mouseRect;
        private Rectangle _buttonRect;

        private SpriteRenderer _sr;

        private bool _inputClicked;
        private bool _inputKeyInteract;
        private string _inputString = String.Empty;
        
        private Keys _currentInputKey = Keys.None;


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
                case ButtonType.QuitToMain:
                    _position = _quitToMainButtonPosition;
                    break;
                case ButtonType.Resume:
                    _position = _ResumeButtonPosition;
                    break;
                case ButtonType.SfxPause:
                    _position = _sfxPauseButtonPosition;
                    break;
                case ButtonType.MusicPause:
                    _position = _musicPauseButtonPosition;
                    break;
                case ButtonType.Submit:
                    _position = _submitButtonPosition;
                    break;
                case ButtonType.InputField:
                    _position = _inputFieldPosition;
                    break;
                case ButtonType.Character1:
                    _position = _character1Position;
                    break;
                case ButtonType.Character2:
                    _position = _character2Position;
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
            _mouseRect = new Rectangle((int) GameWorld.Instance.MyMouse.X, (int) GameWorld.Instance.MyMouse.Y, 10, 10); // mouse rectangle
            _mouseCooldown += (float) gameTime.ElapsedGameTime.TotalSeconds; // negates double input on buttons
            
            

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
                        StartGame();
                        break;
                    case ButtonType.Settings:
                        Settings(gameTime); // gametime is used for preventing unwanted mouse press inputs after the initial mouse press input with a deltatime cooldown
                        break;
                    case ButtonType.Highscores:
                        HighscoreButton();
                        break;
                    case ButtonType.Quit:
                        Quit();
                        break;

                    case ButtonType.Audio:
                        AudioButton();
                        break;

                    case ButtonType.Controls:
                        ControlsButton();
                        break;

                    case ButtonType.Music:
                        MusicToggleButton();
                        break;

                    case ButtonType.Sfx:
                        SoundEffectsToggleButton();
                        break;

                    case ButtonType.Back:
                        BackButton();
                        break;

                    case ButtonType.QuitToMain:
                        QuitToMain();
                        break;
                    case ButtonType.Resume:
                        Resume();
                        break;
                    case ButtonType.SfxPause:
                        SoundEffectsPauseToggleButton();
                        break;
                    case ButtonType.MusicPause:
                        MusicPauseToggleButton();
                        break;
                    case ButtonType.Submit:
                        Submit();
                        break;
                    case ButtonType.InputField:
                        InputFieldLogic();
                        InputField();
                        break;
                    case ButtonType.Character1:
                        CharacterSoldier();
                        break;
                    case ButtonType.Character2:
                        CharacterRanger();
                        break;
                }
            }
            else
            {
                _sr.SetColor(Color.White); // resets hover color
                fireOnce = true;
            }
        }

        private void HighscoreButton()
        {
            if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Pressed && _canIntersect && _mouseCooldown > 0.5f)
            {
                MenuStateHandler.Instance.ChangeState(MenuStateHandler.Instance.Highscore);

                _mouseCooldown += 0;
                _canIntersect = false;
            }
            else if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Released && !_canIntersect)
            {
                _canIntersect = true;
            }
        }


        /// <summary>
        /// Start Game logic button
        /// </summary>
        private void StartGame()
        {
            //Console.WriteLine($"Intersects with {_type}");

            if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Pressed && _canIntersect)
            {
                MenuStateHandler.Instance.ChangeState(MenuStateHandler.Instance.CharacterSelection);

                _canIntersect = false;
            }
            else if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Released && !_canIntersect)
            {
                _canIntersect = true;
            }
        }

        /// <summary>
        /// Main settings menu button 
        /// </summary>
        /// <param name="gameTime"></param>
        private void Settings(GameTime gameTime)
        {
            //Console.WriteLine($"Intersects with {_type}");

            if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Pressed && _canIntersect)
            {
                MenuStateHandler.Instance.ChangeState(MenuStateHandler.Instance.SettingsMenu);

                _mouseCooldown += (float) gameTime.ElapsedGameTime.TotalSeconds;
                _canIntersect = false;
            }
            else if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Released && !_canIntersect)
            {
                _canIntersect = true;
            }
        }

        /// <summary>
        /// Quit button logic
        /// </summary>
        private void Quit()
        {
            if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Pressed && _canIntersect)
            {
                GameWorld.Instance.Exit();

                _canIntersect = false;
            }
            else if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Released && !_canIntersect)
            {
                _canIntersect = true;
            }
        }

        /// <summary>
        /// Main audio menu button logic
        /// </summary>
        private void AudioButton()
        {
            //Console.WriteLine($"Intersects with {_type}");

            if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Pressed && _canIntersect && _mouseCooldown > 0.5f)
            {
                MenuStateHandler.Instance.ChangeState(MenuStateHandler.Instance.Audio);

                _mouseCooldown += 0;
                _canIntersect = false;
            }
            else if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Released && !_canIntersect)
            {
                _canIntersect = true;
            }
        }

        /// <summary>
        /// Controls menu button logic
        /// </summary>
        private void ControlsButton()
        {
            //Console.WriteLine($"Intersects with {_type}");

            if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Pressed && _canIntersect && _mouseCooldown > 0.5f)
            {
                MenuStateHandler.Instance.ChangeState(MenuStateHandler.Instance.Controls);

                _mouseCooldown += 0;


                _canIntersect = false;
            }
            else if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Released && !_canIntersect)
            {
                _canIntersect = true;
            }
        }

        /// <summary>
        /// Music menu settings button logic
        /// </summary>
        private void MusicToggleButton()
        {
            // Console.WriteLine($"Intersects with {_type}");

            if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Pressed && _canIntersect && _mouseCooldown > 0.5f)
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
            else if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Released && !_canIntersect)
            {
                SoundManager.Instance.PlayClip("menu_click");

                _canIntersect = true;
            }
        }

        /// <summary>
        /// SFX button logic
        /// </summary>
        private void SoundEffectsToggleButton()
        {
            // Console.WriteLine($"Intersects with {_type}");

            if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Pressed && _canIntersect && _mouseCooldown > 0.5f)
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
            else if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Released && !_canIntersect)
            {
                SoundManager.Instance.PlayClip("menu_click");

                _canIntersect = true;
            }
        }

        /// <summary>
        /// Back button logic
        /// </summary>
        private void BackButton()
        {
            //Console.WriteLine($"Intersects with {_type}");

            if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Pressed && _canIntersect && _mouseCooldown > 0.5f)
            {
                // return to Main menu from settings
                if (MenuStateHandler.Instance.CurrentMenuState is SettingsMenu) 
                {
                    MenuStateHandler.Instance.ChangeState(MenuStateHandler.Instance.MainMenu);
                }
                
                // return to main settings menu from audio settings
                else if (MenuStateHandler.Instance.CurrentMenuState is Audio) 
                {
                    MenuStateHandler.Instance.ChangeState(new SettingsMenu());
                }
                
                // return to main settings menu from control settings
                else if (MenuStateHandler.Instance.CurrentMenuState is Controls) 
                {
                    MenuStateHandler.Instance.ChangeState(new SettingsMenu());
                }

                // return to main settings menu from highscore menu
                else if (MenuStateHandler.Instance.CurrentMenuState is Highscore)
                {
                    MenuStateHandler.Instance.ChangeState(new MainMenu());
                }



                _mouseCooldown += 0;


                _canIntersect = false;
            }
            else if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Released && !_canIntersect)
            {
                _canIntersect = true;
            }
        }

        /// <summary>
        /// Quit to main button logic
        /// </summary>
        private void QuitToMain()
        {
            // Console.WriteLine($"Intersects with {_type}");

            if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Pressed && _canIntersect && _mouseCooldown > 0.5f)
            {
                MenuStateHandler.Instance.ChangeState(MenuStateHandler.Instance.MainMenu);

                _mouseCooldown += 0;


                _canIntersect = false;
            }
            else if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Released && !_canIntersect)
            {
                _canIntersect = true;
            }
        }

        private void Resume()
        {
            // Console.WriteLine($"Intersects with {_type}");

            if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Pressed && _canIntersect && _mouseCooldown > 0.5f)
            {
                GameWorld.Instance.IsPaused = false;


                _canIntersect = false;
            }
            else if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Released && !_canIntersect)
            {
                _canIntersect = true;
            }
        }

        /// <summary>
        /// SFX pause menu button logic
        /// </summary>
        private void SoundEffectsPauseToggleButton()
        {
            // Console.WriteLine($"Intersects with {_type}");

            if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Pressed && _canIntersect && _mouseCooldown > 0.5f)
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
            else if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Released && !_canIntersect)
            {
                SoundManager.Instance.PlayClip("menu_click");

                _canIntersect = true;
            }
        }

        /// <summary>
        /// Music pause menu settings button logic
        /// </summary>
        private void MusicPauseToggleButton()
        {
            // Console.WriteLine($"Intersects with {_type}");

            if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Pressed && _canIntersect && _mouseCooldown > 0.5f)
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
            else if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Released && !_canIntersect)
            {
                SoundManager.Instance.PlayClip("menu_click");

                _canIntersect = true;
            }
        }

        private void Submit()
        {
            // Console.WriteLine($"Intersects with {_type}");
            
            if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Pressed && _canIntersect)
            {
                if (MenuStateHandler.Instance.PlayerName != string.Empty)
                {
                    Console.WriteLine($"Player name is: {MenuStateHandler.Instance.PlayerName}");
                    MenuStateHandler.Instance.ChangeState( MenuStateHandler.Instance.MainMenu);
                }

                _canIntersect = false;
            }
            else if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Released && !_canIntersect)
            {
                _canIntersect = true;
            }
        }

        private void InputFieldLogic()
        {
            if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Pressed && _canIntersect)
            {
                //TODO: Add input field logic

                // Console.WriteLine($"Intersects with {_type}");
                _inputClicked = true;
                _canIntersect = false;
            }
            else if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Released && !_canIntersect)
            {
                _canIntersect = true;
            }
        }
        
        List<char> _name = new List<char>();

        private void InputField()
        {
            if (_inputClicked)
            {
                KeyboardState keyboardState = Keyboard.GetState();
                Keys[] keyArray = keyboardState.GetPressedKeys();
                
                foreach (Keys key in keyArray)
                {
                    if (keyboardState.IsKeyDown(key) && !_inputKeyInteract && key != Keys.Space)
                    {
                        _currentInputKey = key;
                    
                        Console.WriteLine($"{_currentInputKey} added");
                        _inputKeyInteract = true;

                        EventManager.Instance.TriggerEvent("OnInput", new Dictionary<string, object>()
                            {
                                {"inputKey", _currentInputKey.ToString()}
                            }
                        );
                        
                        MenuStateHandler.Instance.PlayerName += _currentInputKey.ToString();
                    }
                }

                if (!keyboardState.IsKeyUp(_currentInputKey) || !_inputKeyInteract) return;
                
                _inputKeyInteract = false;
            }


        }

        private void CharacterSoldier()
        {
            // Console.WriteLine($"Intersects with {_type}");

            if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Pressed && _canIntersect && _mouseCooldown > 0.5f)
            {
                MenuStateHandler.Instance.PlayerType = CharacterType.Soldier;
                MenuStateHandler.Instance.ChangeState(MenuStateHandler.Instance.Gameplay);

                _mouseCooldown += 0;
                _canIntersect = false;
            }
            else if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Released && !_canIntersect)
            {
                _canIntersect = true;
            }
        }

        private void CharacterRanger()
        {
            // Console.WriteLine($"Intersects with {_type}");

            if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Pressed && _canIntersect && _mouseCooldown > 0.5f)
            {
                MenuStateHandler.Instance.PlayerType = CharacterType.Ranger;
                MenuStateHandler.Instance.ChangeState(MenuStateHandler.Instance.Gameplay);

                _mouseCooldown += 0;
                _canIntersect = false;
            }
            else if (GameWorld.Instance.MyMouse.LeftButton == ButtonState.Released && !_canIntersect)
            {
                _canIntersect = true;
            }
        }

        #endregion
    }
}