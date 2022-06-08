using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace JumpNGun.StatePattern.GameStates
{
    public class Highscore : IStateMenu
    {
        #region fields

        private MenuStateHandler _pareMenuStateHandler;

        private Texture2D _highscorePanel;

        private SpriteFont _scoreFont;

        private List<int> _score = new List<int>(); // list for storing scores from tuple
        private List<string> _name = new List<string>(); // list for storing names from tuple

        //predefined name positions
        private Vector2[] _namePositions = new Vector2[10]
        {
            new Vector2(415,285),
            new Vector2(415,315),
            new Vector2(415,345),
            new Vector2(415,375),
            new Vector2(415,405),
            new Vector2(415,435),
            new Vector2(415,465),
            new Vector2(415,495),
            new Vector2(415,525),
            new Vector2(415,555),


        };

        //predefined score positions
        private Vector2[] _scorePositions = new Vector2[10]
        {
            new Vector2(889,285),
            new Vector2(889,315),
            new Vector2(889,345),
            new Vector2(889,375),
            new Vector2(889,405),
            new Vector2(889,435),
            new Vector2(889,465),
            new Vector2(889,495),
            new Vector2(889,525),
            new Vector2(889,555),


        };

        #endregion


        /// <summary>
        /// initializes code that runs when Highscore state is instansiated
        /// LAVET AF KEAN & NICHLAS
        /// </summary>
        /// <param name="parent"></param>
        public void Enter(MenuStateHandler parent)
        {
            _pareMenuStateHandler = parent;

            foreach (var go in GameWorld.Instance.GameObjects)
            {
                go.Awake();
            }

            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Back));

            // gets sorted item list from tuple in ScoreHandler.cs into lists
            _score = ScoreHandler.Instance.GetSortedScores().Item1;
            _name = ScoreHandler.Instance.GetSortedScores().Item2;

        }

        /// <summary>
        /// Updates logic when state is Highscore
        /// LAVET AF KEAN & NICHLAS
        /// </summary>
        /// <param name="gameTime"></param>
        public void Execute(GameTime gameTime)
        {
            //call update method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.GameObjects.Count; i++)
            {
                GameWorld.Instance.GameObjects[i].Update(gameTime);
            }

            //call cleanup in every cycle
            GameWorld.Instance.CleanUpGameObjects();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // highscore list starting number
            int count = 1; 

            //draw sprites of every active gameObject in list
            for (int i = 0; i < GameWorld.Instance.GameObjects.Count; i++)
            {
                GameWorld.Instance.GameObjects[i].Draw(spriteBatch);
            }

            spriteBatch.Draw(_highscorePanel, new Rectangle(370,180,_highscorePanel.Width,_highscorePanel.Height), Color.White);

            // for loop for drawing scores to screen, it used int count + iteration to show ranked highscore number
            for (int i = 0; i < 10; i++)
            {
                spriteBatch.DrawString(_scoreFont, (count + i).ToString() + ". " + _name[i], new Vector2(_namePositions[i].X, _namePositions[i].Y), Color.White) ; 
            }

            // for loop for drawing names to screen, it used int count + iteration to show ranked highscore number
            for (int i = 0; i < 10; i++)
            {
                spriteBatch.DrawString(_scoreFont, _score[i].ToString(), new Vector2(_scorePositions[i].X, _scorePositions[i].Y), Color.White);

            }

            // game title texture
            spriteBatch.Draw(_pareMenuStateHandler.GameTitle,
                new Rectangle(400, 60, _pareMenuStateHandler.GameTitle.Width, _pareMenuStateHandler.GameTitle.Height), null,
                Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);


            spriteBatch.End();
        }

        public void LoadContent()
        {
            //call start method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.GameObjects.Count; i++)
            {
                GameWorld.Instance.GameObjects[i].Start();
            }

            _highscorePanel = GameWorld.Instance.Content.Load<Texture2D>("highscore_panel");
            _scoreFont = GameWorld.Instance.Content.Load<SpriteFont>("highscoreFont");
        }

        /// <summary>
        /// Code that runs when state is changed
        /// LAVET AF KEAN & NICHLAS
        /// </summary>
        public void Exit()
        {
            _pareMenuStateHandler.ComponentCleanUp();
        }

        

    }
}