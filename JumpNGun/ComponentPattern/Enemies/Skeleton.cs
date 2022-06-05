using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    class Skeleton : Enemy
    {
        private float _gravityPull; // How strong the force of gravity is
        private int _gravityMultiplier = 100; // Used to multiply the gravity over time making it stronger
        private bool _isGrounded = false;
        private List<Rectangle> locations = new List<Rectangle>();
        private Rectangle _groundCollision = Rectangle.Empty;
        private Rectangle _currentRectangle = Rectangle.Empty;
        private bool _locationFound;

        private float _originalSpeed;

        public Skeleton(Vector2 position)
        {
            int rndSpeed = rnd.Next(40, 51);
            
            GameObject.Transform.Position = position;
            health = 100;
            Damage = 20;
            Speed = rndSpeed;
            _originalSpeed = Speed;
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
            SetLocationRectangle();
            CreateMovementArea();
            SetVelocity();
            HandleGravity();
            CheckCollision();
            ChasePlayer();
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
                if (location.Contains(this.GameObject.Transform.Position) && !_locationFound)
                {
                    Console.WriteLine("location found");
                    _currentRectangle = location;
                    _locationFound = true;
                }
            }
        }

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
        private void SetVelocity()
        {
            //if position is close to right, move left
            if (this.GameObject.Transform.Position.X >= _currentRectangle.Right - SpriteRenderer.Sprite.Width)
            {
                Velocity = new Vector2(-1, 0);
            }
            //if position is close to left, move right
            if (this.GameObject.Transform.Position.X <= _currentRectangle.Left + SpriteRenderer.Sprite.Width)
            {
                Velocity = new Vector2(1, 0);
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
        /// Check if object collides with player or ground
        /// </summary>
        public override void CheckCollision()
        {
            foreach (Collider col in GameWorld.Instance.Colliders)
            {
                if (col.GameObject.Tag == "Platform" && col.CollisionBox.Intersects(Collider.CollisionBox))
                {
                    _isGrounded = true;
                    _groundCollision = col.CollisionBox;
                }
                if (_isGrounded && !Collider.CollisionBox.Intersects(_groundCollision)) _isGrounded = false;

                if (col.GameObject.Tag == "player" && col.CollisionBox.Intersects(Collider.CollisionBox))
                {
                    // Attack();
                }
                // else if (col.GameObject.Tag == "player" && !col.CollisionBox.Intersects(collider.CollisionBox)) canAttack = false;
            }
        }

        protected override void ChasePlayer()
        {
            Collider playerCol = (Player.GameObject.GetComponent<Collider>() as Collider);

            if (playerCol.CollisionBox.Intersects(_currentRectangle) && playerCol.CollisionBox.Bottom < _currentRectangle.Center.Y)
            {
                Speed = 100;

                if (Player.Position.X < this.GameObject.Transform.Position.X)
                {
                    Velocity = new Vector2(-1, 0);
                }
                else if (Player.Position.X > this.GameObject.Transform.Position.X)
                {

                    Velocity = new Vector2(1, 0);
                }
            }
            else Speed = _originalSpeed;
        }
    }
}
