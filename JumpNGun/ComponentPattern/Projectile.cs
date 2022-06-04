using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
    public class Projectile : Component
    {
        public Vector2 Velocity { get; set; }
        public float Speed { get; set; }

        public override void Start()
        {
            SetSpeed();
        }


        public override void Update(GameTime gameTime)
        {
            Move();
        }

        private void Move()
        {
            if (Velocity == Vector2.Zero) return; // Guard clause

            Velocity.Normalize();

            GameObject.Transform.Translate(Velocity * GameWorld.DeltaTime);


            // if(GameObject.Transform.Position.X < GameWorld.Instance.ScreenSize.X || GameObject.Transform.Position.X > GameWorld.Instance.ScreenSize.X )
            // {
            //     GameWorld.Instance.Destroy(this.GameObject);
            // }
        }

        private void SetSpeed()
        {
            Velocity *= Speed;
        }
    }
}