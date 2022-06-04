using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun.StatePattern.GameStates
{
    public class CharacterSelection : State
    {
        private Texture2D _gameTitle;


        public override void Initialize()
        {
            ComponentCleanUp();

            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }

            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Character1));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Character2));

            Console.WriteLine("Character selection state");
        }

        public override void LoadContent()
        {
            //call start method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Start();
            }

            _gameTitle = GameWorld.Instance.Content.Load<Texture2D>("game_title");




        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            //draw sprites of every active gameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Draw(spriteBatch);
            }


            spriteBatch.Draw(_gameTitle, new Rectangle(screenSizeY / 2, 150, _gameTitle.Width, _gameTitle.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);


            

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

            //call cleanup in every cycle
            GameWorld.Instance.CleanUp();
        }
    }
}
