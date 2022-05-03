using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public interface IState
    {
        void Enter(GameObject parent);
        void Execute(GameTime gameTime);
        void Exit();

    }
}
