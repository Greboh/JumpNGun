namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af Nichlas Hoberg
    /// </summary>
    public class ShootCommand : ICommand
    {
        public void Execute(Player player)
        {
            player.Shoot();
        }
    }
}