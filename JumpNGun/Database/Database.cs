using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af alle
    /// </summary>
    public class Database
    {
        private static Database _instance;

        public static Database Instance
        {
            get
            {
                _instance ??= new Database();


                return _instance;
            }
        }

        private SQLiteConnection _connection;



        private Database()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            _connection = new SQLiteConnection("Data Source=highscores.db;Version=3;New=True");

            _connection.Open();
            SQLiteCommand command = new SQLiteCommand($"CREATE TABLE IF NOT EXISTS scores (ID INTEGER PRIMARY KEY, NAME VARCHAR(50), SCORE INTEGER)", _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }


        public void AddScore(string name, int score)
        {
            _connection.Open();

            SQLiteCommand command = new SQLiteCommand(
                $"INSERT INTO scores (NAME, SCORE) VALUES ('{name}', '{score}' )", (SQLiteConnection) _connection);
            command.ExecuteNonQuery();

            _connection.Close();
        }

        public void PrintScore()
        {
            _connection.Open();

            SQLiteCommand command = new SQLiteCommand("SELECT * FROM scores", _connection);

            SQLiteDataReader dataset = command.ExecuteReader();

            while (dataset.Read())
            {
                string name = dataset.GetString(1);
                int score = dataset.GetInt32(2);
                
                Console.WriteLine($"Name: {name}, Score: {score}");
            }

            _connection.Close();
        }
        
        public Tuple<List<string>, List<int>> GetHighScores()
        {
            List<string> names = new List<string>();
            List<int> scores = new List<int>();
            
            _connection.Open();

            SQLiteCommand command = new SQLiteCommand("SELECT * FROM scores", _connection);

            SQLiteDataReader dataset = command.ExecuteReader();

            while (dataset.Read())
            {
                names.Add(dataset.GetString(1));
                scores.Add( dataset.GetInt32(2));

            }

            _connection.Close();

            return Tuple.Create(names, scores);
        }

    }
}