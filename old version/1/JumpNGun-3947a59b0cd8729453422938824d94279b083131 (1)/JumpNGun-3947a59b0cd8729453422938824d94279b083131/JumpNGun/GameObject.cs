using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public class GameObject
    {
        private List<Component> components = new List<Component>();//list for relevant components

        public Transform Transform { get; set; } = new Transform();

        public string Tag { get; set; }

        public int Id { get; set; }


        /// <summary>
        /// call awake on all components
        /// </summary>
        public void Awake()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Awake();
            }
        }

        /// <summary>
        /// call start on all components
        /// </summary>
        public void Start()
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Start();
            }
        }

        /// <summary>
        /// Update all components
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Update(gameTime);
            }
        }

        /// <summary>
        /// Draw relevant sprite for all components
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].Draw(spriteBatch);
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

            components.Add(component);

            return component;
        }

        /// <summary>
        /// Get component of GameObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Component GetComponent<T>() where T : Component
        {
            return components.Find(x => x.GetType() == typeof(T));
        }

        /// <summary>
        /// Return component of GameObject if it exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool HasComponent<T>() where T : Component
        {
            Component c = components.Find(x => x.GetType() == typeof(T));

            return c != null;
        }
    }
}
