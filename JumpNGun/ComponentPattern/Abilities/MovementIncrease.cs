using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class MovementIncrease : Ability
    {
        public override void Create()
        {
            EventHandler.Instance.Subscribe("NextLevel", OnNextLevel);

            abilityName = "Movement speed increase over time";
            abilityDescription = "Increase your movement speed after each stage";

            amount = 10.0f;
            
            isStartAbility = true;
        }

        public override void Selected()
        {
            Console.WriteLine($"\nAbility {abilityName} Selected!");
            Console.WriteLine(abilityDescription);
        }

        protected override void LoadContent()
        {
            abilitySprite = GameWorld.Instance.Content.Load<Texture2D>("icon_MoveSpeed");
            abilitySmallSprite = GameWorld.Instance.Content.Load<Texture2D>("iconSmall_MoveSpeed");

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