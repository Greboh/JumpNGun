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
        private float _xpAmount; // Amount of xp an orb gives
        private Vector2 _position; // The orb's position
       

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
                    if (otherCollision.GameObject.Tag == "player" )
                    {
                        Console.WriteLine($"this {GameObject.Tag} collided with {otherCollision.GameObject.Tag}");

                        SoundManager.Instance.PlayClip("pickup");

                        // TriggerEvent
                        EventManager.Instance.TriggerEvent("OnExperienceGain", new Dictionary<string, object>()
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