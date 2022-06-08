using Microsoft.Xna.Framework;

namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af Nichlas Hoberg
    /// </summary>
    public class MoveCommand : ICommand
    {
        private Vector2 _velocity;

        public MoveCommand(Vector2 velocity)
        {
            this._velocity = velocity;
        }

        public void Execute(Player player)
        {
            player.Move(_velocity);
        }
    }
}