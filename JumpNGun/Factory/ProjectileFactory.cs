﻿using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
    /// <summary>
    /// LAVET AF Nichlas
    /// </summary>
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

        public override GameObject Create(Enum type, [Optional] Vector2 position)
        {
            // Create new gameObject 
            GameObject projectile = new GameObject();
            
            // Add Components
            SpriteRenderer sr = (SpriteRenderer)projectile.AddComponent(new SpriteRenderer());
            projectile.AddComponent(new Collider());
            _animator = new Animator();
            
            // Add Type specific components and Tag
            switch (type)
            {
                case CharacterType.Soldier:
                    {
                        sr.SetSprite("Bullet");
                        projectile.Tag = "p_Projectile";
                    }
                    break;
                case CharacterType.Ranger:
                    {
                        sr.SetSprite("Arrow");
                        projectile.Tag = "p_Projectile";
                    }
                    break;
                case EnemyType.Mushroom:
                    {
                        sr.SetSprite("mush_projectile");
                        projectile.Tag = "e_Projectile";
                    }
                    break;
                case EnemyType.Worm:
                    {
                        projectile.AddComponent(_animator);
                        sr.SetSprite("fireball1");
                        projectile.Tag = "e_Projectile";
                    }break;
            }

            projectile.AddComponent(new Projectile());
            // return GameObject
            return projectile;
        }
    }
}