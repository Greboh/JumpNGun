using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
    public class AbilitySystem : Component
    {
        public List<Ability> PlayerAbilities { get; set; } = new List<Ability>();
        public List<Ability> AbilitiesToPickFrom { get; set; } = new List<Ability>();
        public bool NoAbilitiesLeft { get; private set; }
        
        private Player _player;
        private List<Ability> _allAbilities = new List<Ability>();

        #region Abilities

        private Ability _healthRegeneration = new HealthRegen();
        private Ability _damageIncrease = new DamageIncrease();
        private Ability _movementIncrease = new MovementIncrease();
        private Ability _projectileWrapAround = new ProjectileWrap();
        private Ability _vampiricBite = new VampiricBite();
        private Ability _magneticPull = new MagneticPull();

        #endregion

        #region Component Methods
        public override void Awake()
        {
            // Get player component
            _player = GameObject.GetComponent<Player>() as Player;
        }
        public override void Start()
        {
            AddAllAbilities();
            
            // Start all Abilities
            foreach (Ability ability in _allAbilities)
            {
                ability.Start();
            }
        }
        public override void Update(GameTime gameTime)
        {
            // Dont run ability.Update if the player doesn't have any abilities.
            if (PlayerAbilities.Count <= 0) return;

            // Call Update in all abilities the player has
            foreach (var ability in PlayerAbilities)
            {
                ability.Update(_player);
            }
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// Adds all abilities to allAbilities
        /// </summary>
        private void AddAllAbilities()
        {
            _allAbilities.Add(_healthRegeneration);
            _allAbilities.Add(_damageIncrease);
            _allAbilities.Add(_movementIncrease);

            _allAbilities.Add(_projectileWrapAround);
            _allAbilities.Add(_vampiricBite);
            _allAbilities.Add(_magneticPull);
        }

        /// <summary>
        /// Offers only abilities that is marked as startAbility
        /// </summary>
        public void PickAtStart()
        {
            // Find all start Abilities
            foreach (Ability ability in _allAbilities)
            {
                if (ability.IsStartAbility)
                {
                    AbilitiesToPickFrom.Add(ability);
                }
            }
            
            // Remove all start abilites
            foreach (var ability in AbilitiesToPickFrom)
            {
                _allAbilities.Remove(ability);
            }

            // Trigger "fake" levelUp Event. So player can pick Starting ability
            EventHandler.Instance.TriggerEvent("OnLevelUp", new Dictionary<string, object>());
        }

        /// <summary>
        /// Offers all abilities that isn't marked as start
        /// </summary>
        public void PickAbility()
        {
            // Clear the list
            AbilitiesToPickFrom.Clear();

            // Make sure there is any abilities left
            if (_allAbilities.Count > 0)
            {
                // Add the ability to list
                foreach (Ability ability in _allAbilities)
                {
                    AbilitiesToPickFrom.Add(ability);
                }
            }
            // Set bool to make sure levelUpOverlay doesn't come up
            else NoAbilitiesLeft = true;
        }
        
        /// <summary>
        /// Handles logic for when an ability is picked
        /// </summary>
        /// <param name="ability">The picked ability</param>
        public void SelectedAbility(Ability ability)
        {
            // Add to player's abilities
            PlayerAbilities.Add(ability);
            // Remove from allAbilities
            _allAbilities.Remove(ability);
            // Run the ability's selected Method
            ability.Selected();

            // Trigger event that the an ability has been selected
            EventHandler.Instance.TriggerEvent("OnAbilitySelected", new Dictionary<string, object>());
        }
        #endregion
    }
}