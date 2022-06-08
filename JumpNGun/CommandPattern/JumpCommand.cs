namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af Nichlas Hoberg
    /// </summary>
    public class JumpCommand : ICommand
    {
        public void Execute(Player player)
        {
            player.Jump();
        }
    }
}