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
        
        public bool StartSpawning { get => _startSpawning; set => _startSpawning = value; }

        private PlatformFactory _factory = new PlatformFactory();
        private float _spawnTimer = 20;
        private bool _startSpawning = false;

        public void SpawnGround()
        {
            GameWorld.Instance.Instantiate(_factory.Create(PlatformType.ground));

            for (int i = 0; i < 8; i++)
            {
                GameWorld.Instance.Instantiate(_factory.Create(PlatformType.startPlatform));
            }
        }

        public void SpawnPlatform()
        {
            if (_spawnTimer > GameWorld.DeltaTime && _startSpawning == true)
            {
                Console.WriteLine("Spawning platforms");
                GameWorld.Instance.Instantiate(new PlatformFactory().Create(PlatformType.grass));
            }
        }
    }
}
