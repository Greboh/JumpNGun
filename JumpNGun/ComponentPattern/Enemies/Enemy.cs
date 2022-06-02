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
        protected float originalspeed;

        protected Vector2 position;
        protected Vector2 velocity;

        protected SpriteRenderer sr;
        protected Animator animator;
        protected Collider collider;
        protected Player player;

        protected bool isImmune;
        protected bool canAttack;
        private bool canMove = true;
        protected bool isDead;

        public override void Awake()
        {
            EventManager.Instance.Subscribe("Freeze", FreezeMovement);
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
            TakeDamage();
            UpdatePositionReference();
            Die();
        }

        public abstract void Attack();

        public abstract void CheckCollision();
        
        public abstract void HandleAnimations();

        public virtual void ChasePlayer()
        {
            Vector2 sourceToTarget = Vector2.Subtract(player.Position, GameObject.Transform.Position);
            sourceToTarget.Normalize();
            sourceToTarget = Vector2.Multiply(sourceToTarget, player.Speed);

            velocity = sourceToTarget;
        }


        /// <summary>
        /// Initiates movement of object
        /// </summary>
        private void Move()
        {
            if (canAttack || !canMove || isDead) return;
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
            if (!canAttack)
            {
                sr.SpriteEffects = velocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }
            else
            {
                sr.SpriteEffects = player.GameObject.Transform.Position.X > this.GameObject.Transform.Position.X ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }
        }

        /// <summary>
        /// Deal damage to Enemy when colliding with Player projectile
        /// </summary>
        private void TakeDamage()
        {
            foreach (Collider col in GameWorld.Instance.Colliders)
            {
                if (col.CollisionBox.Intersects(collider.CollisionBox) && col.GameObject.Tag == "P_Projectile")
                {
                    health -= 20;
                    GameWorld.Instance.Destroy(col.GameObject);
                }
            }
        }

        /// <summary>
        /// Freeze movement when event triggered
        /// </summary>
        /// <param name="ctx"></param>
        private void FreezeMovement(Dictionary<string, object> ctx)
        {
            canMove = (bool)ctx["freeze"];
        }

        /// <summary>
        /// Trigger event, add to score, and destroy this gameobject when health goes below or equal to 0
        /// </summary>
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
