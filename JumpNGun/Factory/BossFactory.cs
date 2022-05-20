using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public enum BossType { DeathBoss, StoneBoss, SandBoss }
    
    class BossFactory : Factory
    {
        private static BossFactory _instance;

        public static BossFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BossFactory();
                }
                return _instance;
            }
        }
        public override GameObject Create(Enum type)
        {
            throw new NotImplementedException();
        }

        public override GameObject Create(Enum type, Vector2 position)
        {
            GameObject gameObject = new GameObject();
            SpriteRenderer _sr = (SpriteRenderer)gameObject.AddComponent(new SpriteRenderer());
            gameObject.AddComponent(new Collider());

            Animator _animator = (Animator)gameObject.AddComponent(new Animator());

            _animator.AddAnimation(BuildAnimations("reaper_idle", new string[] { "reaper_idle1", "reaper_idle2", "reaper_idle3", "reaper_idle4" }));


            switch (type)
            {
                case BossType.StoneBoss:
                    {

                    }
                    break;
                case BossType.SandBoss:
                    {

                    }
                    break;
                case BossType.DeathBoss:
                    {
                        gameObject.AddComponent(new Boss(BossType.DeathBoss, position));
                        _sr.SetSprite("reaper_idle1");
                        _animator.PlayAnimation("reaper_idle");
                    }
                    break;
            }

            return gameObject;
        }

        private Animation BuildAnimations(string animationName, string[] spriteNames)
        {
            Texture2D[] sprites = new Texture2D[spriteNames.Length];

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = GameWorld.Instance.Content.Load<Texture2D>(spriteNames[i]);
            }

            Animation anim = new Animation(animationName, sprites, 5);

            return anim;
        }
    }
}
