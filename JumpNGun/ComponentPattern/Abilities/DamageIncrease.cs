using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class DamageIncrease : Ability
    {
        protected override void Create()
        {
            EventHandler.Instance.Subscribe("NextLevel", OnNextLevel);

            AbilityName = "Damage increase over time";
            AbilityDescription = "Increase your damage after each stage";

            amount = 1.0f;
            IsStartAbility = true;
        }
        public override void Selected()
        {
            Console.WriteLine($"\nAbility {AbilityName} Selected!");
            Console.WriteLine(AbilityDescription);
        }
        protected override void LoadContent()
        {
            AbilitySprite = GameWorld.Instance.Content.Load<Texture2D>("icon_DamageIncrease");
            AbilityIcon = GameWorld.Instance.Content.Load<Texture2D>("iconSmall_DamageIncrease");
        }
        protected override void Execute(Player player)
        {
            if (!isNextLevel) return;

            Console.WriteLine($"Old Damage {player.Damage}");
            player.Damage += amount;
            Console.WriteLine($"new Damage {player.Damage}");

            isNextLevel = false;
        }
        private void OnNextLevel(Dictionary<string, object> obj)
        {
            isNextLevel = true;
        }
    }
}