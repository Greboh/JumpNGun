using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public class Portal :  Component
    {
        private Vector2 _position;

        public Portal()
        {
            _position = new Vector2(1100, 760);
        }


        public override void Awake()
        {
            
        }

        public override void Start()
        {
        }

        public override void Update(GameTime gameTime)
        {
            CheckCollision();
        }


        private void BuildComponents()
        {
            
        }

        private void CheckCollision()
        {
            foreach (Collider collider in GameWorld.Instance.Colliders)
            {
                if (collider.GameObject.HasComponent<Player>() && collider.CollisionBox.Intersects(_portalCollider.CollisionBox))
                {
                    LevelManager.Instance.CanChangeLevel = true;
                }
            }
        }
    }
}
