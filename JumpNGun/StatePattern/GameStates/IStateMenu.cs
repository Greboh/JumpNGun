using Microsoft.Xna.Framework;

namespace JumpNGun
{
    public interface IStateMenu
    {
        /// <summary>
        /// Method called when entering new state
        /// </summary>
        /// <param name="parent"></param>
        void Enter(State parent);

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