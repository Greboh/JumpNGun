using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public abstract class State
    {




        private bool isMenu = true;

        public bool IsMenu { get => isMenu; set => isMenu = value; }
        
        public abstract void Initialize();
        public abstract void LoadContent();
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);


    }
}
