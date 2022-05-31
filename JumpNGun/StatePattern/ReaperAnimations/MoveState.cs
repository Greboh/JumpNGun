using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    class MoveState : IState
    {
        private Reaper parent;
        private Animator animator;

        public void Enter(Reaper parent)
        {
            this.parent = parent;
            animator = parent.GameObject.GetComponent<Animator>() as Animator;
        }

        public void Execute(GameTime gameTime)
        {
            animator.PlayAnimation("reaper_idle");
        }

        public void Exit()
        {
            
        }
    }
}
