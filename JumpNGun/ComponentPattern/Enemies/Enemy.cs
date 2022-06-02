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

        public float ProjectileSpeed { get; set; }

        public float AttackCooldown { get; set; }

        public float Attacktimer { get; set; }


        protected Vector2 position;
        public Vector2 Velocity { get; set; }

        public SpriteRenderer sr { get; private set; }
        public Animator Animator { get; private set; }
        protected Collider collider;
        public Player Player  { get; private set; }

        protected bool isImmune;

        private bool _canMove = true;
        protected bool isDead;
        
        public Rectangle PlatformRectangle { get;  set; } = Rectangle.Empty;


        protected IState currentState;
        protected IState attackState;
        protected IState moveState;
        protected IState deathState;

        public override void Awake()
        {
            EventManager.Instance.Subscribe("Freeze", FreezeMovement);
        }

        public override void Start()
        {
            sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            Animator = GameObject.GetComponent<Animator>() as Animator;
            collider = GameObject.GetComponent<Collider>() as Collider;
            Player = GameWorld.Instance.FindObjectOfType<Player>() as Player;
            
            InitializeStates();
            
            ChangeState(moveState);
        }
        
        private void InitializeStates()
        {
            moveState = new MoveState();
            attackState = new AttackState();
            // deathState = new AttackState();
        }

        public override void Update(GameTime gameTime)
        {
            FlipSprite();
            Move();
            TakeDamage();
            UpdatePositionReference();
            Die();
            
            currentState?.Execute();
        }

        public abstract void Attack();

        public abstract void CheckCollision();

        public abstract void HandleAnimations();

        protected virtual void ChasePlayer()
        {
            Vector2 sourceToTarget = Vector2.Subtract(Player.Position, GameObject.Transform.Position);
            sourceToTarget.Normalize();
            sourceToTarget = Vector2.Multiply(sourceToTarget, Player.Speed);

            Velocity = sourceToTarget;
        }


        /// <summary>
        /// Initiates movement of object
        /// </summary>
        private void Move()
        {
            GameObject.Transform.Translate(Velocity * speed * GameWorld.DeltaTime);
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
            // if (!canAttack)
            // {
            //     sr.SpriteEffects = Velocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            // }
            // else
            // {
            //     sr.SpriteEffects = player.GameObject.Transform.Position.X > this.GameObject.Transform.Position.X ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            // }
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
            _canMove = (bool) ctx["freeze"];
        }

        /// <summary>
        /// Trigger event, add to score, and destroy this gameobject when health goes below or equal to 0
        /// </summary>
        private void Die()
        {
            if (health <= 0)
            {
                if (Animator.IsAnimationDone)
                {
                    EventManager.Instance.TriggerEvent("OnEnemyDeath", new Dictionary<string, object>()
                        {{"enemyDeath", 1}});

                    ScoreHandler.Instance.AddToScore(20);
                    ScoreHandler.Instance.PrintScore();
                    GameWorld.Instance.Destroy(this.GameObject);
                    GameWorld.Instance.Instantiate(ExperienceOrbFactory.Instance.Create(ExperienceOrbType.Small, GameObject.Transform.Position));
                }
            }
        }

        #region State Methods

        protected void ChangeState(IState newState)
        {
            if (newState == currentState) return;
            
            Console.WriteLine(newState.GetType().Name);
            currentState?.Exit();

            currentState = newState;
            currentState.Enter(this);
        }

        #endregion
    }
}