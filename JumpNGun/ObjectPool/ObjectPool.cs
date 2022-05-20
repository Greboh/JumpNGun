using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public abstract class ObjectPool
    {
        //TODO Objectpool platforms - NOT DONE

        protected List<GameObject> active = new List<GameObject>(); //gameobjects in game(active)

        protected Stack<GameObject> inactive = new Stack<GameObject>();//inactive gameobjects(does not exist in game)


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

        /// <summary>
        /// Remove GameObject from game and add to inactive list
        /// </summary>
        /// <param name="gameObject"></param>
        public void ReleaseObject(GameObject gameObject)
        {
            //remove form active list
            active.Remove(gameObject);

            //add to inactive stack
            inactive.Push(gameObject);

            //cleanup on gameobject
            CleanUp(gameObject);

            GameWorld.Instance.Destroy(gameObject);

        }

        /// <summary>
        /// Method to create new gameobject
        /// </summary>
        /// <returns></returns>
        protected abstract GameObject CreateObject();

        /// <summary>
        /// Cleanup and remove components from game
        /// </summary>
        /// <param name="gameObject"></param>
        protected abstract void CleanUp(GameObject gameObject);
    }
}
