using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    class Mushroom : Enemy
    {
        //TODO Refactor this whole mess - KRISTIAN;

        private float _gravityPull; // How strong the force of gravity is
        private int _gravityMultiplier = 100; // Used to multiply the gravity over time making it stronger
        private bool _isGrounded = false;

        private List<Rectangle> locations = new List<Rectangle>();
        private Rectangle _groundCollision = Rectangle.Empty;
        private Rectangle _currentRectangle = Rectangle.Empty;
        private List<Rectangle> _fieldOfView = new List<Rectangle>();

        private bool _locationRectangleFound;
        private bool _collidingWithPlayer;
        private bool _canRangeAttack;
        private bool _isAttacking;
        private int rangeDammage;

        public Mushroom(Vector2 position)
        {
            this.position = position;
            health = 60;
            speed = 40;
            damage = 20;
            rangeDammage = 15;
            //TODO - Maybe make speed an overload for constructor - KRISTIAN
        }

        public override void Awake()
        {
            animator = GameObject.GetComponent<Animator>() as Animator;
            sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            enemyCollider = GameObject.GetComponent<Collider>() as Collider;
            GameObject.Transform.Position = position;
        }

        public override void Start()
        {
            FindPlayerObject();
            GetLocations();
        }

        public override void Update(GameTime gameTime)
        {
            if (_isGrounded)
            {
                SetLocationRectangle();
            }
            ChasePlayer();
            SetVelocity();
            if (!_isAttacking)
            {
                Move();
            }
            UpdatePositionReference();
            Flipsprite();
            HandleGravity();
            CheckCollision();
            Attack();

        }

        #region Movement Methods

        /// <summary>
        /// Find and reference the rectangle containing mushroom object position
        /// </summary>
        private void SetLocationRectangle()
        {
            foreach (Rectangle location in locations)
            {
                if (location.Contains(position) && !_locationRectangleFound)
                {
                    _currentRectangle = location;
                    _locationRectangleFound = true;
                    UpdateFieldofView(_currentRectangle);
                }
            }
        }

        /// <summary>
        /// Sets velocity according to position in rectangle and calls find location methods
        /// </summary>
        private void SetVelocity()
        {
            if (position.X >= (_currentRectangle.Right - sr.Sprite.Width))
            {
                velocity = new Vector2(-50, 0);

                FindLocationLeft();
            }
            if (position.X <= (_currentRectangle.Left + sr.Sprite.Width))
            {
                velocity = new Vector2(50, 0);
                FindLocationRight();
            }
        }

        /// <summary>
        /// Set current location to right rectangle if it contains a platform
        /// </summary>
        private void FindLocationRight()
        {
            for (int i = 0; i < locations.Count; i++)
            {
                if (_currentRectangle.X + 222 == locations[i].X && _currentRectangle.Y == locations[i].Y)
                {
                    _currentRectangle = locations[i];
                    UpdateFieldofView(_currentRectangle);
                }
            }
        }

        /// <summary>
        /// Set current location to left rectangle if it contains a platform
        /// </summary>
        private void FindLocationLeft()
        {
            for (int i = 0; i < locations.Count; i++)
            {
                if (_currentRectangle.X - 222 == locations[i].X && _currentRectangle.Y == locations[i].Y)
                {
                    _currentRectangle = locations[i];

                    UpdateFieldofView(_currentRectangle);
                   
                }
            }
        }

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
        /// Checks for relevant collision with this object
        /// </summary>
        public override void CheckCollision()
        {
            //TODO - Refactor this - KRISTIAN
            foreach (Collider col in GameWorld.Instance.Colliders)
            {
                if (col.GameObject.HasComponent<Platform>())
                {
                    if (col.CollisionBox.Intersects(enemyCollider.CollisionBox))
                    {
                        _isGrounded = true;
                        _groundCollision = col.CollisionBox;
                    }
                }
                if (_isGrounded && !enemyCollider.CollisionBox.Intersects(_groundCollision))
                {
                    _isGrounded = false;
                }
                else if (col.GameObject.HasComponent<Player>())
                {
                    if (col.CollisionBox.Intersects(enemyCollider.CollisionBox))
                    {
                        _collidingWithPlayer = true;
                    }
                    else _collidingWithPlayer = false;
                }
                if (col.GameObject.HasComponent<Projectile>())
                {
                    if (col.CollisionBox.Intersects(enemyCollider.CollisionBox))
                    {
                        health -= 30;
                        GameWorld.Instance.Destroy(col.GameObject);
                    }
                }
            }
        }

        /// <summary>
        /// Locate player within fieldOfview rectangles and set rangeAttack to either true or false
        /// </summary>
        public override void ChasePlayer()
        {
            Rectangle playerCol = (player.GameObject.GetComponent<Collider>() as Collider).CollisionBox;
            int outOfView = 0;

            for (int i = 0; i < _fieldOfView.Count; i++)
            {
                //if player intersects with any of one _fieldofview and is above center, set range attack to true;
                if (playerCol.Intersects(_fieldOfView[i]) && playerCol.Bottom < _fieldOfView[i].Center.Y)
                {
                    Console.WriteLine("player in sight");
                    _canRangeAttack = true;
                }

                //If no _fieldofView intersects with player collisionbox, set range attacck to false;
                if (!playerCol.Intersects(_fieldOfView[i]))
                {
                    outOfView++;
                }
                if (outOfView == _fieldOfView.Count)
                {
                    _canRangeAttack = false;
                }

            }
        }

        /// <summary>
        /// Initiate attack depending on relevant bool
        /// </summary>
        public override void Attack()
        {
            if (_collidingWithPlayer)
            {
                Console.WriteLine("physical attack");
                _isAttacking = true;
            }
            else if (_canRangeAttack)
            {
                Console.WriteLine("range attack");
                _isAttacking = true;
            }
            if (!_canRangeAttack && !_collidingWithPlayer)
            {
                _isAttacking = false;
            }
        }

        /// <summary>
        /// Add view to fieldOfView list, if it doesn't contain it
        /// </summary>
        /// <param name="view">rectangle to be added</param>
        private void UpdateFieldofView(Rectangle view)
        {
            if (!_fieldOfView.Contains(view))
            {
                _fieldOfView.Add(_currentRectangle);
            }
        }

        /// <summary>
        /// Set reference to player component
        /// </summary>
        public override void FindPlayerObject()
        {
            foreach (GameObject go in GameWorld.Instance.GameObjects)
            {
                if (go.HasComponent<Player>())
                {
                    player = go.GetComponent<Player>() as Player;
                }
            }
        }

        /// <summary>
        /// Play relevant animation
        /// </summary>
        public override void HandleAnimations()
        {
            if (_collidingWithPlayer)
            {
                //play close attack animation
                animator.PlayAnimation("");
            }
            if (_canRangeAttack)
            {
                //play range attack animation
                animator.PlayAnimation("");
            }
            if (health <= 0)
            {
                //play death animation
                animator.PlayAnimation("");
            }

        }

    }
}
