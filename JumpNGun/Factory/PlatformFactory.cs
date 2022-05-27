using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
    /// <summary>
    /// Different types of platforms 
    /// </summary>
    public enum PlatformType { grassGround, dessertGround, graveGround, graveyard, grass, dessert, startPlatform }
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
        private Vector2 _groundPosition = new Vector2((GameWorld.Instance.GraphicsDevice.Viewport.Width /2), 795);

        /// <summary>
        /// Takes in 1 overload and creates ground
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override GameObject Create(Enum type)
        {
            GameObject gameObject = new GameObject();
            SpriteRenderer sr = (SpriteRenderer)gameObject.AddComponent(new SpriteRenderer());
            gameObject.Tag = "ground";


            gameObject.AddComponent(new Collider());

            switch (type)
            {
                case PlatformType.grassGround:
                    {
                        gameObject.AddComponent(new Platform(_groundPosition));
                        sr.SetSprite("grass_ground");

                    }
                    break;
                case PlatformType.dessertGround:
                    {
                        gameObject.AddComponent(new Platform(_groundPosition));
                        sr.SetSprite("dessert_ground");


                    }
                    break;
                case PlatformType.graveGround:
                    {
                        gameObject.AddComponent(new Platform(_groundPosition));
                        sr.SetSprite("graveyard_ground");

                    }
                    break;
            }
            
            return gameObject;

        }

        /// <summary>
        /// Takes in 2 overloads and creates floating platform
        /// </summary>
        /// <param name="type">type of platform</param>
        /// <param name="position">position of platform</param>
        /// <returns></returns>
        public override GameObject Create(Enum type, Vector2 position)
        {
            GameObject gameObject = new GameObject();
            SpriteRenderer sr = (SpriteRenderer)gameObject.AddComponent(new SpriteRenderer());
            gameObject.Tag = "platform";

            gameObject.AddComponent(new Collider());

            switch (type)
            {
                case PlatformType.grass:
                    {
                        gameObject.AddComponent(new Platform(position));
                        sr.SetSprite("Grass platform");
                        Console.WriteLine("Platform position: " + position);
                    }
                    break;
                case PlatformType.dessert:
                    {
                        gameObject.AddComponent(new Platform(position));
                        sr.SetSprite("Desert platform");
                        Console.WriteLine("Platform position: " + position);
                    }
                    break;
                case PlatformType.graveyard:
                    {
                        gameObject.AddComponent(new Platform( position));
                        sr.SetSprite("graveyard platform");
                        Console.WriteLine("Platform position: " + position);
                    }
                    break;

            }
            return gameObject;
        }
    }
}
