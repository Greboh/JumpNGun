using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af Nichlas Hoberg
    /// </summary>
    public class Player : Component
    {
        #region Fields
        
        // Player's max health
        public float MaxHealth { get; set; }
        
        // Player's current health
        public float CurrentHealth { get; set; }
        
        // Reference to the projectile's damage
        public float Damage { get; set; } 

        // Reference our characterType
        private CharacterType _character; 
        // Create dictionary containing movement values
        private Dictionary<Keys, bool> _movementKeys = new Dictionary<Keys, bool>(); 
        // Reference to the SpriteRenderer component
        private SpriteRenderer _sr; 
        // Reference to the Animator component
        private Animator _animator; 
        // Reference to the Input Component
        private Input _input; 
        // Reference to the Collider component
        private Collider _pCollider; 
        
        // The direction the player should move
        private Vector2 _moveDirection; 
        // Reference the player's spawnPosition
        private Vector2 _spawnPosition = new Vector2(40, 705); 
        // Reference the current groundCollision
        private Rectangle _currentGroundCollisionBox = Rectangle.Empty; 

        // Speed at which the player moves
        public float Speed { get; set; } 
        // The jump height of the player
        private float _jumpHeight;
        // The strength of the player dash
        private float _dashStrength; 
        // How strong the force of gravity is
        private float _gravityPull; 
        // Timer on Dash
        private float _dashTimer; 
        // Cooldown on Dash
        private float _dashCooldown; 
        // delay between footstep sound
        private float _footstepCooldown; 
        // Timer on Shoot
        private float _shootTime; 
        // Cooldown on Shoot
        private float _shootCooldown; 
        // Reference to the projectile's speed
        private float _projectileSpeed;

        // Reference to the fillAmount of the healthBar
        private float _healthBarFillAmount;

       
        // The initial force of gravity
        private int _gravity = 50; 
        // Used to multiply the gravity over time making it stronger
        private int _gravityMultiplier = 100; 
        // The current amount of player jumps
        private int _jumpCount; 
        // The max allowed amount of player jumps
        private int _maxJumpCount; 

        
        // Controls if the player is alive or not
        private bool _isAlive; 
        // Can the player jump
        private bool _canJump;
        // Is the player jumping
        private bool _isJumping;
        // Controls if the player canDash
        private bool _canDash = true; 
        // Controls if the player canShoot
        private bool _canShoot = true; 
        // Is the player grounded
        private bool _isGrounded;
        // Reference if the player has collided with ground
        private bool _hasCollidedWithGround; 

        // Reference to the healthBar Texture
        private Texture2D _healthBar;

        #endregion
        

        #region Abilities

        public bool CanMove { get; set; }
        
        public bool HasProjectileWrap { get; set; }
        public bool HasVampiricBite { get; set; }

        #endregion

        
        
        public Player(CharacterType character, float speed, float jumpHeight, float dashStrength, float dashCooldown, float shootCooldown, int maxJumpCount, int maxHealth,
            float projectileSpeed, int damage)
        {
            _character = character;
            Speed = speed;
            _jumpHeight = jumpHeight;
            _dashStrength = dashStrength;
            _maxJumpCount = maxJumpCount;
            _dashCooldown = dashCooldown;
            _shootCooldown = shootCooldown;
            MaxHealth = maxHealth;
            _projectileSpeed = projectileSpeed;
            Damage = damage;
        }

        #region Component Methods
        
        public override void Awake()
        {
            EventHandler.Instance.Subscribe("OnKeyPress", OnKeyPressed);
            EventHandler.Instance.Subscribe("OnTakeDamage", OnTakeDamage);
            
         
        }

        public override void Start()
        {
            // Get the components
            _sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            _input = GameObject.GetComponent<Input>() as Input;
            _animator = GameObject.GetComponent<Animator>() as Animator;
            _pCollider = GameObject.GetComponent<Collider>() as Collider;

            // Set transformPosition to spawnPosition
            GameObject.Transform.Position = _spawnPosition; 
            
            // Set the gravityPull
            _gravityPull = _gravity; 
            
            // set currentHealth to MaxHealth
            CurrentHealth = MaxHealth; 
            
            // Load healthBar
            _healthBar = GameWorld.Instance.Content.Load<Texture2D>("HealthBar");
        }

        public override void Update(GameTime gameTime)
        {
            _input.Execute(this);
            HandleHealth();
            CheckGrounded();
            HandleGravity();

            if (!_isAlive) return;
            
            HandleShootLogic();
            HandleDashLogic();
            HandleAnimations();
            ScreenBounds();
            WalkingSoundEffects();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_healthBar, new Rectangle(100, 50, (int) _healthBarFillAmount, 20), Color.White);
        }

        #endregion

        #region Class Methods
        
        /// <summary>
        /// Check if we are grounded
        /// </summary>
        private void CheckGrounded()
        {
            foreach (Collider otherCollision in GameWorld.Instance.Colliders)
            {
                // If our CollisionBox collides with another CollisionBox and it's tag is ground and we haven't collided with ground yet
                if (_pCollider.CollisionBox.Intersects(otherCollision.CollisionBox) && !_hasCollidedWithGround)
                {
                    if (otherCollision.GameObject.Tag == "ground" || otherCollision.GameObject.Tag == "platform")
                    {
                        _isGrounded = CalculateCollisionLineIntersection(otherCollision);
                    }
                }
            }

            // If we are grounded but do not collide with our groundCollision, then we are not grounded!
            if (_isGrounded && !_pCollider.CollisionBox.Intersects(_currentGroundCollisionBox))
            {
                _isGrounded = false;
                _hasCollidedWithGround = false;
            }
        }
        
        /// <summary>
        /// Creates gravity making sure the player falls unless he is grounded
        /// </summary>
        private void HandleGravity()
        {
            // Guard clause
            if (_isGrounded) return;

            // Makes the gravity stronger over time, creating a feeling of a pull
            _gravityPull += GameWorld.DeltaTime * _gravityMultiplier;

            Vector2 fallDirection = new Vector2(0, 1);

            // Multiply fallDirection with our gravityPull
            fallDirection *= _gravityPull;

            GameObject.Transform.Translate(fallDirection * GameWorld.DeltaTime);
        }
        
        /// <summary>
        /// Called from MoveCommand.cs
        /// Moves the player
        /// </summary>
        /// <param name="velocity">The strength at which we want to move</param>
        public void Move(Vector2 velocity)
        {
            velocity.Normalize(); // Normalize velocity
            
            velocity *= Speed; // Multiply with speed
            
            _moveDirection = velocity; // Set moveDirection To Velocity
            
            // Translate our current position to the new one
            GameObject.Transform.Translate(_moveDirection * GameWorld.DeltaTime); 

            FlipSprite(_moveDirection);
        }

        /// <summary>
        /// Handles jumping. Called from JumpCommand.cs
        /// </summary>
        public void Jump()
        {
            // Check if we are above or at our maxJumpCount
            _canJump = _jumpCount <= _maxJumpCount;

            // Guard clause
            if (!_canJump || _isJumping) return;

            // Sound effect
            SoundManager.Instance.PlayRandomJump();

            // Add to our jump count
            _jumpCount++;

            // Set our targetDirection using our jumpHeight
            Vector2 targetDirection = new Vector2(0, _jumpHeight);

            GameObject.Transform.Translate(targetDirection);
        }

        /// <summary>
        /// Called from DashCommand.cs
        /// Makes the player dash if he can
        /// </summary>
        public void Dash()
        {
            if (!_canDash) return; // Guard clause

            // Multiply with dashStrength
            Vector2 dashDirection = _moveDirection;
            dashDirection *= _dashStrength;

            // Translate our current position to the new one
            GameObject.Transform.Translate(dashDirection * GameWorld.DeltaTime);

            FlipSprite(dashDirection);
            _canDash = false;
        }

        /// <summary>
        /// Called from ShootCommand.cs
        /// Shoots a projectile if we can shoot
        /// </summary>
        public void Shoot()
        {
            if (!_canShoot) return; // Guard clause

            // Create a new GameObject
            GameObject projectile = ProjectileFactory.Instance.Create(_character);

            // Set the GameObjects position
            projectile.Transform.Position = GameObject.Transform.Position;

            // Store shoot Directions in Vector2's
            Vector2 shootRight = new Vector2(1, 0);
            Vector2 shootLeft = new Vector2(-1, 0);

            // Set Projectiles velocity depending on the SpriteEffect
            ((Projectile) projectile.GetComponent<Projectile>()).Velocity = _sr.SpriteEffects == SpriteEffects.None ? shootRight : shootLeft;
            
            // Set Projectiles Speed
            ((Projectile) projectile.GetComponent<Projectile>()).Speed = _projectileSpeed;
            
            // Set Projectiles Damage
            ((Projectile) projectile.GetComponent<Projectile>()).Damage = Damage;
            
            
            
            
            ((Projectile) projectile.GetComponent<Projectile>()).FiredFromPlayer = true;
            ((Projectile) projectile.GetComponent<Projectile>()).HasWrapAbility = HasProjectileWrap;
            ((Projectile) projectile.GetComponent<Projectile>()).HasVampiricAbility = HasVampiricBite;

            
            // Instantiate Projectile
            GameWorld.Instance.Instantiate(projectile);
            _canShoot = false;
        }
        
        /// <summary>
        /// Checks if the player's health is >= 0
        /// </summary>
        private void HandleHealth()
        {
            // Check if the players current health is bigger than max
            if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
            
            // Set the health bar's fill amount
            // We multiply by 100 to make it percentage, then multiply by 10 to stretch the sprite
            _healthBarFillAmount =  (CurrentHealth / MaxHealth  * 100) * 10;
            
            // Check if player is alive
            if (CurrentHealth >= 1)
            {
                _isAlive = true;
                return;
            }
            
            
            _isAlive = false;
            
            // Play death animation
            _animator.PlayAnimation("Death");
            // Check if the death animation is done
            if (_animator.IsAnimationDone)
            {
                EventHandler.Instance.TriggerEvent("OnPlayerDeath", new Dictionary<string, object>());
                GameWorld.Instance.Destroy(this.GameObject);
            }
        }

        /// <summary>
        /// Plays random footstep out of 4 diffent if player is moving with A or D and is on ground
        /// LAVET AF KEAN
        /// </summary>
        private void WalkingSoundEffects()
        {
            if (_movementKeys.ContainsValue(true) && _isGrounded)
            {
                _footstepCooldown += GameWorld.DeltaTime;
                if (_footstepCooldown > 0.3f)
                {
                    SoundManager.Instance.PlayRandomFootstep();
                    _footstepCooldown = 0;
                }
            }
        }

        /// <summary>
        /// Handles when to set Animations 
        /// </summary>
        private void HandleAnimations()
        {
            // If we are not grounded we are in the air and should play the jump animation
            if (!_isGrounded && _isAlive) _animator.PlayAnimation("Jump");

            // If there isn't any values that is true in movementKeys and we are grounded play idle
            if (!_movementKeys.ContainsValue(true) && _isGrounded && _isAlive) _animator.PlayAnimation("Idle");
           
            // If there is any values that is true in movementKeys and we are grounded play idle
            if (_movementKeys.ContainsValue(true) && _isGrounded && _isAlive) _animator.PlayAnimation("Run");
            
        }
        
        
        #endregion

        #region Helper Methods
        
        /// <summary>
        /// Calculates which part of the player's CollisionBox it has intersected with and applies logic according to it
        /// </summary>
        /// <param name="collisionCollider">The other-collision's collider</param>
        private bool CalculateCollisionLineIntersection(Collider collisionCollider)
        {
            // If the bottom line of the playerCollider intersects with the other-collision's CollisionBox and we haven't collided with the ground yet
            if (_pCollider.BottomLine.Intersects(collisionCollider.CollisionBox))
            {
                // Check if the player is inside the ground collision or if hes just a tiny bit above it
                if (_pCollider.CollisionBox.Bottom >= collisionCollider.CollisionBox.Top && _pCollider.CollisionBox.Bottom <= collisionCollider.CollisionBox.Top + 5)
                {
                    _jumpCount = 0; // Reset jump counter
                    _gravityPull = _gravity; // Reset gravity pull
                    _currentGroundCollisionBox = collisionCollider.CollisionBox; // Reference our current collisionColliders CollisionBox
                    _hasCollidedWithGround = true; // We have collided with ground now
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the sprite should be flipped by using the moveDirection
        /// </summary>
        private void FlipSprite(Vector2 moveDirection)
        {
            // Set SpriteEffect depending on our moveDirection.x
            _sr.SpriteEffects = moveDirection.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        }
        
        /// <summary>
        /// Logic for handling when we can dash
        /// </summary>
        private void HandleDashLogic()
        {
            if (_canDash) return; // Guard clause
            
            _dashTimer += GameWorld.DeltaTime; // Add to our dashTimer

            // Check if its bigger than the cooldown
            if (_dashTimer > _dashCooldown)
            {
                _canDash = true;
                _dashTimer = 0;
            }
        }

        /// <summary>
        /// Logic for handling when we can shoot
        /// </summary>
        private void HandleShootLogic()
        {
            if (_canShoot) return; // Guard clause

            // Check if shootTime is bigger than the shootCooldown
            if (_shootTime > _shootCooldown)
            {
                _canShoot = true;
                _shootTime = 0;
            }
            else _shootTime += GameWorld.DeltaTime; // Add to our shootTime
        }

        /// <summary>
        /// Change player position to oposite side if he exceeds screenbounds
        /// </summary>
        private void ScreenBounds()
        {
            //If player moves beyond 0 on x-axis move player max x-value on axis
            if (GameObject.Transform.Position.X < 0)
            {
                GameObject.Transform.Transport(new Vector2(GameWorld.Instance.GraphicsDevice.Viewport.Width, GameObject.Transform.Position.Y));
            }
            //If player moves beyond max value on x-axis move player to 0 on x-axis 
            if (GameObject.Transform.Position.X > GameWorld.Instance.GraphicsDevice.Viewport.Width)
            {
                GameObject.Transform.Transport(new Vector2(10, GameObject.Transform.Position.Y));
            }
        }

        #endregion

        #region Event Methods

        /// <summary>
        /// Event that gets triggered when the player takes damage
        /// </summary>
        /// <param name="ctx">The context that gets sent from the trigger in Projectile.cs</param>
        private void OnTakeDamage(Dictionary<string, object> ctx)
        {
            GameObject collisionObject = (GameObject) ctx["object"];
            float damageTaken = (float)ctx["damage"];
            GameObject projectile = (GameObject) ctx["projectile"];

            // If the collisionObject is not this one return
            if (collisionObject != this.GameObject) return;
            
            if (projectile != null)
            {
                if (projectile.Tag == "e_Projectile")
                {
                    CurrentHealth -= damageTaken;
                    GameWorld.Instance.Destroy(projectile);
                }
            }
            else CurrentHealth -= damageTaken;

        }

        /// <summary>
        /// Event that receives all key presses
        /// And handles logic depending on the key and if its pressed down
        /// </summary>
        /// <param name="ctx">The context that gets sent from the trigger in InputHandler.cs</param>
        private void OnKeyPressed(Dictionary<string, object> ctx)
        {
            // Get the current pressed key
            Keys currentKey = (Keys) ctx["key"];

            // If the currentKey is either the same as  our MoveLeft or MoveRight KeyCode
            if (currentKey == _input.MoveLeft.KeyboardBinding ||
                currentKey == _input.MoveRight.KeyboardBinding)
            {
                _movementKeys[currentKey] = (bool) ctx["isKeyDown"];
            }
            // If the currentKey is the same as our Jump KeyCode
            else if (currentKey == _input.Jump.KeyboardBinding)
            {
                _isJumping = (bool) ctx["isKeyDown"];
            }
        }

        #endregion
    }
}