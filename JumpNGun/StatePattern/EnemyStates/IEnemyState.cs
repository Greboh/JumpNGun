namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af Nichlas Hoberg og Kristian J. Fich
    /// </summary>
    public interface IEnemyState
    {
        /// <summary>
        /// When we enter the state
        /// Called once
        /// </summary>
        /// <param name="parent"></param>
        void Enter(Enemy parent);
        
        /// <summary>
        /// When we need to update
        /// Called every update
        /// </summary>
        void Execute();

        /// <summary>
        /// Handles Animation in states
        /// </summary>
        void Animate();
        
        /// <summary>
        /// Gets called when we exit a state
        /// Called once
        /// </summary>
        void Exit();

    }
}