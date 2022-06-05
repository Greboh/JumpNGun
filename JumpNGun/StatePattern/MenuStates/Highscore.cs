using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun.StatePattern.GameStates
{
    public class Highscore : IStateMenu
    {
        private MenuStateHandler _pareMenuStateHandler;

        private Texture2D _highscorePanel;

        private SpriteFont _scoreFont;



        public void Enter(MenuStateHandler parent)
        {
            _pareMenuStateHandler = parent;

            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }

            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Back));



            Console.WriteLine("Highscore state");
        }

        public void Execute(GameTime gameTime)
        {
            //call update method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Update(gameTime);
            }

            //call cleanup in every cycle
            GameWorld.Instance.CleanUpGameObjects();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            //draw sprites of every active gameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Draw(spriteBatch);
            }
            spriteBatch.Draw(_highscorePanel, new Rectangle(370,180,_highscorePanel.Width,_highscorePanel.Height), Color.White);

            spriteBatch.Draw(_pareMenuStateHandler.GameTitle,
                new Rectangle(400, 60, _pareMenuStateHandler.GameTitle.Width, _pareMenuStateHandler.GameTitle.Height), null,
                Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);


            spriteBatch.End();
        }

        public void LoadContent()
        {
            //call start method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Start();
            }

            _highscorePanel = GameWorld.Instance.Content.Load<Texture2D>("highscore_panel");
            _scoreFont = GameWorld.Instance.Content.Load<SpriteFont>("font");
        }

        public void Exit()
        {
            _pareMenuStateHandler.ComponentCleanUp();
        }

    }
}