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
                        gameObject.AddComponent(new Platform(10, 200, _groundPosition, "ground"));
                        sr.SetSprite("grass_ground");
                        Console.WriteLine("grass ground created");
                    }
                    break;
                case PlatformType.dessertGround:
                    {
                        gameObject.AddComponent(new Platform(10, 200, _groundPosition, "ground"));
                        sr.SetSprite("dessert_ground");
                        Console.WriteLine("dessert ground created");

                    }
                    break;
                case PlatformType.graveGround:
                    {
                        gameObject.AddComponent(new Platform(10, 200, _groundPosition, "ground"));
                        sr.SetSprite("graveyard_ground");
                        Console.WriteLine("graveyard ground created");

                    }
                    break;
            }
            return gameObject;

        }

        public override GameObject Create(Enum type, Vector2 position)
        {
            GameObject gameObject = new GameObject();
            SpriteRenderer sr = (SpriteRenderer)gameObject.AddComponent(new SpriteRenderer());
            gameObject.Tag = "ground";

            gameObject.AddComponent(new Collider());

            switch (type)
            {
                case PlatformType.grass:
                    {
                        gameObject.AddComponent(new Platform(10, 200, position, "grass"));
                        sr.SetSprite("Grass platform");
                        Console.WriteLine("Platform position: " + position);
                    }
                    break;
                case PlatformType.dessert:
                    {
                        gameObject.AddComponent(new Platform(10, 200, position, "dessert"));
                        sr.SetSprite("Desert platform");
                    }
                    break;
                case PlatformType.graveyard:
                    {
                        gameObject.AddComponent(new Platform(10, 200, position, "graveyard"));
                        sr.SetSprite("graveyard platform");
                    }
                    break;

            }
            return gameObject;
        }
    }
}
