using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public abstract class State 
    {
        /*
            [Description]
            Abstract class for menu state methods to be used in GameWorld.
        */

        protected static int screenSizeX = (int)GameWorld.Instance.ScreenSize.X;
        protected static int screenSizeY = (int)GameWorld.Instance.ScreenSize.Y;

        private bool isInitialized;


        private bool isMenu = true;

        public bool IsMenu { get => isMenu; set => isMenu = value; }
        
        public abstract void Initialize();
        public abstract void LoadContent();
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Makes sure Initialize only runs once in child state class update method with a bool check
        /// </summary>
        public void InitializeCheck()
        {
            if (!isInitialized) Initialize();
            isInitialized = true;
        }

        public void ComponentCleanUp()
        {
            foreach (GameObject go in GameWorld.Instance.gameObjects)
            {
                if (go.HasComponent<Player>() || go.HasComponent<Platform>() || go.HasComponent<Portal>() || go.HasComponent<ExperienceOrb>() || go.HasComponent<Button>() || go.HasComponent<Mushroom>())
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
