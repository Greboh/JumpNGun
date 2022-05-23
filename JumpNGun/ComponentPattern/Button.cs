using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun.ComponentPattern
{
    class Button : Component
    {

        private Vector2 _position;

        public Vector2 Position { get => _position; set => _position = value; }

        public Button(Vector2 position)
        {
            _position = position;
        }


    }
}
