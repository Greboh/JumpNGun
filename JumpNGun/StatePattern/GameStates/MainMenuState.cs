using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun.StatePattern.GameStates
{
    public class MainMenuState : State
    {

        public MainMenuState(GameWorld gameworld, GraphicsDevice graphics, ContentManager content)
            : base(gameworld, graphics, content)
        {

        }

        public override void LoadContent()
        {
           
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GameWorld.Instance.GraphicsDevice.Clear(Color.Red);

        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                _gameworld.ChangeState(new MainGameState(_gameworld, _graphics, _content));
            }
        }
    }
}
