using Microsoft.Xna.Framework;
using System;

namespace JumpNGun
{
    class ReaperMinion : Enemy
    {
        public ReaperMinion(Vector2 position)
        {
            spawnPosition = position;
            health = 10;
            Speed = 1f;
            Damage = 10;
            IsRanged = false;
            IsBoss = true;
            AttackCooldown = 1;
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
        /// //LAVET AF NICHLAS HOBERG 
        /// </summary>
        private void CalculateAttack()
        {
            Vector2 target = GameObject.Transform.Position - Player.GameObject.Transform.Position;

            // Find the length of the target Vector2
            // The equation for finding a vectors magnitude is: (x * x + y * y)
            
            float targetMagnitude = MathF.Sqrt(target.X * target.X + target.Y * target.Y);
            
            ChangeState(targetMagnitude <= detectionRange ? attackEnemyState : moveEnemyState);
        }
    }
}
