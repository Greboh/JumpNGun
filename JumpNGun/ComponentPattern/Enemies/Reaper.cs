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
        private float _teleportCooldown = 5;
        private Vector2[] minionPositions;
        private IState _currentState;

        public float OriginalSpeed { get; private set; }

        public Reaper()
        {
            this.position = new Vector2(662, 400);
            health = 200;
            damage = 20;
            Speed = 0.7f;
            OriginalSpeed = Speed;
        }
        public override void Awake()
        {
            
            base.Awake();
        }

        public override void Start()
        {
            GameObject.Transform.Position = position;
            ChangeState(new MoveState());
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            _currentState.Execute(gameTime);
            ChasePlayer();
            HandleAnimations();
            CheckCollision();
            Teleport();
            HandleTeleportLogic();
            IncreaseDifficulty();
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


        /// <summary>
        /// Handle all relevant animation for Reaper
        /// </summary>
        public override void HandleAnimations()
        {
            //if (!isAttacking) animator.PlayAnimation("reaper_idle");
            //if (health <= 0) animator.PlayAnimation("reaper_death");
            //if (isAttacking) animator.PlayAnimation("reaper_attack");
        }

        #region ABILITIES


        /// <summary>
        /// Teleports Reaper close to player object if conditions are met
        /// </summary>
        private void Teleport()
        {
            if (!_canTeleport || !CalculateDistanceToEnemy() && health >= 50) return;
            
            ChangeState(new TeleportState(new Vector2 (player.Position.X + sr.Sprite.Width, player.Position.Y)));

            _canTeleport = false;
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
        private void SummonMinions()
        {
            if (!_canSpawn) return;


            _spawningFinished = true;

            CreateMinionPositions();

            for (int i = 0; i < _miniomAmount; i++)
            {
                GameWorld.Instance.Instantiate(EnemyFactory.Instance.Create(EnemyType.ReaperMinion, minionPositions[i]));
            }

            _canSpawn = false;
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
        public void ChangeState(IState newState)
        {
            _currentState = newState;
            _currentState.Enter(this);
        }

        //TODO - TEST THIS - Kristian
        private void IncreaseDifficulty()
        {
            switch (health)
            {
                case 250:
                    {
                        _miniomAmount = 2;
                    }
                    break;
                case 200:
                    {
                        _miniomAmount = 3;
                    }
                    break;
                case 150:
                    {
                        _miniomAmount = 4;
                    }break;
                case 50:
                    {
                        _spawnCooldown = 2;
                        _canTeleport = false;
                    }break;
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
