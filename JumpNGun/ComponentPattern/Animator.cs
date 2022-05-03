using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public class Animator : Component
    {
        public int CurrentIndex { get; private set; }

        private float timeElapsed = 0;

        private SpriteRenderer spriteRenderer;

        private Dictionary<string, Animation> animations = new Dictionary<string, Animation>();

        private Animation currentAnimation;

        public override void Start()
        {
            spriteRenderer = (SpriteRenderer)GameObject.GetComponent<SpriteRenderer>();
        }

        public override void Update(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentAnimation != null)
            {
                CurrentIndex = (int)(timeElapsed * currentAnimation.FPS);

                if (CurrentIndex > currentAnimation.Sprites.Length - 1)
                {
                    timeElapsed = 0;
                    CurrentIndex = 0;
                }

                spriteRenderer.Sprite = currentAnimation.Sprites[CurrentIndex];
            }


        }

        public void AddAnimation(Animation animation)
        {
            animations.Add(animation.Name, animation);

            if (currentAnimation == null)
            {
                currentAnimation = animation;
            }
        }

        public void PlayAnimation(string animationName)
        {
            if (animationName != currentAnimation.Name)
            {
                currentAnimation = animations[animationName];
                timeElapsed = 0;
                CurrentIndex = 0;
            }
        }
    }
}

public class Animation
{
    public float FPS { get; private set; }

    public string Name { get; private set; }

    public Texture2D[] Sprites { get; private set; }


    public Animation(string name, Texture2D[] sprites, float fps)
    {
        this.Sprites = sprites;
        this.Name = name;
        this.FPS = fps;
    }
}

