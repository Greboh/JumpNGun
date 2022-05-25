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

        private Random _random  = new Random();
        private Vector2 _spawnPosition;
        private List<Rectangle> _hasEnemy = new List<Rectangle>();


        public void GenerateEnemies(int amountOfEnemies, EnemyType type, List<Rectangle> locations)
        {
            for (int i = 0; i < amountOfEnemies; i++)
            {
                _spawnPosition = GeneratePosition(locations);

                GameWorld.Instance.Instantiate(EnemyFactory.Instance.Create(type, _spawnPosition));
                Console.WriteLine("Enemy position: " + _spawnPosition);
            }
        }


        private Vector2 GeneratePosition(List<Rectangle> locations)
        {

            Rectangle rect = locations[_random.Next(0, locations.Count)];

            if (_hasEnemy.Contains(rect)) return new Vector2(rect.Center.X + 50, rect.Center.Y - 50);

            _hasEnemy.Add(rect);


            return new Vector2(rect.Center.X, rect.Center.Y - 50);
        }


    }
}
