using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af alle
    /// </summary>
    public class Animator : Component
    {
        public int CurrentIndex { get; private set; } //used to determine current animation sprite
        
        public bool IsAnimationDone { get; private set; }

        private float _timeElapsed; //timeElapsed through animation

        private SpriteRenderer _spriteRenderer; //spriterenderer used to draw

        private Dictionary<string, Animation> _animations = new Dictionary<string, Animation>(); /// dictionary used for animations. string refers to animation

        public Animation CurrentAnimation { get; private set; }//currently being animated
        
        public override void Awake()
        {
        }

        public override void Start()
        {
            //gets spriterender component for animator
            _spriteRenderer = (SpriteRenderer)GameObject.GetComponent<SpriteRenderer>();
        }

        public override void Update(GameTime gameTime)
        {
            HandleAnimationLogic(gameTime);
        }
        
        /// <summary>
        /// Handles how to play an animation
        /// </summary>
        /// <param name="gameTime"></param>
        private void HandleAnimationLogic(GameTime gameTime)
        {
            _timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //initiates animation if no current animation is being animated
            if (CurrentAnimation != null)
            {
                CurrentIndex = (int)(_timeElapsed * CurrentAnimation.FPS);

                //sets animation index to 0, so animations gets replayed - loops animation 
                if (CurrentIndex > CurrentAnimation.Sprites.Length - 1)
                {
                    _timeElapsed = 0;
                    CurrentIndex = 0;
                    IsAnimationDone = true;
                }
                else IsAnimationDone = false;

                //set sprite to the current animation sprite
                _spriteRenderer.Sprite = CurrentAnimation.Sprites[CurrentIndex];
            }

        }

        /// <summary>
        /// Add animation to animation dicitonary
        /// </summary>
        /// <param name="animation">animation that will be added</param>
        public void AddAnimation(Animation animation)
        {
            _animations.Add(animation.Name, animation);

            if (CurrentAnimation == null)
            {
                CurrentAnimation = animation;
            }
        }

        /// <summary>
        /// Play animation based of string dictionary key
        /// </summary>
        /// <param name="animationName">Name of animation to be played</param>
        public void PlayAnimation(string animationName)
        {
            if (animationName != CurrentAnimation.Name && _animations.ContainsKey(animationName))
            {
                // Console.WriteLine($"Playing animationSet: {animationName}");
                CurrentAnimation = _animations[animationName];
                _timeElapsed = 0;
                CurrentIndex = 0;
            }
        }
    }
}

public class Animation
{
    //frames per second
    public float FPS { get; private set; } 

    //name of animation
    public string Name { get; private set; }

    //sprites for animation
    public Texture2D[] Sprites { get; private set; }
    
    public Animation(string name, Texture2D[] sprites, float fps)
    {
        this.Sprites = sprites;
        this.Name = name;
        this.FPS = fps;
    }
}

