using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af Nichlas Hoberg
    /// </summary>
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

        public override GameObject Create(Enum type, [Optional] Vector2 position)
        {
            // Create new gameObject 
            GameObject orb = new GameObject(); 
            
            // Add Components
            SpriteRenderer sr = (SpriteRenderer)orb.AddComponent(new SpriteRenderer());
            orb.AddComponent(new Collider());
            Animator animator = (Animator) orb.AddComponent(new Animator());
            
            // Give the GameObject a tag
            orb.Tag = "xpOrb";  

            // Add Type specific components
            switch (type)
            {
                case ExperienceOrbType.Small:
                {
                    sr.SetSprite("1_Small_Orb");
                    animator.AddAnimation(BuildAnimation("Idle", new []{"1_Small_Orb", "2_Small_Orb", "3_Small_Orb", "4_Small_Orb"}));
                    orb.AddComponent(new ExperienceOrb(100, position));

                }break;
                
                case ExperienceOrbType.Medium:
                {
                    sr.SetSprite("1_Medium_Orb");
                    animator.AddAnimation(BuildAnimation("Idle", new []{"1_Medium_Orb", "2_Medium_Orb", "3_Medium_Orb", "4_Medium_Orb"}));
                    orb.AddComponent(new ExperienceOrb(250, position));

                }break;

                case ExperienceOrbType.Large:
                {
                    sr.SetSprite("1_Large_Orb");
                    animator.AddAnimation(BuildAnimation("Idle", new []{"1_Large_Orb", "2_Large_Orb", "3_Large_Orb", "4_Large_Orb"}));
                    orb.AddComponent(new ExperienceOrb(350, position));

                }break;
                    
            }
            return orb; // Return the Gameobject
        }
    }
}