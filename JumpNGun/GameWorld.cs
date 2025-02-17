﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af alle
    /// </summary>
    public class GameWorld : Game
    {
        private static GameWorld _instance;

        /// <summary>
        /// Property to set the GameWorld instance
        /// </summary>
        public static GameWorld Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameWorld();
                }
                return _instance;
            }
        }
       
        public GraphicsDeviceManager Graphics { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }

        public List<GameObject> GameObjects { get; private set; } = new List<GameObject>();//List of active GameObjects

        public List<GameObject> newGameObjects = new List<GameObject>();//List of newly added/instatiated GameObjects

        public List<GameObject> destroyedGameObjects = new List<GameObject>();//List of GameObjects that will be destroyed or removed to object pool

        public List<Collider> Colliders { get; private set; } = new List<Collider>();//List of current active Colliders
        
        private int _screenWidth = 1325;
        private int _screenHeight = 800;
        
        public bool IsPaused { get; set; }

        private Background _background;
        public MouseState MyMouse { get; private set; }
        public Vector2 MousePosition { get; private set; }
        public Vector2 ScreenSize { get; private set; }

        public static float DeltaTime { get; private set; }

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
            _background = new Background();
            
            MenuStateHandler.Instance.ChangeState(new LandingScreen());
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            _background.LoadContent();
            MenuStateHandler.Instance.LoadContent();
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            _background.Update();
            MyMouse = Mouse.GetState();
            MousePosition = new Vector2(MyMouse.X, MyMouse.Y);
            DeltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            
            MenuStateHandler.Instance.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _background.Draw(SpriteBatch);
            
            MenuStateHandler.Instance.Draw(SpriteBatch);
            
            base.Draw(gameTime);

        }

        #region Class Methods

        /// <summary>
        /// Instantiate object by adding them to list of newGameObjects
        /// </summary>
        /// <param name="go">GameObject to be added to game</param>
        public void Instantiate(GameObject go)
        {
            newGameObjects.Add(go);
        }

        /// <summary>
        /// Remove active GameObject from game by adding them to list of destroyedGameObjects
        /// </summary>
        /// <param name="go">GameObject to be destoyed</param>
        public void Destroy(GameObject go)
        {
            destroyedGameObjects.Add(go);
        }

        /// <summary>
        /// Removes, adds, and activates relevant GameObjects and components from game
        /// </summary>
        public void CleanUpGameObjects()
        {
            for (int i = 0; i < newGameObjects.Count; i++)
            {
                AddCollider(newGameObjects[i]);
                GameObjects.Add(newGameObjects[i]);
                newGameObjects[i].Awake();
                newGameObjects[i].Start();
            }

            for (int i = 0; i < destroyedGameObjects.Count; i++)
            {
                RemoveCollider(destroyedGameObjects[i]);
                GameObjects.Remove(destroyedGameObjects[i]);
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
            foreach (GameObject gameObject in GameObjects)
            {
                Component c = gameObject.GetComponent<T>();

                if (c != null)
                {
                    return c;
                }
            }
            return null;
        }

        #endregion
    }
}
