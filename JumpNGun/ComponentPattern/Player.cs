using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JumpNGun
{
    public class Player : Component
    {
        private CharacterType _character;
        
        private float _speed; // Speed at which the player moves
        private float _jumpHeight = -100; // The jump height of the player

        private float _gravity = 50; // The force of gravity
        private float _gravityMultiplier; // Used to multiply the gravity over time making it stronger

        private bool _isGrounded = false; // Is the player grounded
        private bool _canJump; // Can the player jump
        private bool _isJumping; // Is the player jumping

        private int _jumpCount = 0; // The current amount of player jumps
        private int _maxJumpCount = 2; // The max allowed amount of player jumps

        private bool _canShoot = true;
        private float _shootTime;
        private float _shootCooldown;

        private SpriteRenderer _sr; // Reference to the SpriteRenderer component
        private Animator _animator; // Reference to the Animator component

        private Dictionary<Keys, bool> _movementKeys = new Dictionary<Keys, bool>();
        
        public Player(CharacterType character)
        {
            switch (character)
            {
                case CharacterType.Soldier:
                    _speed = 100;
                    _shootCooldown = 2f;
                    break;

            }
            
            _character = character;
        }

        public override void Awake()
        {
            EventManager.Instance.Subscribe("OnCollisionEnter", OnCollisionEnter);
            EventManager.Instance.Subscribe("OnCollisionExit", OnCollisionExit);

            EventManager.Instance.Subscribe("OnJump", OnKeysPressed);
        }


        public override void Start()
        {
            _sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            _sr.SetSprite("1_Soldier_idle");
            
            _animator = GameObject.GetComponent<Animator>() as Animator;

            GameObject.Transform.Position = new Vector2(200, 420);
            _gravityMultiplier = _gravity;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
            InputHandler.Intance.Execute(this);
            
            HandleShooting();
            
            if(!_movementKeys.ContainsValue(true)) _animator.PlayAnimation("Idle");

            
            HandleGravity(gameTime);
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

            Vector2 _moveDirection = velocity;


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
            if(moveDirection.X < 0)
            {
                _sr.SpriteEffects = SpriteEffects.FlipHorizontally;
            }
            // If we are moving right, unflip the sprite
            else if (moveDirection.X > 0)
            {
                _sr.SpriteEffects = SpriteEffects.None;
            }
        }

        public void Jump()
        {
            _canJump = _jumpCount < _maxJumpCount;


            if (!_canJump || _isJumping)
            {
                return;
            }
            _jumpCount++;


            Vector2 targetDirection = new Vector2(0, _jumpHeight);
            GameObject.Transform.Translate(targetDirection);
            _animator.PlayAnimation("Jump");
        }

        private void OnKeysPressed(Dictionary<string, object> ctx)
        {
            if ((Keys)ctx["key"] == Keys.A || (Keys)ctx["key"] == Keys.D || (Keys)ctx["key"] == Keys.W)
            {
                _movementKeys[(Keys) ctx["key"]] = (bool) ctx["isKeyDown"];
                
                if((Keys)ctx["key"] == Keys.W && (bool)ctx["isKeyDown"])
                {
                    _isJumping = (bool) ctx["isKeyDown"];
                    Console.WriteLine(_isJumping);
                }
            }
            
            

        }


        private void HandleGravity(GameTime gameTime)
        {
            if (_isGrounded) return;

            _gravityMultiplier += (float) gameTime.ElapsedGameTime.TotalSeconds * 100;
            
            Vector2 fallDirection = new Vector2(0, 1);
            // Console.WriteLine($"GravityMultipler: {_gravityMultipler}");

            fallDirection *= _gravityMultiplier;

            GameObject.Transform.Translate(fallDirection * GameWorld.DeltaTime);
        }


        /// <summary>
        /// Called from ShootCommand.cs
        /// Shoots a projectile
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
        
        private void HandleShooting()
        {
            if(!_canShoot)
            {
                _shootTime += GameWorld.DeltaTime;
                
                if(_shootTime > _shootCooldown)
                {
                    _canShoot = true;
                    _shootTime = 0;
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
                _gravityMultiplier = _gravity;
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