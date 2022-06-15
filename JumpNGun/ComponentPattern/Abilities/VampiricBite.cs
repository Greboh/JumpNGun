using System;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class VampiricBite : Ability
    {
        protected override void Create()
        {
            AbilityName = "Vampiric Bite";
            AbilityDescription = "Heal you for 10% of your damage whenever you hit a enemy";
            IsStartAbility = false;
        }

        public override void Selected()
        {
            Console.WriteLine($"\nAbility {AbilityName} Selected!");
            Console.WriteLine(AbilityDescription);
        }

        protected override void LoadContent()
        {
            AbilitySprite = GameWorld.Instance.Content.Load<Texture2D>("icon_VampiricBite");
            AbilityIcon = GameWorld.Instance.Content.Load<Texture2D>("iconSmall_VampiricBite");
        }

        protected override void Execute(Player player)
        {
            if (!player.HasVampiricBite) player.HasVampiricBite = true;
        }
    }
}