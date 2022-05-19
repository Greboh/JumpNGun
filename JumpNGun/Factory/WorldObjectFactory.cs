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

            animator.AddAnimation(BuildAnimations("idle", new string[] { "portal_idle_1", "portal_idle_2", "portal_idle_3", "portal_idle_4", "portal_idle_5", "portal_idle_6", "portal_idle_7", "portal_idle_8", }));
            animator.AddAnimation(BuildAnimations("open", new string[] { "portal_open_1", "portal_open_2", "portal_open_3", "portal_open_4", "portal_open_5", "portal_open_6", "portal_open_7", "portal_open_8" }));
            animator.AddAnimation(BuildAnimations("close", new string[] { "portal_open_8", "portal_open_7", "portal_open_6", "portal_open_5", "portal_open_4", "portal_open_3", "portal_open_2", "portal_open_1" }));
           
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
