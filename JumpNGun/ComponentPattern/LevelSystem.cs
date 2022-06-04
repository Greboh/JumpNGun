using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class LevelSystem : Component
    {
        private int _currentLevel;
        
        private float _currentXpAmount;
        
        private float _experienceRequirement = 500;

        private float _fillAmount;
        
        private Texture2D _xpBarTexture2D;
        
        public override void Awake()
        {
            EventManager.Instance.Subscribe("OnExperienceGain", OnExperienceGain);
        }

        public override void Start()
        {
            _xpBarTexture2D = GameWorld.Instance.Content.Load<Texture2D>("ExperienceBar");
        }

        public override void Update(GameTime gameTime)
        {
            _fillAmount = (_currentXpAmount / _experienceRequirement * 100) * 10;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_xpBarTexture2D, new Rectangle(100, 0, (int) _fillAmount, 20), Color.White);
        }

        private void LevelUp()
        {
            _currentLevel++;
            _currentXpAmount -= _experienceRequirement;
            _experienceRequirement += 500;
            Console.WriteLine($"Player gained a level!");
            Console.WriteLine($"Player is now level: {_currentLevel}");
            Console.WriteLine($"Player's experience is now {_currentXpAmount}");
        }
        
        private void GetExperience(float amount)
        {
            Console.WriteLine($"Player's gained {amount} experience");
            
            _currentXpAmount += amount;


            Console.WriteLine($"Player's experience is now {_currentXpAmount}");
            
            if (_currentXpAmount >= _experienceRequirement)
            {
                LevelUp();
            }
        }
        
        public int GetLevel()
        {
            return _currentLevel;
        }

        private void OnExperienceGain(Dictionary<string, object> ctx)
        {
            GetExperience((float)ctx["xpAmount"]);
        }
        
        
    }
}