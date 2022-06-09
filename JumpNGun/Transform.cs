using Microsoft.Xna.Framework;

namespace JumpNGun
{
    /// <summary>
    /// Lavet af alle
    /// </summary>
    public class Transform
    {
        public Vector2 Position { get; set; }

        /// <summary>
        /// Moves the object by adding to it's current position
        /// </summary>
        /// <param name="translation">The amount to add to the current position</param>
        public void Translate(Vector2 translation)
        {
            if (!float.IsNaN(translation.X) && !float.IsNaN(translation.Y))
            {
                Position += translation;
            }
        }

        /// <summary>
        /// Moves the object ignoring it's current position
        /// </summary>
        /// <param name="transportation">The new location of the gameobject</param>
        public void Transport(Vector2 transportation)
        {
            if (!float.IsNaN(transportation.X) && !float.IsNaN(transportation.Y))
            {
                Position = transportation;
            }
        }
    }
}
