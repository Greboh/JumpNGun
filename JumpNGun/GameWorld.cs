using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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

  

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private List<GameObject> gameObjects = new List<GameObject>();//List of active GameObjects

        private List<GameObject> newGameObjects = new List<GameObject>();//List of newly added/instatiated GameObjects

        private List<GameObject> destroyedGameObjects = new List<GameObject>();//List of GameObjects that will be destroyed or removed to object pool

        public List<Collider> Colliders { get; private set; } = new List<Collider>();//List of current active Colliders

        private int _screenWidth = 1200;
        private int _screenHeight = 600;

        public Vector2 ScreenSize { get; private set; }

        public static float DeltaTime { get; private set; }

        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = _screenWidth;
            _graphics.PreferredBackBufferHeight = _screenHeight;
            ScreenSize = new Vector2(_screenWidth, _screenHeight);
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            Director playerDirector = new Director(new PlayerBuilder(CharacterType.Soldier));
            gameObjects.Add(playerDirector.Construct());
            
            //call awake method on every active GameObject in list
            foreach (var go in gameObjects)
            {
                go.Awake();
            }
            for (int i = 0; i < 11; i++)
            {
                Instantiate(PlatformFactory.Instance.Create(PlatformType.ground));
            }

            Instantiate(PlatformFactory.Instance.Create(PlatformType.grass));
            
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //call start method on every active GameObject in list
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Start();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            //call update method on every active GameObject in list
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update(gameTime);
            }
            
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;



            //call cleanup in every cycle
            CleanUp();
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            //draw sprites of every active gameObject in list
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Draw(_spriteBatch);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
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
        private void CleanUp()
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
