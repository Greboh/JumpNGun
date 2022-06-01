using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun.StatePattern.GameStates
{

    public enum PauseState
    {
        unpaused,
        paused,
    }

    public class MainGameState : State
    {
        private Texture2D _pausedOverlay;
        private Texture2D _avatar_1; // temporary
        private Texture2D _enabled;
        private Texture2D _disabled;
        private Texture2D _musicStatus;
        private Texture2D _sfxStatus;

        private SpriteFont _scoreFont;

        static int screenSizeX = (int)GameWorld.Instance.ScreenSize.X;
        static int screenSizeY = (int)GameWorld.Instance.ScreenSize.Y;

        private bool isInitialized;
        private bool isPaused;

        private bool pauseKeyPressed;

        private PauseState currentPauseState = PauseState.unpaused;

       


        public override void LoadContent()
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
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Begin();

            //draw sprites of every active gameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Draw(spriteBatch);
            }

            //Handles draws when pause menu is open / closed
            switch (currentPauseState)
            {
                case PauseState.unpaused:
                    //removes buttons instansitated in pause menu
                    foreach (GameObject go in GameWorld.Instance.gameObjects)
                    {
                        if (go.HasComponent<Button>())
                        {
                            GameWorld.Instance.Destroy(go);
                        }
                    }

                    isPaused = false;
                    break;
                case PauseState.paused:
                  
                    spriteBatch.Draw(_pausedOverlay, new Rectangle(357, 212, _pausedOverlay.Width, _pausedOverlay.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);
                    spriteBatch.Draw(_avatar_1, new Rectangle(401, 325, _avatar_1.Width, _avatar_1.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);
                    spriteBatch.DrawString(_scoreFont, "Score : " + ScoreHandler.Instance.GetScore(), new Vector2(401, 515), Color.White); ;
                    spriteBatch.DrawString(_scoreFont, "Level : " + 8, new Vector2(401, 535), Color.White);

                    spriteBatch.Draw(_musicStatus, new Rectangle(1269, 20, _enabled.Width, _enabled.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);
                    spriteBatch.Draw(_sfxStatus, new Rectangle(1269, 88, _disabled.Width, _disabled.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);
                    if (!isPaused)
                    {
                        
                        GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.MusicPause));
                        GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.SfxPause));
                        GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.QuitToMain));
                        
                        isPaused = true;
                    }
                    

                    break;
            }


            spriteBatch.End();

        }
        public override void Update(GameTime gameTime)
        {

            // checking if Initialize() has run if it has then skip
            if (!isInitialized)
            {
                Initialize();
            }

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

            MusicToggleStatus();

            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                EventManager.Instance.TriggerEvent("Freeze", new Dictionary<string, object>()
                {
                    {"freeze", false}
                });
            }


            LevelManager.Instance.ChangeLevelDebug();
            LevelManager.Instance.GenerateLevel();
            LevelManager.Instance.CheckForClearedLevelDebug();


            //TODO: [for testing only] - Remove this and implement back button states
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {

                GameWorld.Instance.ChangeState(new MainMenuState());
                SoundManager.Instance.StopClip("soundtrack_1");

                ClearObjects();


            }

            //call update method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Update(gameTime);

            }

            //call cleanup in every cycle
            GameWorld.Instance.CleanUp();
        }

        private void MusicToggleStatus()
        {
            if (SoundManager.Instance._musicDisabled == true)
            {
                _musicStatus = _disabled;
            }
            else
            {
                _musicStatus = _enabled;
            }

            if (SoundManager.Instance._sfxDisabled == true)
            {
                _sfxStatus = _disabled;
            }
            else
            {
                _sfxStatus = _enabled;
            }
        }

        //Initialize is used similar to initialize in GameWorld
        public override void Initialize()
        {
            SoundManager.Instance.StopClip("soundtrack_2");
            SoundManager.Instance.PlayClip("soundtrack_1");


            Director playerDirector = new Director(new PlayerBuilder(CharacterType.Soldier));
            GameWorld.Instance.newGameObjects.Add(playerDirector.Construct());

            LevelManager.Instance.GenerateLevel();
            

            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }
        
            ExperienceOrbFactory orbFactory = new ExperienceOrbFactory();


            foreach (GameObject go in GameWorld.Instance.gameObjects)
            {
                if (go.HasComponent<Button>())
                {
                    GameWorld.Instance.Destroy(go);

                }
            }

            isInitialized = true;

            
        }

        

        /// <summary>
        /// Destroys all relevant gameObjects from world
        /// </summary>
        private void ClearObjects()
        {
            foreach (GameObject go in GameWorld.Instance.gameObjects)
            {
                if (go.HasComponent<Player>() || go.HasComponent<Platform>() || go.HasComponent<Portal>() || go.HasComponent<Mushroom>() || go.HasComponent<ExperienceOrb>())
                {
                    GameWorld.Instance.Destroy(go);
                    LevelManager.Instance.LevelIsGenerated = false;
                    LevelManager.Instance.ResetLevel();
                }
                if (go.HasComponent<Button>())
                {
                    GameWorld.Instance.Destroy(go);

                }
            }
        }


    }
}
