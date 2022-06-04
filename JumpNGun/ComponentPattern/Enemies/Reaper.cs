using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JumpNGun
{
    public class Reaper : Enemy
    {
        public bool CanSummon { get; set; }
        public bool CanTeleport { get; set; }
        public bool ShouldUseAbility { get; set; }
        public float DefaultSpeed { get; private set; }
        

        private float _abilityCooldown = 5;
        private float _abilityTimer;

        private Random _rnd = new Random();
        
        public Reaper()
        {
            spawnPosition = new Vector2(662, 400);
            health = 200;
            damage = 20;
            Speed = 0.5f;
            AttackCooldown = 1;
            DefaultSpeed = Speed;
            IsRanged = false;
            IsBoss = true;
        }
        
        public override void Start()
        {
            base.Start();

            detectionRange = SpriteRenderer.Sprite.Width;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            CheckCollision();
            CalculateAttack();
            AbilityLogic();
        }
        
        /// <summary>
        /// Calculate if we are in attack range and should change state
        /// </summary>
        private void CalculateAttack()
        {
            if(ShouldUseAbility) return;
            
            Vector2 target = GameObject.Transform.Position - Player.GameObject.Transform.Position;

            // Find the length of the target Vector2
            // The equation for finding a vectors magnitude is: (x * x + y * y)
            
            float targetMagnitude = MathF.Sqrt(target.X * target.X + target.Y * target.Y);
            
            ChangeState(targetMagnitude <= detectionRange ? attackState : moveState);
        }

        private void AbilityLogic()
        {
            if (ShouldUseAbility) return;

            if (_abilityTimer > _abilityCooldown)
            {
                _abilityTimer = 0;
                PickAbility();
            }
            else _abilityTimer += GameWorld.DeltaTime;

        }

        private void PickAbility()
        {
            ShouldUseAbility = true;
            int rndNumber = _rnd.Next(1, 3);

            switch (rndNumber)
            {
                case 1:
                {
                    CanTeleport = true;
                } break;
                
                case 2:
                {
                    CanSummon = true;
                } break;
            }
            
            ChangeState(abilityState);
            
        }
    }
}
