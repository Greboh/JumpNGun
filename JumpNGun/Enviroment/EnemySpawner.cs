using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace JumpNGun
{
    class EnemySpawner
    {
        private Random _random = new Random();


        public void ExecuteEnemySpawn(int amountOfEnemies, EnemyType type)
        {
            Thread spawnThread = new Thread(() => GenerateEnemies(amountOfEnemies, type));
            spawnThread.IsBackground = true;
            spawnThread.Start();
        }




        /// <summary>
        /// Generates X amount of enemies based on parameters
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        /// <param name="amountOfEnemies">amount of enemies to be instantiated</param>
        /// <param name="type">type of enemy to be instantieated</param>
        /// <param name="locations">valid locations for any given enemy</param>
        private void GenerateEnemies(int amountOfEnemies, EnemyType type)
        {
            for (int i = 0; i < amountOfEnemies; i++)
            {
                GameWorld.Instance.Instantiate(EnemyFactory.Instance.Create(type, SelectTile()));
            }
        }

        private Vector2 SelectTile()
        {
            Tile tile = Map.Instance.TileMap[_random.Next(0, Map.Instance.TileMap.Count)];
            if (!tile.HasEnemy && tile.HasPlatform)
            {
                tile.HasEnemy = true;
                return tile.EnemyPosition;
            }
            else return SelectTile();
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

