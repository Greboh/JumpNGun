using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;
using JumpNGun.StatePattern.GameStates;
using System.Linq;
using JumpNGun.ComponentPattern;

namespace JumpNGun
{
    public class GameWorld : Game
    {
        private static GameWorld instance;

        /// <summary>
        /// Property to set the GameWorld instance
        /// </summary>
        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();
                }
                return instance;
            }
        }

        public GraphicsDeviceManager Graphics { get; private set; }
        private SpriteBatch _spriteBatch;

        public List<GameObject> gameObjects = new List<GameObject>();//List of active GameObjects

        public List<GameObject> newGameObjects = new List<GameObject>();//List of newly added/instatiated GameObjects

        public List<GameObject> destroyedGameObjects = new List<GameObject>();//List of GameObjects that will be destroyed or removed to object pool

        public List<Collider> Colliders { get; private set; } = new List<Collider>();//List of current active Colliders

        public LinkedList<State> PreviousStates = new LinkedList<State>();

        private int _screenWidth = 1325;
        private int _screenHeight = 800;

        public State _currentState;
        private State _nextState;
        public State _previousState;

        private Background _background;

        private bool isRunning = false;

        public MouseState myMouse { get; private set; }
        public Vector2 MousePosition { get; private set; }
        public Vector2 ScreenSize { get; private set; }

        public static float DeltaTime { get; private set; }
        public List<GameObject> GameObjects { get => gameObjects; set => gameObjects = value; }
        public bool IsRunning { get => isRunning; set => isRunning = value; }

        public GameWorld()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Graphics.PreferredBackBufferWidth = _screenWidth;
            Graphics.PreferredBackBufferHeight = _screenHeight;
            ScreenSize = new Vector2(_screenWidth, _screenHeight);
            Graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            SoundManager.Instance.InitDictionary();
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _background = new Background();
            _background.LoadContent();

            _currentState = new MainMenuState(); // sets first state to show on startup
            _currentState.LoadContent(); // loads state content into GameWorld content
            _nextState = null; // makes sure next state is empty on startup
        }

        protected override void Update(GameTime gameTime)
        {
            //if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            //if (Keyboard.GetState().IsKeyDown(Keys.U)) SoundManager.Instance.toggleSFXOff();
            //if (Keyboard.GetState().IsKeyDown(Keys.I)) SoundManager.Instance.toggleSFXOn();

            _background.Update(gameTime);



            myMouse = Mouse.GetState();
            MousePosition = new Vector2(myMouse.X, myMouse.Y);

            DeltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            
            if (_nextState != null)
            {
                _currentState = _nextState;
                _currentState.LoadContent();
                _nextState = null;
            }
            else _currentState.Update(gameTime);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _background.Draw(_spriteBatch);

            _currentState.Draw(gameTime, _spriteBatch);

            //_spriteBatch.Begin();

            //_spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// // sets nextState with state recived from Button.cs
        /// </summary>
        /// <param name="state"></param>
        public void ChangeState(State state)
        {
            _previousState = _currentState;
            _nextState = state; 
        }

        public State GetCurrentState()
        {
            return _currentState;
        }

        

        /// <summary>
        /// Instantiate object by adding them to list of newGameObjects
        /// </summary>
        /// <param name="go">GameObject to be added to game</param>
        public void Instantiate(GameObject go)
        {
            newGameObjects.Add(go);
        }

        /// <summary>
        /// Destroy and remove active GameObject from game by adding them to list of destroyedGameObjects
        /// </summary>
        /// <param name="go">GameObject to be destoyed</param>
        public void Destroy(GameObject go)
        {
            destroyedGameObjects.Add(go);
        }

        /// <summary>
        /// Removes, adds, and activates relevant GameObjects and components from game
        /// </summary>
        public void CleanUp()
        {
            for (int i = 0; i < newGameObjects.Count; i++)
            {
                gameObjects.Add(newGameObjects[i]);
                newGameObjects[i].Awake();
                newGameObjects[i].Start();
                AddCollider(newGameObjects[i]);
            }

            for (int i = 0; i < destroyedGameObjects.Count; i++)
            {
                gameObjects.Remove(destroyedGameObjects[i]);

                RemoveCollider(destroyedGameObjects[i]);

            }
            destroyedGameObjects.Clear();
            newGameObjects.Clear();
        }

        /// <summary>
        /// Adds collider of gameObject to game if needed
        /// </summary>
        /// <param name="gameObject"></param>
        private void AddCollider(GameObject gameObject)
        {
            Collider col = (Collider)gameObject.GetComponent<Collider>();
            if (col != null)
            {
                Colliders.Add(col);
            }
        }

        /// <summary>
        /// Removes collider of gameObject from game if needed
        /// </summary>
        /// <param name="gameObject"></param>
        private void RemoveCollider(GameObject gameObject)
        {
            Collider col = (Collider)gameObject.GetComponent<Collider>();

            if (col != null)
            {
                Colliders.Remove(col);
            }
        }

        /// <summary>
        /// Find GameObjects with a specific component
        /// </summary>
        /// <typeparam name="T">The component to find</typeparam>
        /// <returns>GameObject with the component</returns>
        public Component FindObjectOfType<T>() where T : Component
        {
            foreach (GameObject gameObject in gameObjects)
            {
                Component c = gameObject.GetComponent<T>();

                if (c != null)
                {
                    return c;
                }
            }

            return null;


        }


    }
}
