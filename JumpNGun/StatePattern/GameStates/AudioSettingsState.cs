using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun.StatePattern.GameStates
{
    public class AudioSettingsState : State
    {
        public override void LoadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            GameWorld.Instance.GraphicsDevice.Clear(Color.Green);

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public override void Init()
        {
        }

        

        
    }
}
