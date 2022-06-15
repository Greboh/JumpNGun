using System;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class ProjectileWrap : Ability
    {
        protected override void Create()
        {
            AbilityName = "Projectile Wrap";
            AbilityDescription = "Makes your projectiles wrap around the screen";
            IsStartAbility = false;
        }
        public override void Selected()
        {
            Console.WriteLine($"\nAbility {AbilityName} Selected!");
            Console.WriteLine(AbilityDescription);
        }
        protected override void LoadContent()
        {
            AbilitySprite = GameWorld.Instance.Content.Load<Texture2D>("icon_ProjectileWrap");
            AbilityIcon = GameWorld.Instance.Content.Load<Texture2D>("iconSmall_ProjectileWrap");
        }
        protected override void Execute(Player player)
        {
            if (!player.HasProjectileWrap) player.HasProjectileWrap = true;
        }
    }
}