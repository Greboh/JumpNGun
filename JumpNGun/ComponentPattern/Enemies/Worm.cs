using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    class Worm : Enemy
    {
        //TODO fix worm Sprites and Animations - KRISTIAN
        
        private float _gravityPull; // How strong the force of gravity is
        private int _gravityMultiplier = 100; // Used to multiply the gravity over time making it stronger
        private bool _isGrounded = false; //whether object is on ground or falling
        private List<Rectangle> locations = new List<Rectangle>();
        private Rectangle _groundCollision = Rectangle.Empty;
        private Rectangle _currentRectangle = Rectangle.Empty;
        private bool _locationFound;
        private float _projectileSpeed = 150;
        private bool _canShoot = true;
        private float _shootTime;
        private float _shootCooldown = 1f;

        public Worm(Vector2 position)
        {
            this.position = position;
            health = 50;
            speed = 50;
        }

        public override void Awake()
        {
            base.Awake();
            GameObject.Transform.Position = position;
        }

        public override void Start()
        {
            GetLocations();
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            SetLocationRectangle();
            CreateMovementArea();
            HandleGravity();
            SetVelocity();
            CheckCollision();
            HandleShootLogic();
            HandleAnimations();
            Attack();
            base.Update(gameTime);
        }

        #region Movement Methods

        /// <summary>
        /// Set _currentRectangle to rectangle within position lies. 
        /// </summary>
        private void SetLocationRectangle()
        {
            if (!_isGrounded) return;

            foreach (Rectangle location in locations)
            {
                if (location.Contains(position) && !_locationFound)
                {
                    Console.WriteLine("location found");
                    _currentRectangle = location;
                    _locationFound = true;
                }
            }
        }

        /// <summary>
        /// Sets bounderies for object movement according to platform amount around _currentRectangle
        /// </summary>
        private void CreateMovementArea()
        {
            //TODO - make an algorithm that run once. - KRISTIAN
            for (int i = 0; i < locations.Count; i++)
            {
                if (_currentRectangle.Right == locations[i].Left && _currentRectangle.Y == locations[i].Y)
                {
                    _currentRectangle = Rectangle.Union(_currentRectangle, locations[i]);
                }
                if (_currentRectangle.Left == locations[i].Right && _currentRectangle.Y == locations[i].Y)
                {
                    _currentRectangle = Rectangle.Union(_currentRectangle, locations[i]);
                }
            }
        }

        /// <summary>
        /// Set direction of movement according to position on platform
        /// </summary>
        private void SetVelocity()
        {
            //if position is close to right, move left
            if (position.X >= _currentRectangle.Right - sr.Sprite.Width)
            {
                velocity = new Vector2(-1, 0);
            }
            //if position is close to left, move right
            if (position.X <= _currentRectangle.Left + sr.Sprite.Width)
            {
                velocity = new Vector2(1, 0);
            }
        }

        /// <summary>
        /// Get all rectangles that contain platforms
        /// </summary>
        private void GetLocations()
        {
            for (int i = 0; i < LevelManager.Instance.UsedLocations.Count; i++)
            {
                locations.Add(LevelManager.Instance.UsedLocations[i]);
            }
        }
        #endregion

        /// <summary>
        /// Creates gravity making sure the object falls unless grounded
        /// </summary>
        private void HandleGravity()
        {
            if (_isGrounded) return;

            // Makes the gravity stronger over time, creating a feeling of a pull
            _gravityPull += GameWorld.DeltaTime * _gravityMultiplier;

            Vector2 fallDirection = new Vector2(0, 1);

            // Multiply fallDirection with our gravityPull
            fallDirection *= _gravityPull;

            GameObject.Transform.Translate(fallDirection * GameWorld.DeltaTime);
        }
        
        /// <summary>
        /// Check collision with ground to deploy gravity
        /// </summary>
        public override void CheckCollision()
        {
            foreach (Collider col in GameWorld.Instance.Colliders)
            {
                if (col.GameObject.HasComponent<Platform>())
                {
                    if (col.CollisionBox.Intersects(collider.CollisionBox))
                    {
                        _isGrounded = true;
                        _groundCollision = col.CollisionBox;
                    }
                }
                if (_isGrounded && !collider.CollisionBox.Intersects(_groundCollision))
                {
                    _isGrounded = false;
                }
            }
        }

        /// <summary>
        /// Initialize attack if player is in field of view
        /// </summary>
        public override void Attack()
        {
            Collider playerCol = (player.GameObject.GetComponent<Collider>() as Collider);

            if (playerCol.CollisionBox.Intersects(_currentRectangle) && playerCol.CollisionBox.Bottom < _currentRectangle.Center.Y)
            {
                Shoot();
                canAttack = true;
            }
            else canAttack = false;
        }

        /// <summary>
        /// Reset ability to shoot after cooldown
        /// </summary>
        private void HandleShootLogic()
        {
            if (_canShoot) return;

            _shootTime += GameWorld.DeltaTime;

            if (_shootTime > _shootCooldown)
            {
                _canShoot = true;
                _shootTime = 0;
            }
        }

        /// <summary>
        /// Logic for shooting and instantiating projectile
        /// </summary>
        private void Shoot()
        {
            if (!_canShoot) return;

            GameObject projectile = ProjectileFactory.Instance.Create(EnemyType.Worm);

            projectile.Transform.Position = GameObject.Transform.Position;

            if (player.Position.X < position.X)
            {
                ((Projectile)projectile.GetComponent<Projectile>()).Velocity = new Vector2(-1, 0);
            }
            else
            {
                ((Projectile)projectile.GetComponent<Projectile>()).Velocity = new Vector2(1, 0);
            }
            Console.WriteLine("Shot fired");
            ((Projectile)projectile.GetComponent<Projectile>()).Speed = _projectileSpeed;
            GameWorld.Instance.Instantiate(projectile);
            _canShoot = false;
        }

        public override void HandleAnimations()
        {
            if (!canAttack && health > 0) animator.PlayAnimation("worm_walk");
            if (canAttack && health > 0) animator.PlayAnimation("worm_attack");
            if (health <= 0)
            {
                speed = 0;
                animator.PlayAnimation("worm_death");
            }
        }
    }
}
