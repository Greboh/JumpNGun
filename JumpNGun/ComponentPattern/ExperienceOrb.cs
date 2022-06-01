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
            EventManager.Instance.Subscribe("OnCollision", OnCollision);
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
            
        }

        #endregion

        #region Event methods

        private void OnCollision(Dictionary<string, object> ctx)
        {
            Collider collider = GameObject.GetComponent<Collider>() as Collider;
            
            GameObject collision = (GameObject) ctx["collider"];
            Rectangle collisonBox = (collision.GetComponent<Collider>() as Collider).CollisionBox;

            
            if (collider.CollisionBox.Intersects((collision.GetComponent<Collider>() as Collider).CollisionBox) && 
                                                GameWorld.Instance.GameObjects.Contains(this.GameObject) && 
                                                collisonBox.Intersects(collider.CollisionBox)
                                                && !_hasCollided) //TODO HeLP FIX ThIS
            {
                switch (collision.Tag)
                {
                    case "Player":
                    {
                        _hasCollided = true;
                        EventManager.Instance.TriggerEvent("OnExperienceGain", new Dictionary<string, object>()
                            {
                                {"xpAmount", _xpAmount}
                            }
                        );
                        
                        Console.WriteLine("orb col");
                        SoundManager.Instance.PlayClip("pickup");
                        GameWorld.Instance.Destroy(collider.GameObject);
                    }break;
                    
                    
                }
            }
        }

        #endregion
    }
}