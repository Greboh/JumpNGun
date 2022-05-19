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

        private bool _dropGround = false;//used to initiate increment of platform y-position (falling)

        public Vector2 Position { get => _position; set => _position = value; }
        public bool DropGround { get => _dropGround; set => _dropGround = value; }

        public Platform(int speed, int timeBeforeFall, Vector2 position, string tag)
        {
            _speed = speed;
            _timeBeforeFall = timeBeforeFall;
            _tag = tag;
            _position = position;
        }

        public override void Awake()
        {
            SetVelocity();
            EventManager.Instance.Subscribe("OnCollision", OnCollision);
        }

 

        public override void Start()
        {
            GameObject.Transform.Position = _position;
        }

        public override void Update(GameTime gameTime)
        {
            InitiateFalling(gameTime);
            DestroyGround();
        }

        /// <summary>
        /// Initiates fall of ground
        /// </summary>
        /// <param name="gameTime"></param>
        private void InitiateFalling(GameTime gameTime)
        {
            if (_dropGround)
            {
                Fall(gameTime);
            }
        }

        /// <summary>
        /// Moves platform and startground up the Y-axis (down in the game window)
        /// </summary>
        /// <param name="gameTime"></param>
        private void Fall(GameTime gameTime)
        {
            GameObject.Transform.Translate(_moveDirection * (float)gameTime.ElapsedGameTime.TotalSeconds);

        }

        /// <summary>
        /// Set velocity of platform
        /// </summary>
        private void SetVelocity()
        {
            _moveDirection = _velocity * _speed;
        }

        /// <summary>
        /// Removes platform from game when position exceeds screen height
        /// </summary>
        private void DestroyGround()
        {
            if (_position.Y >= GameWorld.Instance.GraphicsDevice.Viewport.Height && _tag=="ground") 
            {
                GameWorld.Instance.Destroy(GameObject);
                Console.WriteLine("Ground has been destroyed");
            }
        }

        /// <summary>
        /// Initiates falling of platforms when player jumps from startground
        /// </summary>
        /// <param name="ctx"></param>
        private void OnCollision(Dictionary<string, object> ctx)
        {
            // Collider lastCollision = (Collider) ctx["collider"];
            // Collider origin = (Collider) ctx["origin"];
            //
            // //Console.WriteLine($"CollisionExit with {lastCollision.Tag}");
            //
            // if (lastCollision.GameObject.Tag == "Player")
            // {
            //     //_dropGround = true;
            //     PlatformSpawner.Instance.StartSpawning = true;
            //     //Console.WriteLine("Ground falling now!");
            // }
        }

    }
}
