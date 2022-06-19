using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    //HELE KLASSEN ER LAVET AF KRISTAIN

    public enum WorldObjectType { Portal, GrassObject, DessertObject, GraveObject  }
    public class WorldObjectFactory : Factory
    {
        private static WorldObjectFactory _instance;

        public static WorldObjectFactory Instance
        {
            get { return _instance ??= new WorldObjectFactory(); }
        }

        private Animator _animator;
        private Random _rand = new Random();

        public override GameObject Create(Enum type, Vector2 position)
        {
            //instatiate gameobject
            GameObject gameObject = new GameObject();

            //instantiate spritrerenderer component and add to gameobject
            SpriteRenderer sr = (SpriteRenderer)gameObject.AddComponent(new SpriteRenderer());

            //instantiate animator component and add to gameobject
            _animator = (Animator)gameObject.AddComponent(new Animator());

            //add relevent component to gamobject by WorldObjectType 
            switch (type)
            {
                case WorldObjectType.Portal:
                    {
                        //add and instantiate collider component 
                        gameObject.AddComponent(new Collider());
                        gameObject.AddComponent(new Portal(position));
                        sr.SetSprite("portal_idle_1");
                        BuildPortalAnimations();
                        gameObject.Tag = "portal";
                    }
                    break;
                case WorldObjectType.DessertObject:
                    {
                        gameObject.AddComponent(new EnviromentObject(position));
                        int picker = _rand.Next(1, 4);

                        if (picker == 1)
                        {
                            gameObject.AddComponent(new EnviromentObject(new Vector2(position.X, position.Y - 55)));
                            sr.SetSprite("cactus1");
                        }
                        else if (picker == 2)
                        {
                            gameObject.AddComponent(new EnviromentObject(new Vector2(position.X, position.Y - 40)));
                            sr.SetSprite("cactus2");
                        }
                        else if (picker == 3)
                        {
                            gameObject.AddComponent(new EnviromentObject(new Vector2(position.X, position.Y - 40)));
                            sr.SetSprite("skeleton1");
                        }
                    }
                    break;
                case WorldObjectType.GrassObject:
                    {
                        int picker = _rand.Next(1, 3);
                        
                        if (picker == 1)
                        {
                            sr.SetSprite("grasstree");
                            gameObject.AddComponent(new EnviromentObject(new Vector2(position.X, position.Y - 70)));
                        }
                        else if (picker == 2)
                        {
                            sr.SetSprite("bush1");
                            gameObject.AddComponent(new EnviromentObject(new Vector2(position.X, position.Y - 30)));
                        }

                    }
                    break;
                case WorldObjectType.GraveObject:
                    {
                        gameObject.AddComponent(new EnviromentObject(position));
                        int picker = _rand.Next(1, 4);

                        if (picker == 1)
                        {
                            gameObject.AddComponent(new EnviromentObject(new Vector2(position.X, position.Y - 25)));
                            sr.SetSprite("tombstone1");
                        }
                        else if (picker == 2)
                        {
                            gameObject.AddComponent(new EnviromentObject(new Vector2(position.X, position.Y - 40)));
                            sr.SetSprite("tombstone2");
                        }
                        else if (picker == 3)
                        {
                            gameObject.AddComponent(new EnviromentObject(new Vector2(position.X, position.Y - 70)));
                            sr.SetSprite("graveTree");
                        }
                    }
                    break;
            }
            return gameObject;
        }


        private void BuildPortalAnimations()
        {
            //build all relevant animations for portal
            _animator.AddAnimation(BuildAnimations("idle", new string[] { "portal_idle_1", "portal_idle_2", "portal_idle_3", "portal_idle_4", "portal_idle_5", "portal_idle_6", "portal_idle_7", "portal_idle_8", }));
            _animator.AddAnimation(BuildAnimations("open", new string[] { "portal_open_1", "portal_open_2", "portal_open_3", "portal_open_4", "portal_open_5", "portal_open_6", "portal_open_7", "portal_open_8" }));
            _animator.AddAnimation(BuildAnimations("close", new string[] { "portal_open_8", "portal_open_7", "portal_open_6", "portal_open_5", "portal_open_4", "portal_open_3", "portal_open_2", "portal_open_1" }));
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
