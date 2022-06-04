using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public enum ExperienceOrbType {Small, Medium, Large}
    
    public class ExperienceOrb : Component
    {
        private float _xpAmount;
        private Vector2 _position;
        private bool _hasCollided = false;
       

        public ExperienceOrb(float xpAmount, Vector2 position)
        {
            _xpAmount = xpAmount;
            _position = position;
        }

        #region Component Methods

        public override void Awake()
        {
        }
        
        public override void Start()
        {
            GameObject.Transform.Position = _position;
            
            //(GameObject.GetComponent<Animator>() as Animator).PlayAnimation("Idle");
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public override void Update(GameTime gameTime)
        {
            OnCollision();
        }

        #endregion

        #region Event methods

        private void OnCollision()
        {
            Collider collider = GameObject.GetComponent<Collider>() as Collider;
            
            Collider goCollider = GameObject.GetComponent<Collider>() as Collider;

            foreach (Collider otherCollision in GameWorld.Instance.Colliders)
            {
                if (goCollider.CollisionBox.Intersects(otherCollision.CollisionBox))
                {
                    if (otherCollision.GameObject.Tag == "player" )
                    {
                        Console.WriteLine($"this {GameObject.Tag} collided with {otherCollision.GameObject.Tag}");
                        
                        GameWorld.Instance.Destroy(collider.GameObject);
                        
                        EventManager.Instance.TriggerEvent("OnExperienceGain", new Dictionary<string, object>()
                            {
                                {"xpAmount", _xpAmount}
                            }
                        );

                    }
                }
            }
        }

        #endregion
    }
}