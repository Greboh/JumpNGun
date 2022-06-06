using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public class Portal : Component
    {
        private Vector2 _position; // position of portal
        private bool _open = false; //Determines animation to play
        private bool isStartPortal; //Determines if its spawn portal or next lvl portal
        private Animator _animator; // plays animations

        public Portal(Vector2 position)
        {
            _position = position;

            //Sets isStartportal to true according to position and calls HandlePlayerRendering
            if (position == new Vector2(40, 705))
            {
                isStartPortal = true;
                StopPlayerRendering();
            }
        }

        public override void Awake()
        {
            GameObject.Transform.Position = _position;
        }

        public override void Start()
        {
            _animator = GameObject.GetComponent<Animator>() as Animator;
            _animator.PlayAnimation("open");
            _open = true;
        }

        public override void Update(GameTime gameTime)
        {
            CheckCollision();
            HandleAnimations();
        }

        /// <summary>
        /// Changes between animations
        /// </summary>
        private void HandleAnimations()
        {
            //Changes from open animation to idle animation
            if (_animator.CurrentIndex == 7 && _open)
            {
                _animator.PlayAnimation("idle");
            }
            //when open is false plays close animation and calls ClosePortal()
            else if (!_open)
            {
                _animator.PlayAnimation("close");
                
                if (_animator.CurrentIndex >= 6)
                {
                    ClosePortal();
                }
            }
        }

        /// <summary>
        /// Triggers event or destorys portal when animation plays through
        /// </summary>
        private void ClosePortal()
        {
            if (!isStartPortal)
            {
                EventManager.Instance.TriggerEvent("NextLevel", new Dictionary<string, object>
                    {
                        {"NewLevel", null}
                    });
            }
            else
            {
                GameWorld.Instance.Destroy(this.GameObject);
            }
        }

        /// <summary>
        /// Sets _open to false if portal collides with player
        /// </summary>
        private void CheckCollision()
        {
            //the portal collider
            Collider _portalCollider = GameObject.GetComponent<Collider>() as Collider;

            //checks every collisionbox in game and see if they intersect with _portalCollider
            foreach (Collider otherCollider in GameWorld.Instance.Colliders)
            {
                //Terminate if the collision is the portal itself
                if (otherCollider == _portalCollider) return;

                //if other collider is player set _open to false and stop rendering of player
                if(_portalCollider.CollisionBox.Intersects(otherCollider.CollisionBox) && otherCollider.GameObject.Tag == "player")
                {
                    StopPlayerRendering();
                    _open = false;
                }
            }
        }

        /// <summary>
        /// Stops and starts player rendering
        /// </summary>
        private void StopPlayerRendering()
        {
            //TODO move handleplayerrendering to Player collision method - KRISTIAN
            foreach (GameObject go in GameWorld.Instance.GameObjects)
            {
                if (go.HasComponent<Player>() && !isStartPortal)
                {
                    (go.GetComponent<SpriteRenderer>() as SpriteRenderer).StopRendering = true;
                }
                else if (go.HasComponent<Player>() && isStartPortal)
                {
                    (go.GetComponent<SpriteRenderer>() as SpriteRenderer).StopRendering = false;
                }
            }
        }
    }
}