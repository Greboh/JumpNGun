using System;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
    public class AttackState : IState
    {
        private Enemy _parent;
        
        private bool _canShoot;
        
        public void Enter(Enemy parent)
        {
            _parent = parent;
            if(_parent.GameObject.HasComponent<Mushroom>())
            {
                _parent = _parent.GameObject.GetComponent<Mushroom>() as Mushroom;
            }
            
        }

        public void Execute()
        {
            HandleAttackLogic();
            Attack();
            Animate();
        }

        public void Animate()
        {
            _parent.Animator.PlayAnimation("mushroom_attack");
        }
        
        public void Exit()
        {
            
        }

        /// <summary>
        /// Instantiate relevant projectile with velocity set according to player position
        /// </summary>
        private void Attack()
        {
            if (!_canShoot) return;
            
            GameObject projectile = ProjectileFactory.Instance.Create(EnemyType.Mushroom);

            projectile.Transform.Position = _parent.GameObject.Transform.Position;
            
            Vector2 targetDirection = Vector2.Subtract(_parent.Player.Position, _parent.GameObject.Transform.Position);
            targetDirection.Normalize();
            targetDirection = Vector2.Multiply(targetDirection, _parent.Player.Speed);
            
            
            ((Projectile) projectile.GetComponent<Projectile>()).Velocity = targetDirection;
            

            ((Projectile) projectile.GetComponent<Projectile>()).Speed = _parent.ProjectileSpeed;
            GameWorld.Instance.Instantiate(projectile);
            _canShoot = false;
        }
        
        /// <summary>
        /// Reset ability to shoot after cooldown
        /// </summary>
        private void HandleAttackLogic()
        {
            if (_canShoot) return;
        
            _parent.Attacktimer += GameWorld.DeltaTime;
            
            if (_parent.Attacktimer > _parent.AttackCooldown)
            {
                _canShoot = true;
                _parent.Attacktimer = 0;
            }
        }
    }
}