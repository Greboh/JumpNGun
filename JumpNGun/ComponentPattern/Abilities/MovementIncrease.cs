using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class MovementIncrease : Ability
    {
        protected override void Create()
        {
            EventHandler.Instance.Subscribe("NextLevel", OnNextLevel);

            AbilityName = "Movement speed increase over time";
            AbilityDescription = "Increase your movement speed after each stage";

            amount = 10.0f;
            
            IsStartAbility = true;
        }

        public override void Selected()
        {
            Console.WriteLine($"\nAbility {AbilityName} Selected!");
            Console.WriteLine(AbilityDescription);
        }

        protected override void LoadContent()
        {
            AbilitySprite = GameWorld.Instance.Content.Load<Texture2D>("icon_MoveSpeed");
            AbilityIcon = GameWorld.Instance.Content.Load<Texture2D>("iconSmall_MoveSpeed");

        }

        protected override void Execute(Player player)
        {
            if (!isNextLevel) return;

            Console.WriteLine($"Old speed {player.Speed}");
            player.Speed += amount;
            Console.WriteLine($"New Speed {player.Speed}");
            isNextLevel = false;
        }
        
        private void OnNextLevel(Dictionary<string, object> obj)
        {
            isNextLevel = true;
        }
    }
}