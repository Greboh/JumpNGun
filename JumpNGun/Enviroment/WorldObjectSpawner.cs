using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace JumpNGun
{
    class WorldObjectSpawner
    {
        private Random _rand = new Random();

        public void ExecuteObjectSpawn(int amountOfObjects, WorldObjectType type)
        {
            Thread spawnThread = new Thread(() => SpawnObjects(amountOfObjects, type));
            spawnThread.IsBackground = true;
            spawnThread.Start();
        }

        private void SpawnObjects(int amountOfObjects, WorldObjectType type)
        {
            for (int i = 0; i < amountOfObjects; i++)
            {
                GameWorld.Instance.Instantiate(WorldObjectFactory.Instance.Create(type, SelectTile()));
            }
        }

        private Vector2 SelectTile()
        {
            Tile tile = Map.Instance.TileMap[_rand.Next(0, Map.Instance.TileMap.Count)];
            if (!tile.HasWorldObject && tile.HasPlatform)
            {
                tile.HasWorldObject = true;
                return tile.WorldObjectPosition;
            }
            else return SelectTile();
        }
    }
}
