using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    class EnemyGenerator
    {
        private static EnemyGenerator _instance;

        public static EnemyGenerator Instance
        {
            get { return _instance ??= new EnemyGenerator(); }
        }

        private Random _random = new Random();

        /// <summary>
        /// Generates X amount of enemies based on parameters
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        /// <param name="amountOfEnemies">amount of enemies to be instantiated</param>
        /// <param name="type">type of enemy to be instantieated</param>
        /// <param name="locations">valid locations for any given enemy</param>
        public void GenerateEnemies(int amountOfEnemies, EnemyType type, List<Rectangle> locations)
        {
            for (int i = 0; i < amountOfEnemies; i++)
            {
                GameWorld.Instance.Instantiate(EnemyFactory.Instance.Create(type, GeneratePosition(locations[i + _random.Next(1, 2)])));
            }
        }

        /// <summary>
        /// Creates and returns a a vector in the center of rect
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        /// <param name="rect">rectangle within vector will be created</param>
        /// <returns></returns>
        private Vector2 GeneratePosition(Rectangle rect)
        {
            return new Vector2(rect.Center.X, rect.Center.Y - 70);
        }

        /// <summary>
        /// Instantiate boss by type
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        /// <param name="type">type of boss to be instantiated</param>
        public void GenerateBoss(EnemyType type)
        {
            GameWorld.Instance.Instantiate(EnemyFactory.Instance.Create(type, Vector2.Zero));
        }
    }
}

