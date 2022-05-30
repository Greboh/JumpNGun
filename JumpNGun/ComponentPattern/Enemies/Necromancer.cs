using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun.ComponentPattern.Enemies
{
    class Necromancer : Enemy
    {
        private bool _isImmune = true;

        public Necromancer(Vector2 position)
        {
            this.position = position;
            health = 150;
            speed = 20;
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

        public override void Attack()
        {
            throw new NotImplementedException();
        }

        public override void ChasePlayer()
        {
            throw new NotImplementedException();
        }

        public override void CheckCollision()
        {
            throw new NotImplementedException();
        }

        public override void HandleAnimations()
        {
            throw new NotImplementedException();
        }
    }
}
