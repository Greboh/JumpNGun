using System;

namespace JumpNGun
{
    public interface IEnemyState
    {
        void Enter(Enemy parent);
        void Execute();

        void Animate();
        void Exit();

    }
}