using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    class EnemyGenerator
    {
        //TODO - fix enemy spawning
        private static EnemyGenerator _instance;

        public static EnemyGenerator Instance
        {
            get { return _instance ??= new EnemyGenerator(); }
        }

        private Random _random = new Random();
        private Vector2 _spawnPosition;
        private List<Rectangle> _hasEnemy = new List<Rectangle>();

        /// <summary>
        /// Generates X amount of enemies based on inputs
        /// </summary>
        /// <param name="amountOfEnemies">amount of enemies to be instantiated</param>
        /// <param name="type">type of enemy to be instantieated</param>
        /// <param name="locations">location rectangle for enemey</param>
        public void GenerateEnemies(int amountOfEnemies, EnemyType type, List<Rectangle> locations)
        {

            for (int i = 0; i < amountOfEnemies; i++)
            {
                _spawnPosition = GeneratePosition(locations);

                GameWorld.Instance.Instantiate(EnemyFactory.Instance.Create(type, _spawnPosition));
                Console.WriteLine("Enemy position: " + _spawnPosition);
            }
        }

        /// <summary>
        /// Creates and returns a random Vector2 that lies within a rectangle from list
        /// </summary>
        /// <param name="locations"></param>
        /// <returns></returns>
        private Vector2 GeneratePosition(List<Rectangle> locations)
        {
            //chose random rectangle from list
            Rectangle rect = locations[_random.Next(0, locations.Count)];

            //if a vector2 already has been made within rectangle, return new in same rectangle with added X-value
            if (_hasEnemy.Contains(rect)) return new Vector2(rect.Center.X + 50, rect.Center.Y - 70);

            //add rectangle to list 
            _hasEnemy.Add(rect);

            //return Vector2 that lies within rect. 
            return new Vector2(rect.Center.X, rect.Center.Y - 70);
        }

        public void GenerateBoss(EnemyType type)
        {
            GameWorld.Instance.Instantiate(EnemyFactory.Instance.Create(type));
        }
    }
}
