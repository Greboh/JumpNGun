using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class EnemyAbilityState : IEnemyState
    {
        private Enemy _parent;

        private bool _shouldTeleport;
        private bool _shouldSummon;
        
        private Vector2[] _minionPositions;
        private int _miniomAmount = 2;


        
        public void Enter(Enemy parent)
        {
            if (parent.GameObject.HasComponent<Reaper>())
            {
                _parent = parent.GameObject.GetComponent<Reaper>() as Reaper;

                _shouldTeleport = (_parent.GameObject.GetComponent<Reaper>() as Reaper).CanTeleport;
                _shouldSummon = (_parent.GameObject.GetComponent<Reaper>() as Reaper).CanSummon;
            }
        }

        public void Execute()
        {
            HandleAbilityLogic();
            Animate();
        }

        private void HandleAbilityLogic()
        {
            _parent.Speed = 0;
            
            if (_shouldTeleport)
            {
                Vector2 teleportDestination = new Vector2(_parent.Player.Position.X + _parent.SpriteRenderer.Sprite.Width , _parent.Player.Position.Y);

                if (_parent.Animator.CurrentIndex >= 17)
                {
                    _parent.GameObject.Transform.Position = teleportDestination;

                    Exit();
                }
            }
            else if (_shouldSummon)
            {
                CreateMinionPositions(); 

                // Raise the number of enemies in the levelManager
                LevelManager.Instance.EnemyCurrentAmount += _miniomAmount;
                
                //instantiate 4 minios around reaper
                for (int i = 0; i < _miniomAmount; i++)
                {
                    GameWorld.Instance.Instantiate(EnemyFactory.Instance.Create(EnemyType.ReaperMinion, _minionPositions[i]));
                }

                Exit();
            }
        }

        public void Animate()
        {
            if (_shouldTeleport)
            {
                _parent.Animator.PlayAnimation("death");
            }
            else if (_shouldSummon)
            {
                _parent.Animator.PlayAnimation("special_summon");
            }
        }
        
        /// <summary>
        /// Creates an array holding 4 positions around reaper object
        /// </summary>
        private void CreateMinionPositions()
        {
            _minionPositions = new Vector2[]
            {
                new Vector2(_parent.GameObject.Transform.Position.X + 75, _parent.GameObject.Transform.Position.Y),
                new Vector2(_parent.GameObject.Transform.Position.X, _parent.GameObject.Transform.Position.Y + 75),
                new Vector2(_parent.GameObject.Transform.Position.X-75, _parent.GameObject.Transform.Position.Y),
                new Vector2(_parent.GameObject.Transform.Position.X, _parent.GameObject.Transform.Position.Y - 75)
            };
        }

        public void Exit()
        {
            _parent.Speed = (_parent.GameObject.GetComponent<Reaper>() as Reaper).DefaultSpeed;
            (_parent.GameObject.GetComponent<Reaper>() as Reaper).CanSummon = false;
            (_parent.GameObject.GetComponent<Reaper>() as Reaper).CanTeleport = false;
            (_parent.GameObject.GetComponent<Reaper>() as Reaper).ShouldUseAbility = false;
            
        }
    }
}