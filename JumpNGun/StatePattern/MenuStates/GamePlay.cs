using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace JumpNGun
{
    public enum PauseState
    {
        Unpaused,
        Paused,
    }

    public class GamePlay : IStateMenu
    {
        private MenuStateHandler _pareMenuStateHandler;

        private Texture2D _pausedOverlay;
        private Texture2D _avatar1; // temporary
        private Texture2D _enabled;
        private Texture2D _disabled;
        private Texture2D _musicStatus;
        private Texture2D _sfxStatus;

        private SpriteFont _scoreFont;

        private bool _isPaused;

        private bool _pauseKeyPressed;

        private PauseState _currentPauseState = PauseState.Unpaused;

        private LevelSystem _levelSystem;

        public void Enter(MenuStateHandler parent)
        {
            _pareMenuStateHandler = parent;

            SoundManager.Instance.StopClip("soundtrack_2");
            SoundManager.Instance.PlayClip("soundtrack_1");


            Director playerDirector = new Director(new PlayerBuilder(CharacterType.Soldier));
            GameWorld.Instance.newGameObjects.Add(playerDirector.Construct());

            LevelManager.Instance.ExecuteLevelGeneration();
            
            EventManager.Instance.Subscribe("OnPlayerDeath", OnGameover);

            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }

        }

        private void OnGameover(Dictionary<string, object> ctx)
        {
            MenuStateHandler.Instance.ChangeState(_pareMenuStateHandler.MainMenu);
        }

        public void Execute(GameTime gameTime)
        {
            _levelSystem ??= GameWorld.Instance.FindObjectOfType<LevelSystem>() as LevelSystem;

            PauseMenuHandling();
            SetAudioStatusIcons();
            
            //call update method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Update(gameTime);
            }

            //call cleanup in every cycle
            GameWorld.Instance.CleanUpGameObjects();
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            
            //draw sprites of every active gameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Draw(spriteBatch);
            }
            
            HandlePauseLogic(spriteBatch);
            
            spriteBatch.End();
        }

        public void LoadContent()
        {
            //call start method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Start();
            }


            // asset content loading
            _pausedOverlay = GameWorld.Instance.Content.Load<Texture2D>("paused_overlay");
            _avatar1 = GameWorld.Instance.Content.Load<Texture2D>("avatar_1");
            _enabled = GameWorld.Instance.Content.Load<Texture2D>("checkmark");
            _disabled = GameWorld.Instance.Content.Load<Texture2D>("crossedout");

            _musicStatus = GameWorld.Instance.Content.Load<Texture2D>("checkmark");
            _sfxStatus = GameWorld.Instance.Content.Load<Texture2D>("checkmark");

            _scoreFont = GameWorld.Instance.Content.Load<SpriteFont>("font");
        }

        public void Exit()
        {
            _pareMenuStateHandler.ComponentCleanUp();
            
            Database.Instance.AddScore(_pareMenuStateHandler.PlayerName, ScoreHandler.Instance.GetScore());
        }

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

        private void HandlePauseLogic(SpriteBatch spriteBatch)
        {
            //Handles draws when pause menu is open / closed
            switch (_currentPauseState)
            {
                case PauseState.Unpaused:
                {
                    //removes buttons instansitated in pause menu
                    foreach (GameObject go in GameWorld.Instance.gameObjects)
                    {
                        if (go.HasComponent<Button>())
                        {
                            GameWorld.Instance.Destroy(go);
                        }
                    }
                    
                    // UnFreeze gameobjects
                    EventManager.Instance.TriggerEvent("OnFreeze", new Dictionary<string, object>()
                        {
                            {"freeze", false}
                        }
                    );

                    _isPaused = false;
                } break;

                case PauseState.Paused:
                {
                    spriteBatch.Draw(_pausedOverlay, new Rectangle(357, 212, _pausedOverlay.Width, _pausedOverlay.Height), null,
                        Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                    spriteBatch.Draw(_avatar1, new Rectangle(401, 325, _avatar1.Width, _avatar1.Height), null, Color.White,
                        0, new Vector2(0, 0), SpriteEffects.None, 1);

                    spriteBatch.DrawString(_scoreFont, "Score : " + ScoreHandler.Instance.GetScore(), new Vector2(401, 515), Color.White);
                    

                    spriteBatch.DrawString(_scoreFont, "Level : " + _levelSystem.GetLevel() , new Vector2(401, 535), Color.White);


                    spriteBatch.Draw(_musicStatus, new Rectangle(1269, 20, _enabled.Width, _enabled.Height), null, Color.White,
                        0, new Vector2(0, 0), SpriteEffects.None, 1);
                    spriteBatch.Draw(_sfxStatus, new Rectangle(1269, 88, _disabled.Width, _disabled.Height), null, Color.White,
                        0, new Vector2(0, 0), SpriteEffects.None, 1);

                    // Freeze gameobjects
                    EventManager.Instance.TriggerEvent("OnFreeze", new Dictionary<string, object>()
                        {
                            {"freeze", true}
                        }
                    );
                    
                    //instansiates buttons used if paused
                    if (!_isPaused)
                    {
                        GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.MusicPause, Vector2.Zero));
                        GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.SfxPause, Vector2.Zero));
                        GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.QuitToMain, Vector2.Zero));

                        _isPaused = true;
                    }
                } break;
            }
        }

        /// <summary>
        /// Sets music/sfx status icons
        /// </summary>
        private void SetAudioStatusIcons()
        {
            _musicStatus = SoundManager.Instance._musicDisabled ? _disabled : _enabled;

            _sfxStatus = SoundManager.Instance._sfxDisabled ? _disabled : _enabled;
        }
    }
}