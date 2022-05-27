using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public enum EnemyType {GrassBoss, SandBoss, DeathBoss, Mushroom}
    
    class EnemyFactory : Factory
    {
        private static EnemyFactory _instance;

        public static EnemyFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EnemyFactory();
                }
                return _instance;
            }
        }

        Animator _animator;

        public override GameObject Create(Enum type)
        {
            throw new NotImplementedException();
        }

        public override GameObject Create(Enum type, Vector2 position)
        {
            GameObject gameObject = new GameObject();
            SpriteRenderer _sr = (SpriteRenderer)gameObject.AddComponent(new SpriteRenderer());
            gameObject.AddComponent(new Collider());

            _animator = (Animator)gameObject.AddComponent(new Animator());



            switch (type)
            {
                case EnemyType.GrassBoss:
                    {
                        CreateGolemAnimations();

                    }
                    break;
                case EnemyType.SandBoss:
                    {

                    }
                    break;
                case EnemyType.DeathBoss:
                    {
                        CreateReaperAnimations();
                        gameObject.AddComponent(new Reaper(position));
                        _sr.SetSprite("reaper_idle1");
                        _animator.PlayAnimation("reaper_idle");
                    }
                    break;
                case EnemyType.Mushroom:
                    {
                        gameObject.AddComponent(new Mushroom(position));
                        _sr.SetSprite("mushroom_idle1");
                    }break;
            }

            return gameObject;
        }

        /// <summary>
        /// Adds and builds all animations for EnemyType.GrassBoss
        /// </summary>
        private void CreateGolemAnimations()
        {
            _animator.AddAnimation(BuildAnimations("golem_idle", new string[] {"golem_idle1", "golem_idle2", "golem_idle3", "golem_idle4"}));
        }

        /// <summary>
        /// Adds and builds all animations for BossType.DeathBoss
        /// </summary>
        private void CreateReaperAnimations()
        {
            _animator.AddAnimation(BuildAnimations("reaper_idle", new string[] { "reaper_idle1", "reaper_idle2", "reaper_idle3", "reaper_idle4" }));

            _animator.AddAnimation(BuildAnimations("reaper_attack", new string[] {"reaper_attack1", "reaper_attack2", "reaper_attack3", "reaper_attack4", "reaper_attack5", "reaper_attack6", "reaper_attack7", "reaper_attack8",
                        "reaper_attack9","reaper_attack10","reaper_attack11","reaper_attack12","reaper_attack13"}));

            _animator.AddAnimation(BuildAnimations("reaper_summon", new string[] { "reaper_summon1", "reaper_summon2", "reaper_summon3", "reaper_summon4", "reaper_summon5", }));

            _animator.AddAnimation(BuildAnimations("reaper_death", new string[] { "reaper_death1", "reaper_death2", "reaper_death3", "reaper_death4", "reaper_death5", "reaper_death6",
                         "reaper_death7", "reaper_death8", "reaper_death9", "reaper_death10","reaper_death11","reaper_death12","reaper_death13","reaper_death14","reaper_death15","reaper_death16","reaper_death17","reaper_death18",}));
        }

        /// <summary>
        /// Adds and builds all animations for Enemytype.Mushroom
        /// </summary>
        private void CreateMushroomAnimations()
        {
            //_animator.AddAnimation(BuildAnimations("mushroom_attack", new string[] { "mush_attack1", "mush_attack2", "mush_attack3", "mush_attack4", "mush_attack5", "mush_attack6", "mush_attack7", "mush_attack8", }));
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
