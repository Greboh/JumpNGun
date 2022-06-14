using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class DamageIncrease : Ability
    {
        public override void Create()
        {
            EventHandler.Instance.Subscribe("NextLevel", OnNextLevel);

            abilityName = "Damage increase over time";
            abilityDescription = "Increase your damage after each stage";

            amount = 1.0f;

            isStartAbility = true;
        }

        public override void Selected()
        {
            Console.WriteLine($"\nAbility {abilityName} Selected!");
            Console.WriteLine(abilityDescription);
        }

        protected override void LoadContent()
        {
            abilitySprite = GameWorld.Instance.Content.Load<Texture2D>("icon_DamageIncrease");
            abilitySmallSprite = GameWorld.Instance.Content.Load<Texture2D>("iconSmall_DamageIncrease");
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