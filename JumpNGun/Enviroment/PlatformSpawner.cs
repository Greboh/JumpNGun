using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class PlatformSpawner
    {
        private static PlatformSpawner _instance;

        public static PlatformSpawner Instance
        {
            get { return _instance ??= new PlatformSpawner(); }
        }

        #region  Rectangles
        
        private Texture2D _texture;

        public Rectangle TopLine { get; private set; }
        public Rectangle BottomLine { get; private set; }
        public Rectangle RightLine { get; private set; }
        public Rectangle LeftLine { get; private set; }
        #endregion

        //List of locations valid for spawn
        private List<Rectangle> _validLocations = new List<Rectangle>();

        //List of locations invalid for spawn
        private List<Rectangle> _usedLocations = new List<Rectangle>();

        //List of all possible rectangle locations on map
        private Rectangle[] _locations = new Rectangle[]
{
            new Rectangle(0, 125, 222, 125),
            new Rectangle(222, 125, 222, 125),
            new Rectangle(444, 125, 222, 125),
            new Rectangle(666, 125, 222, 125),
            new Rectangle(1110, 125, 222, 125),

            new Rectangle(0, 250, 222, 125),
            new Rectangle(444, 250, 222, 125),
            new Rectangle(666, 250, 222, 125),
            new Rectangle(888, 250, 222, 125),
            new Rectangle(1110, 250, 222, 125),

            new Rectangle(0, 375, 222, 125),
            new Rectangle(222, 375, 222, 125),
            new Rectangle(444, 375, 222, 125),
            new Rectangle(666, 375, 222, 125),
            new Rectangle(888, 375, 222, 125),

            new Rectangle(0, 500, 222, 125),
            new Rectangle(444, 500, 222, 125),
            new Rectangle(666, 500, 222, 125),
            new Rectangle(888, 500, 222, 125),
            new Rectangle(1110, 500, 222, 125),

            new Rectangle(222, 625, 222, 125),
            new Rectangle(444, 625, 222, 125),
            new Rectangle(666, 625, 222, 125),
            new Rectangle(888, 625, 222, 125)
};

        //Valid distance from a rectangle's center to a vertical, diagonal or horizontal alligned rectangle
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

        //used to check if values in _validDistances has been changed
        private bool _hasAltered;

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
            _texture = GameWorld.Instance.Content.Load<Texture2D>("Pixel");
        }

        private void DrawRectangle(Rectangle collisionBox, SpriteBatch spriteBatch)
        {
            TopLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);
            BottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);
            RightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);
            LeftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);


            spriteBatch.Draw(_texture, TopLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(_texture, BottomLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(_texture, RightLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            spriteBatch.Draw(_texture, LeftLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
        }

        #endregion

        /// <summary>
        /// Spawns x-amount of platforms on map
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        /// <param name="amountOfPlatforms"></param>
        /// <param name="type"></param>
        public void GeneratePlatforms(int amountOfPlatforms, PlatformType type)
        {
            //set _currentRectangle equal to the returned rectangle from method
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
        }

        /// <summary>
        /// Instantiates ground platform and first platform on random position close to ground
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        /// <returns>current rectangle</returns>
        private Rectangle SpawnFirstPlatform(PlatformType type)
        {
            //Choose the first location rectangle
            Rectangle rectangle = _locations[_random.Next(20, 23)];

            //Create a vector position in the center of first location rectangle
            Vector2 position = new Vector2(rectangle.Center.X, rectangle.Center.Y);

            //Instantiate platform in our first location;
            GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(type, position));

            //make rectangle invalid for platform spawn
            _usedLocations.Add(rectangle);

            //return the first location rectangle
            return rectangle;
        }

        /// <summary>
        /// Finds new rectangle and generates position
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        /// <param name="rectangle">current rectangle</param>
        /// <returns>position and rectangle containing position</returns>
        private Tuple<Vector2, Rectangle> GeneratePositions(Rectangle rectangle)
        {
            //Loops through all rectangles in array
            for (int i = 0; i < _locations.Length; i++)
            {
                //calculates distance from rectangle center to locations[i] center 
                Point distance = _locations[i].Center - rectangle.Center;

                //loops through all points in array
                for (int j = 0; j < _validDistances.Length; j++)
                {
                    //checks if distance is viable and if _locations[i] isn't used. Adds to validlocations if conditions are met
                    if (_validDistances[j].Equals(distance) && !_usedLocations.Contains(_locations[i]))
                    {
                        _validLocations.Add(_locations[i]);
                    }
                }
            }

            //if any _validLocations contains a rectangle we pick one at random 
            if (_validLocations.Count > 0)
            {
                rectangle = _validLocations[_random.Next(0, _validLocations.Count)];
            }
            //in case of no rectangles in _validLocations we call method again with new points in _validDistances
            else
            {
                SetDistancesUp();
                return GeneratePositions(rectangle);
            }

            //remove valid locations from list 
            _validLocations.Clear();

            //make rectangle invalid by adding it to _usedLocations
            _usedLocations.Add(rectangle);

            //return all valid distances to original. 
            SetDistancesBack();


            return Tuple.Create(new Vector2(rectangle.Center.X, rectangle.Center.Y), rectangle);
        }

        /// <summary>
        /// Adds the value of every point in _validDistances to itself.
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        private void SetDistancesUp()
        {
            for (int i = 0; i < _validDistances.Length; i++)
            {
                if (_validDistances[i].X > 0) _validDistances[i].X += 222;
                if (_validDistances[i].X < 0) _validDistances[i].X -= 222;
                if (_validDistances[i].Y > 0) _validDistances[i].Y += 125;
                if (_validDistances[i].Y < 0) _validDistances[i].Y -= 125;
            }
            _hasAltered = true;
        }

        /// <summary>
        /// Alters points back to original points, if they have been altered
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        private void SetDistancesBack()
        {
            if (!_hasAltered) return;

            for (int i = 0; i < _validDistances.Length; i++)
            {
                if (_validDistances[i].X > 0) _validDistances[i].X = 222;
                if (_validDistances[i].X < 0) _validDistances[i].X = -222;
                if (_validDistances[i].Y > 0) _validDistances[i].Y = 125;
                if (_validDistances[i].Y < 0) _validDistances[i].Y = -125;
            }
            _hasAltered = false;
        }

        /// <summary>
        /// Returns list of rectangles that contain a platform
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        /// <returns></returns>
        public List<Rectangle> GetLocations()
        {
            return _usedLocations;
        }

    }
}