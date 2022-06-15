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
        #region Old and unchanged code

        public Vector2 Velocity { get; set; }
        public float Speed { get; set; }
        public float Damage { get; set; }
        
        private Collider _collider; 

        #endregion
        
        public bool FiredFromPlayer { get; set; }
        public bool HasWrapAbility { get; set; }
        public bool HasVampiricAbility { get; set; }
        
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
            // Check if the projectile is fired from player and if player has the wrap ability
            if (!HasWrapAbility || !FiredFromPlayer)
            {
                //Destroy projectile for exceeding min/max screen width
                if (GameObject.Transform.Position.X > GameWorld.Instance.ScreenSize.X || GameObject.Transform.Position.X < 0)
                    GameWorld.Instance.Destroy(GameObject);
                    
                //Destroy projectile for exceeding min/max screen height
                else if (GameObject.Transform.Position.Y > GameWorld.Instance.ScreenSize.Y || GameObject.Transform.Position.Y < 0)
                    GameWorld.Instance.Destroy(GameObject);
            }
            else
            {
                //If the projectile moves beyond 0 on x-axis move projectiles to max x-axis value
                if (GameObject.Transform.Position.X < 0)
                    GameObject.Transform.Transport(new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width, GameObject.Transform.Position.Y));
                    
                //If projectile moves beyond max value on x-axis move projectile to 0 on x-axis 
                else if (GameObject.Transform.Position.X > GameWorld.Instance.GraphicsDevice.Viewport.Width)
                    GameObject.Transform.Transport(new Vector2(10, GameObject.Transform.Position.Y));
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