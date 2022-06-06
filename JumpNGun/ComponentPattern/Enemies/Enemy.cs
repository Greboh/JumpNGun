using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
namespace JumpNGun
{
    public abstract class Enemy : Component
    {
        protected int health;
        public int Damage { get; protected set; }

        public float Speed { get; set; }

        public Collider Collider { get; private set; }
        protected float detectionRange;

        protected Random rnd = new Random();
        
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


        public Rectangle PlatformRectangle { get; protected set; } = Rectangle.Empty;


        protected IEnemyState currentEnemyState;
        protected IEnemyState attackEnemyState;
        protected IEnemyState moveEnemyState;
        protected IEnemyState deathEnemyState;
        protected IEnemyState abilityEnemyState;

        #region Component Methods

        public override void Awake()
        {
            EventManager.Instance.Subscribe("OnTakeDamage", OnTakeDamage);
        }

        public override void Start()
        {
            SpriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            Animator = GameObject.GetComponent<Animator>() as Animator;
            Collider = GameObject.GetComponent<Collider>() as Collider;
            Player = GameWorld.Instance.FindObjectOfType<Player>() as Player;

            InitializeStates();

            ChangeState(moveEnemyState);
        }

        private void InitializeStates()
        {
            moveEnemyState = new EnemyMoveState();
            attackEnemyState = new EnemyAttackState();
            deathEnemyState = new EnemyDeathState();
            abilityEnemyState = new EnemyAbilityState();
        }

        public override void Update(GameTime gameTime)
        {
            Move();
            HandleDeath();
            currentEnemyState?.Execute();
        }

        #endregion

        #region Class Methods

        protected virtual void CheckCollision()
        {
            
        }

        public Vector2 CalculatePlayerDirection()
        {
            Vector2 targetDirection = Vector2.Subtract(Player.GameObject.Transform.Position, GameObject.Transform.Position);
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
        
        private void HandleDeath()
        {
            if (health <= 0)
                ChangeState(deathEnemyState);
        }

        #endregion

        #region State Methods

        protected void ChangeState(IEnemyState newEnemyState)
        {
            if (newEnemyState == currentEnemyState) return;

            // Console.WriteLine($" Enemy: {this.GetType().Name} entered state: {newEnemyState.GetType().Name}");
            currentEnemyState?.Exit();

            currentEnemyState = newEnemyState;
            currentEnemyState.Enter(this);
        }

        #endregion

        #region Event Methods

        /// <summary>
        /// Deal damage to Enemy when colliding with Player projectile
        /// </summary>
        /// <param name="ctx">The context that gets sent from the trigger in Projectile.cs</param>
        private void OnTakeDamage(Dictionary<string, object> ctx)
        {
            GameObject collisionObject = (GameObject) ctx["object"];
            int damageTaken = (int) ctx["damage"];
            GameObject projectile = (GameObject) ctx["projectile"];
            
            if(collisionObject == this.GameObject && projectile.Tag == "p_Projectile")
            {
                health -= damageTaken;
                GameWorld.Instance.Destroy(projectile);

            }
        }

        #endregion
    }
}