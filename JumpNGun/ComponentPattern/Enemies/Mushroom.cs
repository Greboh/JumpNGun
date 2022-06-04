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

        private List<Rectangle> _locations = new List<Rectangle>();
        private Rectangle _groundCollision = Rectangle.Empty;
        private bool _locationRectangleFound;


        public Mushroom(Vector2 position)
        {
            spawnPosition = position;
            health = 20;
            Speed = 40;
            damage = 20;
            ProjectileSpeed = 1;
            AttackCooldown = 1;
            detectionRange = 350;
            IsRanged = true;
        }

        public override void Start()
        {
            base.Start();

            GameObject.Transform.Position = spawnPosition;
            
            GetAllRectangleLocations();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            CalculateMovementArea();
            CreateMovementArea();

            HandleGravity();
            CheckCollision();
            CalculateAttack();
        }

        #region Movement Methods

        /// <summary>
        /// Find and reference the rectangle containing mushroom object position
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
        /// Create a rectangle that consist of all rectangles containing platforms and are alligned
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
        /// Get all rectangles that contain a platform
        /// </summary>
        private void GetAllRectangleLocations()
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
            
            ChangeState(targetMagnitude <= detectionRange ? attackState : moveState);
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
            }
        }
    }
}
