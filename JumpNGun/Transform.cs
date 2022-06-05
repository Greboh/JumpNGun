using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public class Transform
    {
        public string tag;
        public Vector2 Position { get; set; }

        public void Translate(Vector2 translation)
        {
            if (!float.IsNaN(translation.X) && !float.IsNaN(translation.Y))
            {
                Position += translation;
            }
        }

        public void Change(Vector2 newPos)
        {
            if (!float.IsNaN(newPos.X) && !float.IsNaN(newPos.Y))
            {
                Position = newPos;
            }
        }
    }
}
