using System;

namespace JumpNGun
{
    public interface IState
    {
        void Enter(Enemy parent);
        void Execute();

        void Animate();
        void Exit();

    }
}