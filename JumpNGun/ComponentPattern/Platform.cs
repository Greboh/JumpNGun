using Microsoft.Xna.Framework;

namespace JumpNGun
{
    //HELE KLASSEN ER LAVET AF KRISTIAN J. FICH

    public class Platform : Component
    {
        private Vector2 _position; //position of platform
        
        /// <summary>
        /// Constrocter takes a position in for platform sprite
        /// </summary>
        /// <param name="position"></param>
        public Platform(Vector2 position)
        {
            _position = position;
        }

        public override void Awake()
        {
            GameObject.Transform.Position = _position;
        }
    }
}
