using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun.ComponentPattern.Enemies
{
    class ReaperMinion : Enemy
    {
        private Reaper _parentReaper;
        private ReaperMinion _reaperMinion;

        public ReaperMinion(Vector2 position)
        {
            this.position = position;
            health = 10;
            speed = 60;
            damage = 10;
            isColliding = false;
        }
        
        public override void Start()
        {
            base.Start();

            _parentReaper = GameObject.GetComponent<Reaper>() as Reaper;
        }

        public override void Update(GameTime gameTime)
        {
            CheckCollision();
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
            foreach (Collider col in GameWorld.Instance.Colliders)
            {
                if (collider.CollisionBox.Intersects(col.CollisionBox))
                {
                    //deal damage to the player if reaperminion and player collides
                    if (col.GameObject.HasComponent<Player>())
                    {
                        //get player component and attack player
                        //destroy reaperminion
                    }

                    //Destroy platform and reaperminion if they collide
                    if (col.GameObject.HasComponent<Platform>())
                    {
                        GameWorld.Instance.Destroy(col.GameObject);
                        GameWorld.Instance.Destroy(this.GameObject);
                    }
                }
            }
        }

        public override void HandleAnimations()
        {
            throw new NotImplementedException();
        }
    }
}
