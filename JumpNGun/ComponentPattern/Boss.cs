using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    class Boss : Component
    {
        private int _health;
        private int _damage;
        private float _speed;

        private bool _isColliding = false;

        private Vector2 _velocity;
        private Vector2 _position;

        private EnemyType _currentBossType;

        #region Components
        private SpriteRenderer _sr;
        private Animator _animator;
        private Player _target;
        private Boss _source;
        private Collider _bossCollider;
        #endregion

        public Boss(EnemyType bossType, Vector2 position)
        {
            _position = position;

            switch (bossType)
            {
                case EnemyType.DeathBoss:
                    {
                        _currentBossType = bossType;
                        _health = 100;
                        _damage = 20;
                        _speed = 40;
                    }
                    break;
                case EnemyType.SandBoss:
                    {
                        _currentBossType = bossType;
                        _health = 150;
                        _damage = 15;
                        _speed = 30;
                    }
                    break;
            }
        }

        public override void Awake()
        {
            GameObject.Transform.Position = _position;
            _bossCollider = GameObject.GetComponent<Collider>() as Collider;
            _sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            _animator = GameObject.GetComponent<Animator>() as Animator;
            _source = GameObject.GetComponent<Boss>() as Boss;
        }

        public override void Start()
        {
            FindPlayerObject();
        }

        public override void Update(GameTime gameTime)
        {
            UpdatePositionReference();
            Chase();
            HandleAnimations();
            FlipSprite();
            CheckCollision();
            Move();
        }

        /// <summary>
        /// Make boss source chase player target
        /// </summary>
        /// <param name="source">the object location traveling from</param>
        /// <param name="target">the object that will chased </param>
        private void Chase()
        {
            Vector2 sourceToTarget = Vector2.Subtract(_target.Position, _source._position);
            sourceToTarget.Normalize();
            sourceToTarget = Vector2.Multiply(sourceToTarget, _source._speed);

            _source._velocity = sourceToTarget;
        }

        /// <summary>
        /// Starts movement of boss
        /// </summary>
        private void Move()
        {
            GameObject.Transform.Translate(_velocity * GameWorld.DeltaTime);
        }

        /// <summary>
        /// Find player and Boss and set _source and _target reference accordingly
        /// </summary>
        /// <returns></returns>
        public void FindPlayerObject()
        {
            foreach (GameObject go in GameWorld.Instance.GameObjects)
            {
                if (go.HasComponent<Player>())
                {
                    _target = go.GetComponent<Player>() as Player;
                }
            }
        }

        /// <summary>
        /// Updates _position to current position during gametime
        /// </summary>
        private void UpdatePositionReference()
        {
            _position = GameObject.Transform.Position;
        }

        /// <summary>
        /// Handles and plays animations of all BossTypes 
        /// </summary>
        private void HandleAnimations()
        {
            switch (_currentBossType)
            {
                case EnemyType.DeathBoss:
                    {
                        if (!_isColliding)
                        {
                            _animator.PlayAnimation("reaper_idle");
                        }
                        if (_isColliding)
                        {
                            _animator.PlayAnimation("reaper_attack");
                        }
                    }
                    break;
                case EnemyType.GrassBoss:
                    {
                        if (!_isColliding)
                        {
                            _animator.PlayAnimation("golem_idle");
                        }
                        if (_isColliding)
                        {
                            _animator.PlayAnimation("reaper_attack");
                        }
                    }break;
            }
        }


        private void SpawnGhostminons()
        {
            //TODO - Spawn GhostMinions after gametime has passed - KRISTIAN

        }


        private void Attack(Player player)
        {
            //TODO add attack logic - KRISTIAN
        }

        /// <summary>
        /// Checks for collision with player and calls Attack method
        /// </summary>
        private void CheckCollision()
        {
            foreach (Collider col in GameWorld.Instance.Colliders)
            {
                if (col.GameObject.Tag == "Player" && _bossCollider.CollisionBox.Intersects(col.CollisionBox))
                {
                    _isColliding = true;
                    Attack(col.GameObject.GetComponent<Player>() as Player);
                }
                if (col.GameObject.Tag == "Player" && !_bossCollider.CollisionBox.Intersects(col.CollisionBox))
                {
                    _isColliding = false;
                }
            }
        }

        /// <summary>
        /// Flip sprite according to moving direction
        /// </summary>
        private void FlipSprite()
        {
            // If we are moving left, flip the sprite
            if (_velocity.X < 0)
            {
                _sr.SpriteEffects = SpriteEffects.FlipHorizontally;
            }
            // If we are moving right, unflip the sprite
            else if (_velocity.X > 0)
            {
                _sr.SpriteEffects = SpriteEffects.None;
            }
        }
    }
}
