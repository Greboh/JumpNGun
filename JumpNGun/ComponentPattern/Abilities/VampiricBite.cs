using System;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class VampiricBite : Ability
    {
        public override void Create()
        {
            abilityName = "Vampiric Bite";
            abilityDescription = "Heal you for 10% of your damage whenever you hit a enemy";
            amount = 0;
            isStartAbility = false;
        }

        public override void Selected()
        {
            Console.WriteLine($"\nAbility {abilityName} Selected!");
            Console.WriteLine(abilityDescription);
        }

        protected override void LoadContent()
        {
            abilitySprite = GameWorld.Instance.Content.Load<Texture2D>("icon_VampricBite");
            abilitySmallSprite = GameWorld.Instance.Content.Load<Texture2D>("iconSmall_VampricBite");
        }

        protected override void Execute(Player player)
        {
            if (!player.HasVampiricBite) player.HasVampiricBite = true;
        }
    }
}