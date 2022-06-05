using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun.StatePattern.GameStates
{
    public class Highscore : IStateMenu
    {
        private MenuStateHandler _pareMenuStateHandler;

        private Texture2D _highscorePanel;

        private SpriteFont _scoreFont;

        private List<int> _score = new List<int>();
        private List<string> _name = new List<string>();

        private int xPos = 415;
        private int yPos = 335;
        private int yOffset = 20;

        private bool _scoresPrinted;

        private Vector2[] _namePositions = new Vector2[10]
        {
            new Vector2(415,335),
            new Vector2(415,355),
            new Vector2(415,375),
            new Vector2(415,395),
            new Vector2(415,415),
            new Vector2(415,435),
            new Vector2(415,455),
            new Vector2(415,475),
            new Vector2(415,495),
            new Vector2(415,515),


        };

        private Vector2[] _scorePositions = new Vector2[10]
        {
            new Vector2(889,335),
            new Vector2(889,355),
            new Vector2(889,375),
            new Vector2(889,395),
            new Vector2(889,415),
            new Vector2(889,435),
            new Vector2(889,455),
            new Vector2(889,475),
            new Vector2(889,495),
            new Vector2(889,515),


        };

        public void Enter(MenuStateHandler parent)
        {
            _pareMenuStateHandler = parent;

            foreach (var go in GameWorld.Instance.gameObjects)
            {
                go.Awake();
            }

            GameWorld.Instance.Instantiate(ButtonFactory.Instance.Create(ButtonType.Back));


            _score = ScoreHandler.Instance.GetSortedScores().Item1;
            _name = ScoreHandler.Instance.GetSortedScores().Item2;



            Console.WriteLine("Highscore state");
        }

        public void Execute(GameTime gameTime)
        {
            //call update method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Update(gameTime);
            }

            //call cleanup in every cycle
            GameWorld.Instance.CleanUpGameObjects();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            //draw sprites of every active gameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Draw(spriteBatch);
            }
            spriteBatch.Draw(_highscorePanel, new Rectangle(370,180,_highscorePanel.Width,_highscorePanel.Height), Color.White);

            for (int i = 0; i < _score.Count; i++)
            {
                spriteBatch.DrawString(_scoreFont, _name[i].ToString(), new Vector2(_namePositions[i].X, _namePositions[i].Y), Color.White);

            }

            for (int i = 0; i < _score.Count; i++)
            {
                spriteBatch.DrawString(_scoreFont, _score[i].ToString(), new Vector2(_scorePositions[i].X, _scorePositions[i].Y), Color.White);

            }







            spriteBatch.Draw(_pareMenuStateHandler.GameTitle,
                new Rectangle(400, 60, _pareMenuStateHandler.GameTitle.Width, _pareMenuStateHandler.GameTitle.Height), null,
                Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1);


            spriteBatch.End();
        }

        public void LoadContent()
        {
            //call start method on every active GameObject in list
            for (int i = 0; i < GameWorld.Instance.gameObjects.Count; i++)
            {
                GameWorld.Instance.gameObjects[i].Start();
            }

            _highscorePanel = GameWorld.Instance.Content.Load<Texture2D>("highscore_panel");
            _scoreFont = GameWorld.Instance.Content.Load<SpriteFont>("font");
        }

        public void Exit()
        {
            _pareMenuStateHandler.ComponentCleanUp();
        }

        

    }
}