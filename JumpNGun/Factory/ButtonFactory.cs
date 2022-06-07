using JumpNGun;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace JumpNGun
{



    // enums of buttons used in Button.cs
    public enum ButtonType {
                            Start,
                            Settings,
                            Highscores,
                            Quit,
                            Audio,
                            Controls,
                            Music,
                            Sfx,
                            Back,
                            QuitToMain,
                            Resume,
                            SfxPause,
                            MusicPause,
                            Submit,
                            InputField,
                            Character1,
                            Character2,
                           }
    class ButtonFactory : Factory
    {
        #region fields
        private ButtonType _type; // determines which button to create.
        #endregion

        #region instance
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

        #endregion

        #region Class methods

       
        /// <summary>
        ///  Uses factory pattern to create a gameObject with the corresponding button enum and sprite
        /////LAVET AF KEAN
        /// </summary>
        /// <param name="type">used for determining button type</param>
        /// <param name="position">is optional because this isn't used on this particular class</param>
        /// <returns></returns>
        public override GameObject Create(Enum type, [Optional] Vector2 position)
        {
            GameObject gameObject = new GameObject();
            SpriteRenderer sr = (SpriteRenderer)gameObject.AddComponent(new SpriteRenderer());

            _type = (ButtonType)type; // sets _type from recived type parameter
            
            // adding sprites to buttons
            switch (_type)
            {
                case ButtonType.Start:
                    {
                        sr.SetSprite("start_button");

                        gameObject.AddComponent(new Button(_type));

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
                case ButtonType.Resume:
                    {
                        sr.SetSprite("resume_button");

                        gameObject.AddComponent(new Button(_type));

                    }
                    break;
                case ButtonType.MusicPause:
                    {
                        sr.SetSprite("music");

                        gameObject.AddComponent(new Button(_type));

                    }
                    break;
                case ButtonType.SfxPause:
                    {
                        sr.SetSprite("sfx");

                        gameObject.AddComponent(new Button(_type));

                    }
                    break;
                case ButtonType.Submit:
                    {
                        sr.SetSprite("submit_button");

                        gameObject.AddComponent(new Button(_type));

                    }
                    break;
                case ButtonType.InputField:
                    {
                        sr.SetSprite("input_field");

                        gameObject.AddComponent(new Button(_type));

                    }
                    break;
                case ButtonType.Character1:
                    {
                        sr.SetSprite("avatar_1");

                        gameObject.AddComponent(new Button(_type));

                    }
                    break;
                case ButtonType.Character2:
                    {
                        sr.SetSprite("avatar_2");

                        gameObject.AddComponent(new Button(_type));

                    }
                    break;

            }
            return gameObject;
        }
        #endregion
    }
}
