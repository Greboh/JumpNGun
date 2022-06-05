using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using JumpNGun;

namespace JumpNGun
{
    public class Collider : Component
    {
        private Texture2D _texture; //texture to be drawn by renderer

        private SpriteRenderer _spriteRenderer; //spriterender for drawing

        public Rectangle TopLine { get; set; }
        public Rectangle BottomLine { get; set; }
        public Rectangle RightLine { get; set; }
        public Rectangle LeftLine { get; set; }

        /// <summary>
        /// Property for rectangle colisionbox  for sprite
        /// </summary>
        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle
                (
                    (int) (GameObject.Transform.Position.X - _spriteRenderer.Sprite.Width / 2),
                    (int) (GameObject.Transform.Position.Y - _spriteRenderer.Sprite.Height / 2),
                    _spriteRenderer.Sprite.Width,
                    _spriteRenderer.Sprite.Height
                );
            }
        }



        public override void Start()
        {
            //get spriterenderer component of GameObject
            _spriteRenderer = (SpriteRenderer) GameObject.GetComponent<SpriteRenderer>();

            //load pixeltexture for texture. Texture used to create visible colisionbox for debugging
            _texture = GameWorld.Instance.Content.Load<Texture2D>("Pixel");

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawRectangle(CollisionBox, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            CheckCollision();
        }


        /// <summary>
        /// Will draw rectangle for visible colisionbox for debugging
        /// </summary>
        /// <param name="collisionBox"></param>
        /// <param name="spriteBatch"></param>
        private void DrawRectangle(Rectangle collisionBox, SpriteBatch spriteBatch)
        {
            TopLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);
            BottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);
            RightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);
            LeftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);


            spriteBatch.Draw(_texture, TopLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(_texture, BottomLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(_texture, RightLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(_texture, LeftLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
        }


        /// <summary>
        /// Checks if any colliders (collisionbox) intersects with each other and trigger event if they do
        /// </summary>
        private void CheckCollision()
        {

        }
    }
}
