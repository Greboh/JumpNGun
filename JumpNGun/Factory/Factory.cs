using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
    public abstract class Factory
    {
        /// <summary>
        /// Methods for creating GameObjects by Enum type
        /// </summary>
        /// <param name="type">type of specific GameObject to be created</param>
        /// <returns></returns>
        public abstract GameObject Create(Enum type);
    }
}
