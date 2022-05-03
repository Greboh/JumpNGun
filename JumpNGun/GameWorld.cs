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
        /// GameWorld set to a singleton
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

        private List<GameObject> gameObjects = new List<GameObject>();

        private List<GameObject> newGameObjects = new List<GameObject>();

        private List<GameObject> destroyedGameObjects = new List<GameObject>();

        public List<Collider> Colliders { get; private set; } = new List<Collider>();

        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            foreach (var go in gameObjects)
            {
                go.Awake();
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Start();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update(gameTime);
            }

            base.Update(gameTime);

            CleanUp();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Draw(_spriteBatch);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Instantiate(GameObject go)
        {
            newGameObjects.Add(go);
        }

        public void Destroy(GameObject go)
        {
            destroyedGameObjects.Add(go);
        }

        private void CleanUp()
        {
            for (int i = 0; i < newGameObjects.Count; i++)
            {
                gameObjects.Add(newGameObjects[i]);
                newGameObjects[i].Awake();
                newGameObjects[i].Start();

                Collider col = (Collider)newGameObjects[i].GetComponent<Collider>();
                if (col != null)
                {
                    Colliders.Add(col);
                }
            }

            for (int i = 0; i < destroyedGameObjects.Count; i++)
            {
                gameObjects.Remove(destroyedGameObjects[i]);

                Collider col = (Collider)destroyedGameObjects[i].GetComponent<Collider>();

                if (col != null)
                {
                    Colliders.Remove(col);
                }

            }
            destroyedGameObjects.Clear();
            newGameObjects.Clear();
        }
    }
}
