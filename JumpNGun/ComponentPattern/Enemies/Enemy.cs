using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public abstract class Enemy : Component
    {
        protected int health;
        protected int damage;

        public float Speed { get; set; }

        protected Collider collider;
        protected float detectionRange;

        public SpriteRenderer SpriteRenderer { get; private set; }
        public Animator Animator { get; private set; }
        public Player Player { get; private set; }

        public Vector2 Velocity { get; set; }
        protected Vector2 spawnPosition;
        
        public bool IsRanged { get; protected set; }
        public bool IsBoss { get; protected set; }

        public float ProjectileSpeed { get; protected set; }
        public float AttackTimer { get; set; }
        public float AttackCooldown { get; protected set; }


        public Rectangle PlatformRectangle { get; set; } = Rectangle.Empty;


        protected IState currentState;
        protected IState attackState;
        protected IState moveState;
        protected IState deathState;
        protected IState abilityState;

        public override void Awake()
        {
        }

        public override void Start()
        {
            SpriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
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
            deathState = new DeathState();
            abilityState = new AbilityState();
        }

        public override void Update(GameTime gameTime)
        {
            Move();
            TakeDamage();

            currentState?.Execute();
        }
        
        public virtual void CheckCollision()
        {
            
        }

        protected virtual void ChasePlayer()
        {

        }
        
        public Vector2 CalculatePlayerDirection()
        {
            Vector2 targetDirection = Vector2.Subtract(Player.Position, GameObject.Transform.Position);
            targetDirection.Normalize();
            targetDirection = Vector2.Multiply(targetDirection, Player.Speed);

            return targetDirection;
        }

        /// <summary>
        /// Applies movement to the enemy
        /// </summary>
        private void Move()
        {
            GameObject.Transform.Translate(Velocity * Speed * GameWorld.DeltaTime);
        }
        
        /// <summary>
        /// Deal damage to Enemy when colliding with Player projectile
        /// </summary>
        private void TakeDamage()
        {
            foreach (Collider col in GameWorld.Instance.Colliders)
            {
                if (col.CollisionBox.Intersects(collider.CollisionBox) && col.GameObject.Tag == "p_Projectile")
                {
                    health -= 20;
                    GameWorld.Instance.Destroy(col.GameObject);
                }
            }
            
            if (health <= 0)
                ChangeState(deathState);
            
        }

        #region State Methods

        protected void ChangeState(IState newState)
        {
            if (newState == currentState) return;

            Console.WriteLine($" Enemy: {this.GetType().Name} entered state: {newState.GetType().Name}");
            currentState?.Exit();

            currentState = newState;
            currentState.Enter(this);
        }

        #endregion
    }
}