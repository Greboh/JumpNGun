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
        private int _speed;
        private float _timeBeforeFall;

        private Vector2 _velocity = new Vector2(0, 1);
        private Vector2 _position;
        private Vector2 _moveDirection;
        

        public bool _hasBeenTouched = false;


        public Platform(int speed, int timeBeforeFall, Vector2 position)
        {
            _speed = speed;
            _timeBeforeFall = timeBeforeFall;
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
        }


        private void FallStartGround(GameTime gameTime)
        {
            if (_hasBeenTouched)
            {
                Fall(gameTime);
            }
            
        }


        private void CheckForFall()
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

        private void TEST_InitiateFall()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _hasBeenTouched = true;
            }
        }


    }
}
