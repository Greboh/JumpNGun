using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun.StatePattern.GameStates
{
    public class CharacterSelection : State
    {
        private Texture2D _game_title;

        public override void Initialize()
        {
            ComponentCleanUp();

            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }
        }

        public override void LoadContent()
        {
            _game_title = GameWorld.Instance.Content.Load<Texture2D>("game_title");

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_game_title, new Rectangle(screenSizeY / 2, 150, _game_title.Width, _game_title.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);

        }

        public override void Update(GameTime gameTime)
        {
            InitializeCheck();
        }
    }
}
