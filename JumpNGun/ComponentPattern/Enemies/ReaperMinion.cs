using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun.ComponentPattern.Enemies
{
    class ReaperMinion : Enemy
    {
        private Reaper _reaper;
        private ReaperMinion _reaperMinion;

        public ReaperMinion(Vector2 position)
        {
            this.position = position;
            health = 10;
            speed = 60;
            damage = 10;
            isColliding = false;
        }

        public override void Awake()
        {
            sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            _reaper = GameObject.GetComponent<Reaper>() as Reaper;
            
        }

        public override void Start()
        {
            
        }

        public override void Update(GameTime gameTime)
        {



            UpdatePositionReference();
            Flipsprite();
            Move();
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

        public override void FindPlayerObject()
        {
            throw new NotImplementedException();
        }

        public override void HandleAnimations()
        {
            throw new NotImplementedException();
        }
    }
}
