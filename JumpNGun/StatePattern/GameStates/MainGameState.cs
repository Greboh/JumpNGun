using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun.StatePattern.GameStates
{
    public class MainGameState : State
    {

        public MainGameState(GameWorld gameworld, GraphicsDevice graphics, ContentManager content)
            :base(gameworld,graphics,content)
        {

        }

        public override void LoadContent()
        {
            
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GameWorld.Instance.GraphicsDevice.Clear(Color.Green);

        }
        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
