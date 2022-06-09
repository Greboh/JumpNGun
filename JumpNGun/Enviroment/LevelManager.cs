using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JumpNGun
{
    /// <summary>
    /// LAVET AF Kristian
    /// </summary>
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
        private int _platformAmount = 19;

        // current amount of enemies through game
        public int EnemyCurrentAmount { get; set; } = 2;

        // current platform used in game
        private PlatformType _currentPlatformType;

        // currennt ground platform being used in game
        private PlatformType _currentGroundPlatform;

        // current enemy being used in gamge
        private EnemyType _currentEnemyType;
        private bool _canPress = true;

        //List for storing rectangles that contain a platform
        public List<Rectangle> UsedLocations { get; private set; }

        private LevelManager()
        {
            //subscribe to events 
            EventHandler.Instance.Subscribe("NextLevel", ChangeLevel);
            EventHandler.Instance.Subscribe("OnEnemyDeath", OnEnemyDeath);
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

            //Create first portal
            GameWorld.Instance.Instantiate(WorldObjectFactory.Instance.Create(WorldObjectType.Portal, new Vector2(40, 705)));

            //Create all relevant platforms
            PlatformSpawner.Instance.GeneratePlatforms(_platformAmount, _currentPlatformType);

            //Get all rectangles that contain platforms 
            UsedLocations = PlatformSpawner.Instance.GetLocations();

            //Create relevant boss or all relevant enemies 
            if (_isBossLevel) EnemySpawner.Instance.GenerateBoss(_currentEnemyType);
            else EnemySpawner.Instance.GenerateEnemies(_enemyStartAmount, _currentEnemyType, UsedLocations);
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

                    _isBossLevel = false;
                }
                    break;
                case 6:
                {
                    EnemyCurrentAmount = 1;
                    _currentPlatformType = PlatformType.Grass;
                    _currentEnemyType = EnemyType.Reaper;

                    _isBossLevel = true;
                }
                    break;
                case 7:
                {
                    EnemyCurrentAmount = 5;
                    _currentPlatformType = PlatformType.Dessert;
                    _currentGroundPlatform = PlatformType.DessertGround;
                    _currentEnemyType = EnemyType.Worm;

                    _isBossLevel = false;
                }
                    break;
                case 12:
                {
                    EnemyCurrentAmount = 1;
                    _currentPlatformType = PlatformType.Graveyard;
                    _currentEnemyType = EnemyType.Reaper;

                    _isBossLevel = true;
                }
                    break;
                case 13:
                {
                    EnemyCurrentAmount = 8;
                    _currentEnemyType = EnemyType.Skeleton;
                    _currentPlatformType = PlatformType.Graveyard;
                    _currentGroundPlatform = PlatformType.GraveGround;

                    _isBossLevel = false;
                }
                    break;
                case 18:
                {
                    EnemyCurrentAmount = 1;
                    _currentPlatformType = PlatformType.Graveyard;
                    _currentEnemyType = EnemyType.Reaper;

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
                IncrementLevel();
                CleanLevel();
                ExecuteLevelGeneration();
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

            //Clear lists containing rectangles with platforms
            UsedLocations.Clear();

            //Set position of player to left corner of screen
            (GameWorld.Instance.FindObjectOfType<Player>() as Player).GameObject.Transform.Position = new Vector2(40, 705);
        }

        /// <summary>
        /// Increments level and amount of platforms. 
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        private void IncrementLevel()
        {
            //increment _level
            _level++;

            //increment _platformAmount
            _platformAmount++;

            //if level is odd increment amount of enemies
            if (_level % 2 != 0) _enemyStartAmount++;

            //set EnemyCurrenAmount equal to _enemyStartAmount
            EnemyCurrentAmount = _enemyStartAmount;

            //amount of platforms capped at 19, to avoid overcrowding screen and errors
            if (_platformAmount > 19) _platformAmount = 19;
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

            //set enemyStart amount to 2
            _enemyStartAmount = 2;

            //set EnemyCurrentAmount to 2
            EnemyCurrentAmount = 2;

            //Clear used locations if it isn't null
            if (UsedLocations != null) UsedLocations.Clear();
        }


        public void ChangeLevelDebug()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.K) && _canPress)
            {
                _canPress = false;
                IncrementLevel();
                CleanLevel();
                ExecuteLevelGeneration();
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.K) && _canPress == false)
            {
                _canPress = true;
            }
        }
    }
}