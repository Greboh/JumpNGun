using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
    public class PlatformSpawner
    {
        private static PlatformSpawner instance;

        public static PlatformSpawner Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlatformSpawner();
                }
                return instance;
            }
        }

        private int _spawnTimer = 20;


        public void SpawnGround()
        {
            PlatformFactory factory = new PlatformFactory();
            
            
            GameWorld.Instance.Instantiate(factory.Create(PlatformType.ground));
            
            for (int i = 0; i < 4; i++)
            {
                // GameWorld.Instance.Instantiate(new PlatformFactory().Create(PlatformType.grass));
                GameWorld.Instance.Instantiate(factory.Create(PlatformType.grass));
            }


        }

        public void Spawner()
        {
            if (_spawnTimer > GameWorld.DeltaTime)
            {
                GameWorld.Instance.Instantiate(new PlatformFactory().Create(PlatformType.grass));
            }
        }
    }
}
