using System;
using System.Collections.Generic;
using System.Threading;
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

        //Used to check for boss level
        private bool _isBossLevel;

        //Used to change level
        private int _level = 1;

        // initial amount of enemies at start of game
        private int _enemyStartAmount = 2;

        // determines amount of platform pr. level
        private int _platformAmount = 4;

        private int _objectAmount = 2; 

        // current amount of enemies through game
        public int EnemyCurrentAmount { get; set; } = 2;

        // current platform used in game
        private PlatformType _currentPlatformType;

        // currennt ground platform being used in game
        private PlatformType _currentGroundPlatform;

        // current enemy being used in gamge
        private EnemyType _currentEnemyType;

        private WorldObjectType _currenWorObjectType;

        private EnemySpawner _enemySpawner = new EnemySpawner();
        private WorldObjectSpawner _worldObjectSpawner = new WorldObjectSpawner();
        private PlatformSpawner _platformSpawner = new PlatformSpawner();

        //private PlatformGenerator _plaformSpawner = new PlatformGenerator();

        private bool canPress;

        //List for storing rectangles that contain a platform
        public List<Rectangle> UsedLocations { get; private set; }


        private LevelManager()
        {
            //subscribe to events 
            EventManager.Instance.Subscribe("NextLevel", ChangeLevel);
            EventManager.Instance.Subscribe("OnEnemyDeath", OnEnemyDeath);
        }

        /// <summary>
        /// Start a thread that executes all relevant level generation logic
        /// //LAVET AF NICHLAS HOBERG, KRISTIAN J. FICH
        /// </summary>
        public void ExecuteLevelGeneration()
        {
            new Thread(GenerateLevel) {IsBackground = true}.Start();
        }

        /// <summary>
        /// Creates platforms and relevant Enviroment objects
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        private void GenerateLevel()
        {
            //Change all relevant enum types
            ChangeEnviroment();

            //Create ground
            GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(_currentGroundPlatform, Vector2.Zero));
            
            //Create map.
            Map.Instance.CreateMap();

            //Create first portal
            GameWorld.Instance.Instantiate(WorldObjectFactory.Instance.Create(WorldObjectType.Portal, new Vector2(40, 705)));

            //Create all relevant platforms
            _platformSpawner.GeneratePlatforms(_platformAmount, _currentPlatformType);

            //Create all relevant worldObjects
            _worldObjectSpawner.ExecuteObjectSpawn(_objectAmount, _currenWorObjectType);

            //Create relevant boss or all relevant enemies 
            if (_isBossLevel) _enemySpawner.GenerateBoss(_currentEnemyType);
            else _enemySpawner.ExecuteEnemySpawn(_enemyStartAmount, _currentEnemyType);
        }

        /// <summary>
        /// Instantiates portal when level has been cleared of enemies
        /// //LAVET AF NICHLAS HOBERG
        /// </summary>
        /// <param name="ctx"></param>
        private void OnEnemyDeath(Dictionary<string, object> ctx)
        {
            EnemyCurrentAmount -= (int) ctx["enemyDeath"];

            if (EnemyCurrentAmount <= 0)
            {
                GameWorld.Instance.Instantiate(WorldObjectFactory.Instance.Create(WorldObjectType.Portal, new Vector2(1210, 700)));
            }
        }

        /// <summary>
        /// Change enemies, platforms and implement boss
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        private void ChangeEnviroment()
        {
            

            switch (_level)
            {
                case 1:
                    {
                        _currentPlatformType = PlatformType.Grass;
                        _currentGroundPlatform = PlatformType.GrassGround;
                        _currentEnemyType = EnemyType.Mushroom;
                        _currenWorObjectType = WorldObjectType.GrassObject;
                    }
                    break;
                case 7:
                    {
                        _currentPlatformType = PlatformType.Dessert;
                        _currentGroundPlatform = PlatformType.DessertGround;
                        _currentEnemyType = EnemyType.Worm;
                        _currenWorObjectType = WorldObjectType.DessertObject;
                    }
                    break;
                case 13:
                    {
                        _currentEnemyType = EnemyType.Skeleton;
                        _currentPlatformType = PlatformType.Graveyard;
                        _currentGroundPlatform = PlatformType.GraveGround;
                        _currenWorObjectType = WorldObjectType.GraveObject;
                    }
                    break;
                case 18:
                    {
                        EnemyCurrentAmount = 1;
                        _currentPlatformType = PlatformType.Graveyard;
                        _currentEnemyType = EnemyType.Reaper;
                        _currenWorObjectType = WorldObjectType.GraveObject;
                        _isBossLevel = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// Change level when event message is recieved by calling relevant level change methods
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        /// <param name="ctx">The context that gets sent from trigger in Portal.cs</param>
        private void ChangeLevel(Dictionary<string, object> ctx)
        {
            if (ctx.ContainsKey("NewLevel"))
            {
                IncrementObjectAmount();
                CleanLevel();
                ExecuteLevelGeneration();
                Console.WriteLine("Current level:" + _level);
                Console.WriteLine("Current enemyAmount:" + EnemyCurrentAmount);
            }
        }

        /// <summary>
        /// Removes all current objects from game and resets player position
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        private void CleanLevel()
        {
            //Destroy all objects besides player
            foreach (GameObject go in GameWorld.Instance.GameObjects)
            {
                if (go.Tag != "player")
                {
                    GameWorld.Instance.Destroy(go);
                }
            }

            Map.Instance.TileMap.Clear();

            //Set position of player to left corner of screen
            (GameWorld.Instance.FindObjectOfType<Player>() as Player).GameObject.Transform.Position = new Vector2(40, 705);
        }

        /// <summary>
        /// Increments level and amount of platforms. 
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        private void IncrementObjectAmount()
        {
            //increment _level
            _level++; 

            //increment _platformAmount
            _platformAmount++;

            //if level is odd increment amount of enemies
            if (_level % 2 != 0) _enemyStartAmount++;

            if (_level % 2 == 0) _objectAmount++; 

            //set EnemyCurrenAmount equal to _enemyStartAmount
            EnemyCurrentAmount = _enemyStartAmount;

            //amount of platforms capped at 19, to avoid overcrowding screen and errors
            if (_platformAmount > 19) _platformAmount = 19;
            if (_enemyStartAmount > 13) _platformAmount = 13;
            if (_objectAmount > 10) _objectAmount = 10;
        }

        /// <summary>
        /// Reset current level and associated values to initial values
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        public void ResetLevel()
        {
            //set level to level 1
            _level = 1;

            //set platform amount to 4
            _platformAmount = 4;

            _objectAmount = 2;

            //set enemyStart amount to 2
            _enemyStartAmount = 2;

            //set EnemyCurrentAmount to 2
            EnemyCurrentAmount = 2;
        }

        public void TestGenerationDEBUG()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.X) && canPress == true)
            {
                IncrementObjectAmount();
                CleanLevel();
                ExecuteLevelGeneration();
                canPress = false;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.X) && canPress == false)
            {
                canPress = true;
            }
        }
    }
}