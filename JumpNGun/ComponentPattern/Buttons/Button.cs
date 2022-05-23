using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    class Buttons : Component
    {

        private Vector2 _position;

        private enum _type;

  
        public Buttons(Enum type)
        {
            _type = type;
        }

        public override void Start()
        {
            GameObject.Transform.Position = _position;
            
        }



        


    }
}
