using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
namespace JumpNGun
{
    public abstract class Enemy : Component
    {
        //health for enemy
        protected int health;

        //damage for enemey
        public int Damage { get; protected set; }

        //speed for enemy
        public float Speed { get; set; }

        //range from which and enemy will be ablte to attack
        protected float detectionRange;

        //random to pick random numbers when needed
        protected Random rnd = new Random();
        
        //collider component
        public Collider Collider { get; private set; }

        //spriterender component
        public SpriteRenderer SpriteRenderer { get; private set; }

        //animator component
        public Animator Animator { get; private set; }

        //player component
        public Player Player { get; private set; }

        //velocity/direction of enemy
        public Vector2 Velocity { get; set; }

        //initial position of enemu
        protected Vector2 spawnPosition;
        
        //determines whether enemy is ranged or not
        public bool IsRanged { get; protected set; }

        //determines whether enemy is boss or not
        public bool IsBoss { get; protected set; }

        //speed for projectiles, instantiated by enemeies
        public float ProjectileSpeed { get; protected set; } 

        //attack timer to manage ability to attack
        public float AttackTimer { get; set; }

        //cooldown to reset the ability to attack
        public float AttackCooldown { get; protected set; }

        //rectangle from which the enemy position originates 
        public Rectangle PlatformRectangle { get; protected set; } = Rectangle.Empty;

        //the current state for enemy
        protected IEnemyState currentEnemyState;

        //the current state for enemy
        protected IEnemyState attackEnemyState;

        //to store EnemyMoveState
        protected IEnemyState moveEnemyState;

        //to store EnemyDeathState
        protected IEnemyState deathEnemyState;

        //to store EnemyDeathState
        protected IEnemyState abilityEnemyState;


        #region Component Methods

        public override void Awake()
        {
            //subscribe to event
            EventManager.Instance.Subscribe("OnTakeDamage", OnTakeDamage);
        }

        public override void Start()
        {
            //get SpriteRenderer component of this gameobject
            SpriteRenderer = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            //get animator component of this gameobject
            Animator = GameObject.GetComponent<Animator>() as Animator;
            //get collider component of this gameobject
            Collider = GameObject.GetComponent<Collider>() as Collider;
            //get gameobject with player component
            Player = GameWorld.Instance.FindObjectOfType<Player>() as Player;
            InitializeStates();
            ChangeState(moveEnemyState);
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

        /// <summary>
        /// Set velocity to target player position
        /// //LAVET AF NICHLAS HOBERG, KRISTIAN J. FICH
        /// </summary>
        /// <returns></returns>
        public Vector2 CalculatePlayerDirection()
        {
            //get direction of by subtractic relevant vectors
            Vector2 targetDirection = Vector2.Subtract(Player.Position, GameObject.Transform.Position);

            //normalize vector to the order of 1
            targetDirection.Normalize();

            //multiply normalized vector by relevant speed
            targetDirection = Vector2.Multiply(targetDirection, Player.Speed);

            return targetDirection;
        }


        /// <summary>
        /// Applies movement to the enemy
        /// //LAVET AF KRISTIAN J. FICH
        /// </summary>
        private void Move()
        {
            GameObject.Transform.Translate(Velocity * Speed * GameWorld.DeltaTime);
        }
        
        /// <summary>
        /// Change state to death state when health goes below or equal to 0
        /// //LAVET AF NICHLAS HOBERG
        /// </summary>
        private void HandleDeath()
        {
            if (health <= 0)
                ChangeState(deathEnemyState);
        }

        #endregion

        #region State Methods

        /// <summary>
        /// Cache all states, to avoid instantiating new states every time we change state
        /// //LAVET AF NICHLAS HOBERG 
        /// </summary>
        private void InitializeStates()
        {
            moveEnemyState = new EnemyMoveState();
            attackEnemyState = new EnemyAttackState();
            deathEnemyState = new EnemyDeathState();
            abilityEnemyState = new EnemyAbilityState();
        }

        /// <summary>
        /// Change currentEnemyState to newEnemyState
        /// //LAVET AF NICHLAS HOBERG
        /// </summary>
        /// <param name="newEnemyState">state we will change to and enter/execute</param>
        protected void ChangeState(IEnemyState newEnemyState)
        {
            if (newEnemyState == currentEnemyState) return;

            currentEnemyState?.Exit();

            currentEnemyState = newEnemyState;
            currentEnemyState.Enter(this);
        }

        #endregion

        #region Event Methods

        /// <summary>
        /// Deal damage to Enemy when colliding with Player projectile
        /// //LAVET AF NICHLAS HOBERG
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