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
        private int _platformAmount = 4; // determines amount of platform pr. level

        //for testing
        private bool canPress = true;
        private bool canPressL = true;
        private PlatformType _currentPlatformType = PlatformType.grass;
        private PlatformType _currentGroundPlatform = PlatformType.grassGround;

        private LevelManager()
        {
            EventManager.Instance.Subscribe("NextLevel", ChangeLevel);
        }

        /// <summary>
        /// Creates platforms and relevant Enviroment objects
        /// </summary>
        public void GenerateLevel()
        {
            if (!_levelIsGenerated)
            {
                GameWorld.Instance.Instantiate(WorldObjectFactory.Instance.Create(WorldObjectType.portal, new Vector2(40, 705)));
                LevelGenerator.Instance.GeneratePlatforms(_platformAmount, _currentPlatformType);
                GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(_currentGroundPlatform));
                _levelIsGenerated = true;
                ChangeEnviroment();
            }
        }

        /// <summary>
        /// Change platform sprites according to level
        /// </summary>
        public void ChangeEnviroment()
        {
            switch (_level)
            {
                case 7:
                    {
                        _currentPlatformType = PlatformType.dessert;
                        _currentGroundPlatform = PlatformType.dessertGround;
                    }
                    break;
                case 13:
                    {
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
                _levelIsGenerated = (bool)message["NewLevel"];
                CleanLevel();
            }
        }

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
            if (Keyboard.GetState().IsKeyDown(Keys.K) && canPress)
            {
                IncrementLevel();
                _levelIsGenerated = false;
                CleanLevel();
                canPress = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.K))
            {
                canPress = true;
            }
        }

        /// <summary>
        /// Removes all current objects from game and resets player position
        /// </summary>
        private void CleanLevel()
        {
            foreach (GameObject go in GameWorld.Instance.GameObjects)
            {
                if (go.HasComponent<Platform>() || go.HasComponent<Portal>())
                {
                    GameWorld.Instance.Destroy(go);
                }
            }
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
                if (!go.HasComponent<Enemy>())
                {
                    //Level is cleared initiate portal spawn
                }
            }
        }

        /// <summary>
        /// Increments level and amount of platforms. 
        /// </summary>
        private void IncrementLevel()
        {
            _level++;
            _platformAmount++;

            //amount of platforms capped at 19, to avoid overcrowding screen and errors
            if (_platformAmount > 19)
            {
                _platformAmount = 19;
            }
        }
    }
}
