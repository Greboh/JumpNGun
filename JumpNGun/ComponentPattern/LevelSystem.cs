using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
    public class LevelSystem : Component
    {
        
        //TODO MAKE SINGLETON NOT COMPONENT!
        private float _startXPAmount = 0;
        private float _currentXPAmount = 0;

        private int _startLevel = 1;
        private int _currentLevel;

        public override void Awake()
        {
            EventManager.Instance.Subscribe("OnExperienceGain", OnExperienceGain);
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void LevelUp()
        {
            
        }
        
        private void GetExperience(float amount)
        {
            Console.WriteLine($"Player's gained {amount} experience");
            
            _currentXPAmount += amount;

            Console.WriteLine($"Player's experience is now at {_currentXPAmount}");
        }

        private void OnExperienceGain(Dictionary<string, object> ctx)
        {
            GetExperience((float)ctx["xpAmount"]);
        }
        
        
    }
}