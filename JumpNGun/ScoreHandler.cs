using System;
using System.Collections.Generic;
using SharpDX.Direct3D11;

namespace JumpNGun
{
    public class ScoreHandler
    {
        private static ScoreHandler _instance;

        private List<string> _highscoreNames;
        private List<int> _highScoreScores;
        
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
        
        private void SortScore()
        {
            _highScoreScores = Database.Instance.GetHighScores().Item2;
            _highscoreNames = Database.Instance.GetHighScores().Item1;
            
        }

        public void GetHighScores()
        {
            
        }

    }
}