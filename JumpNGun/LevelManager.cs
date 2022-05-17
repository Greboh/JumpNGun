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

        public bool CanChangeLevel { get => _canChangeLevel; set => _canChangeLevel = value; }

        private bool _levelIsGenerated = false;
        private int _level = 0;
        private int _platformAmount = 4;
        private bool _canChangeLevel = false;


        //for testing
        private bool canPress = true;


        public void GenerateLevel()
        {
            if (!_levelIsGenerated)
            {
                if (_level < 6)
                {
                    LevelGenerator.Instance.GeneratePlatforms(_platformAmount, PlatformType.grass);
                    GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(PlatformType.grassGround));
                    GameWorld.Instance.Instantiate(WorldObjectFactory.Instance.Create(WorldObjectType.portal, new Vector2(1140, 700)));
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
                }
                else
                {
                    LevelGenerator.Instance.GeneratePlatforms(_platformAmount, PlatformType.graveyard);
                    GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(PlatformType.graveGround));
                }
                IncrementLevel();
                _levelIsGenerated = true;
            }
        }


        public void ChangeLevel()
        {
            if (_canChangeLevel == true)
            {
                _levelIsGenerated = false;
                IncrementLevel();
            }
        }


        public void ChangeLevelDebug()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.K) && canPress)
            {
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
                if (go.HasComponent<Platform>())
                {
                    GameWorld.Instance.Destroy(go);
                }
            }
            (GameWorld.Instance.FindObjectOfType<Player>() as Player).GameObject.Transform.Position = new Vector2(40, 705);
        }

        /// <summary>
        /// Increments level and amount of platforms. 
        /// </summary>
        private void IncrementLevel()
        {
            _level++;
            _platformAmount++;

            //amount of platforms capped at 19, to avoid overcrowding screen and stackoverflow
            if (_platformAmount > 19)
            {
                _platformAmount = 19;
            }
        }
    }
}
