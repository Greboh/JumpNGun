using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af Nichlas Hoberg
    /// </summary>
    public class Projectile : Component
    {
        public Vector2 Velocity { get; set; }
        public float Speed { get; set; }
        public float Damage { get; set; }
        
        public bool FiredFromPlayer { get; set; }
        public bool HasWrapAbility { get; set; }
        public bool HasVampiricAbility { get; set; }

        private Collider _collider;
        
        

        public override void Start()
        {
            SetSpeed();
            
            _collider = GameObject.GetComponent<Collider>() as Collider;
        }


        public override void Update(GameTime gameTime)
        {
            Move();
            ScreenBounds();
        }

        /// <summary>
        /// Destroy projectile if it exceeds screen limitations
        /// </summary>
        private void ScreenBounds()
        {
            if (FiredFromPlayer)
            {
                if (!HasWrapAbility)
                {
                    //Destroy projectile for exceeding min/max width
                    if (GameObject.Transform.Position.X > GameWorld.Instance.GraphicsDevice.Viewport.Width || GameObject.Transform.Position.X < 0)
                        GameWorld.Instance.Destroy(GameObject);

                    //Destroy projectile for excceeding min/max height
                    if (GameObject.Transform.Position.Y > GameWorld.Instance.GraphicsDevice.Viewport.Height || GameObject.Transform.Position.Y < 0)
                        GameWorld.Instance.Destroy(GameObject);
                }
                else
                {
                    //If player moves beyond 0 on x-axis move player max x-value on axis
                    if (GameObject.Transform.Position.X < 0)
                    {
                        GameObject.Transform.Transport(new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width, GameObject.Transform.Position.Y));
                    }
                    //If player moves beyond max value on x-axis move player to 0 on x-axis 
                    else if (GameObject.Transform.Position.X > GameWorld.Instance.GraphicsDevice.Viewport.Width)
                    {
                        GameObject.Transform.Transport(new Vector2(10, GameObject.Transform.Position.Y));
                        // Velocity = -Velocity;
                    }

                }
            }
            else
            {
                //Destroy projectile for exceeding min/max width
                if (GameObject.Transform.Position.X > GameWorld.Instance.GraphicsDevice.Viewport.Width || GameObject.Transform.Position.X < 0)
                    GameWorld.Instance.Destroy(GameObject);

                //Destroy projectile for excceeding min/max height
                if (GameObject.Transform.Position.Y > GameWorld.Instance.GraphicsDevice.Viewport.Height || GameObject.Transform.Position.Y < 0)
                    GameWorld.Instance.Destroy(GameObject);
            } 
            
            
        }

        private void Move()
        {
            if (Velocity == Vector2.Zero) return; // Guard clause

            Velocity.Normalize();

            GameObject.Transform.Translate(Velocity * GameWorld.DeltaTime);
            
            // Check if the projectile collide with another object
            foreach (Collider col in GameWorld.Instance.Colliders)
            {
                // Make sure that it doesnt intersect with its own collider
                if (col.CollisionBox.Intersects(_collider.CollisionBox) && col != _collider)
                {
                    // Trigger event
                    EventHandler.Instance.TriggerEvent("OnTakeDamage", new Dictionary<string, object>()
                        {
                            {"damage", Damage}, // The projectile's damage
                            {"object", col.GameObject}, // The collision object 
                            {"projectile", this.GameObject} // The projectile object
                        }
                    );
                }
            }
        }

        private void SetSpeed()
        {
            Velocity *= Speed;
        }
    }
}