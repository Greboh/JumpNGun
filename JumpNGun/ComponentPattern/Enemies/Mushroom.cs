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
        private bool _locationRectangleFound;
        

        public Mushroom(Vector2 position)
        {
            this.position = position;
            health = 20;
            speed = 40;
            damage = 20;
            ProjectileSpeed = 1;
            AttackCooldown = 1;
  
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

            HandleGravity();
            CheckCollision();
            HandleAnimations();
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
            for (int i = 0; i < locations.Count; i++)
            {
                if (PlatformRectangle.Right == locations[i].Left && PlatformRectangle.Y == locations[i].Y)
                {
                    PlatformRectangle = Rectangle.Union(PlatformRectangle, locations[i]);
                }
                if (PlatformRectangle.Left == locations[i].Right && PlatformRectangle.Y == locations[i].Y)
                {
                    PlatformRectangle = Rectangle.Union(PlatformRectangle, locations[i]);
                }
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
            Vector2 target = GameObject.Transform.Position - Player.GameObject.Transform.Position;

            float maxDistance = 350;

            // Finding the length of the vectors
            // The formal for finding a vectors magnitude / length is: (x * x + y * y)
            
            float targetMagnitude = MathF.Sqrt(target.X * target.X + target.Y * target.Y);


            if (targetMagnitude <= maxDistance)
            {
                ChangeState(attackState);
            }
            else
            {
                ChangeState(moveState);
            } 
        }

        /// <summary>
        /// Play relevant animation
        /// </summary>
        public override void HandleAnimations()
        {
            if (health <= 0)
            {
                isDead = true;
                speed = 0;
                //TODO - make enemy stop shooting when death animation playing - KRISTIAN
                Animator.PlayAnimation("mushroom_death");
            }
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
                
                if(col.GameObject.Tag == "p_Projectile" && col.CollisionBox.Intersects(collider.CollisionBox))
                {
                    health -= 20;
                    GameWorld.Instance.Destroy(col.GameObject);
                }
            }
        }

        

      

    }
}
