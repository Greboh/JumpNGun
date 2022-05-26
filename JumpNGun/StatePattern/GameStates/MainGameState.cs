using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun.StatePattern.GameStates
{
    public class MainGameState : State
    {
        private Texture2D _background_image;


        static int screenSizeX = (int)GameWorld.Instance.ScreenSize.X;
        static int screenSizeY = (int)GameWorld.Instance.ScreenSize.Y;

        private bool isInitialized;


        

        public override void LoadContent()
        {
            
            //call start method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Start();
            }

            LevelGenerator.Instance.LoadContent();

            // asset content loading
            _background_image = GameWorld.Instance.Content.Load<Texture2D>("background_image");
            


        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Begin();


            spriteBatch.Draw(_background_image, new Vector2(0, 0), Color.White);


            LevelGenerator.Instance.Draw(spriteBatch);

            //draw sprites of every active gameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Draw(spriteBatch);
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


            LevelManager.Instance.ChangeLevelDebug();
            LevelManager.Instance.GenerateLevel();
            LevelManager.Instance.CheckForClearedLevelDebug();


            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                GameWorld.Instance.Instantiate(ExperienceOrbFactory.Instance.Create(ExperienceOrbType.Small));
            }

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
        }

        private void ClearObjects()
        {
            foreach (GameObject go in GameWorld.Instance.gameObjects)
            {
                if (go.HasComponent<Player>() || go.HasComponent<Platform>() || go.HasComponent<Portal>())
                {
                    GameWorld.Instance.Destroy(go);
                    LevelManager.Instance.LevelIsGenerated = false;
                    LevelManager.Instance.ResetLevel();
                }
            }
        }

        
    }
}
