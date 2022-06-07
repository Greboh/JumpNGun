using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af Nichlas Hoberg og Kristian J. Fich
    /// </summary>
    public class EnemyDeathState : IEnemyState
    {
        private Enemy _parent;

        public void Enter(Enemy parent)
        {
            _parent = parent;
            
            _parent.Velocity = Vector2.Zero;
            
            Animate();
        }

        public void Execute()
        {
            // Check if death animation is done
            if (_parent.Animator.IsAnimationDone)
            {
                
                // TriggerEvent in LevelManager.cs
                EventManager.Instance.TriggerEvent("OnEnemyDeath", new Dictionary<string, object>()
                {
                    {"enemyDeath", 1}
                });
                
                ScoreHandler.Instance.AddToScore(20);
                GameWorld.Instance.Destroy(_parent.GameObject);
                GameWorld.Instance.Instantiate(ExperienceOrbFactory.Instance.Create(ExperienceOrbType.Small, _parent.GameObject.Transform.Position));
            }        
        }

        /// <summary>
        /// Play Animation
        /// </summary>
        public void Animate()
        {
            _parent.Animator.PlayAnimation("death");
        }

        public void Exit()
        {
            
        }
    }
}