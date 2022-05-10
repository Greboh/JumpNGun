using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JumpNGun
{
    public class Player : Component
    {
        private float _speed; // Speed at which the player moves
        private float _jumpHeight = -100; // The jump height of the player
        private Vector2 _currentVelocity;

        private float _gravity = 50; // The force of gravity
        private float _gravityMultipler;


        private bool _isGrounded = false;
        private bool _canJump;
        private bool _isJumping;

        private int _jumpCount = 0;
        private int _maxJumpCount = 2;

        public Vector2 CurrentVelocity { get => _currentVelocity; set => _currentVelocity = value; }

        public Player(float speed)
        {
            _speed = speed;
        }

        public override void Awake()
        {
            //EventManager.Instance.Subscribe("OnCollisionEnter", OnCollisionEnter);
            //EventManager.Instance.Subscribe("OnCollisionExit", OnCollisionExit);

            EventManager.Instance.Subscribe("OnJump", OnJumpPressed);
        }


        public override void Start()
        {
            SpriteRenderer sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.SetSprite("1_Soldier_idle");

            GameObject.Transform.Position = new Vector2(600, 650);
            _gravityMultipler = _gravity;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
            InputHandler.Intance.Execute(this);
            CheckCollision();
            HandleGravity(gameTime);
        }


        /// <summary>
        /// Called from MoveCommand.cs
        /// Moves the player
        /// </summary>
        /// <param name="velocity">The strength at which we want to move</param>
        public void Move(Vector2 velocity)
        {
            if (velocity == Vector2.Zero) return; // Guard clause

            // Normalize velocity
            velocity.Normalize();

            // Multiply with speed
            velocity *= _speed;

            // Translate our current position to the new one
            GameObject.Transform.Translate(velocity * GameWorld.DeltaTime);
            _currentVelocity = velocity;
        }

        public void Jump()
        {
            _canJump = _jumpCount < _maxJumpCount;


            if (!_canJump || _isJumping)
            {
                // Console.WriteLine("Cant jump!");
                // Console.WriteLine($"JumpCount: {_jumpCount}");
                // Console.WriteLine($"maxJumpCount: {_maxJumpCount}");
                return;
            }
            _jumpCount++;

            _isGrounded = false;
            Vector2 targetDirection = new Vector2(0, _jumpHeight);
            GameObject.Transform.Translate(targetDirection);
        }

        private void OnJumpPressed(Dictionary<string, object> ctx)
        {
            ButtonState currentState = (ButtonState) ctx["buttonState"];
            _isJumping = currentState == ButtonState.Down;
        }


        private void HandleGravity(GameTime gameTime)
        {
            if (_isGrounded) return;

            _gravityMultipler += (float) gameTime.ElapsedGameTime.TotalSeconds * 100;
            
            Vector2 fallDirection = new Vector2(0, 1);
            // Console.WriteLine($"GravityMultipler: {_gravityMultipler}");

            fallDirection *= _gravityMultipler;

            GameObject.Transform.Translate(fallDirection * GameWorld.DeltaTime);
        }


        private void OnCollisionEnter(Dictionary<string, object> ctx)
        {
            GameObject otherCollision = (GameObject) ctx["otherCollision"];

            //Console.WriteLine($"CollisionEnter with {otherCollision.Tag}");

            if (otherCollision.Tag == "ground")
            {
                _isGrounded = true;
                _jumpCount = 0;
                _gravityMultipler = _gravity;
            }
        }

        private void OnCollisionExit(Dictionary<string, object> ctx)
        {
            GameObject lastCollision = (GameObject) ctx["lastCollision"];
            
            if (lastCollision.Tag == "ground")
            {
                _isGrounded = false;
            }
        }

        private void CheckCollision()
        {
            Collider p_collider = GameObject.GetComponent<Collider>() as Collider;

            foreach (Collider otherCollider in GameWorld.Instance.Colliders)
            {
                if (otherCollider.GameObject.Tag == p_collider.GameObject.Tag) return;

                if (otherCollider.CollisionBox.Top == p_collider.CollisionBox.Bottom && otherCollider.TopLine.Contains(p_collider.BottomLine))
                {
                    _isGrounded = true;
                    _jumpCount = 0;
                    _gravityMultipler = _gravity;
                    Console.WriteLine("Top position: " + otherCollider.CollisionBox.Top);
                    Console.WriteLine("Bottom position: " + p_collider.CollisionBox.Bottom);
                }
            }
            
        }
    }
}