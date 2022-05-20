using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace JumpNGun
{
    public class Platform : Component
    {
        private Vector2 _position; //position of platform

        //Property to gain acces to platform position
        public Vector2 Position { get => _position; private set => _position = value; }

        public Platform(Vector2 position)
        {
            _position = position;
        }

        public override void Awake()
        {
            GameObject.Transform.Position = _position;
        }
    }
}
