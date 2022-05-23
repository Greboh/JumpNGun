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
        public static float DeltaTime { get; private set; }




        public MainGameState(GameWorld gameworld, GraphicsDevice graphics, ContentManager content)
            :base(gameworld,graphics,content)
        {

        }

        

        public override void LoadContent()
        {
            LevelGenerator.Instance.LoadContent();
            //call start method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Start();
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GameWorld.Instance.GraphicsDevice.Clear(Color.Green);

            spriteBatch.Begin();
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
            LevelManager.Instance.ChangeLevelDebug();
            LevelManager.Instance.GenerateLevel();
            LevelManager.Instance.CheckForClearedLevelDebug();


            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                GameWorld.Instance.Instantiate(ExperienceOrbFactory.Instance.Create(ExperienceOrbType.Small));
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

        public override void Init()
        {
            Director playerDirector = new Director(new PlayerBuilder(CharacterType.Soldier));
            GameWorld.Instance.newGameObjects.Add(playerDirector.Construct());

            LevelManager.Instance.GenerateLevel();

            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }

            ExperienceOrbFactory orbFactory = new ExperienceOrbFactory();
        }

        
    }
}
