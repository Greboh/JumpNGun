using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public class AudioSettingsState : State
    {
        static int screenSizeX = (int)GameWorld.Instance.ScreenSize.X;
        static int screenSizeY = (int)GameWorld.Instance.ScreenSize.Y;
        private Texture2D _background_image;
        private Texture2D _game_title;
        private Texture2D _enabled;
        private Texture2D _disabled;
        private Texture2D _musicStatus;
        private Texture2D _sfxStatus;
        private bool isInitialized;
        
        public override void LoadContent()
        {

            // asset content loading
            _background_image = GameWorld.Instance.Content.Load<Texture2D>("background_image");
            _game_title = GameWorld.Instance.Content.Load<Texture2D>("game_title");
            _enabled = GameWorld.Instance.Content.Load<Texture2D>("checkmark");
            _disabled = GameWorld.Instance.Content.Load<Texture2D>("crossedout");

            _musicStatus = GameWorld.Instance.Content.Load<Texture2D>("checkmark");
            _sfxStatus = GameWorld.Instance.Content.Load<Texture2D>("checkmark");

        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_background_image, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(_game_title, new Rectangle(screenSizeY / 2, 150, _game_title.Width, _game_title.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);


            spriteBatch.Draw(_musicStatus, new Rectangle(715, 373, _enabled.Width, _enabled.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);
            spriteBatch.Draw(_sfxStatus, new Rectangle(715, 442, _disabled.Width, _disabled.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);





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
            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }

            ClearObjects();

            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Music));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Sfx));
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
