using JumpNGun.ComponentPattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public class Settings : State
    {


        private Texture2D _gameTitle;

        //Initialize is used similar to initialize in GameWorld
        public override void Initialize()
        {
            ComponentCleanUp();

            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }


            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Audio));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Controls));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Back));


        }

        //TODO: Remove previous buttons from drawing
        public override void LoadContent()
        {
            // asset content loading
            _gameTitle = GameWorld.Instance.Content.Load<Texture2D>("game_title");  

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            #region SpriteBatch Draws
            spriteBatch.Draw(_gameTitle, new Rectangle(screenSizeY / 2, 150, _gameTitle.Width, _gameTitle.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

            // draws active GameObjects in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Draw(spriteBatch);
            }

            #endregion

            spriteBatch.End();

        }
        public override void Update(GameTime gameTime)
        {
            InitializeCheck();


            //call update method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Update(gameTime);

            }

            GameWorld.Instance.CleanUp();

        }

        



        

        
    }
}
