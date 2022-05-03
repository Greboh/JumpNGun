using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public abstract class ObjectPool
    {
        protected List<GameObject> active = new List<GameObject>();

        protected Stack<GameObject> inactive = new Stack<GameObject>();


        public GameObject GetObject()
        {

            if (inactive.Count < 15)
            {
                //Console.WriteLine("Created new");

                return CreateObject();
            }
            else
            {
                //Console.WriteLine("Spawned inactive");
                GameObject go = inactive.Pop();
                active.Add(go);
                return go;
            }
        }

        public void ReleaseObject(GameObject gameObject)
        {
            active.Remove(gameObject);
            inactive.Push(gameObject);
            CleanUp(gameObject);
            GameWorld.Instance.Destroy(gameObject);

        }

        protected abstract GameObject CreateObject();

        protected abstract void CleanUp(GameObject gameObject);
    }
}
