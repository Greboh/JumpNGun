using System;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class AttackState : IState
    {
        private Enemy _parent;
        
        private bool _canShoot;

        private SpriteEffects _oldSpriteFlip;
        
        public void Enter(Enemy parent)
        {
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

            _oldSpriteFlip = _parent.SpriteRenderer.SpriteEffects;

        }

        public void Execute()
        {
            if (_parent.IsRanged)
            {
                HandleRangeAttackLogic();
                RangeAttack();
            }
            else
            {
                MeleeAttack();
            }
            
            Animate();
        }

        public void Animate()
        {
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
            if (!_canShoot) return;
            
            Vector2 targetDirection = _parent.CalculatePlayerDirection();
            
            FlipSprite(targetDirection);
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
            Vector2 targetDirection = _parent.Player.GameObject.Transform.Position;
            
            FlipSprite(targetDirection);
        }
        
        private void FlipSprite(Vector2 direction)
        {
            _parent.SpriteRenderer.SpriteEffects = direction.X <= _parent.GameObject.Transform.Position.X ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        }
        
        private void InstantiateProjectile(Vector2 direction)
        {
            GameObject projectile = ProjectileFactory.Instance.Create(EnemyType.Mushroom);
            projectile.Transform.Position = _parent.GameObject.Transform.Position;
            
            (projectile.GetComponent<Projectile>() as Projectile).Velocity = direction;
            (projectile.GetComponent<Projectile>() as Projectile).Speed = _parent.ProjectileSpeed;
            
            GameWorld.Instance.Instantiate(projectile);
        }
    }
}