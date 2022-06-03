using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public class Animator : Component
    {
        public int CurrentIndex { get; private set; } //used to determine current animation sprite
        
        public bool IsAnimationDone { get; private set; }

        private float timeElapsed = 0; //timeElapsed through animation

        private SpriteRenderer spriteRenderer; //spriterenderer used to draw

        private Dictionary<string, Animation> animations = new Dictionary<string, Animation>(); /// dictionary used for animations. string refers to animation

        private Animation currentAnimation; //currently being animated
        

        public override void Start()
        {
            //gets spriterender component for animator
            spriteRenderer = (SpriteRenderer)GameObject.GetComponent<SpriteRenderer>();
        }

        public override void Update(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //initiates animation if no current animation is being animated
            if (currentAnimation != null)
            {
                CurrentIndex = (int)(timeElapsed * currentAnimation.FPS);

                //sets animation index to 0, so animations gets replayed - loops animation 
                if (CurrentIndex > currentAnimation.Sprites.Length - 1)
                {
                    timeElapsed = 0;
                    CurrentIndex = 0;
                    IsAnimationDone = true;
                }
                else IsAnimationDone = false;

                //set sprite to the current animation sprite
                spriteRenderer.Sprite = currentAnimation.Sprites[CurrentIndex];
            }


        }

        /// <summary>
        /// Add animation to animation dicitonary
        /// </summary>
        /// <param name="animation">animation that will be added</param>
        public void AddAnimation(Animation animation)
        {
            animations.Add(animation.Name, animation);

            if (currentAnimation == null)
            {
                currentAnimation = animation;
            }
        }

        /// <summary>
        /// Play animation based of string dictionary key
        /// </summary>
        /// <param name="animationName">Name of animation to be played</param>
        public void PlayAnimation(string animationName)
        {
            if (animationName != currentAnimation.Name && animations.ContainsKey(animationName))
            {
                // Console.WriteLine($"Playing animationSet: {animationName}");
                currentAnimation = animations[animationName];
                timeElapsed = 0;
                CurrentIndex = 0;
            }
        }
        //public Animation BuildAnimations(string animationName, string[] spriteNames)
        //{
        //    Texture2D[] sprites = new Texture2D[spriteNames.Length];

        //    for (int i = 0; i < sprites.Length; i++)
        //    {
        //        sprites[i] = GameWorld.Instance.Content.Load<Texture2D>(spriteNames[i]);
        //    }

        //    Animation anim = new Animation(animationName, sprites, 5);

        //    return anim;
        //}

    }
}

public class Animation
{
    public float FPS { get; private set; } //frames per second

    public string Name { get; private set; }//name of animation

    public Texture2D[] Sprites { get; private set; }//sprites for animation


    public Animation(string name, Texture2D[] sprites, float fps)
    {
        this.Sprites = sprites;
        this.Name = name;
        this.FPS = fps;
    }
}

