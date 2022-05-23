using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;


namespace JumpNGun
{
    public class SpriteRenderer : Component
    {
        public int number = 0;

        public Texture2D Sprite { get; set; } //texture for sprite

        public Vector2 Origin { get; set; } //origin for sprite

        public Color Color { get; set; }//color for sprite

        public SpriteEffects SpriteEffects { get; set; }//spriteeffects for sprite. flips, etc

        public override void Start()
        {
            Origin = new Vector2(Sprite.Width / 2, Sprite.Height / 2);

        }


        /// <summary>
        /// Set sprite to a specific png
        /// </summary>
        /// <param name="spriteName"></param>
        public void SetSprite(string spriteName)
        {
            Sprite = GameWorld.Instance.Content.Load<Texture2D>(spriteName);
        }

        /// <summary>
        /// Draw sprite
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, GameObject.Transform.Position, null, Color.White, 0, Origin, 1, SpriteEffects, 1);
        }
    }
}
