using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace JumpNGun
{
    // check pause enums
    public enum PauseState
    {
        Unpaused,
        Paused,
        LevelUp,
    }

    /// <summary>
    /// LAVET AF KEAN & NICHLAS
    /// </summary>
    public class GamePlay : IStateMenu
    {
        #region oldFields unchanged fields

        private MenuStateHandler _pareMenuStateHandler;

        private Texture2D _pausedOverlay; // pause overlay texture
        private Texture2D _characterAvatar; // Icon of the character
        private Texture2D _enabled; // checkmark texture for audio
        private Texture2D _disabled; // crossed out texture for audio
        private Texture2D _musicStatus; // holds music status texture from either _enabled or _disabled
        private Texture2D _sfxStatus; // holds sfx status texture from either _enabled or _disabled

        private SpriteFont _font; // font used for displaying scores in pause menu

        private bool _isPaused; // pause bool check

        private bool _pauseKeyPressed; // bool for toggling pause menu

        private PauseState _currentPauseState = PauseState.Unpaused; // sets default PauseState to enum Unpaused

        private LevelSystem _levelSystem;

        #endregion

        #region Fields

        private bool _startOfGame = true;

        private AbilitySystem _abilitySystem;

        private Texture2D _levelUpOverlay;

        private string _abilityName = "Press and hold on an ability!";

        private string _abilityDescription = string.Empty;

        #endregion

        #region Methods

        /// <summary>
        ///Initializes code that runs when GamePlay state is instansiated
        ///LAVET AF KEAN & NICHLAS
        /// </summary>
        /// <param name="parent"></param>
        public void Enter(MenuStateHandler parent)
        {
            #region Old unchanged code

            _pareMenuStateHandler = parent;
            SoundManager.Instance.StopClip("soundtrack_2");
            SoundManager.Instance.PlayClip("soundtrack_1");
            Director playerDirector = new Director(new PlayerBuilder(_pareMenuStateHandler.PlayerType));
            GameWorld.Instance.newGameObjects.Add(playerDirector.Construct());
            LevelManager.Instance.ExecuteLevelGeneration();
            EventHandler.Instance.Subscribe("OnPlayerDeath", OnGameover);

            #endregion

            EventHandler.Instance.Subscribe("OnLevelUp", OnLevelUp);
            EventHandler.Instance.Subscribe("OnAbilitySelected", OnAbilitySelected);
            EventHandler.Instance.Subscribe("OnShowAbilityDescription", OnShowAbilityDescription);
            
            foreach (var go in GameWorld.Instance.GameObjects)
            {
                go.Awake();
            }
        }
        /// <summary>
        /// Updates logic when state is GamePlay, also handles pause menu input
        /// LAVET AF KEAN & NICHLAS
        /// </summary>
        /// <param name="gameTime"></param>
        public void Execute(GameTime gameTime)
        {
            #region Old unchanged code

            _levelSystem ??= GameWorld.Instance.FindObjectOfType<LevelSystem>() as LevelSystem;

            PauseMenuHandling();
            SetAudioStatusIcons();

            //call update method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.GameObjects.Count; i++)
            {
                GameWorld.Instance.GameObjects[i].Update(gameTime);
            }

            //call cleanup in every cycle
            GameWorld.Instance.CleanUpGameObjects();

            #endregion

            // Run in update to make sure it gets the AbilitySystem. Only gets it once!
            if (_startOfGame)
            {
                _abilitySystem ??= GameWorld.Instance.FindObjectOfType<AbilitySystem>() as AbilitySystem;
                _abilitySystem.PickAtStart();
                _startOfGame = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront);

            #region Old unchanged code

            //draw sprites of every active gameObject in list
            for (int i = 0; i < GameWorld.Instance.GameObjects.Count; i++)
            {
                GameWorld.Instance.GameObjects[i].Draw(spriteBatch);
            }


            // Makes sure PlayerType is not set to none
            // Needs to be in a method that updates since we set CharacterType later than Gameplay!
            if (_pareMenuStateHandler.PlayerType != CharacterType.None)
            {
                switch (_pareMenuStateHandler.PlayerType)
                {
                    // Loads the CharacterAvatar
                    case CharacterType.Soldier:
                        _characterAvatar = GameWorld.Instance.Content.Load<Texture2D>("avatar_1");
                        break;
                    case CharacterType.Ranger:
                        _characterAvatar = GameWorld.Instance.Content.Load<Texture2D>("avatar_2");
                        break;
                    case CharacterType.Wizard:
                        break;
                }
            }

            #endregion

            //draws pause menu overlay
            HandlePauseLogic(spriteBatch);

            spriteBatch.End();
        }
        public void LoadContent()
        {
            #region Old unchanged code
            //call start method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.GameObjects.Count; i++)
            {
                GameWorld.Instance.GameObjects[i].Start();
            }

            // asset content loading
            _pausedOverlay = GameWorld.Instance.Content.Load<Texture2D>("paused_overlay");
            _enabled = GameWorld.Instance.Content.Load<Texture2D>("checkmark");
            _disabled = GameWorld.Instance.Content.Load<Texture2D>("crossedout");
            _musicStatus = GameWorld.Instance.Content.Load<Texture2D>("checkmark");
            _sfxStatus = GameWorld.Instance.Content.Load<Texture2D>("checkmark");
            _font = GameWorld.Instance.Content.Load<SpriteFont>("font");

            #endregion
            
            // Loads the levelUpOverlay
            _levelUpOverlay = GameWorld.Instance.Content.Load<Texture2D>("levelUpOverlay");
        }
        /// <summary>
        /// Code that runs when state is changed
        /// LAVET AF KEAN & NICHLAS
        /// </summary>
        public void Exit()
        {
            #region Old unchanged code

            _currentPauseState = PauseState.Unpaused;
            Database.Instance.AddScore(_pareMenuStateHandler.PlayerName, ScoreHandler.Instance.GetScore());
            _pareMenuStateHandler.ComponentCleanUp();
            ScoreHandler.Instance.ResetScore();
            _levelSystem.Resetlevel();

            #endregion

            // Set start of game to True
            _startOfGame = true;
            // Unsubscribe to events
            EventHandler.Instance.Unsubscribe("OnLevelUp", OnLevelUp);
            EventHandler.Instance.Unsubscribe("OnAbilitySelected", OnAbilitySelected);
            EventHandler.Instance.Unsubscribe("OnShowAbilityDescription", OnShowAbilityDescription);
        }

        /// <summary>
        /// Handles what assets and button types to draw when PauseState is set to .Paused
        /// LAVET AF KEAN
        /// </summary>
        /// <param name="spriteBatch"></param>
        private void HandlePauseLogic(SpriteBatch spriteBatch)
        {
            //Handles draws when pause menu is open / closed
            switch (_currentPauseState)
            {
                case PauseState.LevelUp:
                {
                    if (!_abilitySystem.NoAbilitiesLeft)
                    {
                        // Freeze gameobjects
                        EventHandler.Instance.TriggerEvent("OnFreeze", new Dictionary<string, object>()
                            {
                                {"freeze", true}
                            }
                        );

                        DrawLevelUp(spriteBatch);
                    }
                    else _currentPauseState = PauseState.Unpaused;

                }
                    break;
                
                #region Old unchanged code

                case PauseState.Unpaused:
                {
                    //removes buttons instansitated in pause menu
                    foreach (GameObject go in GameWorld.Instance.GameObjects)
                    {
                        if (go.HasComponent<Button>())
                        {
                            GameWorld.Instance.Destroy(go);
                        }
                    }

                    // UnFreeze gameobjects
                    EventHandler.Instance.TriggerEvent("OnFreeze", new Dictionary<string, object>()
                        {
                            {"freeze", false}
                        }
                    );

                    _isPaused = false;
                }
                    break;

                #endregion
                
                case PauseState.Paused:
                {
                    DrawAbilityDescription(spriteBatch);
                        
                    #region Old unchanged code

                    spriteBatch.Draw(_pausedOverlay, new Rectangle(357, 212, _pausedOverlay.Width, _pausedOverlay.Height), null,
                        Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0.9f);

                    spriteBatch.Draw(_characterAvatar, new Rectangle(401, 325, _characterAvatar.Width, _characterAvatar.Height), null, Color.White,
                        0, new Vector2(0, 0), SpriteEffects.None, 0.8f);

                    spriteBatch.DrawString(_font, "Score : " + ScoreHandler.Instance.GetScore(), new Vector2(401, 513), Color.White);


                    spriteBatch.DrawString(_font, "Level : " + _levelSystem.GetLevel(), new Vector2(401, 533), Color.White);


                    spriteBatch.Draw(_musicStatus, new Rectangle(1269, 20, _enabled.Width, _enabled.Height), null, Color.White,
                        0, new Vector2(0, 0), SpriteEffects.None, 1);
                    spriteBatch.Draw(_sfxStatus, new Rectangle(1269, 88, _disabled.Width, _disabled.Height), null, Color.White,
                        0, new Vector2(0, 0), SpriteEffects.None, 1);

                    // Freeze gameobjects
                    EventHandler.Instance.TriggerEvent("OnFreeze", new Dictionary<string, object>()
                        {
                            {"freeze", true}
                        }
                    );
                    #endregion

                    //instansiates buttons used if paused
                    if (!_isPaused)
                    {
                        DrawAbilityIcons();
                        GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.MusicPause, Vector2.Zero));
                        GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.SfxPause, Vector2.Zero));
                        GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.QuitToMain, Vector2.Zero));
                        _isPaused = true;
                    }
                }
                    break;
            }
        }
        
        private void DrawLevelUp(SpriteBatch spriteBatch)
        {
            // Draw LevelUpOverlay
            spriteBatch.Draw(_levelUpOverlay, new Rectangle(357, 85, _levelUpOverlay.Width, _levelUpOverlay.Height), null,
                Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0.9f);

            // Make sure to only instantiate buttons once.
            if (!_isPaused)
            {
                HandleLevelUp();

                _isPaused = true;
            }
        }
        
        private void HandleLevelUp()
        {
            // Gets the count of pickable abilities
            int numberOfPickableAbilities = _abilitySystem.AbilitiesToPickFrom.Count;
            
            for (int i = 0; i < numberOfPickableAbilities; i++)
            {
                GameObject abilityPickButton = ButtonFactory.Instance.Create(ButtonType.AbilityPick);

                (abilityPickButton.GetComponent<Button>() as Button).AbilityPickIndex = i;
                GameWorld.Instance.Instantiate(abilityPickButton);
            }
        }

        private void DrawAbilityIcons()
        {
            // Gets the count of current abilities
            int numberOfCurrentAbilities = _abilitySystem.PlayerAbilities.Count;
            
            // Make sure we dont have 0 abilities
            if (numberOfCurrentAbilities == 0) return;

            for (int i = 0; i < numberOfCurrentAbilities; i++)
            {
                GameObject abilityButton = ButtonFactory.Instance.Create(ButtonType.AbilityIcon);

                (abilityButton.GetComponent<Button>() as Button).AbilityIconIndex = i;
                GameWorld.Instance.Instantiate(abilityButton);
            }
        }

        private void DrawAbilityDescription(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "Ability Name: " + _abilityName, new Vector2(401, 553), Color.White);
            spriteBatch.DrawString(_font, "Ability Description: " + _abilityDescription, new Vector2(401, 573), Color.White);
        }
        
        private void OnLevelUp(Dictionary<string, object> ctx)
        {
            // Set pauseState
            _currentPauseState = PauseState.LevelUp;
            
            // If start of game return
            if (_startOfGame) return;
            
            // Call PickAbility
            _abilitySystem.PickAbility();
        }

        private void OnAbilitySelected(Dictionary<string, object> ctx)
        {
            // Set pauseState
            _currentPauseState = PauseState.Unpaused;
        }

        private void OnShowAbilityDescription(Dictionary<string, object> ctx)
        {
            // Get bool from event
            bool shouldShow = (bool) ctx["shouldShow"];

            // Check if we should show description
            if (shouldShow)
            {
                Ability ability = (Ability) ctx["ability"];

                // Set ability name and description to the ability's name and description
                _abilityName = ability.AbilityName;
                _abilityDescription = ability.AbilityDescription;
            }
            else
            {
                // Reset strings
                _abilityName = "Press and hold on an ability!";
                _abilityDescription = string.Empty;
            }
        }

        #endregion
        
        #region old

        /// <summary>
        /// Handles keyboard inputs for opening and closing pause menu
        /// LAVET AF KEAN
        /// </summary>
        private void PauseMenuHandling()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && _currentPauseState == PauseState.Unpaused && _pauseKeyPressed == false)
            {
                _currentPauseState = PauseState.Paused;
                _pauseKeyPressed = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Escape))
            {
                _pauseKeyPressed = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && _currentPauseState == PauseState.Paused && _pauseKeyPressed == false)
            {
                //_keypressCooldown = GameWorld.DeltaTime;
                _currentPauseState = PauseState.Unpaused;
                _pauseKeyPressed = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Escape))
            {
                _pauseKeyPressed = false;
            }
        }

        /// <summary>
        /// Sets music/sfx status icons
        /// LAVET AF KEAN
        /// </summary>
        private void SetAudioStatusIcons()
        {
            //sets _musicStatus texture to _disabled if false otherwise sets it to _enabled
            _musicStatus = SoundManager.Instance.MusicDisabled ? _disabled : _enabled;

            //sets _sfxStatus texture to _disabled if false otherwise sets it to _enabled
            _sfxStatus = SoundManager.Instance.SfxDisabled ? _disabled : _enabled;
        }
        
        /// <summary>
        /// Event that gets trigger when the player dies   
        /// LAVET AF NICHLAS
        /// </summary>
        /// <param name="ctx">The context that gets sent from the trigger in Player.cs</param>
        private void OnGameover(Dictionary<string, object> ctx)
        {
            MenuStateHandler.Instance.ChangeState(_pareMenuStateHandler.MainMenu);
        }

        #endregion
    }
}