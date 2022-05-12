using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JumpNGun
{
    enum Level { Level_One, Level_Two, Level_Three }

    public class LevelManager
    {
        private static LevelManager _instance;

        public static LevelManager Instance
        {
            get { return _instance ??= new LevelManager(); }
        }

        private Level _currentLevel;
        private bool _levelIsGenerated;

        private Vector2 groundPosition = new Vector2(600, 780);

        private Vector2[] _level1Positions =
        {
           new Vector2(100, 450),
           new Vector2(350, 350),
           new Vector2(575, 225),
           new Vector2(800, 700),
           new Vector2(200, 600),
           new Vector2(900, 530),
           new Vector2(1000, 650),
           new Vector2(700, 400),
        };

        private Vector2[] _level2Positions =
        {
            new Vector2(200, 450),
           new Vector2(400, 350),
           new Vector2(600, 225),
           new Vector2(800, 700),
           new Vector2(200, 550),
           new Vector2(900, 530),
           new Vector2(1000, 650),
           new Vector2(1100, 400),
        };

        private Vector2[] _level3Positions = 
        {
           new Vector2(200, 450),
           new Vector2(400, 100),
           new Vector2(600, 225),
           new Vector2(800, 700),
           new Vector2(200, 550),
           new Vector2(700, 350),
           new Vector2(1000, 650),
           new Vector2(1100, 300),
        };

        //for testing
        private bool canPress = true;

        public void GenerateLevel()
        {

            switch (_currentLevel)
            {
                case Level.Level_One:
                    {
                        if (!_levelIsGenerated)
                        {
                            GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(PlatformType.ground, groundPosition));

                            for (int i = 0; i < _level1Positions.Length; i++)
                            {
                                GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(PlatformType.grass, _level1Positions[i]));
                            }

                            _levelIsGenerated = true;
                            Console.WriteLine("LEVEL 1");
                        }
                    }
                    break;

                case Level.Level_Two:
                    {
                        if (!_levelIsGenerated)
                        {
                            GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(PlatformType.ground, groundPosition));
                            for (int i = 0; i < _level2Positions.Length; i++)
                            {
                                GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(PlatformType.grass, _level2Positions[i]));
                            }

                            Console.WriteLine("LEVEL 2");
                        }
                        _levelIsGenerated = true;
                    }
                    break;

                case Level.Level_Three:
                    {
                        if (!_levelIsGenerated)
                        {
                            GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(PlatformType.ground, groundPosition));
                            for (int i = 0; i < _level2Positions.Length; i++)
                            {
                                GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(PlatformType.grass, _level3Positions[i]));
                            }

                            Console.WriteLine("LEVEL 3");
                        }
                        _levelIsGenerated = true;
                    }
                    break;
            }

        }

        public void ChangeLevel()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.K) && _currentLevel == Level.Level_One && canPress)
            {
                _currentLevel = Level.Level_Two;
                _levelIsGenerated = false;
                CleanLevel();
                canPress = false;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.K) && _currentLevel == Level.Level_Two && canPress)
            {
                _currentLevel = Level.Level_Three;
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
    }
}
