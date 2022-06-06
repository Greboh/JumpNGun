using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af Nichlas Hoberg
    /// </summary>
    public enum ExperienceOrbType {Small, Medium, Large}
    
    public class ExperienceOrb : Component
    {
        private float _xpAmount; // Amount of xp an orb gives
        private Vector2 _position; // The orb's position
        private bool _hasCollided = false; // if the orb has collided with player
       

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
        }

        #endregion

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
                    // Check if our collision has the tag player
                    if (otherCollision.GameObject.Tag == "player" )
                    {
                        // TriggerEvent
                        EventManager.Instance.TriggerEvent("OnExperienceGain", new Dictionary<string, object>()
                            {
                                {"xpAmount", _xpAmount}
                            }
                        );

                        // Destroy this object
                        GameWorld.Instance.Destroy(collider.GameObject);
                    }
                }
            }
        }

        #endregion
    }
}