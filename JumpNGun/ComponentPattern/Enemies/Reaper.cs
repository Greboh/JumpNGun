using Microsoft.Xna.Framework;
using System;

namespace JumpNGun
{
    public class Reaper : Enemy
    {
        //determines whether reaper can summon
        public bool CanSummon { get; set; } 

        //determines whether reaper can teleport
        public bool CanTeleport { get; set; }

        //determines if reaper should use ability or not
        public bool ShouldUseAbility { get; set; }

        //used to save initial speed of reaper 
        public float DefaultSpeed { get; private set; }
        
        //cooldown after used ability
        private float _abilityCooldown = 5;

        //timer to handle ability call
        private float _abilityTimer;

        public Reaper()
        {
            spawnPosition = new Vector2(662, 400);
            health = 200;
            Damage = 20;
            Speed = 0.5f;
            AttackCooldown = 1;
            DefaultSpeed = Speed;
            IsRanged = false;
            IsBoss = true;
        }
        
        public override void Start()
        {
            base.Start();

            //set detectionRange to sprite width
            detectionRange = SpriteRenderer.Sprite.Width;
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            CalculateAttack();
            AbilityLogic();
        }

        /// <summary>
        /// Calculate if we are in attack range and should change state
        /// //LAVET AF NICHLAS HOBERG
        /// </summary>
        private void CalculateAttack()
        {
            if(ShouldUseAbility) return;
            
            Vector2 target = GameObject.Transform.Position - Player.GameObject.Transform.Position;

            // Find the length of the target Vector2
            // The equation for finding a vectors magnitude is: (x * x + y * y)
            
            float targetMagnitude = MathF.Sqrt(target.X * target.X + target.Y * target.Y);
            
            ChangeState(targetMagnitude <= detectionRange ? attackEnemyState : moveEnemyState);
        }

        /// <summary>
        /// Handles ability to select an ability for reaper
        /// ////LAVET AF NICHLAS HOBERG, KRISTIAN J. FICH
        /// </summary>
        private void AbilityLogic()
        {
            //return if ShouldUseAbility is true;
            if (ShouldUseAbility) return;

            //reset ability timer and call PickAbility() if cooldown is surpassed
            if (_abilityTimer > _abilityCooldown)
            {
                _abilityTimer = 0;
                PickAbility();
            }
            //else add time to _abilitytimer
            else _abilityTimer += GameWorld.DeltaTime;

        }

        /// <summary>
        /// Pick random ability and change to ability state
        /// ////LAVET AF NICHLAS HOBERG, KRISTIAN J. FICH
        /// </summary>
        private void PickAbility()
        {
            //should use ability set to true
            ShouldUseAbility = true;

            //generate random number to pick ability
            int rndNumber = rnd.Next(1, 3);

            //pick ability by random number
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
            
            //change state to abilityEnemyState to execute picked ability
            ChangeState(abilityEnemyState);
            
        }
    }
}
