using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class EnemyMoveState : IEnemyState
    {
        private Enemy _parent;

        private Vector2 oldVelocity = Vector2.Zero;
        private float _originalSpeed;
        
        public void Enter(Enemy parent)
        {
            _parent = parent;

            if(parent.GameObject.HasComponent<Mushroom>())
            {
                _parent = parent.GameObject.GetComponent<Mushroom>() as Mushroom;
            }
            else if(parent.GameObject.HasComponent<Worm>())
            {
                _parent = parent.GameObject.GetComponent<Worm>() as Worm;
            }
            else if(parent.GameObject.HasComponent<Reaper>())
            {
                _parent = parent.GameObject.GetComponent<Reaper>() as Reaper;
            }            
            else if(parent.GameObject.HasComponent<ReaperMinion>())
            {
                _parent = parent.GameObject.GetComponent<ReaperMinion>() as ReaperMinion;
            }
            else if (parent.GameObject.HasComponent<Skeleton>())
            {
                _parent = parent.GameObject.GetComponent<Skeleton>() as Skeleton;
                _originalSpeed = _parent.Speed;
            }
        }

        public void Execute()
        {
            CalculateMovementDirection();
            //SkeletonAbility();
            Animate();
        }

        public void Animate()
        {
            _parent.Animator.PlayAnimation("run");
        }

        public void Exit()
        {
            oldVelocity = _parent.Velocity;
            _parent.Velocity = Vector2.Zero;
        }
        
        
        /// <summary>
        /// Set direction of movement according to position on platform
        /// </summary>
        private void CalculateMovementDirection()
        {
            // Check if the enemy is a boss
            if (!_parent.IsBoss)
            {
                // If our velocity is zero, we revert back to our old velocity
                if (_parent.Velocity == Vector2.Zero)
                {
                    _parent.Velocity = oldVelocity;
                }

                //if position is close to right, move left
                if (_parent.GameObject.Transform.Position.X >= _parent.PlatformRectangle.Right - _parent.SpriteRenderer.Sprite.Width)
                {
                    _parent.Velocity = new Vector2(-1, 0);
                    _parent.SpriteRenderer.SpriteEffects = SpriteEffects.FlipHorizontally;
                }

                //if position is close to left, move right
                if (_parent.GameObject.Transform.Position.X <= _parent.PlatformRectangle.Left + _parent.SpriteRenderer.Sprite.Width)
                {
                    _parent.Velocity = new Vector2(1, 0);
                    _parent.SpriteRenderer.SpriteEffects = SpriteEffects.None;
                }
            }
            else
            {
                _parent.Velocity = _parent.CalculatePlayerDirection();
                
                // Flip the sprite depending on which side of the boss the player is on
                _parent.SpriteRenderer.SpriteEffects = _parent.Player.GameObject.Transform.Position.X <= _parent.GameObject.Transform.Position.X ? 
                    SpriteEffects.FlipHorizontally : SpriteEffects.None;
            } 
        }
        
        private void SkeletonAbility()
        {
            if (!_parent.GameObject.HasComponent<Skeleton>()) return;


            Collider playerCol = (_parent.Player.GameObject.GetComponent<Collider>() as Collider);

            if (playerCol.CollisionBox.Intersects(_parent.PlatformRectangle) && playerCol.CollisionBox.Bottom < _parent.PlatformRectangle.Center.Y)
            {
                _parent.Speed = 100;

                if (_parent.Player.Position.X < _parent.GameObject.Transform.Position.X)
                {
                    _parent.Velocity = new Vector2(-1, 0);
                }
                else if (_parent.Player.Position.X > _parent.GameObject.Transform.Position.X)
                {

                    _parent.Velocity = new Vector2(1, 0);
                }
            }
            else _parent.Speed = _originalSpeed;
        }
        

    }
}