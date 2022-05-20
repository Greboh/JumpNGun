using System;
using SharpDX;

namespace JumpNGun
{
    public class ProjectileFactory : Factory
    {
        private static ProjectileFactory _instance;
        
        public static ProjectileFactory Instance
        {
            get
            {
                return _instance ??= new ProjectileFactory();
            }
        }
        
        public override GameObject Create(Enum type)
        {
            GameObject projectile = new GameObject();
            SpriteRenderer sr = (SpriteRenderer)projectile.AddComponent(new SpriteRenderer());
            projectile.AddComponent(new Collider());
            projectile.Tag = "P_Projectile";
            
            switch (type)
            {
                case CharacterType.Soldier:
                    sr.SetSprite("Bullet");
                    projectile.AddComponent(new Projectile());
                    break;
            }
            return projectile;
        }

        public override GameObject Create(Enum type, Microsoft.Xna.Framework.Vector2 position)
        {
            throw new NotImplementedException();
        }
    }
}