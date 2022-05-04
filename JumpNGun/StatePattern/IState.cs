using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public interface IState
    {
        /// <summary>
        /// Method called when entering new state
        /// </summary>
        /// <param name="parent"></param>
        void Enter(GameObject parent);

        /// <summary>
        /// Logic to execute when in specific state
        /// </summary>
        /// <param name="gameTime"></param>
        void Execute(GameTime gameTime);

        /// <summary>
        /// Logic to execute before/as leaving state
        /// </summary>
        void Exit();

    }
}
