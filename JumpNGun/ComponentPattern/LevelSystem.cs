using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    public class LevelSystem : Component
    {
        private float _currentXPAmount = 0;

        private int _currentLevel = 0;
        
        private float _experienceRequierment = 500;

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
            _fillAmount = (_currentXPAmount / _experienceRequierment * 100) * 10;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_xpBarTexture2D, new Rectangle(100, 0, (int) _fillAmount, 20), Color.White);
        }

        private void LevelUp()
        {
            _currentLevel++;
            _currentXPAmount -= _experienceRequierment;
            _experienceRequierment += 500;
            Console.WriteLine($"Player gained a level!");
            Console.WriteLine($"Player is now level: {_currentLevel}");
            Console.WriteLine($"Player's experience is now {_currentXPAmount}");
        }
        
        private void GetExperience(float amount)
        {
            Console.WriteLine($"Player's gained {amount} experience");
            
            _currentXPAmount += amount;


            Console.WriteLine($"Player's experience is now {_currentXPAmount}");
            
            if (_currentXPAmount >= _experienceRequierment)
            {
                LevelUp();
            }
        }

        private void OnExperienceGain(Dictionary<string, object> ctx)
        {
            GetExperience((float)ctx["xpAmount"]);
        }
        
        
    }
}