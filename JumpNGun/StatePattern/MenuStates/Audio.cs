using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public class Audio : IStateMenu
    {
        #region fields
        private Texture2D _enabled;
        private Texture2D _disabled;
        private Texture2D _musicStatus;
        private Texture2D _sfxStatus;
        
        private MenuStateHandler _pareMenuStateHandler;

        #endregion

        #region methods

        /// <summary>
        /// initializes code that runs when Audio state is instansiated
        /////LAVET AF KEAN & NICHLAS
        /// </summary>
        /// <param name="parent"></param>
        public void Enter(MenuStateHandler parent)
        {
            _pareMenuStateHandler = parent;
            
            foreach (var go in GameWorld.Instance.GameObjects)
            {
                go.Awake();
            }

            //instansiates button when initializng state
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Music, Vector2.Zero));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Sfx, Vector2.Zero));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Back, Vector2.Zero));
        }

        /// <summary>
        /// Updates gameobjects when state is audio
        /////LAVET AF KEAN & NICHLAS
        /// </summary>
        /// <param name="gameTime"></param>
        public void Execute(GameTime gameTime)
        {
            for (int i = 0; i < GameWorld.Instance.GameObjects.Count; i++)
            {
                GameWorld.Instance.GameObjects[i].Update(gameTime);
            }


            SetAudioStatusIcons(); // logic for toggling sfx/music on/off textures

            GameWorld.Instance.CleanUpGameObjects();        
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();


            #region SpriteBatch draws

            spriteBatch.Draw(_pareMenuStateHandler.GameTitle, new Rectangle(400, 150, _pareMenuStateHandler.GameTitle.Width, _pareMenuStateHandler.GameTitle.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

            spriteBatch.Draw(_musicStatus, new Rectangle(715, 373, _enabled.Width, _enabled.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

            spriteBatch.Draw(_sfxStatus, new Rectangle(715, 442, _disabled.Width, _disabled.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);


            // draws active GameObjects in list

            for (int i = 0; i < GameWorld.Instance.GameObjects.Count; i++)

            {
                GameWorld.Instance.GameObjects[i].Draw(spriteBatch);
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

        /// <summary>
        /// Code that runs when state is changed
        /////LAVET AF KEAN & NICHLAS
        /// </summary>
        public void Exit()
        {
            _pareMenuStateHandler.ComponentCleanUp();
        }


        /// <summary>
        /// Sets music/sfx status icons
        /////LAVET AF KEAN
        /// </summary>
        private void SetAudioStatusIcons()
        {
            //sets texture to _disabled if false otherwise sets it to _enabled
            _musicStatus = SoundManager.Instance._musicDisabled ? _disabled : _enabled;

            //sets texture to _disabled if false otherwise sets it to _enabled
            _sfxStatus = SoundManager.Instance._sfxDisabled ? _disabled : _enabled;
        }
        #endregion
    }
}
