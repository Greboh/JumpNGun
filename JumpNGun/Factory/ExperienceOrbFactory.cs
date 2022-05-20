using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class ExperienceOrbFactory : Factory
    {
        private static ExperienceOrbFactory _instance;

        public static ExperienceOrbFactory Instance
        {
            get
            {
                return _instance ??= new ExperienceOrbFactory();
            }
        }
        
        private Vector2[] _startPositions =
        {
            new Vector2(300, 700),
            new Vector2(400, 700),
            new Vector2(500, 700),
        };
        private int _positionIncrement;

        public override GameObject Create(Enum type)
        {
            GameObject orb = new GameObject();
            SpriteRenderer sr = (SpriteRenderer)orb.AddComponent(new SpriteRenderer());
            orb.AddComponent(new Collider());
            Animator animator = (Animator) orb.AddComponent(new Animator());
            orb.Tag = "Experience_Orb";

            switch (type)
            {
                case ExperienceOrbType.Small:
                {
                    sr.SetSprite("1_Small_Orb");
                    animator.AddAnimation(BuildAnimation("Idle", new []{"1_Small_Orb", "2_Small_Orb", "3_Small_Orb", "4_Small_Orb"}));
                    orb.AddComponent(new ExperienceOrb(100, new Vector2(400, 700)));

                }break;
                
                case ExperienceOrbType.Medium:
                {
                    sr.SetSprite("1_Medium_Orb");
                    animator.AddAnimation(BuildAnimation("Idle", new []{"1_Medium_Orb", "2_Medium_Orb", "3_Medium_Orb", "4_Medium_Orb"}));
                    orb.AddComponent(new ExperienceOrb(500, new Vector2(500, 700)));

                }break;

                case ExperienceOrbType.Large:
                {
                    sr.SetSprite("1_Large_Orb");
                    animator.AddAnimation(BuildAnimation("Idle", new []{"1_Large_Orb", "2_Large_Orb", "3_Large_Orb", "4_Large_Orb"}));
                    orb.AddComponent(new ExperienceOrb(1000, new Vector2(600, 700)));

                }break;
                    
            }
            
            return orb;
        }
        
        /// <summary>
        /// Build all animations relevant to movement 
        /// </summary>
        /// <param name="animationName">Name of the animation set</param>
        /// <param name="spriteNames">Name of the sprites in the animation set </param>
        /// <returns></returns>
        private Animation BuildAnimation(string animationName, string[] spriteNames)
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