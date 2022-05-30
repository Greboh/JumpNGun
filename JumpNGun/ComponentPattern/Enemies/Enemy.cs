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

        protected bool isColliding = false;

        protected SpriteRenderer sr;
        protected Animator animator;
        protected Collider collider;
        protected Player player;

        protected bool canAttack;
        protected bool isAttacking;

        public abstract void Attack();

        public abstract void CheckCollision();

        public abstract void ChasePlayer();

        public abstract void HandleAnimations();

        public override void Awake()
        {

        }

        public override void Start()
        {
            sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            animator = GameObject.GetComponent<Animator>() as Animator;
            collider = GameObject.GetComponent<Collider>() as Collider;
            player = GameWorld.Instance.FindObjectOfType<Player>() as Player;
        }


        public override void Update(GameTime gameTime)
        {
            FlipSprite();
            Move();
            Collision();
            Die();
            UpdatePositionReference();
        }

        /// <summary>
        /// Initiates movement of object
        /// </summary>
        private void Move()
        {
            if (isAttacking) return;
            GameObject.Transform.Translate(velocity * speed * GameWorld.DeltaTime);
        }

        /// <summary>
        /// Updates position during gametime
        /// </summary>
        private void UpdatePositionReference()
        {
            position = GameObject.Transform.Position;
        }

        /// <summary>
        /// Flip sprite according to velocity
        /// </summary>
        private void FlipSprite()
        {
            if (!isAttacking)
            {
                // If we are moving left, flip the sprite
                if (velocity.X < 0)
                    sr.SpriteEffects = SpriteEffects.FlipHorizontally;
                
                // If we are moving right, unflip the sprite
                else if (velocity.X > 0)
                    sr.SpriteEffects = SpriteEffects.None;
                
            }
            else
            {
                if (player.GameObject.Transform.Position.X < this.GameObject.Transform.Position.X)
                    sr.SpriteEffects = SpriteEffects.FlipHorizontally;
                
                else if (player.GameObject.Transform.Position.X > this.GameObject.Transform.Position.X)
                    sr.SpriteEffects = SpriteEffects.None;
            }
        }

        private void Collision()
        {
            foreach (Collider col in GameWorld.Instance.Colliders)
            {
                if (col.CollisionBox.Intersects(collider.CollisionBox) && col.GameObject.Tag == "P_Projectile")
                {

                    health -= 20;
                    GameWorld.Instance.Destroy(col.GameObject);

                }
                if (col.CollisionBox.Intersects(collider.CollisionBox) && col.GameObject.Tag == "Player")
                {
                    canAttack = true;
                }
                else if (!col.CollisionBox.Intersects(collider.CollisionBox) && col.GameObject.Tag == "Player") canAttack = false;
            }
        }

        private void Die()
        {
            if (health <= 0)
            {
                if (animator.IsAnimationDone)
                {
                    EventManager.Instance.TriggerEvent("OnEnemyDeath", new Dictionary<string, object>()
                    {
                    {"enemyDeath", 1}
                     });
                    ScoreHandler.Instance.AddToScore(20);
                    ScoreHandler.Instance.PrintScore();
                    GameWorld.Instance.Destroy(this.GameObject);
                    GameWorld.Instance.Instantiate(ExperienceOrbFactory.Instance.Create(ExperienceOrbType.Small, GameObject.Transform.Position));
                }
            }
        }
    }
}
