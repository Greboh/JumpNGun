using Microsoft.Xna.Framework;

namespace JumpNGun
{
    public class JumpCommand : ICommand
    {
        public void Execute(Player player)
        {
            player.Jump();
        }
    }
}