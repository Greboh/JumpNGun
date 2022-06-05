using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using SharpDX.Direct3D9;

namespace JumpNGun
{
    class LandingScreen : IStateMenu
    {
        private Texture2D _inputField;
        private Texture2D _inputFieldTitle;
        private string _inputString = String.Empty;
        private SpriteFont _inputFont;

        private MenuStateHandler _pareMenuStateHandler;

        public void Enter(MenuStateHandler parent)
        {
            _pareMenuStateHandler = parent;
            
            EventManager.Instance.Subscribe("OnInput", OnInput);
            
            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Submit));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.InputField));
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

            spriteBatch.Draw(_pareMenuStateHandler.GameTitle, new Rectangle((int) GameWorld.Instance.ScreenSize.X / 2, 150, _pareMenuStateHandler.GameTitle.Width, _pareMenuStateHandler.GameTitle.Height), null, Color.White, 0,
                new Vector2(0, 0), SpriteEffects.None, 1);
            spriteBatch.Draw(_inputFieldTitle, new Rectangle(594, 396, _inputFieldTitle.Width, _inputFieldTitle.Height), null, Color.White, 0, new Vector2(0, 0),
                SpriteEffects.None, 1);
            spriteBatch.DrawString(_inputFont, _inputString, new Vector2(594, 450), Color.Black);

            // draws active GameObjects in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        public void LoadContent()
        {
            _inputField = GameWorld.Instance.Content.Load<Texture2D>("input_field");
            _inputFieldTitle = GameWorld.Instance.Content.Load<Texture2D>("enter_name_text");
            _inputFont = GameWorld.Instance.Content.Load<SpriteFont>("font");
        }


        public void Exit()
        {
            _pareMenuStateHandler.ComponentCleanUp();
        }
        

        private void OnInput(Dictionary<string, object> ctx)
        {
            _inputString += (string) ctx["inputKey"];
        }
    }
}