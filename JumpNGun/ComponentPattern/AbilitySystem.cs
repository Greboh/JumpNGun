using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
    public class AbilitySystem : Component
    {
        private Player _player;
        
        public List<Ability> PlayerAbilities { get; set; }= new List<Ability>();
        public List<string> PlayerAbilityDescription { get; set; }= new List<string>();


        public List<Ability> AllAbilities { get; set; }= new List<Ability>();

        public List<Ability> AbilitiesToPickFrom { get; set; } = new List<Ability>();
        
        #region Abilities

        public Ability HealthRegeneration { get; } = new HealthRegen();
        public Ability DamageIncrease { get; } = new DamageIncrease();
        public Ability MovementIncrease { get; } = new MovementIncrease();
        public Ability ProjectileWrapAround { get; } = new ProjectileWrap();
        public Ability VampiricBite { get; } = new VampiricBite();
        

        #endregion

        #region Component Methods

        public override void Awake()
        {
            _player = GameObject.GetComponent<Player>() as Player;

        }

        public override void Start()
        {
            InitializeAllAbilities();

            foreach (Ability ability in AllAbilities)
            {
                ability.Start();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (PlayerAbilities.Count <= 0) return;
            
            foreach (var ability in PlayerAbilities)
            {
                ability.Update(_player);
            }
        }

        #endregion
        
        private void InitializeAllAbilities()
        {
            AllAbilities.Add(HealthRegeneration);
            AllAbilities.Add(DamageIncrease);
            AllAbilities.Add(MovementIncrease);
            AllAbilities.Add(ProjectileWrapAround);
            AllAbilities.Add(VampiricBite);
        }

        public void PickAtStart()
        {
            foreach (Ability ability in AllAbilities)
            {
                if(ability.isStartAbility)
                {
                    AbilitiesToPickFrom.Add(ability);
                }
            }
            
            EventHandler.Instance.TriggerEvent("OnLevelUp", new Dictionary<string, object>());
            
        }
        
        public void PickAbility()
        {
            AbilitiesToPickFrom.Clear();
            foreach (Ability ability in AllAbilities)
            {
                if(!ability.isStartAbility && AbilitiesToPickFrom.Count <= 3)
                {
                    AbilitiesToPickFrom.Add(ability);
                }
            }
        }
        
        public void SelectedAbility(Ability ability)
        {
            PlayerAbilities.Add(ability);
            PlayerAbilityDescription.Add(ability.abilityDescription);
            AllAbilities.Remove(ability);
            ability.Selected();
            
            EventHandler.Instance.TriggerEvent("OnAbilitySelected", new Dictionary<string, object>());
        }
    }
}