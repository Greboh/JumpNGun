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
        private int _level = 0; // used to change level
        private int _platformAmount = 4; // determines amount of platform pr. level

        //for testing
        private bool canPress = true;
        private bool canPressL = true;

        private LevelManager()
        {
            EventManager.Instance.Subscribe("NextLevel", ChangeLevel);
        }


        public void GenerateLevel()
        {
            if (!_levelIsGenerated)
            {
                GameWorld.Instance.Instantiate(WorldObjectFactory.Instance.Create(WorldObjectType.portal, new Vector2(40, 705)));
                //TODO Refactor - NOT DONE
                if (_level < 6)
                {
                    LevelGenerator.Instance.GeneratePlatforms(_platformAmount, PlatformType.grass);
                    GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(PlatformType.grassGround));
                }
                else if (_level == 6)
                {
                    GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(PlatformType.grassGround));
                }
                else if (_level > 6 && _level < 12)
                {
                    LevelGenerator.Instance.GeneratePlatforms(_platformAmount, PlatformType.dessert);
                    GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(PlatformType.dessertGround));
                }
                else if (_level == 12)
                {
                    GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(PlatformType.dessertGround));
                }
                else if (_level > 12 && _level < 18)
                {
                    LevelGenerator.Instance.GeneratePlatforms(_platformAmount, PlatformType.graveyard);
                    GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(PlatformType.graveGround));
                }
                else if (_level == 18)
                {
                    GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(PlatformType.graveGround));
                    GameWorld.Instance.Instantiate(BossFactory.Instance.Create(BossType.DeathBoss, new Vector2(1200, 705)));
                }
                else
                {
                    LevelGenerator.Instance.GeneratePlatforms(_platformAmount, PlatformType.graveyard);
                    GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(PlatformType.graveGround));
                }
                _levelIsGenerated = true;
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
