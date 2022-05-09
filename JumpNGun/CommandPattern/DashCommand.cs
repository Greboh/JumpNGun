using Microsoft.Xna.Framework;

namespace JumpNGun
{
    public class DashCommand : ICommand
    {
        public void Execute(Player player)
        {
            player.Dash();
        }
    }
}