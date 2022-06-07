using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    //HELE KLASSEN ER LAVET AF KRISTIAN J. FICH 
    public enum EnemyType
    {
        GrassBoss,
        SandBoss,
        Reaper,
        Mushroom,
        Skeleton,
        Worm,
        ReaperMinion
    }

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

        public override GameObject Create(Enum type, Vector2 position)
        {
            //Instantiate gameobject
            GameObject gameObject = new GameObject();

            //Instantiate SpriteRenderer and add it to gameObject
            SpriteRenderer sr = (SpriteRenderer) gameObject.AddComponent(new SpriteRenderer());

            //Instantiate and add collider to gameObject
            gameObject.AddComponent(new Collider());

            //Set tag for gameObject to "Enemy"
            gameObject.Tag = "Enemy";

            //Instantiate and add Animtator to gameObject
            _animator = (Animator) gameObject.AddComponent(new Animator());


            /*Add relevant component by type, to instantiate relevant component for enemy. 
             * Build relevant animations and SetSprite*/
            switch (type)
            {
                case EnemyType.ReaperMinion:
                {
                    gameObject.AddComponent(new ReaperMinion(position));
                    sr.SetSprite("ghost_idle1");
                    CreateReaperMinionAnimations();
                }
                    break;
                case EnemyType.Mushroom:
                {
                    gameObject.AddComponent(new Mushroom(position));
                    sr.SetSprite("mush_run1");
                    CreateMushroomAnimations();
                }
                    break;
                case EnemyType.Skeleton:
                {
                    gameObject.AddComponent(new Skeleton(position));
                    sr.SetSprite("skelet_walk1");
                    CreateSkeletonAnimations();
                }
                    break;
                case EnemyType.Worm:
                {
                    gameObject.AddComponent(new Worm(position));
                    sr.SetSprite("worm_walk1");
                    CreateWormAnimations();
                }
                    break;
                
                case EnemyType.Reaper:
                {
                    gameObject.AddComponent(new Reaper());
                    sr.SetSprite("reaper_idle1");
                    CreateReaperAnimations();
                }
                    break;
            }

            return gameObject;
        }

        /// <summary>
        /// Adds and builds all animations for BossType.DeathBoss
        /// </summary>
        private void CreateReaperAnimations()
        {
            _animator.AddAnimation(BuildAnimations("run", new string[] {"reaper_idle1", "reaper_idle2", "reaper_idle3", "reaper_idle4"}, 10));

            _animator.AddAnimation(BuildAnimations("attack", new string[]
            {
                "reaper_attack1", "reaper_attack2", "reaper_attack3", "reaper_attack4", "reaper_attack5", "reaper_attack6", "reaper_attack7", "reaper_attack8",
                "reaper_attack9", "reaper_attack10", "reaper_attack11", "reaper_attack12", "reaper_attack13"
            }, 10));

            _animator.AddAnimation(BuildAnimations("death", new string[]
            {
                "reaper_death1", "reaper_death2", "reaper_death3", "reaper_death4", "reaper_death5", "reaper_death6",
                "reaper_death7", "reaper_death8", "reaper_death9", "reaper_death10", "reaper_death11", "reaper_death12", "reaper_death13", "reaper_death14", "reaper_death15",
                "reaper_death16", "reaper_death17", "reaper_death18",
            }, 20));

            _animator.AddAnimation(BuildAnimations("special_summon", new string[] {"reaper_summon1", "reaper_summon2", "reaper_summon3", "reaper_summon4", "reaper_summon5",}, 5));

            _animator.AddAnimation(BuildAnimations("special_teleport_first", new string[]
            {
                "reaper_death18", "reaper_death17", "reaper_death16", "reaper_death15", "reaper_death14", "reaper_death13",
                "reaper_death12", "reaper_death11", "reaper_death10", "reaper_death9", "reaper_death8", "reaper_death7",
                "reaper_death6", "reaper_death5", "reaper_death4", "reaper_death3", "reaper_death2", "reaper_death1",
            }, 30));
        }

        /// <summary>
        /// Adds and builds animations for Reaperminion
        /// </summary>
        private void CreateReaperMinionAnimations()
        {
            _animator.AddAnimation(BuildAnimations("run", new string[] {"ghost_idle1", "ghost_idle2", "ghost_idle3", "ghost_idle4",}, 5));
        }

        /// <summary>
        /// Adds and builds all animations for Enemytype.Mushroom
        /// </summary>
        private void CreateMushroomAnimations()
        {
            _animator.AddAnimation(
                BuildAnimations("run", new string[] {"mush_run1", "mush_run2", "mush_run3", "mush_run4", "mush_run5", "mush_run6", "mush_run7", "mush_run8"}, 10));
            _animator.AddAnimation(BuildAnimations("attack", new string[] {"mush_attack2", "mush_attack3",}, 3));
            _animator.AddAnimation(BuildAnimations("mushroom_gethit", new string[] {"mush_gethit1", "mush_gethit2", "mush_gethit3", "mush_gethit4"}, 5));
            _animator.AddAnimation(BuildAnimations("death", new string[] {"mush_dead1", "mush_dead2", "mush_dead3", "mush_dead4"}, 5));
        }

        /// <summary>
        /// Adds and builds all animations for EnemyType.Skeleton
        /// </summary>
        private void CreateSkeletonAnimations()
        {
            _animator.AddAnimation(BuildAnimations("run", new string[] {"skelet_walk1", "skelet_walk2", "skelet_walk3", "skelet_walk4"}, 10));
            _animator.AddAnimation(BuildAnimations("attack",
                new string[] {"skelet_attack1", "skelet_attack2", "skelet_attack3", "skelet_attack4", "skelet_attack5", "skelet_attack6", "skelet_attack7", "skelet_attack8"}, 10));
            _animator.AddAnimation(BuildAnimations("death", new string[] {"skelet_death1", "skelet_death2", "skelet_death3", "skelet_death4",}, 10));
        }

        /// <summary>
        /// Adds and builds all animations for Enemytype.Worm
        /// </summary>
        private void CreateWormAnimations()
        {
            _animator.AddAnimation(BuildAnimations("run",
                new string[] {"worm_walk1", "worm_walk2", "worm_walk3", "worm_walk4", "worm_walk5", "worm_walk6", "worm_walk7", "worm_walk8", "worm_walk8",}, 15));
            _animator.AddAnimation(BuildAnimations("attack",
                new string[] {"worm_attack1", "worm_attack2", "worm_attack3", "worm_attack4", "worm_attack5", "worm_attack6", "worm_attack7", "worm_attack8"}, 15));
            _animator.AddAnimation(BuildAnimations("death", new string[] {"worm_death1", "worm_death2", "worm_death3", "worm_death4", "worm_death5", "worm_death6", "worm_death7",},
                15));
            _animator.AddAnimation(BuildAnimations("worm_gethit", new string[] {"worm_gethit1", "worm_gethit2", "worm_gethit3",}, 15));
        }

        private Animation BuildAnimations(string animationName, string[] spriteNames, float fps)
        {
            Texture2D[] sprites = new Texture2D[spriteNames.Length];

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = GameWorld.Instance.Content.Load<Texture2D>(spriteNames[i]);
            }

            Animation anim = new Animation(animationName, sprites, fps);

            return anim;
        }
    }
}