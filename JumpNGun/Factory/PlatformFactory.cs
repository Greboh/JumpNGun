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

        private Random _random = new Random();

        private Vector2[] _startPositions =
        {
            new Vector2(300, 400),
            new Vector2(600, 350),
            new Vector2(800, 275),
            new Vector2(900, 225),
        };

        private Vector2[] _spawnPositions =
        {

        };

        private int _positionIncrement;

        public override GameObject Create(Enum type)
        {
            GameObject gameObject = new GameObject();
            SpriteRenderer sr = (SpriteRenderer)gameObject.AddComponent(new SpriteRenderer());
            gameObject.Tag = "ground";

            gameObject.AddComponent(new Collider());

            _positionIncrement++;

            switch (type)
            {
                case PlatformType.ground:
                    {

                        gameObject.AddComponent(new Platform(10, 200,new Vector2(600, 545), "ground"));
                      
                        sr.SetSprite("ground_platform");
                    }
                    break;
                case PlatformType.grass:
                    {

                        gameObject.AddComponent(new Platform(10, 200, _startPositions[_positionIncrement], "grass"));
                        Console.WriteLine((gameObject.GetComponent<Platform>() as Platform).Position);
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
