using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace JumpNGun
{
    public class Collider : Component
    {
        private Texture2D texture; //texture to be drawn by renderer

        private SpriteRenderer spriteRenderer; //spriterender for drawing
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
                    (int) (GameObject.Transform.Position.X - spriteRenderer.Sprite.Width / 2),
                    (int) (GameObject.Transform.Position.Y - spriteRenderer.Sprite.Height / 2),
                    spriteRenderer.Sprite.Width,
                    spriteRenderer.Sprite.Height
                );
            }
        }

        public override void Start()
        {
            //get spriterenderer component of GameObject
            spriteRenderer = (SpriteRenderer) GameObject.GetComponent<SpriteRenderer>();

            //load pixeltexture for texture. Texture used to create visible colisionbox for debugging
            texture = GameWorld.Instance.Content.Load<Texture2D>("Pixel");
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
            

            spriteBatch.Draw(texture, TopLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(texture, BottomLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(texture, RightLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(texture, LeftLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
        }


        /// <summary>
        /// Checks if any colliders (collisionbox) intersects with each other and trigger event if they do
        /// </summary>
        public void CheckCollision()
        {
            foreach (Collider otherCollision in GameWorld.Instance.Colliders)
            {
                if (this.CollisionBox.Intersects(otherCollision.CollisionBox) && otherCollision != this)
                {
                    if (otherCollision.GameObject != this.GameObject)
                    {
                        EventManager.Instance.TriggerEvent("OnCollision", new Dictionary<string, object>()
                            {
                                {"collider", otherCollision.GameObject },
                            }
                        );
                    }
                }
            }
        }
    }
}