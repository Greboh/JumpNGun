using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    /// <summary>
    /// LAVET AF KEAN & NICHLAS
    /// </summary>
    public interface IStateMenu
    {
        /// <summary>
        /// Method called when entering new state
        /// </summary>
        /// <param name="parent"></param>
        void Enter(MenuStateHandler parent);

        /// <summary>
        /// Logic to execute when in specific state
        /// </summary>
        /// <param name="gameTime"></param>
        public void Execute(GameTime gameTime);

        /// <summary>
        /// Logic to Draw when in specific state
        /// </summary>
        void Draw(SpriteBatch spriteBatch);
        
        /// <summary>
        /// Logic to LoadContent when in specific state
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Logic to execute before/as leaving state
        /// </summary>
        void Exit();
    }

   
}