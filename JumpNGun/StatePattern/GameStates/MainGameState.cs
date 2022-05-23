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

        public static float DeltaTime { get; private set; }




        public MainGameState(GameWorld gameworld, GraphicsDevice graphics, ContentManager content)
            :base(gameworld,graphics,content)
        {

        }

        

        public override void LoadContent()
        {
            
            //call start method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Start();
            }

            LevelGenerator.Instance.LoadContent();

            // asset content loading
            _background_image = _content.Load<Texture2D>("background_image");
            


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
                isInitialized = true;
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
                _gameworld.ChangeState(new MainMenuState(_gameworld, _graphics, _content));
                ClearObjects();


            }

            //call update method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Update(gameTime);

            }

            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //call cleanup in every cycle
            GameWorld.Instance.CleanUp();
        }

        //Initialize is used similar to initialize in GameWorld
        private void Initialize()
        {

            Director playerDirector = new Director(new PlayerBuilder(CharacterType.Soldier));
            GameWorld.Instance.newGameObjects.Add(playerDirector.Construct());

            LevelManager.Instance.GenerateLevel();

            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }
            Console.WriteLine("Main game init");
            ExperienceOrbFactory orbFactory = new ExperienceOrbFactory();
        }

        public override void Init()
        {
            throw new NotImplementedException();
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
