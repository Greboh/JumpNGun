using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    public class EventManager
    {
        private Dictionary<string, Action<Dictionary<string, object>>> _eventDictionary;

        private static EventManager instance;

        /// <summary>
        /// Property to set the _eventManager instance
        /// </summary>
        public static EventManager Instance
        {
            get
            {
                if (instance == null)
                {
                    Console.WriteLine("No EventManager found, creating one!");
                    Console.WriteLine("____________________________________");
                    instance = new EventManager();
                    instance.InitDictionary();
                }
                return instance;
            }
        }

        public Dictionary<string, Action<Dictionary<string, object>>> EventDictionary { get => _eventDictionary; set => _eventDictionary = value; }

        /// <summary>
        /// Initializes the dictionary
        /// </summary>
        private void InitDictionary()
        {
            if (_eventDictionary == null) _eventDictionary = new Dictionary<string, Action<Dictionary<string, object>>>();
        }

        /// <summary>
        /// Subscribe to an event
        /// </summary>
        /// <param name="eventName">Name of the event that it should subscribe to</param>
        /// <param name="listener">Name of Action that it should listen to</param>
        public static void Subscribe(string eventName, Action<Dictionary<string, object>> listener)
        {
            Action<Dictionary<string, object>> thisEvent;

            if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent += listener;
                Instance._eventDictionary[eventName] = thisEvent;
            }
            else
            {
                thisEvent += listener;
                Instance._eventDictionary.Add(eventName, thisEvent);
            }
        }

        /// <summary>
        /// Unsubscribe to an event
        /// </summary>
        /// <param name="eventName">Name of the event that it should subscribe to</param>
        /// <param name="listener">Name of Action that it should listen to</param>
        public static void Unsubscribe(string eventName, Action<Dictionary<string, object>> listener)
        {
            if (instance == null) return;
            Action<Dictionary<string, object>> thisEvent;
            if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent -= listener;
                Instance._eventDictionary[eventName] = thisEvent;
                Instance._eventDictionary.Remove(eventName);
            }
        }

        /// <summary>
        /// Trigger an event
        /// </summary>
        /// <param name="eventName">Name of the event that it should trigger</param>
        /// <param name="message">The value that it sents</param>
        public static void TriggerEvent(string eventName, Dictionary<string, object> message)
        {
            Action<Dictionary<string, object>> thisEvent = null;
            if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                if (thisEvent != null)
                {
                    thisEvent.Invoke(message);
                }
            }
            //else Console.WriteLine($"Cannot trigger event: {eventName} since it doesn't exist!");
        }
    }
}
