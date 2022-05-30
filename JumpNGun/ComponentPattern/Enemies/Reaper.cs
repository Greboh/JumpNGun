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
        private List<Rectangle> spawnLocations = new List<Rectangle>();
        private int _miniomAmount = 1;
        private bool _canSpawn;
        private bool _canTeleport;
        private bool _teleportFinished;
        private bool _isSpawning;

        private Random _rand = new Random();

        private float _spawnTime;
        private float _spawnCooldown = 5;
        private float _teleportTime;
        private float _teleportCooldown = 5;
        private float originalSpeed;


        public Reaper()
        {
            this.position = new Vector2(662, 400);
            health = 100;
            damage = 20;
            speed = 0.7f;
            originalSpeed = speed;
        }
        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            GetLocations();
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            ChasePlayer();
            HandleAnimations();
            CheckCollision();
            //SpawnGhosts();
            //HandleGhostSpawnLogic();
            Teleport();
            HandleTeleportLogic();
            base.Update(gameTime);
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
            //TODO refactor this
            if (!canAttack && !_canTeleport) animator.PlayAnimation("reaper_idle");
            else if (canAttack && !_canTeleport) animator.PlayAnimation("reaper_attack");
            else if (_canTeleport && !_teleportFinished)
            {
                animator.PlayAnimation("reaper_death");
            }
            else if (_teleportFinished && _canTeleport)
            {
                animator.PlayAnimation("reaper_spawn");
                if (animator.IsAnimationDone)
                {
                    _canTeleport = false;
                    _teleportFinished = false;
                    speed = originalSpeed;
                }
            }
        }

        #region ABILITIES
        private void Teleport()
        {
            if (!_canTeleport || !CalculateDistanceToEnemy()) return;

            speed = 0;

            if (animator.IsAnimationDone)
            {
                GameObject.Transform.Position = new Vector2(player.Position.X + _rand.Next(-100, 100), player.Position.Y);
                _teleportFinished = true;
            }
        }

        private bool CalculateDistanceToEnemy()
        {
            if (this.position.X - player.Position.X > 150 || this.position.X - player.Position.X < -150)
            {
                return true;
            }
            if (this.position.Y - player.Position.Y > 150 || this.position.Y - player.Position.Y < -150)
            {
                return true;
            }
            else return false;
        }

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


        private void SpawnGhosts()
        {
            if (!_canSpawn) return;

            speed = 0;


            for (int i = 0; i < _miniomAmount; i++)
            {
                GameWorld.Instance.Instantiate(EnemyFactory.Instance.Create(EnemyType.ReaperMinion, this.position));
            }

            if (animator.IsAnimationDone)
            {
                speed = originalSpeed;
            }

            _canSpawn = false;
        }

        private void HandleGhostSpawnLogic()
        {
            if (_canSpawn) return;

            _spawnTime += GameWorld.DeltaTime;

            if (_spawnTime > _spawnCooldown)
            {
                _canSpawn = true;
                _spawnTime = 0;
            }

        }



        #endregion

        /// <summary>
        /// Get all rectangles that contain platforms
        /// </summary>
        private void GetLocations()
        {
            for (int i = 0; i < LevelManager.Instance.UsedLocations.Count; i++)
            {
                spawnLocations.Add(LevelManager.Instance.UsedLocations[i]);
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
