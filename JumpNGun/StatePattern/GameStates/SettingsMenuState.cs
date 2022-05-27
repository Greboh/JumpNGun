using JumpNGun.ComponentPattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public class SettingsMenuState : State
    {
        static int screenSizeX = (int)GameWorld.Instance.ScreenSize.X;
        static int screenSizeY = (int)GameWorld.Instance.ScreenSize.Y;
        private Texture2D _background_image;
        private Texture2D _game_title;
        

        private bool isInitialized;

        //TODO: Remove previous buttons from drawing
        public override void LoadContent()
        {


            // asset content loading
            _background_image = GameWorld.Instance.Content.Load<Texture2D>("background_image");
            _game_title = GameWorld.Instance.Content.Load<Texture2D>("game_title");
            
            
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //GameWorld.Instance.GraphicsDevice.Clear(Color.Red);
            spriteBatch.Begin();

            spriteBatch.Draw(_background_image, new Vector2(0, 0), Color.White);

            spriteBatch.Draw(_game_title, new Rectangle(screenSizeY / 2, 150, _game_title.Width, _game_title.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);




            // draws active GameObjects in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Draw(spriteBatch);
            }
            spriteBatch.End();

        }
        public override void Update(GameTime gameTime)
        {
            if (!isInitialized)
            {
                Initialize();
                isInitialized = true;
            }

            //call update method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Update(gameTime);

            }


            GameWorld.Instance.CleanUp();

        }

        //Initialize is used similar to initialize in GameWorld
        public override void Initialize()
        {
            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }

            ClearObjects();
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Audio));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Controls));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Back));

            
        }



        private void ClearObjects()
        {
            foreach (GameObject go in GameWorld.Instance.gameObjects)
            {
                if (go.HasComponent<Button>())
                {
                    GameWorld.Instance.Destroy(go);

                }
            }
        }

        
    }
}
