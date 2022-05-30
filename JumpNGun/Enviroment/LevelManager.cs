using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JumpNGun
{
    public class LevelManager
    {
        private static LevelManager _instance;

        public static LevelManager Instance
        {
            get { return _instance ??= new LevelManager(); }
        }


        private bool _levelIsGenerated = false; //bool to control level generation
        private int _level = 1; // used to change level
        private int _enemyStartAmount = 2;
        private int _enemyCurrentAmount = 2;
        private int _platformAmount = 4; // determines amount of platform pr. level

        public bool LevelIsGenerated { get; set; } = false;

        //for testing
        private bool _canPress = true;
        private bool canPressL = true;
        private PlatformType _currentPlatformType;
        private PlatformType _currentGroundPlatform;
        private EnemyType _currentEnemyType;

        public List<Rectangle> UsedLocations { get; private set; }

        private LevelManager()
        {
            EventManager.Instance.Subscribe("NextLevel", ChangeLevel);
            EventManager.Instance.Subscribe("OnEnemyDeath", OnEnemyDeath); //TODO Fix another way
        }

        /// <summary>
        /// Creates platforms and relevant Enviroment objects
        /// </summary>
        public void GenerateLevel()
        {
            if (!_levelIsGenerated)
            {
                //Change all relevant enum types
                ChangeEnviroment();

                //Create first portal
                GameWorld.Instance.Instantiate(WorldObjectFactory.Instance.Create(WorldObjectType.portal, new Vector2(40, 705)));

                //Create all relevant platforms
                PlatformGenerator.Instance.GeneratePlatforms(_platformAmount, _currentPlatformType);

                UsedLocations = PlatformGenerator.Instance.GetLocations();

                //Create all relevant enemies
                EnemyGenerator.Instance.GenerateEnemies(_enemyStartAmount, _currentEnemyType, UsedLocations);

                //Create ground
                GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(_currentGroundPlatform));

                //Stop genering level
                _levelIsGenerated = true;
            }
        }

        private void OnEnemyDeath(Dictionary<string, object> ctx)
        {
            _enemyCurrentAmount -= (int) ctx["enemyDeath"];

            if (_enemyCurrentAmount <= 0)
            {
                GameWorld.Instance.Instantiate(WorldObjectFactory.Instance.Create(WorldObjectType.portal, new Vector2(1210, 700)));
            }
        }

        /// <summary>
        /// Change platform sprites according to level
        /// </summary>
        public void ChangeEnviroment()
        {
            switch (_level)
            {
                case 1:
                    {
                        _currentPlatformType = PlatformType.grass;
                        _currentGroundPlatform = PlatformType.grassGround;
                        _currentEnemyType = EnemyType.Mushroom;
                    }
                    break;
                case 7:
                    {
                        _currentPlatformType = PlatformType.dessert;
                        _currentGroundPlatform = PlatformType.dessertGround;
                        _currentEnemyType = EnemyType.Worm;
                    }
                    break;
                case 13:
                    {
                        _currentEnemyType = EnemyType.Skeleton;
                        _currentPlatformType = PlatformType.graveyard;
                        _currentGroundPlatform = PlatformType.graveGround;
                    }
                    break;
            }
        }

        /// <summary>
        /// Change level when event event message is recieved by calling relevant level change methods
        /// </summary>
        /// <param name="message"></param>
        private void ChangeLevel(Dictionary<string, object> message)
        {
            if (message.ContainsKey("NewLevel"))
            {
                IncrementLevel();
                _levelIsGenerated = (bool) message["NewLevel"];
                CleanLevel();
            }
        }

        /// <summary>
        /// Removes all current objects from game and resets player position
        /// </summary>
        private void CleanLevel()
        {
            foreach (GameObject go in GameWorld.Instance.GameObjects)
            {
                if (!go.HasComponent<Player>())
                {
                    GameWorld.Instance.Destroy(go);
                }
            }

            UsedLocations.Clear();

            (GameWorld.Instance.FindObjectOfType<Player>() as Player).GameObject.Transform.Position = new Vector2(40, 705);
            Console.Clear();
        }

        /// <summary>
        /// Iterates through list of gameobjects to check if any enemies are left in game
        /// or if the level is cleared
        /// </summary>
        public void CheckForClearedLevel()
        {
            foreach (GameObject go in GameWorld.Instance.GameObjects)
            {
                // if(EnemyAmount <= 0 && _levelIsGenerated)
                // {
                //     GameWorld.Instance.Instantiate(WorldObjectFactory.Instance.Create(WorldObjectType.portal, new Vector2(1210, 700)));
                // }
            }
        }

        /// <summary>
        /// Increments level and amount of platforms. 
        /// </summary>
        private void IncrementLevel()
        {
            _level++;
            _platformAmount++;

            if (_level % 2 != 0)
            {
                _enemyStartAmount++;
            }

            _enemyCurrentAmount = _enemyStartAmount;

            //amount of platforms capped at 19, to avoid overcrowding screen and errors
            if (_platformAmount > 19)
            {
                _platformAmount = 19;
            }
        }
        
        public void ResetLevel()
        {
            _level = 1;
            _platformAmount = 4;
        }

        #region Test Methods

        /// <summary>
        /// Check for cleared level debugging
        /// </summary>
        public void CheckForClearedLevelDebug()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.L) && canPressL)
            {
                GameWorld.Instance.Instantiate(WorldObjectFactory.Instance.Create(WorldObjectType.portal, new Vector2(1210, 700)));
                canPressL = false;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.L))
            {
                canPressL = true;
            }
        }

        /// <summary>
        /// Level change for debugging
        /// </summary>
        public void ChangeLevelDebug()
        {
            //TODO FIX THIS CALLING
            CheckForClearedLevel();

            if (Keyboard.GetState().IsKeyDown(Keys.K) && _canPress)
            {
                IncrementLevel();
                _levelIsGenerated = false;
                CleanLevel();
                _canPress = false;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.K))
            {
                _canPress = true;
            }
        }

        //TEST LEVEL FOR GENERATE LEVEL METHOD

        //private Rectangle[] testLevel = new Rectangle[]
        //{
        //    new Rectangle(0, 500, 222, 125),
        //    new Rectangle(222, 500, 222, 125),
        //    new Rectangle(444, 500, 222, 125),
        //    new Rectangle(666, 500, 222, 125),
        //    new Rectangle(888, 500, 222, 125),
        //    new Rectangle(1110, 500, 222, 125),
        //};

        //for (int i = 0; i < 3; i++)
        //{
        //    GameWorld.Instance.Instantiate(EnemyFactory.Instance.Create(EnemyType.Mushroom, new Vector2(600, 0)));
        //    GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(PlatformType.grass, new Vector2(testLevel[i].Center.X, testLevel[i].Center.Y)));
        //    LevelGenerator.Instance.InvalidLocations.Add(testLevel[i]);
        //}
        //GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(PlatformType.grassGround));

        #endregion
    }
}