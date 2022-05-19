using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
    public enum ExperienceOrbType {Small, Medium, Large}
    
    public class ExperienceOrb : Component
    {
        private float _xpAmount;
        private Vector2 _position;

        public ExperienceOrb(float xpAmount, Vector2 position)
        {
            _xpAmount = xpAmount;
            _position = position;
        }

        #region Component Methods

        public override void Awake()
        {
            EventManager.Instance.Subscribe("OnCollision", OnCollision);

            //TODO Make Awake to OnEnable and make a OnDisable!
        }



        public override void Start()
        {
            GameObject.Transform.Position = _position;
        }

        public override void Update(GameTime gameTime)
        {
        }

        #endregion
        
        private void Destroy()
        {
            GameWorld.Instance.Destroy(this.GameObject);
        }

        #region Event methods

        private void OnCollision(Dictionary<string, object> ctx)
        {
            Collider orbCollider = GameObject.GetComponent<Collider>() as Collider;

            Collider collision = (Collider) ctx["collider"];
            
            if (orbCollider.CollisionBox.Intersects(collision.CollisionBox) && GameWorld.Instance.GameObjects.Contains(this.GameObject))
            {
                switch (collision.GameObject.Tag)
                {
                    case "Player":
                    {
                        EventManager.Instance.TriggerEvent("OnExperienceGain", new Dictionary<string, object>()
                            {
                                {"xpAmount", _xpAmount}
                            }
                        );
                        
                    GameWorld.Instance.Destroy(orbCollider.GameObject);
                    }break;
                    
                    
                }
            }
        }

        #endregion
    }
}