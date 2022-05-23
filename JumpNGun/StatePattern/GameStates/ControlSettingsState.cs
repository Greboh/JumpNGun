using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public class ControlSettingsState : State
    {
        public override void LoadContent()
        {
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            GameWorld.Instance.GraphicsDevice.Clear(Color.Green);

        }

        public override void Init()
        {
            
        }
    }
}
