using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
    /// <summary>
    /// Different types of platforms 
    /// </summary>
    public enum PlatformType { ground, graveyard, grass, dessert }
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
        private Vector2 position = new Vector2(0, 545);

        public override GameObject Create(Enum type)
        {
            GameObject gameObject = new GameObject();
            SpriteRenderer sr = (SpriteRenderer)gameObject.AddComponent(new SpriteRenderer());

            gameObject.AddComponent(new Collider());


            switch (type)
            {
                case PlatformType.ground:
                    {

                        gameObject.AddComponent(new Platform(10, 200,position, "ground"));
                        position.X += 129;
                        sr.SetSprite("2");
                    }
                    break;
                case PlatformType.grass:
                    {
                        gameObject.AddComponent(new Platform(10, 200, new Vector2(300, 400), "grass"));
                        sr.SetSprite("Grass platform");
                    }
                    break;
                case PlatformType.dessert:
                    {
                    }
                    break;
                case PlatformType.graveyard:
                    {
                    }break;
            }
            return gameObject;
        }
    }
}
