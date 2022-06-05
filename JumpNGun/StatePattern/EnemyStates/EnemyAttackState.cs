using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class EnemyAttackState : IEnemyState
    {
        private Enemy _parent;
        
        private bool _canShoot;

        // Reference to which direction the sprite was flipped before attacking
        private SpriteEffects _oldSpriteFlip;
        
        public void Enter(Enemy parent)
        {
            // Check which type of enemy the parent is
            if(parent.GameObject.HasComponent<Mushroom>())
            {
                _parent = parent.GameObject.GetComponent<Mushroom>() as Mushroom;
            }
            else if(parent.GameObject.HasComponent<Worm>())
            {
                _parent = parent.GameObject.GetComponent<Worm>() as Worm;
            }
            else if(parent.GameObject.HasComponent<Reaper>())
            {
                _parent = parent.GameObject.GetComponent<Reaper>() as Reaper;
            }            
            else if(parent.GameObject.HasComponent<ReaperMinion>())
            {
                _parent = parent.GameObject.GetComponent<ReaperMinion>() as ReaperMinion;
            }
            else if (parent.GameObject.HasComponent<Skeleton>())
            {
                _parent = parent.GameObject.GetComponent<Skeleton>() as Skeleton;

            }
                // Set the oldSpriteFlip
                _oldSpriteFlip = _parent.SpriteRenderer.SpriteEffects;
        }

        public void Execute()
        {
            // Check if the enemy is ranged or melee
            if (_parent.IsRanged)
            {
                HandleRangeAttackLogic();
                RangeAttack();
            }
            else MeleeAttack();
        }

        /// <summary>
        /// Handles the state animations
        /// </summary>
        public void Animate()
        {
            // Play animation
            _parent.Animator.PlayAnimation("attack");
        }
        
        public void Exit()
        {
            if (_parent.IsRanged)
            {
                _parent.SpriteRenderer.SpriteEffects = _oldSpriteFlip;
            }
        }

        /// <summary>
        /// Instantiate relevant projectile with velocity set according to player position
        /// </summary>
        private void RangeAttack()
        {
            Vector2 targetDirection = _parent.CalculatePlayerDirection();
            FlipSprite();
            
            if (!_canShoot) return;

            InstantiateProjectile(targetDirection);
            
            _canShoot = false;
        }
        
        /// <summary>
        /// Reset ability to shoot after cooldown
        /// </summary>
        private void HandleRangeAttackLogic()
        {
            if (_canShoot) return;
            
            if (_parent.AttackTimer > _parent.AttackCooldown)
            {
                _canShoot = true;
                _parent.AttackTimer = 0;
            }
            else _parent.AttackTimer += GameWorld.DeltaTime;
        }
        
        private void MeleeAttack()
        {
            FlipSprite();
            
            // Trigger event
            EventManager.Instance.TriggerEvent("OnTakeDamage", new Dictionary<string, object>()
                {
                    {"damage", _parent.Damage}, // The projectile's damage
                    {"object", _parent.Player.GameObject}, // The collision object 
                    {"projectile", null} // The projectile object
                }
            );
            
            Animate();
        }
        
        private void FlipSprite()
        {
            _parent.SpriteRenderer.SpriteEffects = _parent.Player.GameObject.Transform.Position.X <= _parent.GameObject.Transform.Position.X ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }
        
        private void InstantiateProjectile(Vector2 direction)
        {
            GameObject projectile = ProjectileFactory.Instance.Create(EnemyType.Mushroom, Vector2.Zero);
            projectile.Transform.Position = _parent.GameObject.Transform.Position;
            
            (projectile.GetComponent<Projectile>() as Projectile).Velocity = direction;
            (projectile.GetComponent<Projectile>() as Projectile).Speed = _parent.ProjectileSpeed;
            (projectile.GetComponent<Projectile>() as Projectile).Damage = _parent.Damage;
            
             Animate();
            GameWorld.Instance.Instantiate(projectile);
        }
    }
}