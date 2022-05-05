using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class Player : Component
    {
        private float _speed; // Speed at which the player moves
        private float _fallSpeed; // Speed at which the player falls

        private bool _isGrounded;
        
        public Player(float speed)
        {
            _speed = speed;
        }

        public override void Awake()
        {
            
        }

        public override void Start()
        {
            SpriteRenderer sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.SetSprite("1_Soldier_idle");

            GameObject.Transform.Position = new Vector2(200, 200);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
            InputHandler.Intance.Execute(this);
            
            HandleGravity();
        }
        
        
        
        
        /// <summary>
        /// Called from MoveCommand.cs
        /// Moves the player
        /// </summary>
        /// <param name="velocity">The strength at which we want to move</param>
        public void Move(Vector2 velocity)
        {
            if (!_isGrounded || velocity == Vector2.Zero) return; // Guard clause
            
            // Normalize velocity
            velocity.Normalize();

            // Multiply with speed
            velocity *= _speed;

            // Translate our current position to the new one
            GameObject.Transform.Translate(velocity * GameWorld.DeltaTime);
            
        }
        
        private void HandleGravity()
        {
            if(_isGrounded) return;
            
            Vector2 fallDirection = new Vector2(0, 1);

            fallDirection *= _fallSpeed;
            
            GameObject.Transform.Translate(fallDirection * GameWorld.DeltaTime);
        }
    }
}