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
        private Thread _spawnerThread;
        private int _miniomAmount = 1;

        public Reaper()
        {
            this.position = new Vector2();
            health = 100;
            damage = 20;
            speed = 40;
        }

        public override void Update(GameTime gameTime)
        {
            ChasePlayer();
            HandleAnimations();
            CheckCollision();
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

        }


        public override void HandleAnimations()
        {
            if (!canAttack) animator.PlayAnimation("reaper_idle");
            if (canAttack) animator.PlayAnimation("reaper_attack");

        }

        private void SpawnGhosts()
        {
            for (int i = 0; i < _miniomAmount; i++)
            {
                
            }
        }


        /// <summary>
        /// Handles damage to player logic 
        /// </summary>
        public override void Attack()
        {
            throw new NotImplementedException();
        }

    }
}
