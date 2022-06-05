using System;

namespace JumpNGun
{
    public class ScoreHandler
    {
        private static ScoreHandler _instance;

        public static ScoreHandler Instance
        {
            get { return _instance ??= new ScoreHandler(); }
        }


        private int _currentScore;

        public int GetScore()
        {
            return _currentScore;
        }
        
        public void AddToScore(int addAmount)
        {
            _currentScore += addAmount;
        }
        
        public void PrintScore()
        {
            Console.WriteLine($"Current score is: {_currentScore}");
        }
        
        
        
        
    }
}