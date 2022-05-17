using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public class Portal : Component
    {
        private Vector2 _position;


        public Portal(Vector2 position)
        {
            _position = position;
        }


        public override void Awake()
        {
            GameObject.Transform.Position = _position;
        }

        public override void Start()
        {
        }

        public override void Update(GameTime gameTime)
        {
            CheckCollision();
        }

        private void CheckCollision()
        {
            Collider _portalCollider = GameObject.GetComponent<Collider>() as Collider;

            foreach (Collider otherCollider in GameWorld.Instance.Colliders)
            {
                if(otherCollider == _portalCollider) return; //Terminate if the collision is the portal itself


                if(_portalCollider.CollisionBox.Intersects(otherCollider.CollisionBox) && otherCollider.GameObject.Tag == "Player")
                {
                    Console.WriteLine("colliding with player");
                }
            }
        }
    }
}
