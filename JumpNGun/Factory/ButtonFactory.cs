using JumpNGun;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public enum ButtonType { Start,Settings,Highscores,Quit}
    class ButtonFactory : Factory
    {
        static int screenSizeX = (int)GameWorld.Instance.ScreenSize.X;
        static int screenSizeY = (int)GameWorld.Instance.ScreenSize.Y;

        private Vector2 _startButtonPosition = new Vector2(screenSizeX / 2, 365);
        private Vector2 _settingButtonPosition = new Vector2(screenSizeX / 2, 437);
        private Vector2 _highscoreButtonPosition = new Vector2(screenSizeX / 2, 505);
        private Vector2 _quitButtonPosition = new Vector2(screenSizeX / 2, 575);

        



        private static ButtonFactory _instance;

        public static ButtonFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ButtonFactory();
                }
                return _instance;
            }
        }

        public override GameObject Create(Enum type)
        {
            GameObject gameObject = new GameObject();
            SpriteRenderer sr = (SpriteRenderer)gameObject.AddComponent(new SpriteRenderer());

            switch (type)
            {
                case ButtonType.Start:
                    {
                        sr.SetSprite("start_button");
                        Rectangle _buttonRectangle = sr.Sprite.Bounds;
                        


                        gameObject.AddComponent(new Buttons(_startButtonPosition,new Rectangle(), "StartButton"));
                        //gameObject.AddComponent(new Button(_startButtonPosition, new Rectangle((int)_startButtonPosition.X, (int)_startButtonPosition.Y, sr.Sprite.Width, sr.Sprite.Height), "StartButton"));
                        Console.WriteLine();

                    }
                    break;
                case ButtonType.Settings:
                    {
                        sr.SetSprite("settings_button");
                        gameObject.AddComponent(new Buttons(_settingButtonPosition, new Rectangle((int)_startButtonPosition.X, (int)_startButtonPosition.Y, sr.Sprite.Width, sr.Sprite.Height), "SettingButton"));

                    }
                    break;
                case ButtonType.Highscores:
                    {
                        sr.SetSprite("highscore_button");
                        gameObject.AddComponent(new Buttons(_highscoreButtonPosition, new Rectangle((int)_startButtonPosition.X, (int)_startButtonPosition.Y, sr.Sprite.Width, sr.Sprite.Height), "HighscoreButton"));

                    }
                    break;
                case ButtonType.Quit:
                    {
                        sr.SetSprite("quit_button");
                        gameObject.AddComponent(new Buttons(_quitButtonPosition, new Rectangle((int)_startButtonPosition.X, (int)_startButtonPosition.Y, sr.Sprite.Width, sr.Sprite.Height), "QuitButton"));

                    }
                    break;
            }
            return gameObject;
        }

        public override GameObject Create(Enum type, Vector2 position)
        {
            throw new NotImplementedException();
        }

        
    }
}
