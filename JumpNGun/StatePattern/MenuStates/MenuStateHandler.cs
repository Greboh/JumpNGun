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
            Abstract class for menu state methods to be used in GameWorld.
        */
        
        public Texture2D GameTitle { get; set; }
        
        public string PlayerName { get; set; } = string.Empty;
        

        private static MenuStateHandler _instance;
        private bool canPress = true;

        public static MenuStateHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MenuStateHandler();
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

        #endregion
        
        
        public void Initialize()
        {
            MainMenu = new MainMenu();
            SettingsMenu = new SettingsMenu();
            Gameplay = new GamePlay();
            CharacterSelection = new CharacterSelection();
            Controls = new Controls();
            Audio = new Audio();

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


        public void ComponentCleanUp()
        {
            //TODO - Ask if this is okay
            foreach (GameObject go in GameWorld.Instance.gameObjects)
            {
                GameWorld.Instance.Destroy(go);
            }
        }

        public void ChangeState(IStateMenu newMenuState)
        {
            if (CurrentMenuState == newMenuState) return;

            if (CurrentMenuState != null) Console.WriteLine($"Changed Menu State from: {CurrentMenuState.GetType().Name} to {newMenuState.GetType().Name}");
            CurrentMenuState?.Exit();

            CurrentMenuState = newMenuState;
            CurrentMenuState.LoadContent();
            CurrentMenuState.Enter(this);
        }


        private void DataBasteTesting()
        {
            Database.Instance.PrintScore();
        }
    }
}