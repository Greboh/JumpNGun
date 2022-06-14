using System;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class ProjectileWrap : Ability
    {
        public override void Create()
        {
            abilityName = "Projectile Wrap";
            abilityDescription = "Makes your projectiles wrap around the screen";
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
            abilitySprite = GameWorld.Instance.Content.Load<Texture2D>("icon_ProjectileWrap");
            abilitySmallSprite = GameWorld.Instance.Content.Load<Texture2D>("iconSmall_ProjectileWrap");
        }

        protected override void Execute(Player player)
        {
            if (!player.HasProjectileWrap) player.HasProjectileWrap = true;
        }
    }
}