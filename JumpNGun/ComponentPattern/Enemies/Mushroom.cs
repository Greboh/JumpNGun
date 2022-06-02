using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JumpNGun
{
    class Mushroom : Enemy
    {
        //TODO - Add comments for methods and fields - KRISTIAN

        private float _gravityPull; // How strong the force of gravity is
        private int _gravityMultiplier = 100; // Used to multiply the gravity over time making it stronger
        private bool _isGrounded = false;

        private List<Rectangle> locations = new List<Rectangle>();
        private Rectangle _groundCollision = Rectangle.Empty;
        private Rectangle _currentRectangle = Rectangle.Empty;
        private bool _locationMade;
        private bool _locationRectangleFound;
        private bool _canShoot = true;
        private float _projectileSpeed = 150;
        private float _shootTime;
        private float _shootCooldown = 1f;
        

        public Mushroom(Vector2 position)
        {
            this.position = position;
            health = 20;
            speed = 40;
            damage = 20;
        }

        public override void Awake()
        {
            base.Awake();
            GameObject.Transform.Position = position;
        }

        public override void Start()
        {
            base.Start();
            GetLocations();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            SetLocationRectangle();
            CreateMovementArea();
            CalculateMovementDirection();
            
            
            HandleGravity();
            CheckCollision();
            HandleAnimations();
            HandleShootLogic();
            Attack();
        }

        #region Movement Methods

        /// <summary>
        /// Find and reference the rectangle containing mushroom object position
        /// </summary>
        private void SetLocationRectangle()
        {
            if (!_isGrounded) return;

            foreach (Rectangle location in locations)
            {
                if (location.Contains(position) && !_locationRectangleFound)
                {
                    _currentRectangle = location;
                    _locationRectangleFound = true;
                }
            }
        }

        /// <summary>
        /// Create a rectangle that consist of all rectangles containing platforms and are alligned
        /// </summary>
        private void CreateMovementArea()
        {
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
        private void CalculateMovementDirection()
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
        /// Get all rectangles that contain a platform
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
        /// Attacks by calling shoot method and changing relevant bool
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
        /// Play relevant animation
        /// </summary>
        public override void HandleAnimations()
        {
            if (health <= 0)
            {
                canAttack = false;
                isDead = true;
                speed = 0;
                //TODO - make enemy stop shooting when death animation playing - KRISTIAN
                animator.PlayAnimation("mushroom_death");
            }
            
            if(isDead) return;

            string currentAnimationName = canAttack ? "mushroom_attack" : "mushroom_run";
            
            animator.PlayAnimation(currentAnimationName);
            
            if (!canAttack && health >0) animator.PlayAnimation("mushroom_run");
            if (canAttack && health >0) animator.PlayAnimation("mushroom_attack");

        }

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
        /// Checks for relevant collision with this object
        /// </summary>
        public override void CheckCollision()
        {
            foreach (Collider col in GameWorld.Instance.Colliders)
            {
                if (col.GameObject.Tag == "platform" && col.CollisionBox.Intersects(collider.CollisionBox))
                {
                    _isGrounded = true;
                    _groundCollision = col.CollisionBox;
                }
                if (_isGrounded && !collider.CollisionBox.Intersects(_groundCollision))
                {
                    _isGrounded = false;
                }
                
                if(col.GameObject.Tag == "p_Projectile" && col.CollisionBox.Intersects(collider.CollisionBox))
                {
                    health -= 20;
                    GameWorld.Instance.Destroy(col.GameObject);
                }
            }
        }

        /// <summary>
        /// Instantiate relevant projectile with velocity set according to player position
        /// </summary>
        private void Shoot()
        {
            if (!_canShoot || isDead) return;

            GameObject projectile = ProjectileFactory.Instance.Create(EnemyType.Mushroom);

            projectile.Transform.Position = GameObject.Transform.Position;

            if (player.Position.X < position.X)
            {
                ((Projectile)projectile.GetComponent<Projectile>()).Velocity = new Vector2(-1, 0);
            }
            else
            {
                ((Projectile)projectile.GetComponent<Projectile>()).Velocity = new Vector2(1, 0);
            }

            ((Projectile)projectile.GetComponent<Projectile>()).Speed = _projectileSpeed;
            GameWorld.Instance.Instantiate(projectile);
            _canShoot = false;
        }

      

    }
}
