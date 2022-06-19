using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public class Tile
    {
        public bool HasEnemy { get; set; }
        public bool HasPlatform { get; set; }
        public bool HasWorldObject { get; set; }
        public Rectangle Location {get; set;}
        public Vector2 PlatformPosition { get; private set; }
        public Vector2 EnemyPosition { get; private set; }
        public Vector2 WorldObjectPosition { get; private set; }

        public Tile(Rectangle location)
        {
            HasEnemy = false;
            HasPlatform = false;
            HasWorldObject = false;
            Location = location;
            PlatformPosition = new Vector2(location.Center.X, location.Center.Y);
            EnemyPosition = new Vector2(location.Center.X, location.Center.Y - 70);
            WorldObjectPosition = new Vector2(location.Center.X, location.Center.Y);
        }
    }
}
