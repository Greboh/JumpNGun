using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class MoveState : IState
    {
        private Enemy _parent;

        private Vector2 oldVelocity = Vector2.Zero;
        
        public void Enter(Enemy parent)
        {
            _parent = parent;

            if(parent.GameObject.HasComponent<Mushroom>())
            {
                _parent = parent.GameObject.GetComponent<Mushroom>() as Mushroom;
            }
            else if(parent.GameObject.HasComponent<Worm>())
            {
                
            }
            
            if(_parent.Velocity == Vector2.Zero)
            {
                _parent.Velocity = oldVelocity;
            }
        }

        public void Execute()
        {
            CalculateMovementDirection();
            Animate();
        }

        public void Animate()
        {
            _parent.Animator.PlayAnimation("mushroom_run");
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
            Console.WriteLine($"Velocity: { _parent.Velocity }");

            //if position is close to right, move left
            if (_parent.GameObject.Transform.Position.X >= _parent.PlatformRectangle.Right - _parent.sr.Sprite.Width)
            {
                _parent.Velocity = new Vector2(-1, 0);
                _parent.sr.SpriteEffects = SpriteEffects.FlipHorizontally;
            }
            //if position is close to left, move right
            if (_parent.GameObject.Transform.Position.X<= _parent.PlatformRectangle.Left + _parent.sr.Sprite.Width)
            {
                _parent.Velocity = new Vector2(1, 0);
                _parent.sr.SpriteEffects = SpriteEffects.None;
            }
        }
        
    }
}