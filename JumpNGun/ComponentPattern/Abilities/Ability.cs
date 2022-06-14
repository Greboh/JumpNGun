using System;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun 
{
    public abstract class Ability
    {
        public string abilityName { get; protected set; }
        public string abilityDescription { get; protected set; }
        public bool isStartAbility { get; protected set; }
        public Texture2D abilitySprite { get; protected set; }
        public Texture2D abilitySmallSprite { get; protected set; }
        
        

        protected bool isNextLevel;
        protected float amount;

        public void Start()
        {
            Create();
            LoadContent();
            Console.WriteLine($"Ability {abilityName} loaded!");
        }

        public void Update(Player player)
        {
             Execute(player);
        }
        

        public abstract void Create();

        public abstract void Selected();



        protected abstract void LoadContent();

        protected abstract void Execute(Player player);

    }
}