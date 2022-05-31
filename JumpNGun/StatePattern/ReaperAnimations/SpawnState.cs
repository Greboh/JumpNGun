using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    class SpawnState : IState
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
            animator.PlayAnimation("reaper_spawn");
            Exit();
        }

        public void Exit()
        {
            if (!animator.IsAnimationDone) return;
            parent.ChangeState(new MoveState());
            parent
        }
    }
}
