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

    public class MainGameState : State
    {
        private Texture2D _background_image;
        
        private Texture2D _pausedOverlay;
        static int screenSizeX = (int)GameWorld.Instance.ScreenSize.X;
        static int screenSizeY = (int)GameWorld.Instance.ScreenSize.Y;

        private bool isInitialized;
        private bool isPaused;
        private PauseState currentPauseState = PauseState.unpaused;
        private float _keypressCooldown; // mouse click cooldown used for avoiding unwanted menu button navigation

        public override void LoadContent()
        {
            
            //call start method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Start();
            }

// <<<<<<<<< Temporary merge branch 1
//             LevelGenerator.Instance.LoadContent();
// =========
//             PlatformGenerator.Instance.LoadContent();
// >>>>>>>>> Temporary merge branch 2



            // asset content loading
            _pausedOverlay = GameWorld.Instance.Content.Load<Texture2D>("paused_overlay");
            


        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Begin();

            //draw sprites of every active gameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Draw(spriteBatch);
            }

            switch (currentPauseState)
            {
                case PauseState.unpaused:
                
                    break;
                case PauseState.paused:
                    GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.QuitToMain));
                    
                    spriteBatch.Draw(_pausedOverlay, new Vector2(0, 0), Color.White);


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

            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && currentPauseState == PauseState.unpaused)
            {
                currentPauseState = PauseState.paused;

            }else if (Keyboard.GetState().IsKeyDown(Keys.Escape) && currentPauseState == PauseState.paused)
            {
                currentPauseState = PauseState.unpaused;

            }

            if (Button._returnedToMenu == true)
            {
                returnToMenu();
                Button._returnedToMenu = false;
            }






            LevelManager.Instance.ChangeLevelDebug();
            LevelManager.Instance.GenerateLevel();
            LevelManager.Instance.CheckForClearedLevelDebug();

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                //TODO: [for testing only] - Remove this and implement back button states
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
            Console.WriteLine("Main game init");
            ExperienceOrbFactory orbFactory = new ExperienceOrbFactory();

            isInitialized = true;

            foreach (GameObject go in GameWorld.Instance.gameObjects)
            {
                if (go.HasComponent<Button>())
                {
                    GameWorld.Instance.Destroy(go);

                }
            }

            isInitialized = true;

            
        }

        public void returnToMenu()
        {
            GameWorld.Instance.ChangeState(new MainMenuState());
            SoundManager.Instance.StopClip("soundtrack_1");

            ClearObjects();
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
