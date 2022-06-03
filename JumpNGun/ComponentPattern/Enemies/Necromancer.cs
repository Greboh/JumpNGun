using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    class Necromancer : Enemy
    {

        public Necromancer(Vector2 position)
        {
            this.GameObject.Transform.Position = position;
            health = 150;
            Speed = 20;
        }

        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

 

        public override void CheckCollision()
        {
            
        }
        
    }
}
