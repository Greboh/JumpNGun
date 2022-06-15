using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af Nichlas Hoberg
    /// </summary>
    public enum ExperienceOrbType {Small, Medium, Large}
    
    public class ExperienceOrb : Component
    {
        #region Old fields nothing new

        private float _xpAmount; // Amount of xp an orb gives
        private Vector2 _position; // The orb's position

        #endregion

        public bool HasMagneticPull { get; set; }
        
        private Vector2 _velocity;
        private float _pullSpeed = 0.05f;
       

        public ExperienceOrb(float xpAmount, Vector2 position)
        {
            _xpAmount = xpAmount;
            _position = position;
        }

        #region Component Methods
        
        public override void Start()
        {
            // Set position of the orb
            GameObject.Transform.Position = _position;
            
            // Play Idle Animation
            (GameObject.GetComponent<Animator>() as Animator).PlayAnimation("Idle");
        }

        public override void Update(GameTime gameTime)
        {
            OnCollision();

            if (!HasMagneticPull) return;
            PullTowardsPlayer();
        }

        #endregion
        
        /// <summary>
        /// Pulls the orb towards the player
        /// </summary>
        private void PullTowardsPlayer()
        {
            _velocity = CalculatePlayerDirection();
            GameObject.Transform.Translate(_velocity * _pullSpeed );
        }

        /// <summary>
        /// Calculates the direction towards the player
        /// </summary>
        /// <returns>Returns the direction towards the player</returns>
        private Vector2 CalculatePlayerDirection()
        {
            // Get reference to player
            Player player = GameWorld.Instance.FindObjectOfType<Player>() as Player;
            
            // Get the direction
            Vector2 targetDirection = Vector2.Subtract(player.GameObject.Transform.Position, GameObject.Transform.Position);
            targetDirection.Normalize();

            //multiply normalized vector by relevant speed
            targetDirection = Vector2.Multiply(targetDirection, player.Speed);

            return targetDirection;
        }
        
        #region Event methods

        private void OnCollision()
        {
            // Reference orbs collider
            Collider collider = GameObject.GetComponent<Collider>() as Collider;
            
            // Check if we collided with anything in our list of colliders
            foreach (Collider otherCollision in GameWorld.Instance.Colliders)
            {
                if (collider.CollisionBox.Intersects(otherCollision.CollisionBox))
                {
                    if (otherCollision.GameObject.Tag == "player" )
                    {
                        SoundManager.Instance.PlayClip("pickup");

                        // TriggerEvent
                        EventHandler.Instance.TriggerEvent("OnExperienceGain", new Dictionary<string, object>()
                            {
                                {"xpAmount", _xpAmount}
                            }
                        );
                        
                        GameWorld.Instance.Destroy(collider.GameObject);
                    }
                }
            }
        }

        #endregion
    }
}