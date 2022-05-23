using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun.StatePattern.GameStates
{
    public class MainMenuState : State
    {

        


        public MainMenuState(GameWorld gameworld, GraphicsDevice graphics, ContentManager content)
            : base(gameworld, graphics, content)
        {

        }

        public override void LoadContent()
        {
            //call start method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Start();
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GameWorld.Instance.GraphicsDevice.Clear(Color.Red);
            spriteBatch.Begin();
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Draw(spriteBatch);
            }
            spriteBatch.End();

        }

        public override void Update(GameTime gameTime)
        {
           

            //DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                _gameworld.ChangeState(new MainGameState(_gameworld, _graphics, _content));
                
            }

            //call update method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Update(gameTime);

            }
            if (Keyboard.GetState().IsKeyDown(Keys.T))
            {

            }

            GameWorld.Instance.CleanUp();
        }

        public override void Init()
        {

            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Start));
        }




    }
}
