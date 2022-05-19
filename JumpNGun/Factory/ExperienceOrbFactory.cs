using System;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
    public class ExperienceOrbFactory : Factory
    {
        private static ExperienceOrbFactory _instance;

        public static ExperienceOrbFactory Instance
        {
            get
            {
                return _instance ??= new ExperienceOrbFactory();
            }
        }
        
        private Vector2[] _startPositions =
        {
            new Vector2(300, 700),
            new Vector2(400, 700),
            new Vector2(500, 700),
        };
        private int _positionIncrement;

        public override GameObject Create(Enum type)
        {
            GameObject orb = new GameObject();
            SpriteRenderer sr = (SpriteRenderer)orb.AddComponent(new SpriteRenderer());
            orb.AddComponent(new Collider());
            orb.Tag = "Experience_Orb";

            switch (type)
            {
                case ExperienceOrbType.Small:
                {
                    sr.SetSprite("1_Small_Orb");
                    orb.AddComponent(new ExperienceOrb(100, _startPositions[_positionIncrement]));
                    _positionIncrement++;

                }break;
                
                case ExperienceOrbType.Medium:
                {
                    // sr.SetSprite("Medium_Orb");

                }break;

                case ExperienceOrbType.Large:
                {
                    // sr.SetSprite("Large_Orb");

                }break;
                    
            }


            return orb;
        }
    }
}