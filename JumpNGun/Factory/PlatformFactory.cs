using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
    // HELE KLASSEN ER LAVET AF KRISTIAN J. FICH 

    /// <summary>
    /// Different types of platforms 
    /// </summary>
    public enum PlatformType
    {
        GrassGround,
        DessertGround,
        GraveGround,
        Graveyard,
        Grass,
        Dessert
    }

    public class PlatformFactory : Factory
    {
        private static PlatformFactory _instance;

        public static PlatformFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlatformFactory();
                }

                return _instance;
            }
        }

        //position for ground platform
        private Vector2 _groundPosition = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width / 2), 795);

        public override GameObject Create(Enum type, [Optional] Vector2 position)
        {
            //instantiate gameobject
            GameObject gameObject = new GameObject();

            //instantiate and add SpriteRenderer component
            SpriteRenderer sr = (SpriteRenderer) gameObject.AddComponent(new SpriteRenderer());
            
            //add and instantiate collider component 
            gameObject.AddComponent(new Collider());

            // Add platform component and set relevant sprite according to the enum PlatformType
            switch (type)
            {
                case PlatformType.GrassGround:
                {
                    gameObject.AddComponent(new Platform(_groundPosition));
                    sr.SetSprite("grass_ground");
                    gameObject.Tag = "ground";
                }
                    break;
                case PlatformType.DessertGround:
                {
                    gameObject.AddComponent(new Platform(_groundPosition));
                    sr.SetSprite("dessert_ground");
                    gameObject.Tag = "ground";
                }
                    break;
                case PlatformType.GraveGround:
                {
                    gameObject.AddComponent(new Platform(_groundPosition));
                    sr.SetSprite("graveyard_ground");
                    gameObject.Tag = "ground";
                }
                    break;

                case PlatformType.Grass:
                {
                    gameObject.AddComponent(new Platform(position));
                    sr.SetSprite("Grass platform");
                    gameObject.Tag = "platform";
                }
                    break;
                case PlatformType.Dessert:
                {
                    gameObject.AddComponent(new Platform(position));
                    sr.SetSprite("Desert platform");
                    gameObject.Tag = "platform";
                }
                    break;
                case PlatformType.Graveyard:
                {
                    gameObject.AddComponent(new Platform(position));
                    sr.SetSprite("graveyard platform");
                    gameObject.Tag = "platform";
                }
                    break;
            }

            return gameObject;
        }
    }
}