using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
    public abstract class Factory
    {
        /// <summary>
        /// Methods for creating GameObjects by Enum type
        /// </summary>
        /// <param name="type">type of specific GameObject to be created</param>
        /// <param name="position">The Spawn position of the GameObject</param>
        /// <returns></returns>
        public abstract GameObject Create(Enum type, [Optional] Vector2 position);

        // public abstract GameObject Create(Enum type, Vector2 position);
    }
}
