using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace JumpNGun
{
    public class Platform : Component
    {
        private int _speed; //speed of which the platform decends with
        private float _timeBeforeFall;//time before the platform starts falling after being collided with

        private Vector2 _velocity = new Vector2(0, 1); //direction of falling
        private Vector2 _position; //position of platform
        private Vector2 _moveDirection;//caculated velocity
        private string _tag;//tag correlates to type of ground 

        private Vector2 groundPosition = new Vector2(0, 545);

        public bool _hasBeenTouched; // true or false according to player having stepped on the platform 

        public Vector2 GroundPosition { get => groundPosition; set => groundPosition = value; }

        public Platform(int speed, int timeBeforeFall, Vector2 position, string tag)
        {
            _speed = speed;
            _timeBeforeFall = timeBeforeFall;
            _tag = tag;
            _hasBeenTouched = false;
            _position = position;
        }

        public override void Awake()
        {
            SetVelocity();
        }

        public override void Start()
        {
            GameObject.Transform.Position = _position;
        }

        public override void Update(GameTime gameTime)
        {
            TEST_InitiateFall();
            FallStartGround(gameTime);
            DestroyGround();
        }

        /// <summary>
        /// Initiates fall of ground
        /// </summary>
        /// <param name="gameTime"></param>
        private void FallStartGround(GameTime gameTime)
        {
            if (_hasBeenTouched && _tag == "ground")
            {
                Fall(gameTime);
            }
        }


        private void FallPlatform()
        {

        }

        private void Fall(GameTime gameTime)
        {
            GameObject.Transform.Translate(_moveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds);

        }

        private void SetVelocity()
        {
            _moveDirection = _velocity * _speed;
        }

        private void DestroyGround()
        {
            if (this._position.Y > 600 && _tag == "ground") 
            {
                GameWorld.Instance.Destroy(this.GameObject);
                Console.WriteLine("Ground has been destroyed");
            }
        }

        private void TEST_InitiateFall()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                this._hasBeenTouched = true;
            }
        }


    }
}
