using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class SettingsMenu : IStateMenu
    {
        private MenuStateHandler _pareMenuStateHandler;

        /// <summary>
        /// initializes code that runs when SettingsMenu state is instansiated
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

            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Audio, Vector2.Zero));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Controls, Vector2.Zero));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Back, Vector2.Zero));
        }

        /// <summary>
        /// Updates gameobjects when state is SettingsMenu.
        /////LAVET AF KEAN & NICHLAS
        /// </summary>
        /// <param name="gameTime"></param>
        public void Execute(GameTime gameTime)
        {
            //call update method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.GameObjects.Count; i++)
            {
                GameWorld.Instance.GameObjects[i].Update(gameTime);
            }

            GameWorld.Instance.CleanUpGameObjects();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            #region SpriteBatch Draws

            // Draws gametitle texure
            spriteBatch.Draw(_pareMenuStateHandler.GameTitle,
                new Rectangle(400, 150, _pareMenuStateHandler.GameTitle.Width, _pareMenuStateHandler.GameTitle.Height),
                null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

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
        }

        /// <summary>
        /// Code that runs when state is changed
        /////LAVET AF KEAN & NICHLAS
        /// </summary>
        public void Exit()
        {
            _pareMenuStateHandler.ComponentCleanUp();
        }
    }
}