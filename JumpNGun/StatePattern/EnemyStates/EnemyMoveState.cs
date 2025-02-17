﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af Nichlas Hoberg og Kristian J. Fich
    /// </summary>
    public class EnemyMoveState : IEnemyState
    {
        // Reference to Enemy
        private Enemy _parent;

        // Used to reference Enemies oldVelocity
        private Vector2 _oldVelocity = Vector2.Zero;
        
        public void Enter(Enemy parent)
        {
            // Check what enemy it is
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
            }
        }

        public void Execute()
        {
            CalculateMovementDirection();
            Animate();
        }

        /// <summary>
        /// Play Animation
        /// </summary>
        public void Animate()
        {
            _parent.Animator.PlayAnimation("run");
        }

        public void Exit()
        {
            _oldVelocity = _parent.Velocity;
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
                    _parent.Velocity = _oldVelocity;
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
    }
}