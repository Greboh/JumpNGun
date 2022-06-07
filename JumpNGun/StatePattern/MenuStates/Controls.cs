using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
             /*
            [Description]
            Class is not fully utilized as we havent implemented UI for changing control scheme
            */

    public class Controls : IStateMenu
    {
        private MenuStateHandler _pareMenuStateHandler;

        public void Enter(MenuStateHandler parent)
        {
            _pareMenuStateHandler = parent;

            foreach (var go in GameWorld.Instance.GameObjects)
            {
                go.Awake();
            }
            
            //instansiates buttons used in state
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Back, Vector2.Zero));
        }

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

            spriteBatch.Draw(_pareMenuStateHandler.GameTitle,
                new Rectangle(400, 150, _pareMenuStateHandler.GameTitle.Width, _pareMenuStateHandler.GameTitle.Height), null,
                Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

            // draws active GameObjects in list
            for (int i = 0; i < GameWorld.Instance.GameObjects.Count; i++)
            {
                GameWorld.Instance.GameObjects[i].Draw(spriteBatch);
            }


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