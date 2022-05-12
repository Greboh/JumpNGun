using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    class SlugEnemy : Component
    {
        private float _speed; // Speed at which the player moves

        public SlugEnemy(float speed)
        {
            _speed = speed;
        }

        public override void Start()
        {
            SpriteRenderer sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.SetSprite("slug_idle_1");

            GameObject.Transform.Position = new Vector2(200, 420);
            
        }


    }
}
