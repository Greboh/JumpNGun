using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace JumpNGun
{
    class Skeleton : Enemy
    {
        // How strong the force of gravity is
        private float _gravityPull;

        // Used to multiply the gravity over time making it stronger
        private int _gravityMultiplier = 100;

        //determines whether object is grounded or not
        private bool _isGrounded;

        //list of valid locations containing platforms
        private List<Rectangle> _locations = new List<Rectangle>();

        //rectangle to contain groundcollision
        private Rectangle _groundCollision = Rectangle.Empty;

        //used to assert that we have found/not found the initial rectangle containg position
        private bool _locationRectangleFound;

        //variable to hold initial speed of object 
        private float _originalSpeed;

        public Skeleton(Vector2 position)
        {
            int rndSpeed = rnd.Next(40, 51);

            spawnPosition = position;
            health = 100;
            Damage = 20;
            Speed = rndSpeed;
            AttackCooldown = 1.2f;
            _originalSpeed = Speed;
            IsBoss = false;
            IsRanged = false;
            
        }

        public override void Start()
        {
            base.Start();
            detectionRange = SpriteRenderer.Sprite.Width;
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
            ChasePlayer();
        }

        #region Movement Methods

        /// <summary>
        /// Find and reference the rectangle containing mushroom object position
        /// //LAVET AF KRISTIAN J. FICH 
        /// </summary>
        private void CalculateMovementArea()
        {
            //return if object isn't grounded
            if (!_isGrounded) return;

            //set PlatformRectangle equal to the rectangle in _locations that contain this objects position
            foreach (Rectangle location in _locations)
            {
                if (location.Contains(GameObject.Transform.Position) && !_locationRectangleFound)
                {
                    PlatformRectangle = location;

                    //ensure that we only find location once
                    _locationRectangleFound = true;
                }
            }
        }

        /// <summary>
        /// Create a rectangle that consists of immidiate alligned rectangles
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        private void CreateMovementArea()
        {
            //loop through _locations 
            for (int i = 0; i < _locations.Count; i++)
            {
                //if any rectangles in _locations allign horizontally and right next to each other, make a combined rectangle of the respective rectangles 
                if (PlatformRectangle.Right == _locations[i].Left && PlatformRectangle.Y == _locations[i].Y)
                {
                    //set Platformrectangle equal to new rectangle 
                    PlatformRectangle = Rectangle.Union(PlatformRectangle, _locations[i]);
                }
                if (PlatformRectangle.Left == _locations[i].Right && PlatformRectangle.Y == _locations[i].Y)
                {
                    //set Platformrectangle equal to new rectangle 
                    PlatformRectangle = Rectangle.Union(PlatformRectangle, _locations[i]);
                }
            }
        }

        /// <summary>
        /// Get relevant list of locations from levelmanager
        /// LAVET AF KRISTIAN J. FICH 
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
        /// //LAVET AF NICHLAS HOBERG
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
        /// LAVET AF NICHLAS HOBERG 
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
        /// LAVET AF KRISTIAN J. FICH 
        /// </summary>
        protected override void CheckCollision()
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

        /// <summary>
        /// Change veloctiy of skeleton object if conditions are met 
        /// //LAVET AF KRISTIAN J. FICH 
        /// </summary>
        protected void ChasePlayer()
        {
            //return if currentEnemyState isn't moveEnemyState
            if (currentEnemyState != moveEnemyState) return;

            //get Collider component of player.Gameobject
            Collider playerCol = (Player.GameObject.GetComponent<Collider>() as Collider);


            //If Collisionbox of player.gameobject is contained withing Skeleton rectangle move towards player gameobject's position 
            if (playerCol.CollisionBox.Intersects(PlatformRectangle) && playerCol.CollisionBox.Bottom < PlatformRectangle.Center.Y)
            {
                //increase speed
                Speed = rnd.Next(80, 120);

                if (Player.GameObject.Transform.Position.X < this.GameObject.Transform.Position.X)
                {
                    Velocity = new Vector2(-1, 0);
                }
                else if (Player.GameObject.Transform.Position.X > this.GameObject.Transform.Position.X)
                {

                    Velocity = new Vector2(1, 0);
                }
            }
            //set speed back to original speed 
            else Speed = _originalSpeed;
        }

    }
}
