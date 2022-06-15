using System;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class MagneticPull : Ability
    {
        protected override void Create()
        {
            AbilityName = "Magnetic Pull";
            AbilityDescription = "Pulls experience orbs towards you";
            IsStartAbility = false;
        }
        public override void Selected()
        {
            Console.WriteLine($"\nAbility {AbilityName} Selected!");
            Console.WriteLine(AbilityDescription);
        }
        protected override void LoadContent()
        {
            AbilitySprite = GameWorld.Instance.Content.Load<Texture2D>("icon_MagneticPull");
            AbilityIcon = GameWorld.Instance.Content.Load<Texture2D>("iconSmall_MagneticPull");
        }
        protected override void Execute(Player player)
        {
            foreach (GameObject gameObject in GameWorld.Instance.GameObjects)
            {
                if (gameObject.HasComponent<ExperienceOrb>())
                {
                    ExperienceOrb orb = gameObject.GetComponent<ExperienceOrb>() as ExperienceOrb;
                    if (!orb.HasMagneticPull) orb.HasMagneticPull = true;
                }
            }
        }
    }
}