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
        private bool _isGrounded; //whether object is on ground or falling
        
        private List<Rectangle> _locations = new List<Rectangle>();
        private Rectangle _groundCollision = Rectangle.Empty;

        private bool _locationRectangleFound;


        public Worm(Vector2 position)
        {
            int rndSpeed = rnd.Next(40, 51);
            
            spawnPosition = position;
            health = 40;
            Speed = rndSpeed;
            Damage = 20;
            ProjectileSpeed = 0.5f;
            AttackCooldown = 0.75f;
            IsRanged = true;
            IsBoss = false;
            detectionRange = 250;
        }

        public override void Awake()
        {
            base.Awake();

        }

        public override void Start()
        {
            base.Start();
            
            GameObject.Transform.Position = spawnPosition;

            GetLocations();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            CalculateMovementArea();
            CreateMovementArea();

            CalculateAttack();
            HandleGravity();
            CheckCollision();
        }

        #region Movement Methods

        /// <summary>
        /// Set _currentRectangle to rectangle within position lies. 
        /// </summary>
        private void CalculateMovementArea()
        {
            if (!_isGrounded) return;

            foreach (Rectangle location in _locations)
            {
                if (location.Contains(GameObject.Transform.Position) && !_locationRectangleFound)
                {
                    PlatformRectangle = location;
                    _locationRectangleFound = true;
                }
            }
        }

        /// <summary>
        /// Sets bounderies for object movement according to platform amount around _currentRectangle
        /// </summary>
        private void CreateMovementArea()
        {
            for (int i = 0; i < _locations.Count; i++)
            {
                if (PlatformRectangle.Right == _locations[i].Left && PlatformRectangle.Y == _locations[i].Y)
                {
                    PlatformRectangle = Rectangle.Union(PlatformRectangle, _locations[i]);
                }
                if (PlatformRectangle.Left == _locations[i].Right && PlatformRectangle.Y == _locations[i].Y)
                {
                    PlatformRectangle = Rectangle.Union(PlatformRectangle, _locations[i]);
                }
            }
        }

        /// <summary>
        /// Get all rectangles that contain platforms
        /// </summary>
        private void GetLocations()
        {
            for (int i = 0; i < LevelManager.Instance.UsedLocations.Count; i++)
            {
                _locations.Add(LevelManager.Instance.UsedLocations[i]);
            }
        }

        #endregion
        
        /// <summary>
        /// Calculate if we are in attack range and should change state
        /// </summary>
        private void CalculateAttack()
        {
            Vector2 target = GameObject.Transform.Position - Player.GameObject.Transform.Position;

            // Find the length of the target Vector2
            // The equation for finding a vectors magnitude is: (x * x + y * y)
            
            float targetMagnitude = MathF.Sqrt(target.X * target.X + target.Y * target.Y);
            
            ChangeState(targetMagnitude <= detectionRange ? attackEnemyState : moveEnemyState);
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
        /// Check collision with ground to deploy gravity
        /// </summary>
        public override void CheckCollision()
        {
            foreach (Collider col in GameWorld.Instance.Colliders)
            {
                if (col.GameObject.Tag == "platform" && col.CollisionBox.Intersects(Collider.CollisionBox))
                {
                    _isGrounded = true;
                    _groundCollision = col.CollisionBox;
                }
                if (_isGrounded && !Collider.CollisionBox.Intersects(_groundCollision)) _isGrounded = false;
            }
        }
    }
}