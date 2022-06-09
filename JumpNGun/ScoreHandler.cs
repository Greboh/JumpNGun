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

        //list containing highscore names
        private List<string> _highscoreNames;

        //list containing highscore scores
        private List<int> _highScoreScores;
        
        public static ScoreHandler Instance
        {
            get { return _instance ??= new ScoreHandler(); }
        }

        private int _currentScore;

        /// <summary>
        /// Get the current score
        /// </summary>
        /// <returns>Current score</returns>
        public int GetScore()
        {
            return _currentScore;
        }
        
        /// <summary>
        /// Add to current score
        /// </summary>
        /// <param name="addAmount">Amount to add</param>
        public void AddToScore(int addAmount)
        {
            _currentScore += addAmount;
        }

        /// <summary>
        /// Sort two litst with bubblesort and return tuple containing the sorted lists 
        /// </summary>
        /// <param name="scores">list containing integers</param>
        /// <param name="names">list containing strings</param>
        /// <returns></returns>
        private Tuple<List<int>, List<string>> Sort(List<int> scores, List<string> names)
        {
            //determines if we should exit while loop for bubblesort
            bool swapped = false;

            //variable to hold the integer value we are swapping
            int scoreHolder;

            //varibale to hold the string value we are swapping
            string nameHolder;

            //sort while swapped is false
            while (!swapped)
            {
                //loop runs x amount according to scores.Count
                for (int i = 1; i < scores.Count; i++)
                {
                    /*if previous index is smaller than current index swap them,
                     and swap the corresponding indexes in the list containing strings*/
                    if (scores[i - 1] < scores[i])
                    {
                        scoreHolder = scores[i - 1];
                        scores[i - 1] = scores[i];
                        scores[i] = scoreHolder;


                        nameHolder = names[i - 1];
                        names[i - 1] = names[i];
                        names[i] = nameHolder;

                        //swapped set to true
                        swapped = true;
                    }
                }

                if (swapped) swapped = false; //if swapped has altered, return to false sort again
                else if (!swapped) swapped = true; //if swapped is false, sorting has finished and we exit loop
            }

            //return the two sorted lists
            return Tuple.Create(scores, names);

        }
        
        /// <summary>
        /// Gets all sorted HighScores. Sorted with BubbleSort
        /// </summary>
        /// <returns>BubbleSorted highscores</returns>
        public Tuple<List<int>, List<string>> GetSortedScores()
        {
            _highScoreScores = Database.Instance.GetHighScores().Item2;
            _highscoreNames = Database.Instance.GetHighScores().Item1;

            return Sort(_highScoreScores, _highscoreNames);

        }
    }
}