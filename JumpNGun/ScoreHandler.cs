using System;
using System.Collections.Generic;


namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af alle
    /// </summary>
    public class ScoreHandler
    {
        private static ScoreHandler _instance;

        private List<string> _highscoreNames;
        private List<int> _highScoreScores;
        Tuple<List<int>, List<string>> SortedLists;

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
        
        public void SortScore()
        {
            _highScoreScores = Database.Instance.GetHighScores().Item2;
            _highscoreNames = Database.Instance.GetHighScores().Item1;

            SortedLists = Sort(_highScoreScores, _highscoreNames);
        }

        private Tuple<List<int>, List<string>> Sort(List<int> scores, List<string> names)
        {
            bool swapped = false;
            int scoreHolder;
            string nameHolder;

            while (!swapped)
            {
                for (int i = 1; i < scores.Count; i++)
                {
                    if (scores[i - 1] < scores[i])
                    {
                        scoreHolder = scores[i - 1];
                        scores[i - 1] = scores[i];
                        scores[i] = scoreHolder;


                        nameHolder = names[i - 1];
                        names[i - 1] = names[i];
                        names[i] = nameHolder;

                        swapped = true;
                    }
                }

                if (swapped) swapped = false;
                else if (!swapped) swapped = true;
            }

            return Tuple.Create(scores, names);

        }

        public Tuple<List<int>, List<string>> GetSortedScores()
        {
            _highScoreScores = Database.Instance.GetHighScores().Item2;
            _highscoreNames = Database.Instance.GetHighScores().Item1;

            return Sort(_highScoreScores, _highscoreNames);
            
        }

        

        public void GetHighScores()
        {



        }

    }
}