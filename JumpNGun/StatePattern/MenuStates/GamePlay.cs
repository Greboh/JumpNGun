using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public enum PauseState
    {
        unpaused,
        paused,
    }

    public class GamePlay : IStateMenu
    {
        private MenuStateHandler _pareMenuStateHandler;

        private Texture2D _pausedOverlay;
        private Texture2D _avatar_1; // temporary
        private Texture2D _enabled;
        private Texture2D _disabled;
        private Texture2D _musicStatus;
        private Texture2D _sfxStatus;

        private SpriteFont _scoreFont;

        private bool isPaused;

        private bool pauseKeyPressed;

        private PauseState currentPauseState = PauseState.unpaused;

        public void Enter(MenuStateHandler parent)
        {
            _pareMenuStateHandler = parent;

            SoundManager.Instance.StopClip("soundtrack_2");
            SoundManager.Instance.PlayClip("soundtrack_1");


            Director playerDirector = new Director(new PlayerBuilder(CharacterType.Soldier));
            GameWorld.Instance.newGameObjects.Add(playerDirector.Construct());

            LevelManager.Instance.ExecuteLevelGeneration();


            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }
        }

        public void Execute(GameTime gameTime)
        {
            PauseMenuHandling();
            SetAudioStatusIcons();

            LevelManager.Instance.ChangeLevelDebug();
            LevelManager.Instance.CheckForClearedLevelDebug();


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
            _avatar_1 = GameWorld.Instance.Content.Load<Texture2D>("avatar_1");
            _enabled = GameWorld.Instance.Content.Load<Texture2D>("checkmark");
            _disabled = GameWorld.Instance.Content.Load<Texture2D>("crossedout");

            _musicStatus = GameWorld.Instance.Content.Load<Texture2D>("checkmark");
            _sfxStatus = GameWorld.Instance.Content.Load<Texture2D>("checkmark");

            _scoreFont = GameWorld.Instance.Content.Load<SpriteFont>("font");
        }

        public void Exit()
        {
            _pareMenuStateHandler.ComponentCleanUp();
        }

        private void PauseMenuHandling()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && currentPauseState == PauseState.unpaused && pauseKeyPressed == false)
            {
                currentPauseState = PauseState.paused;
                pauseKeyPressed = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Escape))
            {
                pauseKeyPressed = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && currentPauseState == PauseState.paused && pauseKeyPressed == false)
            {
                //_keypressCooldown = GameWorld.DeltaTime;
                currentPauseState = PauseState.unpaused;
                pauseKeyPressed = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Escape))
            {
                pauseKeyPressed = false;
            }
        }

        private void HandlePauseLogic(SpriteBatch spriteBatch)
        {
            //Handles draws when pause menu is open / closed
            switch (currentPauseState)
            {
                case PauseState.unpaused:
                {
                    //removes buttons instansitated in pause menu
                    foreach (GameObject go in GameWorld.Instance.gameObjects)
                    {
                        if (go.HasComponent<Button>())
                        {
                            GameWorld.Instance.Destroy(go);
                        }
                    }

                    isPaused = false;
                } break;

                case PauseState.paused:
                {
                    spriteBatch.Draw(_pausedOverlay, new Rectangle(357, 212, _pausedOverlay.Width, _pausedOverlay.Height), null,
                        Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

                    spriteBatch.Draw(_avatar_1, new Rectangle(401, 325, _avatar_1.Width, _avatar_1.Height), null, Color.White,
                        0, new Vector2(0, 0), SpriteEffects.None, 1);

                    spriteBatch.DrawString(_scoreFont, "Score : " + ScoreHandler.Instance.GetScore(), new Vector2(401, 515), Color.White);
                    ;

                    spriteBatch.DrawString(_scoreFont, "Level : " + 8, new Vector2(401, 535), Color.White);


                    spriteBatch.Draw(_musicStatus, new Rectangle(1269, 20, _enabled.Width, _enabled.Height), null, Color.White,
                        0, new Vector2(0, 0), SpriteEffects.None, 1);
                    spriteBatch.Draw(_sfxStatus, new Rectangle(1269, 88, _disabled.Width, _disabled.Height), null, Color.White,
                        0, new Vector2(0, 0), SpriteEffects.None, 1);

                    //instansiates buttons used if paused
                    if (!isPaused)
                    {
                        GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.MusicPause));
                        GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.SfxPause));
                        GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.QuitToMain));

                        isPaused = true;
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