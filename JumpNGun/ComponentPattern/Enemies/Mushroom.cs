using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace JumpNGun
{
    class Mushroom : Enemy
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


        public Mushroom(Vector2 position)
        {
            int rndSpeed = rnd.Next(20, 31);
            
            spawnPosition = position;
            health = 20;
            Speed = rndSpeed;
            Damage = 20;
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
        /// //LAVET AF KRISTIAN J. FICH 
        /// </summary>
        private void CalculateMovementArea()
        {
            //return if object isn't grounded
            if (!_isGrounded) return;

            //set PlatformRectangle equal to the rectangle in _locations that contain this objects position
            foreach (Tile tile in Map.Instance.TileMap)
            {
                if (tile.Location.Contains(GameObject.Transform.Position) && !_locationRectangleFound)
                {
                    PlatformRectangle = tile.Location;
                    
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
            for (int i = 0; i < Map.Instance.TileMap.Count; i++)
            {
                if (Map.Instance.TileMap[i].HasPlatform)
                {
                    //if any rectangles in _locations allign horizontally and right next to each other, make a combined rectangle of the respective rectangles 
                    if (PlatformRectangle.Right == Map.Instance.TileMap[i].Location.Left && PlatformRectangle.Y == Map.Instance.TileMap[i].Location.Y)
                    {
                        //set Platformrectangle equal to new rectangle 
                        PlatformRectangle = Rectangle.Union(PlatformRectangle, Map.Instance.TileMap[i].Location);
                    }
                    if (PlatformRectangle.Left == Map.Instance.TileMap[i].Location.Right && PlatformRectangle.Y == Map.Instance.TileMap[i].Location.Y)
                    {
                        //set Platformrectangle equal to new rectangle 
                        PlatformRectangle = Rectangle.Union(PlatformRectangle, Map.Instance.TileMap[i].Location);
                    }
                }
            }

        }
        
        /// <summary>
        /// Get relevant list of locations from levelmanager
        /// LAVET AF KRISTIAN J. FICH 
        /// </summary>
        private void GetAllRectangleLocations()
        {
            for (int i = 0; i < Map.Instance.TileMap.Count; i++)
            {
                if (Map.Instance.TileMap[i].HasPlatform)
                {
                    _locations.Add(Map.Instance.TileMap[i].Location);
                }
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
        /// //LAVET AF NICHLAS HOBERG
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
        /// Check collision with platforms to deploy gravity
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
    }
}
