using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    class ReaperMinion : Enemy
    {
        public ReaperMinion(Vector2 position)
        {
            this.position = position;
            health = 10;
            speed = 1;
            damage = 10;
        }

        public override void Awake()
        {
            GameObject.Transform.Position = position;
            base.Awake();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            ChasePlayer();
            CheckCollision();
            HandleAnimations();
            base.Update(gameTime);
        }

        public override void Attack()
        {
            throw new NotImplementedException();
        }

        public override void ChasePlayer()
        {
            Vector2 sourceToTarget = Vector2.Subtract(player.Position, GameObject.Transform.Position);
            sourceToTarget.Normalize();
            sourceToTarget = Vector2.Multiply(sourceToTarget, player.Speed);
            velocity = sourceToTarget;
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
                        GameWorld.Instance.Destroy(GameObject);
                    }

                    //Destroy platform and reaperminion if they collide
                    if (col.GameObject.HasComponent<Platform>())
                    {
                        GameWorld.Instance.Destroy(col.GameObject);
                        GameWorld.Instance.Destroy(GameObject);
                    }
                }
            }
        }

        public override void HandleAnimations()
        {
            animator.PlayAnimation("minion_idle");
        }
    }
}
