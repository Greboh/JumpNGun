using Microsoft.Xna.Framework;

namespace JumpNGun
{
    public class MoveCommand : ICommand
    {
        private Vector2 velocity;

        public MoveCommand(Vector2 velocity)
        {
            this.velocity = velocity;
        }
        
        
        public void Execute(Player player)
        {
            player.Move(velocity);
        }
    }
}