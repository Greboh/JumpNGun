using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class Player : Component
    {
        private float _speed; // Speed at which the player moves

        private float _deltaTime; // Time since the last frame

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

            GameObject.Transform.Position = new Vector2(200, 200);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
            _deltaTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            InputHandler.Intance.Execute(this);
        }
        
        
        
        
        /// <summary>
        /// Called from MoveCommand.cs
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
            GameObject.Transform.Translate(velocity * _deltaTime);
        }
    }
}