using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
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

                        gameObject.AddComponent(new Platform(10, 200, position));
                        sr.SetSprite("2");

                        position.X += 125;
                    }
                    break;
                case PlatformType.grass:
                    {

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
