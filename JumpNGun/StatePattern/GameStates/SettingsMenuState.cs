using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public class SettingsMenuState : State
    {
        private bool isInitialized;


        public override void LoadContent()
        {
            
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //GameWorld.Instance.GraphicsDevice.Clear(Color.Red);
            spriteBatch.Begin();
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


        }

        //Initialize is used similar to initialize in GameWorld
        private void Initialize()
        {

            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }

            ClearObjects();
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Audio));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Controls));
            

        }

        public override void Init()
        {
            
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
