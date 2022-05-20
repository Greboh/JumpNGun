using Microsoft.Xna.Framework;
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

        private Vector2 _velocity;
        private Vector2 _position;

        private SpriteRenderer _sr;
        private Animator _animator;
        private Player _target;
        private Boss _source;


        public Boss(BossType bossType, Vector2 position)
        {
            _position = position;

            switch (bossType)
            {
                case BossType.DeathBoss:
                    {
                        _health = 100;
                        _damage = 20;
                        _speed = 50;
                        
                    }
                    break;
                case BossType.SandBoss:
                    {

                    }
                    break;

            }
        }

        public override void Awake()
        {
            GameObject.Transform.Position = _position;
        }

        public override void Start()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            UpdatePositionReference();
            Chase(FindPlayer().Item1, FindPlayer().Item2);
            Move();
        }

        /// <summary>
        /// Make boss source chase player target
        /// </summary>
        /// <param name="source">the object location traveling from</param>
        /// <param name="target">the object that will chased </param>
        private void Chase(Boss source, Player target)
        {
            Vector2 sourceToTarget = Vector2.Subtract(target.Position, source._position);
            sourceToTarget.Normalize();
            sourceToTarget = Vector2.Multiply(sourceToTarget, source._speed);

            source._velocity = sourceToTarget;
        }

        /// <summary>
        /// Starts 
        /// </summary>
        private void Move()
        {
            GameObject.Transform.Translate(_velocity * GameWorld.DeltaTime);
        }

        /// <summary>
        /// Find player and Boss and set _source and _target reference accordingly
        /// </summary>
        /// <returns></returns>
        private Tuple<Boss, Player> FindPlayer()
        {
            foreach (GameObject go in GameWorld.Instance.GameObjects)
            {
                if (go.HasComponent<Player>())
                {
                    _target = go.GetComponent<Player>() as Player;
                }
                if (go.HasComponent<Boss>())
                {
                    _source = go.GetComponent<Boss>() as Boss;
                }
            }

            return Tuple.Create(_source, _target);
        }

        /// <summary>
        /// Updates _position to current position during gametime
        /// </summary>
        private void UpdatePositionReference()
        {
            _position = GameObject.Transform.Position;
        }
    }
}
