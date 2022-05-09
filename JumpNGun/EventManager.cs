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

        /// <summary>
        /// Initializes the dictionary
        /// </summary>
        private void InitDictionary()
        {
            _eventDictionary ??= new Dictionary<string, Action<Dictionary<string, object>>>();
        }

        /// <summary>
        /// Subscribe to an event
        /// </summary>
        /// <param name="eventName">Name of the event that it should subscribe to</param>
        /// <param name="subscriber">Reference of Action that it should subscribe to</param>
        public void Subscribe(string eventName, Action<Dictionary<string, object>> subscriber)
        {
            Action<Dictionary<string, object>> currentEvent;

            // check if the eventName already exists in the dictionary and if it does just add the TValue
            if (Instance._eventDictionary.TryGetValue(eventName, out currentEvent))
            {
                currentEvent += subscriber;
                Instance._eventDictionary[eventName] = currentEvent;
            }
            // If it doesnt exist already add both TKey and TValue. This makes sure that we don't try to create duplicates of the same TKey since this will lead to error
            else
            {
                currentEvent += subscriber;
                Instance._eventDictionary.Add(eventName, currentEvent);
            }
        }

        /// <summary>
        /// Unsubscribe to an event
        /// </summary>
        /// <param name="eventName">Name of the event that it should subscribe to</param>
        /// <param name="subscriber">Reference of Action that it should listen to</param>
        public void Unsubscribe(string eventName, Action<Dictionary<string, object>> subscriber)
        {
            Action<Dictionary<string, object>> currentEvent;
            
            // Check if the eventName matches a TKey and remove the event from dictionary otherwise do nothing.
            if (Instance._eventDictionary.TryGetValue(eventName, out currentEvent))
            {
                currentEvent -= subscriber;
                Instance._eventDictionary[eventName] = currentEvent;
                Instance._eventDictionary.Remove(eventName);
            }
        }

        /// <summary>
        /// Trigger an event
        /// </summary>
        /// <param name="eventName">Name of the event that it should trigger</param>
        /// <param name="context">The value that it sends</param>
        public void TriggerEvent(string eventName, Dictionary<string, object> context)
        {
            Action<Dictionary<string, object>> currentEvent = null;
            
            // Check if the eventName matches a TKey and Invoke event otherwise do nothing.
            if (Instance._eventDictionary.TryGetValue(eventName, out currentEvent))
            {
                currentEvent?.Invoke(context);
            }
            else Console.WriteLine($"Cannot trigger event: {eventName} since it doesn't exist!");
        }
    }
}
