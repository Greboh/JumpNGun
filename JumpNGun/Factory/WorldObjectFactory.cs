using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public enum WorldObjectType { portal }
    public class WorldObjectFactory : Factory
    {
        private static WorldObjectFactory _instance;

        public static WorldObjectFactory Instance
        {
            get { return _instance ??= new WorldObjectFactory(); }
        }

        public override GameObject Create(Enum type)
        {

            throw new NotImplementedException();
        }

        public override GameObject Create(Enum type, Vector2 position)
        {
            GameObject gameObject = new GameObject();
            SpriteRenderer sr = (SpriteRenderer)gameObject.AddComponent(new SpriteRenderer());
            Animator animator = (Animator)gameObject.AddComponent(new Animator());
            gameObject.AddComponent(new Collider());

            //animator.AddAnimation(BuildAnimations("open", new string[] { "" }));

            switch (type)
            {
                case WorldObjectType.portal:
                    {
                        gameObject.AddComponent(new Portal(position));
                        sr.SetSprite("portal_idle_1");
                        Console.WriteLine("Portal added");
                        gameObject.Tag = "portal";
                    }
                    break;
            }
            return gameObject;
        }

        private Animation BuildAnimations(string animationName, string[] spriteNames)
        {
            Texture2D[] sprites = new Texture2D[spriteNames.Length];

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = GameWorld.Instance.Content.Load<Texture2D>(spriteNames[i]);
            }

            Animation anim = new Animation(animationName, sprites, 5);

            return anim;
        }
    }
}
