using System;
using System.Collections.Generic;


namespace JumpNGun
{
    //HELE KLASSEN ER LAVET AF KEAN HEALY, NICHLAS HOBERG OG KRISTIAN J. FICH

    public class ScoreHandler
    {

        private static ScoreHandler _instance;

        //list containing highscore names
        private List<string> _highscoreNames;

        //list containing highscore scores
        private List<int> _highScoreScores;

        //tuple to hold two sorted lists
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


            foreach (int score in SortedLists.Item1)
            {
                Console.WriteLine("SCORE: " + score);
            }

            foreach (string name in SortedLists.Item2)
            {
                Console.WriteLine("NAME: " + name);
            }
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

    }
}