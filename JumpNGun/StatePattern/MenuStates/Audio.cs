using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public class Audio : IStateMenu
    {
        private Texture2D _enabled;
        private Texture2D _disabled;
        private Texture2D _musicStatus;
        private Texture2D _sfxStatus;
        
        private MenuStateHandler _pareMenuStateHandler;

        public void Enter(MenuStateHandler parent)
        {
            _pareMenuStateHandler = parent;
            
            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }

            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Music, Vector2.Zero));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Sfx, Vector2.Zero));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Back, Vector2.Zero));
        }

        public void Execute(GameTime gameTime)
        {
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Update(gameTime);
            }


            SetAudioStatusIcons();

            GameWorld.Instance.CleanUpGameObjects();        
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();


            #region SpriteBatch draws

            spriteBatch.Draw(_pareMenuStateHandler.GameTitle, new Rectangle((int)GameWorld.Instance.ScreenSize.X / 2, 150, _pareMenuStateHandler.GameTitle.Width, _pareMenuStateHandler.GameTitle.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

            spriteBatch.Draw(_musicStatus, new Rectangle(715, 373, _enabled.Width, _enabled.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

            spriteBatch.Draw(_sfxStatus, new Rectangle(715, 442, _disabled.Width, _disabled.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);


            // draws active GameObjects in list

            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)

            {
                GameWorld.Instance.gameObjects[i].Draw(spriteBatch);
            }

            #endregion


            spriteBatch.End();
        }

        public void LoadContent()
        {
            _enabled = GameWorld.Instance.Content.Load<Texture2D>("checkmark");
            _disabled = GameWorld.Instance.Content.Load<Texture2D>("crossedout");


            _musicStatus = GameWorld.Instance.Content.Load<Texture2D>("checkmark");
            _sfxStatus = GameWorld.Instance.Content.Load<Texture2D>("checkmark");
        }

        public void Exit()
        {
            _pareMenuStateHandler.ComponentCleanUp();
        }


        /// <summary>
        /// Sets music/sfx status icons
        /// </summary>
        private void SetAudioStatusIcons()
        {
            _musicStatus = SoundManager.Instance._musicDisabled ? _disabled : _enabled;

            _sfxStatus = SoundManager.Instance._sfxDisabled ? _disabled : _enabled;
        }
    }
}
