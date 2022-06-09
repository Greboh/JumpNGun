using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af Nichlas Hoberg
    /// </summary>
    public class LevelSystem : Component
    {
        // Players current level
        private int _currentLevel;
        
        // Players current amount of xp
        private float _currentXpAmount;
        
        // xp required to level up
        private float _experienceRequirement = 500;

        // How much fill the xpBar should be filled
        private float _xpBarFillAmount;
        
        // The xpBar's texture
        private Texture2D _xpBarTexture2D;
        
        public override void Awake()
        {
            // Subscribe to event
            EventHandler.Instance.Subscribe("OnExperienceGain", OnExperienceGain);
        }

        public override void Start()
        {
            // Load the texture
            _xpBarTexture2D = GameWorld.Instance.Content.Load<Texture2D>("ExperienceBar");
        }

        public override void Update(GameTime gameTime)
        {
            // Set the xpFillAmount
            _xpBarFillAmount = (_currentXpAmount / _experienceRequirement * 100) * 10;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the xpBar
            spriteBatch.Draw(_xpBarTexture2D, new Rectangle(100, 0, (int) _xpBarFillAmount, 20), Color.White);
        }

        /// <summary>
        /// Handles logic when the player levels up
        /// </summary>
        private void LevelUp()
        {
            _currentLevel++; // Raise the current level by 1
            _currentXpAmount -= _experienceRequirement; // currentXp should be subtracted by the required xp
            _experienceRequirement += 500; // Raise the requirement for gaining a level
        }
        
        private void GetExperience(float amount)
        {
            _currentXpAmount += amount; // Give experience to the player
            
            // If we have enough xp, level up
            if (_currentXpAmount >= _experienceRequirement)
                LevelUp();
        }
        
        /// <summary>
        /// Gets the players current level
        /// </summary>
        /// <returns>Player's current level</returns>
        public int GetLevel()
        {
            return _currentLevel;
        }

        /// <summary>
        /// Gets trigger by an event whenever the player receives experience
        /// </summary>
        /// <param name="ctx">The context that gets send  from the trigger in InputHandler.cs</param>
        private void OnExperienceGain(Dictionary<string, object> ctx)
        {
            GetExperience((float)ctx["xpAmount"]);
        }
        
        
    }
}