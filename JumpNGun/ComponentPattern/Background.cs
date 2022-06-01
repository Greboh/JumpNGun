using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    class Background
    {
        private Texture2D _backgroundimage;

        private Texture2D _cloud1;
        private Vector2 _position1;

        private Texture2D _cloud2;
        private Vector2 _position2;

        private Texture2D _cloud3;
        private Vector2 _position3;

        private Texture2D _cloud4;
        private Vector2 _position4;


        public Background()
        {
            
        }

        public void LoadContent()
        {
            _backgroundimage = GameWorld.Instance.Content.Load<Texture2D>("background_image");


            _cloud1 = GameWorld.Instance.Content.Load<Texture2D>("cloud_1");
            _position1 = new Vector2(50, 50);

            _cloud2 = GameWorld.Instance.Content.Load<Texture2D>("cloud_2");
            _position2 = new Vector2(200, 200);

            _cloud3 = GameWorld.Instance.Content.Load<Texture2D>("cloud_3");
            _position3 = new Vector2(400, 600);

            _cloud4 = GameWorld.Instance.Content.Load<Texture2D>("cloud_4");
            _position4 = new Vector2(900, 400);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_backgroundimage, new Vector2(0, 0), Color.White); // background texture

            spriteBatch.Draw(_cloud1, _position1, Color.White);
            spriteBatch.Draw(_cloud2, _position2, Color.White);
            spriteBatch.Draw(_cloud3, _position3, Color.White);
            spriteBatch.Draw(_cloud4, _position4, Color.White);


            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            _position1.X += 0.5f;
            if (_position1.X < (0 + _cloud1.Width))
            {
                
            }
            _position2.X -= 0.2f;
            _position3.X += 0.8f;
            _position4.X -= 0.6f;




        }
    }
}
