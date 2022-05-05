using System;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class PlayerBuilder : IBuilder
    {
        private PlayerType _type;
        private GameObject _gameObject;

        public PlayerBuilder(PlayerType type)
        {
            _type = type;
        }

        public void BuildGameObject()
        {
            _gameObject = new GameObject();
            
            BuildComponents(_type);

            Animator animator = (Animator) _gameObject.GetComponent<Animator>();

    

        }

        private void BuildComponents(PlayerType type)
        {
            _gameObject.AddComponent(new SpriteRenderer());
            _gameObject.AddComponent(new Animator());
            _gameObject.AddComponent(new Collider());

            switch (type)
            {
                case PlayerType.Soldier:
                    break;
            }
        }
        
        private Animation BuildMoveAnimations(string animationName, string[] spriteNames)
        {
            Texture2D[] sprites = new Texture2D[spriteNames.Length];

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = GameWorld.Instance.Content.Load<Texture2D>(spriteNames[i]);
            }

            Animation anim = new Animation(animationName, sprites, 5);
            
            return anim;

        }

        public GameObject GetResult()
        {
            return _gameObject;
        }
    }
}