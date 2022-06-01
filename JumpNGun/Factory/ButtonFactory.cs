using JumpNGun;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public enum ButtonType { Start,Settings,Highscores,Quit,Audio,Controls,Music,Sfx,Back,QuitToMain}
    class ButtonFactory : Factory
    {
        static int screenSizeX = (int)GameWorld.Instance.ScreenSize.X;
        static int screenSizeY = (int)GameWorld.Instance.ScreenSize.Y;

        private Vector2 _startButtonPosition = new Vector2(screenSizeX / 2, 365);
        private Vector2 _settingButtonPosition = new Vector2(screenSizeX / 2, 437);
        private Vector2 _highscoreButtonPosition = new Vector2(screenSizeX / 2, 505);
        private Vector2 _quitButtonPosition = new Vector2(screenSizeX / 2, 575);


        private ButtonType _type;
        



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

            _type = (ButtonType)type;
            
            switch (_type)
            {
                case ButtonType.Start:
                    {
                        sr.SetSprite("start_button");

                        gameObject.AddComponent(new Button(_type));
                        
                        Console.WriteLine();

                    }
                    break;
                case ButtonType.Settings:
                    {
                        sr.SetSprite("settings_button");
                        
                        gameObject.AddComponent(new Button(_type));


                    }
                    break;
                case ButtonType.Highscores:
                    {
                        sr.SetSprite("highscore_button");
                        
                        gameObject.AddComponent(new Button(_type));


                    }
                    break;
                case ButtonType.Quit:
                    {
                        sr.SetSprite("quit_button");
                        
                        gameObject.AddComponent(new Button(_type));
                        
                    }
                    break;
                case ButtonType.Audio:
                    {
                        sr.SetSprite("audio_button");

                        gameObject.AddComponent(new Button(_type));

                    }
                    break;
                case ButtonType.Controls:
                    {
                        sr.SetSprite("controls_button");

                        gameObject.AddComponent(new Button(_type));

                    }
                    break;
                case ButtonType.Music:
                    {
                        sr.SetSprite("music");

                        gameObject.AddComponent(new Button(_type));

                    }
                    break;
                case ButtonType.Sfx:
                    {
                        sr.SetSprite("sfx");

                        gameObject.AddComponent(new Button(_type));

                    }
                    break;
                case ButtonType.Back:
                    {
                        sr.SetSprite("back_button");

                        gameObject.AddComponent(new Button(_type));

                    }
                    break;
                case ButtonType.QuitToMain:
                    {
                        sr.SetSprite("quit_to_menu_button");

                        gameObject.AddComponent(new Button(_type));

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
