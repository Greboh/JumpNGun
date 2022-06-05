using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class MainMenu : IStateMenu
    {
        private MenuStateHandler _pareMenuStateHandler;

        public void Enter(MenuStateHandler parent)
        {
            _pareMenuStateHandler = parent;
            
            
            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }

            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Start));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Settings));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Highscores));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Quit));

            SoundManager.Instance.StopClip("soundtrack_1");
            SoundManager.Instance.PlayClip("soundtrack_2");
            
        }

        public void Execute(GameTime gameTime)
        {
            //call update method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Update(gameTime);
            }

            GameWorld.Instance.CleanUpGameObjects();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            #region SpriteBatch Draws

            spriteBatch.Draw(_pareMenuStateHandler.GameTitle, new Rectangle((int) GameWorld.Instance.ScreenSize.X / 2, 150, _pareMenuStateHandler.GameTitle.Width, _pareMenuStateHandler.GameTitle.Height), null,
                Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);


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
        }

        public void Exit()
        {
           _pareMenuStateHandler.ComponentCleanUp();
        }
    }
}