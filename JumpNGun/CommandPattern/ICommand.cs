namespace JumpNGun
{
    public interface ICommand
    {
        /// <summary>
        /// Executes a command
        /// </summary>
        /// <param name="player"></param>
        void Execute(Player player);
    }
}