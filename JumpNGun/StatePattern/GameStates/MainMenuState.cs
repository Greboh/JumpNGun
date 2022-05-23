using JumpNGun.StatePattern.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public class MainMenuState : State
    {

        static int screenSizeX = (int)GameWorld.Instance.ScreenSize.X;
        static int screenSizeY = (int)GameWorld.Instance.ScreenSize.Y;

        private Texture2D _background_image;
        private Vector2 _backgroundPosition = new Vector2(screenSizeX / 2, 190);
        private Texture2D _game_title;


        private Vector2 MousePosition;
        private Rectangle mouseRectangle;
        private bool isHovering = false;
        private bool isInitialized;

        public MainMenuState(GameWorld gameworld, GraphicsDevice graphics, ContentManager content)
            : base(gameworld, graphics, content)
        {

        }

        public override void LoadContent()
        {
            //call start method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Start();
            }

            // asset content loading
            _background_image = _content.Load<Texture2D>("background_image");
            _game_title = _content.Load<Texture2D>("game_title");
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GameWorld.Instance.GraphicsDevice.Clear(Color.Red);
            spriteBatch.Begin();

            spriteBatch.Draw(_background_image, new Vector2(0, 0), Color.White);

            
            spriteBatch.Draw(_game_title, new Rectangle(screenSizeY/2, 190, _game_title.Width,_game_title.Height), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);


            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Draw(spriteBatch);
            }


            spriteBatch.End();

        }

        public override void Update(GameTime gameTime)
        {
            // checking if Initialize() has run if it has then skip
            if (!isInitialized)
            {
                Initialize();
                isInitialized = true;
            }
            //DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                _gameworld.ChangeState(new MainGameState(_gameworld, _graphics, _content));
                ClearObjects();
            }

            //call update method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Update(gameTime);

            }
            if (Keyboard.GetState().IsKeyDown(Keys.T))
            {

            }

            
            CheckButtonInput();

            GameWorld.Instance.CleanUp();
        }

        //Initialize is used similar to initialize in GameWorld
        private void Initialize()
        {
            
            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Start));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Settings));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Highscores));
            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Quit));


            Console.WriteLine("Menu init");

        }

        private void CheckButtonInput()
        {

            mouseRectangle = new Rectangle(GameWorld.Instance.myMouse.X, GameWorld.Instance.myMouse.Y, 10, 10);



            foreach (GameObject go in GameWorld.Instance.gameObjects)
            {
                if (go.HasComponent<Buttons>())
                {
                    if (mouseRectangle.Intersects((go.GetComponent<Buttons>() as Buttons).Rectangle) && (go.GetComponent<Buttons>() as Buttons).Tag == "StartButton")
                    {
                        Console.WriteLine("test");
                        if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Pressed)
                        {
                            _gameworld.ChangeState(new MainGameState(_gameworld, _graphics, _content));
                            ClearObjects();
                            isInitialized = false;
                        }

                    }

                    if (mouseRectangle.Intersects((go.GetComponent<Buttons>() as Buttons).Rectangle) && (go.GetComponent<Buttons>() as Buttons).Tag == "QuitButton")
                    {

                        if (GameWorld.Instance.myMouse.LeftButton == ButtonState.Pressed)
                        {
                            //GameWorld.Instance.Exit();
                        }

                    }
                }
            }
        }

        private void ClearObjects()
        {
            foreach (GameObject go in GameWorld.Instance.gameObjects)
            {
                if (go.HasComponent<Buttons>())
                {
                    GameWorld.Instance.Destroy(go);
                    
                }
            }
        }

        public override void Init()
        {
            throw new NotImplementedException();
        }
    }
}
