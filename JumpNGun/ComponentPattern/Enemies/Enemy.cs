using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public abstract class Enemy : Component
    {
        protected int health;
        protected int damage;
        
        protected float speed;

        protected Vector2 position;
        protected Vector2 velocity;

        protected bool isColliding;

        protected SpriteRenderer sr;
        protected Animator animator;
        protected Collider enemyCollider;
        protected Player player;

        public abstract void Attack();

        public abstract void CheckCollision();

        public abstract void FindPlayerObject();

        public abstract void ChasePlayer();

        public abstract void HandleAnimations();

        /// <summary>
        /// Initiates movement of object
        /// </summary>
        protected void Move()
        {
            GameObject.Transform.Translate(velocity * GameWorld.DeltaTime);
        }

        /// <summary>
        /// Updates position during gametime
        /// </summary>
        protected void UpdatePositionReference()
        {
            position = GameObject.Transform.Position;
        }

        /// <summary>
        /// Flip sprite according to velocity
        /// </summary>
        protected void Flipsprite()
        {
            // If we are moving left, flip the sprite
            if (velocity.X < 0)
            {
                sr.SpriteEffects = SpriteEffects.FlipHorizontally;
            }
            // If we are moving right, unflip the sprite
            else if (velocity.X > 0)
            {
                sr.SpriteEffects = SpriteEffects.None;
            }
        }

        /// <summary>
        /// Removes Enemy from game when health goes below zero
        /// </summary>
        protected void Death()
        {
            if (health <= 0)
            {
                GameWorld.Instance.Destroy(this.GameObject);
            }
        }
    }
}
