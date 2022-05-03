using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace JumpNGun
{
    public class SpriteRenderer : Component
    {
        public int number = 0;

        public Texture2D Sprite { get; set; }

        public Vector2 Origin { get; set; }

        public Color Color { get; set; }

        public SpriteEffects SpriteEffects { get; set; }

        public void SetSprite(string spriteName)
        {
            Sprite = GameWorld.Instance.Content.Load<Texture2D>(spriteName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, GameObject.Transform.Position, null, Color, 0, Origin, 1, SpriteEffects, 1);
        }
    }
}
