using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public class EnviromentObject : Component
    {
        private Vector2 _position;

        public EnviromentObject(Vector2 position)
        {
            _position = position;
        }

        public override void Awake()
        {
            GameObject.Transform.Position = _position;
        }
    }
}
