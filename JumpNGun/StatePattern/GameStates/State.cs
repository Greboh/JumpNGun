using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun.StatePattern.GameStates
{
    public abstract class State
    {

        protected ContentManager _content;

        protected GraphicsDevice _graphics;

        protected GameWorld _gameworld;

        public State(GameWorld gameworld, GraphicsDevice graphics, ContentManager content)
        {
            _content = content;
            _graphics = graphics;
            _gameworld = gameworld;
        }

        public abstract void LoadContent();

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);

    }
}
