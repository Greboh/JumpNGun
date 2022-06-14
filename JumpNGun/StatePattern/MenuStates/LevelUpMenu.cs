using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class LevelUpMenu : IStateMenu
    {
        private MenuStateHandler _pareMenuStateHandler;



        public void Enter(MenuStateHandler parent)
        {
            _pareMenuStateHandler = parent;
            Console.WriteLine("Level up overlay");

            HandleLevelUpLogic();
        }

        public void Execute(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            //draw sprites of every active gameObject in list
            for (int i = 0; i < GameWorld.Instance.GameObjects.Count; i++)
            {
                GameWorld.Instance.GameObjects[i].Draw(spriteBatch);
            }

            // Draw levelUp menu overlay
            

            
            spriteBatch.End();
        }



        public void LoadContent()
        {

        }

        public void Exit()
        {
            _pareMenuStateHandler.ComponentCleanUp();
   
            _pareMenuStateHandler.ChangeState(_pareMenuStateHandler.Gameplay);
        }
        
        
        private void HandleLevelUpLogic()
        {
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.QuitToMain, Vector2.Zero));
        }
    }
}