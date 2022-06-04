using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun.StatePattern.GameStates
{
    class LandingScreen : State
    {
        private Texture2D _gameTitle;
        private Texture2D _inputField;
        private Texture2D _inputFieldTitle;


        public override void Initialize()
        {
            ComponentCleanUp();

            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }


            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Submit));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.InputField));


        }

        public override void LoadContent()
        {
            _gameTitle = GameWorld.Instance.Content.Load<Texture2D>("game_title");
            _inputField = GameWorld.Instance.Content.Load<Texture2D>("input_field");
            _inputFieldTitle = GameWorld.Instance.Content.Load<Texture2D>("enter_name_text");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(_gameTitle, new Rectangle(screenSizeY / 2, 150, _gameTitle.Width, _gameTitle.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);
            spriteBatch.Draw(_inputFieldTitle, new Rectangle(594, 396, _inputFieldTitle.Width, _inputFieldTitle.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

            // draws active GameObjects in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Draw(spriteBatch);
            }

            spriteBatch.End();

            



        }

        public override void Update(GameTime gameTime)
        {
            InitializeCheck();

            //call update method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Update(gameTime);

            }

            GameWorld.Instance.CleanUp();
        }

    }
}
