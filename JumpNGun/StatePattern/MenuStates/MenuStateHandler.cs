using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using JumpNGun.StatePattern.GameStates;
using Microsoft.Xna.Framework.Input;

namespace JumpNGun
{
    public class MenuStateHandler
    {
        /*
            [Description]
            
        */
        
        public Texture2D GameTitle { get; set; }
        
        public string PlayerName { get; set; } = string.Empty;
        
        public CharacterType PlayerType { get; set; }
        

        private static MenuStateHandler _instance;
        private bool canPress = true;

        public static MenuStateHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MenuStateHandler();
                    _instance.Initialize();
                }

                return _instance;
            }
        }


        #region Menu States Caches

        public IStateMenu CurrentMenuState { get; private set; }
        public IStateMenu MainMenu { get; private set; }
        public IStateMenu SettingsMenu { get; private set; }
        public IStateMenu Gameplay { get; private set; }
        public IStateMenu CharacterSelection { get; private set; }
        public IStateMenu Controls { get; private set; }
        public IStateMenu Audio { get; private set; }
        public IStateMenu Highscore { get; private set; }


        #endregion


        public void Initialize()
        {
            //Caches states for later use
            MainMenu = new MainMenu();
            SettingsMenu = new SettingsMenu();
            Gameplay = new GamePlay();
            CharacterSelection = new CharacterSelection();
            Controls = new Controls();
            Audio = new Audio();
            Highscore = new Highscore();


            GameTitle = GameWorld.Instance.Content.Load<Texture2D>("game_title");
            
        }


        public void Update(GameTime gameTime)
        {
            CurrentMenuState.Execute(gameTime);


        }

        public void LoadContent()
        {
            CurrentMenuState.LoadContent();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentMenuState.Draw(spriteBatch);
        }

        /// <summary>
        /// Cleans up all instansiated components, used for when switching states to insure objects are removed
        /////LAVET AF KEAN
        /// </summary>
        public void ComponentCleanUp()
        {
            foreach (GameObject go in GameWorld.Instance.gameObjects)
            {
                GameWorld.Instance.Destroy(go);
            }
        }

        /// <summary>
        /// Sets current menu state from newMenuState parameter recived when calling ChangeState method
        ///// LAVET AF KEAN & NICHLAS
        /// </summary>
        /// <param name="newMenuState"></param>
        public void ChangeState(IStateMenu newMenuState)
        {
            if (CurrentMenuState == newMenuState) return; 

            CurrentMenuState?.Exit(); // if CurrentMenuState is not null, invoke Exit()

            CurrentMenuState = newMenuState; // sets CurrentMenuState from newMenuState
            CurrentMenuState.LoadContent();
            CurrentMenuState.Enter(this); // invokes Enter() with this class as parameter
        }
    }
}