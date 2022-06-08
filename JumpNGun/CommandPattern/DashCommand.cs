namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af Nichlas Hoberg
    /// </summary>
    public class DashCommand : ICommand
    {
        public void Execute(Player player)
        {
            player.Dash();
        }
    }
}