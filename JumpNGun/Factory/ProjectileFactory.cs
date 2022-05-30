using System;
using Microsoft.Xna.Framework.Graphics;
using SharpDX;

namespace JumpNGun
{
    public class ProjectileFactory : Factory
    {
        private static ProjectileFactory _instance;
        
        public static ProjectileFactory Instance
        {
            get
            {
                return _instance ??= new ProjectileFactory();
            }
        }
        private Animator _animator;

        public override GameObject Create(Enum type)
        {
            GameObject projectile = new GameObject();
            SpriteRenderer sr = (SpriteRenderer)projectile.AddComponent(new SpriteRenderer());
            projectile.AddComponent(new Collider());
            _animator = new Animator();

            switch (type)
            {
                case CharacterType.Soldier:
                    {
                        sr.SetSprite("Bullet");
                        projectile.Tag = "P_Projectile";
                    }
                    break;
                case CharacterType.Ranger:
                    {
                        sr.SetSprite("Arrow");
                        projectile.Tag = "P_Projectile";
                    }
                    break;
                case EnemyType.Mushroom:
                    {
                        sr.SetSprite("mush_projectile");
                        projectile.Tag = "M_Projectile";
                    }
                    break;
                case EnemyType.Worm:
                    {
                        projectile.AddComponent(_animator);
                        sr.SetSprite("fireball1");
                        CreateFireBallAnimations();
                        _animator.PlayAnimation("fireball");
                        projectile.Tag = "M_Projectile";
                    }break;
            }

            projectile.AddComponent(new Projectile());
            return projectile;
        }

        public override GameObject Create(Enum type, Microsoft.Xna.Framework.Vector2 position)
        {
            throw new NotImplementedException();
        }

        private void CreateFireBallAnimations()
        {
            _animator.AddAnimation(BuildAnimations("fireball", new string[] { "fireball1", "fireball2", "fireball3", "fireball4", "fireball5", "fireball6", }));
        }


        public Animation BuildAnimations(string animationName, string[] spriteNames)
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