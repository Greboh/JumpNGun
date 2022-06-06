namespace JumpNGun
{
    /// <summary>
    /// Interfacen er lavet af Nichlas Hoberg
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes a command
        /// </summary>
        /// <param name="player"></param>
        void Execute(Player player);
    }
}