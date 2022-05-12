using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
    /// <summary>
    /// Different types of platforms 
    /// </summary>
    public enum PlatformType { ground, graveyard, grass, dessert, startPlatform }
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

        private Random _random = new Random();

        private Vector2[] _startPositions =
        {
           new Vector2(100, 450),
           new Vector2(350, 350),
           new Vector2(575, 225),
           new Vector2(800, 700),
           new Vector2(200, 600),
           new Vector2(900, 530),
           new Vector2(1000, 650),
           new Vector2(700, 400),
        };

        private int _positionIncrement;

        public override GameObject Create(Enum type)
        {
            GameObject gameObject = new GameObject();
            SpriteRenderer sr = (SpriteRenderer)gameObject.AddComponent(new SpriteRenderer());
            gameObject.Tag = "ground";

            gameObject.AddComponent(new Collider());

      

            switch (type)
            {
                case PlatformType.ground:
                    {

                        gameObject.AddComponent(new Platform(10, 200,new Vector2(600, 780), "ground"));
                        sr.SetSprite("ground_platform");
                    }
                    break;
                case PlatformType.startPlatform:
                    {
                            gameObject.AddComponent(new Platform(10, 200, _startPositions[_positionIncrement], "grass"));
                            sr.SetSprite("Grass platform");
                            _positionIncrement++;
                    
                    }
                    break;
                case PlatformType.grass:
                    {
                        gameObject.AddComponent(new Platform(10, 200, new Vector2(200,200), "grass"));
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

        public override GameObject Create(Enum type, Vector2 position)
        {
            GameObject gameObject = new GameObject();
            SpriteRenderer sr = (SpriteRenderer)gameObject.AddComponent(new SpriteRenderer());
            gameObject.Tag = "ground";

            gameObject.AddComponent(new Collider());



            switch (type)
            {
                case PlatformType.ground:
                    {

                        gameObject.AddComponent(new Platform(10, 200, position, "ground"));
                        sr.SetSprite("ground_platform");
                    }
                    break;
                case PlatformType.grass:
                    {
                        gameObject.AddComponent(new Platform(10, 200, position, "grass"));
                        sr.SetSprite("Grass platform");

                    }
                    break;
                case PlatformType.dessert:
                    {

                    }
                    break;
                case PlatformType.graveyard:
                    {
                    }
                    break;
            }
            return gameObject;
        }
    }
}
