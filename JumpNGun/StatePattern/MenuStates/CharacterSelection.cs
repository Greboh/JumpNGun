using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun.StatePattern.GameStates
{
    public class CharacterSelection : IStateMenu
    {
        #region fields
        private MenuStateHandler _pareMenuStateHandler;

        #endregion

        #region methods
        public void Enter(MenuStateHandler parent)
        {
            _pareMenuStateHandler = parent;

            foreach (var go in GameWorld.Instance.GameObjects)
            {
                go.Awake();
            }

            //instansiates buttons for character selection
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Character1,Vector2.Zero ));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Character2,Vector2.Zero ));

        }

        public void Execute(GameTime gameTime)
        {
            //call update method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.GameObjects.Count; i++)
            {
                GameWorld.Instance.GameObjects[i].Update(gameTime);
            }

            //call cleanup in every cycle
            GameWorld.Instance.CleanUpGameObjects();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            //draw sprites of every active gameObject in list
            for (int i = 0; i < GameWorld.Instance.GameObjects.Count; i++)
            {
                GameWorld.Instance.GameObjects[i].Draw(spriteBatch);
            }

            // game title texture
            spriteBatch.Draw(_pareMenuStateHandler.GameTitle,
                new Rectangle(400, 150, _pareMenuStateHandler.GameTitle.Width, _pareMenuStateHandler.GameTitle.Height), null,
                Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

            spriteBatch.End();
        }

        public void LoadContent()
        {
            //call start method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.GameObjects.Count; i++)
            {
                GameWorld.Instance.GameObjects[i].Start();
            }
        }

        public void Exit()
        {
            _pareMenuStateHandler.ComponentCleanUp();
        }
        #endregion
    }
}