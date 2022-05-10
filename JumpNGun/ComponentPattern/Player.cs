using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JumpNGun
{
    public class Player : Component
    {
        private CharacterType _character;

        private float _speed; // Speed at which the player moves
        private float _jumpHeight; // The jump height of the player
        private float _dashStrength; // The strength of the player dash

        private int _gravity = 50; // The initial force of gravity
        private float _gravityPull; // How strong the force of gravity is
        private int _gravityMultiplier = 100; // Used to multiply the gravity over time making it stronger

        private bool _isGrounded = false; // Is the player grounded
        
        private bool _canJump; // Can the player jump
        private bool _isJumping; // Is the player jumping
        private int _jumpCount = 0; // The current amount of player jumps
        private int _maxJumpCount = 2; // The max allowed amount of player jumps
        
        private bool _canDash = true;
        private float _dashTimer;
        private float _dashCooldown;

        private bool _canShoot = true;
        private float _shootTime;
        private float _shootCooldown;

        private SpriteRenderer _sr; // Reference to the SpriteRenderer component
        private Animator _animator; // Reference to the Animator component

        private Dictionary<Keys, bool> _movementKeys = new Dictionary<Keys, bool>();

        private Vector2 _moveDirection;

        public Player(CharacterType character)
        {
            switch (character)
            {
                case CharacterType.Soldier:
                    _speed = 100;
                    _jumpHeight = -100;
                    _dashStrength = 50;
                    _dashCooldown = 0.5f;
                    _shootCooldown = 2f;
                    break;
            }

            _character = character;
        }

        public override void Awake()
        {
            EventManager.Instance.Subscribe("OnKeyPress", OnKeysPressed);
            
            EventManager.Instance.Subscribe("OnCollisionEnter", OnCollisionEnter);
            EventManager.Instance.Subscribe("OnCollisionExit", OnCollisionExit);

        }


        public override void Start()
        {
            _sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            _sr.SetSprite("1_Soldier_idle");

            _animator = GameObject.GetComponent<Animator>() as Animator;

            GameObject.Transform.Position = new Vector2(200, 420);
            _gravityPull = _gravity;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
            InputHandler.Intance.Execute(this);

            HandleShootLogic();
            HandleDashLogic();

            HandleAnimations();

            CheckCollision();
            
            HandleGravity();
        }

        /// <summary>
        /// Handles when to set Animations 
        /// </summary>
        private void HandleAnimations()
        {
            // If we are not grounded we are in the air and should play the jump animation
            if (!_isGrounded) _animator.PlayAnimation("Jump");
            // If there isn't any values that is true in movementKeys and we are grounded play idle
            if (!_movementKeys.ContainsValue(true) && _isGrounded) _animator.PlayAnimation("Idle");
        }

        /// <summary>
        /// Called from MoveCommand.cs
        /// Moves the player
        /// </summary>
        /// <param name="velocity">The strength at which we want to move</param>
        public void Move(Vector2 velocity)
        {
            // Normalize velocity
            velocity.Normalize();

            // Multiply with speed
            velocity *= _speed;

           _moveDirection = velocity;


            // Translate our current position to the new one
            GameObject.Transform.Translate(_moveDirection * GameWorld.DeltaTime);

            FlipSprite(_moveDirection);

            _animator.PlayAnimation("Run");
        }

        /// <summary>
        /// Checks if the sprite should be flipped by using the moveDirection
        /// </summary>
        private void FlipSprite(Vector2 moveDirection)
        {
            // If we are moving left, flip the sprite
            if (moveDirection.X < 0)
            {
                _sr.SpriteEffects = SpriteEffects.FlipHorizontally;
            }
            // If we are moving right, unflip the sprite
            else if (moveDirection.X > 0)
            {
                _sr.SpriteEffects = SpriteEffects.None;
            }
        }

        /// <summary>
        /// Handles jumping. Called from JumpCommand.cs
        /// </summary>
        public void Jump()
        {
            // Check if we are above or at our maxJumpCount
            _canJump = _jumpCount <= _maxJumpCount;
            
            // Guard clause
            if (!_canJump || _isJumping) return;
            
            // Add to our jump count
            _jumpCount++;
            
            // Set our targetDirection using our jumpHeight
            Vector2 targetDirection = new Vector2(0, _jumpHeight);
            
            GameObject.Transform.Translate(targetDirection);
        }
        
        /// <summary>
        /// Called from DashCommand.cs
        /// Makes the player dash if he can
        /// </summary>
        public void Dash()
        {
            
            if (!_canDash) return; // Guard clause
            
            // Multiply with dashStrength
            Vector2 dashDirection = _moveDirection;
            dashDirection *= _dashStrength;
            
            // Translate our current position to the new one
            GameObject.Transform.Translate(dashDirection * GameWorld.DeltaTime);

            FlipSprite(dashDirection);
            _canDash = false;
        }

        /// <summary>
        /// Logic for handling when we can dash
        /// </summary>
        private void HandleDashLogic()
        {
            
            if (_canDash) return; // Guard clause
            
            
            _dashTimer += GameWorld.DeltaTime; // Add to our dashTimer
            
            // Check if its bigger than the cooldown
            if(_dashTimer > _dashCooldown)
            {
                _canDash = true;
                _dashTimer = 0;
            }
        }

        /// <summary>
        /// Creates gravity making sure the player falls unless he is grounded
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
        /// Called from ShootCommand.cs
        /// Shoots a projectile if we can shoot
        /// </summary>
        public void Shoot()
        {
            if (!_canShoot) return; // Guard clause

            GameObject projectile = ProjectileFactory.Instance.Create(_character);

            projectile.Transform.Position = GameObject.Transform.Position;

            Vector2 shootRight = new Vector2(1, 0);
            Vector2 shootLeft = new Vector2(-1, 0);

            (projectile.GetComponent<Projectile>() as Projectile).Velocity = _sr.SpriteEffects == SpriteEffects.None ? shootRight : shootLeft;
            (projectile.GetComponent<Projectile>() as Projectile).Speed = 100;
            GameWorld.Instance.Instantiate(projectile);

            _canShoot = false;
        }

        /// <summary>
        /// Logic for handling when we can shoot
        /// </summary>
        private void HandleShootLogic()
        {
            // Guard clause
            if (_canShoot) return;
            
            // Add to our shootTime
            _shootTime += GameWorld.DeltaTime;

            // Check if its bigger than the cooldown
            if (_shootTime > _shootCooldown)
            {
                _canShoot = true;
                _shootTime = 0;
            }
        }

        /// <summary>
        /// Event that receives all key presses
        /// And handles logic depending on the key and if its pressed down
        /// </summary>
        /// <param name="ctx">The context from the trigger in InputHandler.cs</param>
        private void OnKeysPressed(Dictionary<string, object> ctx)
        {
            // Check if any of the keys associated with a movement action is pressed
            if ((Keys) ctx["key"] == Keys.A || (Keys)ctx["key"] == Keys.D || (Keys)ctx["key"] == Keys.LeftAlt )
            {
                _movementKeys[(Keys) ctx["key"]] = (bool) ctx["isKeyDown"];
            }
            
            // If we are jumping we wanna set the bool, checking for jump key press
            if ((Keys) ctx["key"] == Keys.W)
            {
                _isJumping = (bool) ctx["isKeyDown"];
            }
        }

        private string _groundCollision;
        
        private void CheckCollision()
        {
            Collider p_Collider = GameObject.GetComponent<Collider>() as Collider;
            
            foreach (Collider otherCollision in GameWorld.Instance.Colliders)
            {
                if (otherCollision == p_Collider) return;
                
                if (p_Collider.CollisionBox.Intersects(otherCollision.TopLine))
                {
                    Console.WriteLine("Setting to true");
                    _isGrounded = true;
                    _jumpCount = 0;
                    _gravityPull = _gravity;
                    _groundCollision = otherCollision.GameObject.Tag;
                }
                
                if(!p_Collider.CollisionBox.Intersects(otherCollision.TopLine))
                {
                    _isGrounded = false;
                }
            }
            
        }

        private void OnCollisionEnter(Dictionary<string, object> ctx)
        {
            GameObject otherCollision = (GameObject) ctx["otherCollision"];

            if (otherCollision.Tag == "P_Projectile") return;

            if (otherCollision.Tag == "ground")
            {
                _isGrounded = true;
                _jumpCount = 0;
                _gravityPull = _gravity;
            }
        }

        private void OnCollisionExit(Dictionary<string, object> ctx)
        {
            GameObject lastCollision = (GameObject) ctx["lastCollision"];

            if (lastCollision.Tag == "P_Projectile") return;

            if (lastCollision.Tag == "ground")
            {
                _isGrounded = false;
            }
        }
    }
}