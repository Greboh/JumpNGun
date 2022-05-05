using System;

namespace JumpNGun
{
    public enum PlayerType
    {
        Soldier
    }

    /// <summary>
    /// Singleton class
    /// </summary>
    public class PlayerFactory : Factory
    {
        private static PlayerFactory _instance;

        public static PlayerFactory Instance
        {
            get { return _instance ??= new PlayerFactory(); }
        }


        public override GameObject Create(Enum type)
        {
            GameObject gameObject = new GameObject();
            SpriteRenderer sr = (SpriteRenderer) gameObject.AddComponent(new SpriteRenderer());

            gameObject.AddComponent(new Collider());
            gameObject.AddComponent(new Animator());

            switch (type)
            {
                case PlayerType.Soldier:
                    sr.SetSprite("Soldier_idle_1");
                    gameObject.AddComponent(new Player(250));
                    break;
            }

            return gameObject;
        }
    }
}