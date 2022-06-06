using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class MainMenu : IStateMenu
    {
        #region fields
        private MenuStateHandler _pareMenuStateHandler;

        #endregion

        #region methods

        /// <summary>
        /// initializes code that runs when MainMenu state is instansiated
        /////LAVET AF KEAN & NICHLAS
        /// </summary>
        /// <param name="parent"></param>
        public void Enter(MenuStateHandler parent)
        {
            _pareMenuStateHandler = parent;
            
            
            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }

            //instansiates relevant buttons
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Start, Vector2.Zero));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Settings, Vector2.Zero));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Highscores, Vector2.Zero));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Quit, Vector2.Zero));

            //stops main game music and starts menu music
            SoundManager.Instance.StopClip("soundtrack_1");
            SoundManager.Instance.PlayClip("soundtrack_2");
            
            ScoreHandler.Instance.SortScore();
            
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

            // game title texture
            spriteBatch.Draw(_pareMenuStateHandler.GameTitle, new Rectangle(400, 150, _pareMenuStateHandler.GameTitle.Width, _pareMenuStateHandler.GameTitle.Height), null,
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
            LevelManager.Instance.ResetLevel();
           _pareMenuStateHandler.ComponentCleanUp();
        }
        #endregion
    }
}