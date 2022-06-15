using System;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun 
{
    public abstract class Ability
    {
        #region Properties

        public string AbilityName { get; protected set; }
        public string AbilityDescription { get; protected set; }
        public bool IsStartAbility { get; protected set; }

        #endregion

        #region Fields

        public Texture2D AbilitySprite { get; protected set; }
        public Texture2D AbilityIcon { get; protected set; }

        protected bool isNextLevel;
        protected float amount;

        #endregion

        #region Class Methods

        public void Start()
        {
            Create();
            LoadContent();
            Console.WriteLine($"Ability {AbilityName} loaded!");
        }

        public void Update(Player player)
        {
            Execute(player);
        }

        #endregion

        #region Abstract Methods

        public abstract void Selected();
        protected abstract void Create();
        protected abstract void LoadContent();
        protected abstract void Execute(Player player);

        #endregion

    }
}