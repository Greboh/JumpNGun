using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af Nichlas Hoberg og Kristian J. Fich
    /// </summary>
    public class EnemyAbilityState : IEnemyState
    {
        // Reference to Enemy
        private Enemy _parent;

        // bools deciding if the ability is used
        private bool _shouldTeleport; 
        private bool _shouldSummon;
        
        // Array containg all possible spawnLocations for minions
        private Vector2[] _minionSpawnLocations;
        
        // The amount of minions getting spawned
        private int _minionAmount = 2;
        
        public void Enter(Enemy parent)
        {
            // Check what enemy it is
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

        /// <summary>
        /// Handles logic regarding the abilities
        /// </summary>
        private void HandleAbilityLogic()
        {
            // Set enemy movement speed to 0
            _parent.Speed = 0;
            
            // Check if we should teleport
            if (_shouldTeleport)
            {
                // Set Teleport Location
                Vector2 teleportDestination = new Vector2(_parent.Player.GameObject.Transform.Position.X + _parent.SpriteRenderer.Sprite.Width , _parent.Player.GameObject.Transform.Position.Y);

                // Check if the animation is done
                if (_parent.Animator.CurrentIndex >= 17)
                {
                    // Set the GameObjects location to the teleport location
                    _parent.GameObject.Transform.Position = teleportDestination;
                    
                    Exit();
                }
            }
            else if (_shouldSummon)
            {
                CreateMinionPositions(); 

                // Raise the number of enemies in the levelManager
                LevelManager.Instance.EnemyCurrentAmount += _minionAmount;
                
                //instantiate 4 minios around reaper
                for (int i = 0; i < _minionAmount; i++)
                {
                    GameWorld.Instance.Instantiate(EnemyFactory.Instance.Create(EnemyType.ReaperMinion, _minionSpawnLocations[i]));
                }

                Exit();
            }
        }

        public void Animate()
        {
            // Handle animation depending on which ability it's using
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
            _minionSpawnLocations = new Vector2[]
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