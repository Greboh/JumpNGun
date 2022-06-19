using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

namespace JumpNGun
{
    public class Map
    {
        private static Map _instance;

        public static Map Instance
        {
            get { return _instance ??= new Map(); }
        }

        private int x = 0;

        private int y = 125;

        private int posChange = 6;

        public bool platformTilesFilled = false;

        private List<Tile> _tileMap = new List<Tile>();

        public List<Tile> TileMap { get => _tileMap; private set => _tileMap = value; }


        public void CreateMap()
        {
            for (int i = 0; i < 30; i++)
            {
                if (i == posChange)
                {
                    y += 125;
                    x = 0;
                    posChange += 6;
                }

                if (i != 6 && i != 15 && i != 19 && i != 24 && i != 29)
                {
                    _tileMap.Add(new Tile(new Rectangle(x, y, 222, 125)));
                    Console.WriteLine(i);
                }

                x += 222;
            }
            ResetValues();
        }
        private void ResetValues()
        {
            x = 0;
            y = 125;
            posChange = 6;
        }

        public void CleanMap()
        {
            for (int i = 0; i < _tileMap.Count; i++)
            {
                _tileMap[i].HasEnemy = false;
                _tileMap[i].HasPlatform = false;
                _tileMap[i].HasWorldObject = false;
            }
        }

    }

}
