using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace JumpNGun
{
    public class PlatformSpawner
    {
        #region FOR RECTANGLES
        private Texture2D texture;

        public Rectangle TopLine { get; private set; }
        public Rectangle BottomLine { get; private set; }
        public Rectangle RightLine { get; private set; }
        public Rectangle LeftLine { get; private set; }
        #endregion

        //List of locations valid for spawn
        private List<Tile> _validLocations = new List<Tile>();

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
        private Tile _currentTile;

        //used to check if values in _validDistances has been changed
        private bool _hasAltered;


        #region FOR DRAWING RECTANGLES
        //public void Draw(SpriteBatch spritebatch)
        //{
        //    for (int i = 0; i < Map.Instance.TileMap.Count; i++)
        //    {
        //            DrawRectangle(Map.Instance.TileMap[i].Location, spritebatch);
        //    }
        //}

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
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        /// <param name="amountOfPlatforms"></param>
        public void GeneratePlatforms(int amountOfPlatforms, PlatformType type)
        {
            //set _currentRectangle equal to the returned rectangle from method
            _currentTile = SpawnFirstPlatform(type);
            
            for (int i = 0; i < amountOfPlatforms; i++)
            {
                _currentTile = GeneratePositions(_currentTile);

                //Instantiate a platform at _spawnposition
                GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(type, _currentTile.PlatformPosition));
            }
        }

        /// <summary>
        /// Instantiates ground platform and first platform on random position close to ground
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        /// <returns>current rectangle</returns>
        private Tile SpawnFirstPlatform(PlatformType type)
        {
            //Choose the first location rectangle
            Tile tile = Map.Instance.TileMap[_random.Next(21, 24)];

            //Instantiate platform in our first location;
            GameWorld.Instance.Instantiate(PlatformFactory.Instance.Create(type, tile.PlatformPosition));

            tile.HasPlatform = true;

            //return the first location rectangle
            return tile;
        }

        /// <summary>
        /// Finds new rectangle and generates position
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        /// <param name="rectangle">current rectangle</param>
        /// <returns>position and rectangle containing position</returns>
        private Tile GeneratePositions(Tile tile)
        {
            //Loop through all tiles in map
            for (int i = 0; i < Map.Instance.TileMap.Count; i++)
            {
                //calculates distance between two tile locations 
                Point distance = Map.Instance.TileMap[i].Location.Center - tile.Location.Center;

                if (_validDistances.Contains(distance) && !Map.Instance.TileMap[i].HasPlatform)
                {
                    _validLocations.Add(Map.Instance.TileMap[i]);
                }
            }
            //if list contains a valid tile we pick one at random 
            if (_validLocations.Count > 0)
            {
                tile = _validLocations[_random.Next(0, _validLocations.Count)];
                tile.HasPlatform = true;
            }
            //in case of no tiles in _validLocations we call method again with new points in _validDistances
            else
            {
                SetDistancesUp();
                return GeneratePositions(tile);
            }

            //remove valid locations from list 
            _validLocations.Clear();

            //return all valid distances to original. 
            SetDistancesBack();

            return tile;
        }

        /// <summary>
        /// Adds the value of every point in _validDistances to itself.
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        /// <param name="change"></param>
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

    }
}