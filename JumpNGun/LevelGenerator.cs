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

            new Rectangle(0, 0, 222, 200),
            new Rectangle(222, 0, 222, 200),
            new Rectangle(444, 0, 222, 200),
            new Rectangle(666, 0, 222, 200),
            new Rectangle(888, 0, 222, 200),
            new Rectangle(1110, 0, 222, 200),

            new Rectangle(0, 200, 222, 200),
            new Rectangle(222, 200, 222, 200),
            new Rectangle(444, 200, 222, 200),
            new Rectangle(666, 200, 222, 200),
            new Rectangle(888, 200, 222, 200),
            new Rectangle(1110, 200, 222, 200),

            new Rectangle(0, 400, 222, 200),
            new Rectangle(222, 400, 222, 200),
            new Rectangle(444, 400, 222, 200),
            new Rectangle(666, 400, 222, 200),
            new Rectangle(888, 400, 222, 200),
            new Rectangle(1110, 400, 222, 200),

            new Rectangle(0, 600, 222, 200),
            new Rectangle(222, 600, 222, 200),
            new Rectangle(444, 600, 222, 200),
            new Rectangle(666, 600, 222, 200),
            new Rectangle(888, 600, 222, 200),
            new Rectangle(1110, 600, 222, 200),
};

        /*Valid distance for a rectangle's center to a vertical/diagonal or horizontal alligned rectangle.
         Distances used to find viable spawn rectangles*/
        private Point[] _viableDistances = new Point[]
        {
            new Point(222, 200),
            new Point(0, 200),
            new Point(-222, 200),
            new Point(222, 0),
            new Point(222, -200),
            new Point(-222, -200),
            new Point(-222, 0),
        };

        //Random used to pick random rectangles
        private Random _random = new Random();

        //variable to store current rectangle in
        private Rectangle _currentRectangle;

        //current position for platform spawn
        private Vector2 _spawnPosition;
        public List<Rectangle> InvalidLocations { get => _invalidLocations; private set => _invalidLocations = value; }


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
        /// Spawns x-amount of platforms on map
        /// </summary>
        /// <param name="x"></param>
        public void GeneratePlatforms(int x, Enum type)
        {
            _currentRectangle = SpawnGroundAndFirstPlatform(type);
            
            for (int i = 0; i < x; i++)
            {
                Tuple<Vector2, Rectangle> positionData = GeneratePositionPath(_currentRectangle);

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
        private Rectangle SpawnGroundAndFirstPlatform(Enum type)
        {
            Rectangle rectangle = _locations[_random.Next(18, 23)];
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
        private Tuple<Vector2, Rectangle> GeneratePositionPath(Rectangle rectangle)
        {
            //Loops through all rectangles in array
            for (int i = 0; i < _locations.Length; i++)
            {
                //calculates distance from current rectangle 
                Point distance = _locations[i].Center - rectangle.Center;

                //loops through all points in array
                for (int j = 0; j < _viableDistances.Length; j++)
                {
                    //checks if distance is viable and if _locations[i] is a valid location then adds to list
                    if (_viableDistances[j].Equals(distance) && !_invalidLocations.Contains(_locations[i]))
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
            //in case of no valid locations we generate a new random location rectangle. 
            else
            {
                //TODO Fix algorithm - find another solution to dead end location rectangles - NOT DONE
                rectangle = GenerateRandomPosition(rectangle);
            }

            //remove valid locations from list 
            _validLocations.Clear();

            //make rectangle invalid 
            _invalidLocations.Add(rectangle);

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
            int index = _random.Next(0, 23);

            //check if location rectangle contains platform
            if (!_invalidLocations.Contains(_locations[index]))
            {
                return  _locations[index];
            }
            //return method
            else return GenerateRandomPosition(rectangle);
        }
    }
}