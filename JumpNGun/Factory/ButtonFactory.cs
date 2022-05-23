using JumpNGun.ComponentPattern;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public enum ButtonType { Start,Settings,Highscores,Quit}
    class ButtonFactory : Factory
    {
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
                        gameObject.AddComponent(new Button(new Vector2(500,500)));
                        sr.SetSprite("start_button");
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
