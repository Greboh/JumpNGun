using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    class ReaperMinion : Enemy
    {
        private bool _spawningDone;
        private float liveTime = 10;
        private float timer;

        public ReaperMinion(Vector2 position)
        {
            spawnPosition = position;
            health = 10;
            Speed = 1f;
            damage = 10;
            IsRanged = false;
            IsBoss = true;
            AttackCooldown = 1;
        }

        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            base.Start();
            
            GameObject.Transform.Position = spawnPosition;
            detectionRange = SpriteRenderer.Sprite.Width;

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            CalculateAttack();
            CheckCollision();
        }

        /// <summary>
        /// Calculate if we are in attack range and should change state
        /// </summary>
        private void CalculateAttack()
        {
            Vector2 target = GameObject.Transform.Position - Player.GameObject.Transform.Position;

            // Find the length of the target Vector2
            // The equation for finding a vectors magnitude is: (x * x + y * y)
            
            float targetMagnitude = MathF.Sqrt(target.X * target.X + target.Y * target.Y);
            
            ChangeState(targetMagnitude <= detectionRange ? attackState : moveState);
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
                        // GameWorld.Instance.Destroy(GameObject);
                    }
                }
            }
        }
    }
}
