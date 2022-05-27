using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JumpNGun
{
    class Reaper : Enemy
    {
        private int _spawnTimer = 20;
        private Reaper _reaper;
        private Thread _spawnerThread;

        public Reaper(Vector2 position)
        {
            this.position = position;
            health = 100;
            damage = 20;
            speed = 40;
        }

        public override void Update(GameTime gameTime)
        {
            ChasePlayer();
            UpdatePositionReference();
            HandleAnimations();
            CheckCollision();
        }

        /// <summary>
        /// Handles damage to player logic 
        /// </summary>
        public override void Attack()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets velocity to target player position
        /// </summary>
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
                if (col.GameObject.Tag == "Player" && collider.CollisionBox.Intersects(col.CollisionBox))
                {
                    isColliding = true;
                }
                if (col.GameObject.Tag == "Player" && !collider.CollisionBox.Intersects(col.CollisionBox))
                {
                    isColliding = false;
                }
            }
        }


        public override void HandleAnimations()
        {
            if (!isColliding)
            {
                animator.PlayAnimation("reaper_idle");
            }
            if (isColliding)
            {
                animator.PlayAnimation("reaper_attack");
            }
        }

        private void SpawnGhosts()
        {

        }

    }
}
