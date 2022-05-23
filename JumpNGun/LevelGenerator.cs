using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class LevelGenerator
    {
        private static LevelGenerator _instance;

        public static LevelGenerator Instance
        {
            get { return _instance ??= new LevelGenerator(); }
        }

        #region FOR RECTANGLES

        private Texture2D texture;

        public Rectangle TopLine { get; private set; }
        public Rectangle BottomLine { get; private set; }
        public Rectangle RightLine { get; private set; }
        public Rectangle LeftLine { get; private set; }

        #endregion

        //List of locations valid for spawn
        private List<Rectangle> _validLocations = new List<Rectangle>();

        //List of locations invalid for spawn
        private List<Rectangle> _invalidLocations = new List<Rectangle>();

        //List of all possible rectangle locations on map
        private Rectangle[] _locations = new Rectangle[]
        {
            new Rectangle(0, 0, 222, 125),
            new Rectangle(222, 0, 222, 125),
            new Rectangle(444, 0, 222, 125),
            new Rectangle(666, 0, 222, 125),
            new Rectangle(888, 0, 222, 125),
            new Rectangle(1110, 0, 222, 125),

            new Rectangle(0, 125, 222, 125),
            new Rectangle(222, 125, 222, 125),
            new Rectangle(444, 125, 222, 125),
            new Rectangle(666, 125, 222, 125),
            new Rectangle(888, 125, 222, 125),
            new Rectangle(1110, 125, 222, 125),

            new Rectangle(0, 250, 222, 125),
            new Rectangle(222, 250, 222, 125),
            new Rectangle(444, 250, 222, 125),
            new Rectangle(666, 250, 222, 125),
            new Rectangle(888, 250, 222, 125),
            new Rectangle(1110, 250, 222, 125),


            new Rectangle(0, 375, 222, 125),
            new Rectangle(222, 375, 222, 125),
            new Rectangle(444, 375, 222, 125),
            new Rectangle(666, 375, 222, 125),
            new Rectangle(888, 375, 222, 125),
            new Rectangle(1110, 375, 222, 125),

            new Rectangle(0, 500, 222, 125),
            new Rectangle(222, 500, 222, 125),
            new Rectangle(444, 500, 222, 125),
            new Rectangle(666, 500, 222, 125),
            new Rectangle(888, 500, 222, 125),
            new Rectangle(1110, 500, 222, 125),

            new Rectangle(222, 625, 222, 125),
            new Rectangle(444, 625, 222, 125),
            new Rectangle(666, 625, 222, 125),
            new Rectangle(888, 625, 222, 125)
        };

        //Valid distance for a rectangle's center to a vertical/diagonal or horizontal alligned rectangle
        private Point[] _validDistances = new Point[]
        {
            new Point(222, 125),
            new Point(0, 125),
            new Point(-222, 125),
            new Point(222, 0),
            new Point(222, -125),
            new Point(-222, -125),
            new Point(-222, 0),
        };

        //Random used to pick random rectangles
        private Random _random = new Random();

        //variable to store current rectangle in
        private Rectangle _currentRectangle;

        //current position for platform spawn
        private Vector2 _spawnPosition;
        private bool _hasAltered;

        public List<Rectangle> InvalidLocations
        {
            get => _invalidLocations;
            private set => _invalidLocations = value;
        }


        #region FOR DRAWING RECTANGLES

        public void Draw(SpriteBatch spritebatch)
        {
            for (int i = 0; i < _locations.Length; i++)
            {
                DrawRectangle(_locations[i], spritebatch);
            }
        }

        public void LoadContent()
        {
            texture = GameWorld.Instance.Content.Load<Texture2D>("Pixel");
        }

        private void DrawRectangle(Rectangle collisionBox, SpriteBatch spriteBatch)
        {
            TopLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);
            BottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);
            RightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);
            LeftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);


            spriteBatch.Draw(texture, TopLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(texture, BottomLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(texture, RightLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(texture, LeftLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
        }

        #endregion

        /// <summary>
        /// if true, double amount of all points in valid distances. if false return all points in valid distances to original. 
        /// </summary>
        /// <param name="change"></param>
        private void AlterValidDistances(bool change)
        {
            for (int i = 0; i < _validDistances.Length; i++)
            {
                if (change)
                {
                    if (_validDistances[i].X > 0) _validDistances[i].X += 222;
                    if (_validDistances[i].X < 0) _validDistances[i].X -= 222;
                    if (_validDistances[i].Y > 0) _validDistances[i].Y += 125;
                    if (_validDistances[i].Y < 0) _validDistances[i].Y -= 125;
                    _hasAltered = true;
                }
                else
                {
                    if (_validDistances[i].X > 0) _validDistances[i].X = 222;
                    if (_validDistances[i].X < 0) _validDistances[i].X = -222;
                    if (_validDistances[i].Y > 0) _validDistances[i].Y = 125;
                    if (_validDistances[i].Y < 0) _validDistances[i].Y = -125;
                }
            }
        }

        /// <summary>
        /// Spawns x-amount of platforms on map
        /// </summary>
        /// <param name="amountOfPlatforms"></param>
        public void GeneratePlatforms(int amountOfPlatforms, PlatformType type)
        {
            _currentRectangle = SpawnFirstPlatform(type);

            for (int i = 0; i < amountOfPlatforms; i++)
            {
                Tuple<Vector2, Rectangle> positionData = GeneratePositions(_currentRectangle);

                //vector returned from GeneratePositionPath method
                _spawnPosition = positionData.Item1;

                //rectangle returned from GeneratePositionPath method
                _currentRectangle = positionData.Item2;

                //Instantiate a platform at _spawnposition
                GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(type, _spawnPosition));
            }

            _invalidLocations.Clear();
        }

        /// <summary>
        /// Instantiates ground platform and first platform on random position close to ground
        /// </summary>
        /// <returns>current rectangle</returns>
        private Rectangle SpawnFirstPlatform(PlatformType type)
        {
            Rectangle rectangle = _locations[_random.Next(30, 33)];
            Vector2 position = new Vector2(rectangle.Center.X, rectangle.Center.Y);
            GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(type, position));
            _invalidLocations.Add(rectangle);
            return rectangle;
        }

        /// <summary>
        /// Uses rectangle to generate platform position
        /// </summary>
        /// <param name="rectangle">current rectangle</param>
        /// <returns>position and new rectangle</returns>
        private Tuple<Vector2, Rectangle> GeneratePositions(Rectangle rectangle)
        {
            //Loops through all rectangles in array
            for (int i = 0; i < _locations.Length; i++)
            {
                //calculates distance from current rectangle 
                Point distance = _locations[i].Center - rectangle.Center;

                //loops through all points in array
                for (int j = 0; j < _validDistances.Length; j++)
                {
                    //checks if distance is viable and if _locations[i] is a valid location then adds to list
                    if (_validDistances[j].Equals(distance) && !_invalidLocations.Contains(_locations[i]))
                    {
                        _validLocations.Add(_locations[i]);
                    }
                }
            }

            //if any valid locations exists we pick a random one(rectangle)
            if (_validLocations.Count > 0)
            {
                rectangle = _validLocations[_random.Next(0, _validLocations.Count)];
            }
            //in case of no valid locations we call method recursive with new points in _validDistances
            else
            {
                AlterValidDistances(true);
                return GeneratePositions(rectangle);
            }

            //remove valid locations from list 
            _validLocations.Clear();

            //make rectangle invalid 
            _invalidLocations.Add(rectangle);

            //return all valid distances to original. 
            if (_hasAltered == true)
            {
                AlterValidDistances(false);
                _hasAltered = false;
            }


            return Tuple.Create(new Vector2(rectangle.Center.X, rectangle.Center.Y), rectangle);
        }

        /// <summary>
        /// Generates a new valid location rectangle
        /// </summary>
        /// <param name="rectangle">current location rectangle</param>
        /// <returns>valid location rectangle or recursive</returns>
        private Rectangle GenerateRandomPosition(Rectangle rectangle)
        {
            //generated random index for array
            int index = _random.Next(0, 33);

            //check if location rectangle contains platform
            if (!_invalidLocations.Contains(_locations[index]))
            {
                return _locations[index];
            }
            //return method
            else return GenerateRandomPosition(rectangle);
        }
    }
}