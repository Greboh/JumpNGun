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
        private int _miniomAmount = 1;
        private bool _canSpawn;
        private bool _canTeleport;
        

        private float _spawnTime;
        private float _spawnCooldown = 20;
        private float _teleportTime;
        private float _teleportCooldown = 10;
        private Vector2[] minionPositions;


        public Reaper()
        {
            this.position = new Vector2(662, 400);
            health = 200;
            damage = 20;
            speed = 0.5f;
            originalspeed = speed;
        }
        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            GameObject.Transform.Position = position;
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            ChasePlayer();
            HandleAnimations();
            CheckCollision();
            Teleport();
            HandleTeleportLogic();
            base.Update(gameTime);
        }

        /// <summary>
        /// Checks for collision with enemy
        /// </summary>
        public override void CheckCollision()
        {
            foreach (Collider col in GameWorld.Instance.Colliders)
            {
                if (col.GameObject.Tag == "player" && col.CollisionBox.Intersects(collider.CollisionBox))
                {
                    Attack();
                }
            }
        }


        /// <summary>
        /// Handle all relevant animation for Reaper
        /// </summary>
        public override void HandleAnimations()
        {
            if (!canAttack && !_canTeleport) animator.PlayAnimation("reaper_idle");
            if (canAttack && !_canTeleport) animator.PlayAnimation("reaper_attack");
            if (health <= 0) animator.PlayAnimation("reaper_death");
            if (_canTeleport) animator.PlayAnimation("reaper_teleport");
        }

        #region ABILITIES

        /// <summary>
        /// Teleports Reaper close to player object if conditions are met
        /// </summary>
        private void Teleport()
        {
            if (!_canTeleport || health <= 50) return;

            speed = 0; 
            
            if (animator.CurrentIndex >= 17)
            {
                GameObject.Transform.Position = new Vector2(player.Position.X + sr.Sprite.Width, player.Position.Y);
            }
            if (animator.IsAnimationDone)
            {
                _canTeleport = false;
                speed = originalspeed;
            }
        }

        /// <summary>
        /// Returns true/false depending on this object's distance to player
        /// </summary>
        /// <returns></returns>
        private bool CalculateDistanceToEnemy()
        {
            if (this.position.X - player.Position.X > 200 || this.position.X - player.Position.X < -200) return true;
            if (this.position.Y - player.Position.Y > 200 || this.position.Y - player.Position.Y < -200) return true;
            else return false;
        }

        /// <summary>
        /// Resets ability to teleport after cooldown
        /// </summary>
        private void HandleTeleportLogic()
        {
            if (_canTeleport) return;

            _teleportTime += GameWorld.DeltaTime;

            if (_teleportTime > _teleportCooldown)
            {
                _canTeleport = true;
                _teleportTime = 0;
            }

        }

        /// <summary>
        /// Spawn Reaperminions around Reaper if conditions are met
        /// </summary>
        /// 
        private void SummonMinions()
        {
            if (!_canSpawn) return; //exit if reaper can't summon

            CreateMinionPositions(); 

            //instantiate 4 minios around reaper
            for (int i = 0; i < _miniomAmount; i++)
            {
                GameWorld.Instance.Instantiate(EnemyFactory.Instance.Create(EnemyType.ReaperMinion, minionPositions[i]));
            }

            _canSpawn = false; // stop summoning
        }

        /// <summary>
        /// Resets ability to spawn ReaperMinions 
        /// </summary>
        private void HandleSummonMinionLogic()
        {
            if (_canSpawn) return;

            _spawnTime += GameWorld.DeltaTime;

            if (_spawnTime > _spawnCooldown)
            {
                _canSpawn = true;
                _spawnTime = 0;
            }
        }

        /// <summary>
        /// Creates an array holding 4 positions around reaper object
        /// </summary>
        private void CreateMinionPositions()
        {
            minionPositions = new Vector2[]
            {
                new Vector2(position.X + 75, position.Y),
                new Vector2(position.X, position.Y + 75),
                new Vector2(position.X-75, position.Y),
                new Vector2(position.X, position.Y - 75)
            };
        }

        #endregion

        /// <summary>
        /// Handles damage to player logic 
        /// </summary>
        public override void Attack()
        {
            //add attack event
        }

    }
}
