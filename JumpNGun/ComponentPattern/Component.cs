using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public class Component
    {
        public GameObject GameObject { get; set; }

        public bool IsDisabled { get; private set; } = false;

        public virtual void Awake()
        {
            EventManager.Instance.Subscribe("OnFreeze", OnFreeze);
        }

        public virtual void Start()
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
        
        private void OnFreeze(Dictionary<string, object> ctx)
        {
            if (!GameObject.HasComponent<Button>())
            {
                IsDisabled = (bool) ctx["freeze"];
            }
        }
    }
}
