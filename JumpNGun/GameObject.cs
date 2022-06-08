using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace JumpNGun
{
    public class GameObject
    {
        // List of all components
        private List<Component> _components = new List<Component>(); //list for relevant components

        // Reference to GameOjbects transform
        public Transform Transform { get; set; } = new Transform();

        // Reference to GameObjects Tag
        public string Tag { get; set; }

        /// <summary>
        /// call awake on all components
        /// </summary>
        public void Awake()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Awake();
            }
        }

        /// <summary>
        /// call start on all components
        /// </summary>
        public void Start()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Start();
            }
        }

        /// <summary>
        /// Update all components
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            foreach (var component in _components)
            {
                if (component.IsDisabled) return;

                component.Update(gameTime);
            }
        }

        /// <summary>
        /// Draw relevant sprite for all components
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Add component to a GameObject
        /// </summary>
        /// <param name="component">component to be added</param>
        /// <returns></returns>
        public Component AddComponent(Component component)
        {
            component.GameObject = this;

            _components.Add(component);

            return component;
        }

        /// <summary>
        /// Get component of GameObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Component GetComponent<T>() where T : Component
        {
            return _components.Find(x => x.GetType() == typeof(T));
        }

        /// <summary>
        /// Return component of GameObject if it exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool HasComponent<T>() where T : Component
        {
            Component c = _components.Find(x => x.GetType() == typeof(T));

            return c != null;
        }
    }
}