using System;
using System.Collections.Generic;

namespace JumpNGun
{
    /// <summary>
    /// Klassen er lavet af Nichlas Hoberg
    /// OBS denne har vi brugt i ældre projekter også!
    /// Den er baseret på en artikel fra Unity Learn, se mere i Rapporten!
    /// </summary>
    public class EventHandler
    {
        // Contains all events
        private Dictionary<string, Action<Dictionary<string, object>>> _events; 
        
        private static EventHandler _instance;

        /// <summary>
        /// Property to set the _eventManager instance
        /// </summary>
        public static EventHandler Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventHandler();
                    _instance.InitDictionary();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Initializes the dictionary
        /// </summary>
        private void InitDictionary()
        {
            _events ??= new Dictionary<string, Action<Dictionary<string, object>>>();
        }

        /// <summary>
        /// Subscribe to an event
        /// </summary>
        /// <param name="name">Name of the event that it should subscribe to</param>
        /// <param name="subscriber">Reference of Action that it should subscribe to</param>
        public void Subscribe(string name, Action<Dictionary<string, object>> subscriber)
        {
            // check if the name already exists in the dictionary and if it does just add the TValue
            if (Instance._events.TryGetValue(name, out Action<Dictionary<string, object>> currentEvent))
            {
                currentEvent += subscriber;
                
                // Override the old value with the new
                Instance._events[name] = currentEvent;
            }
            // If it doesnt exist already add both TKey and TValue
            else
            {
                currentEvent += subscriber;
                Instance._events.Add(name, currentEvent);
            }
        }

        /// <summary>
        /// Unsubscribe to an event
        /// </summary>
        /// <param name="name">Name of the event that it should subscribe to</param>
        /// <param name="unsubscriber">Reference of Action that it should unsubscribe to</param>
        public void Unsubscribe(string name, Action<Dictionary<string, object>> unsubscriber)
        {
            // Check if the name matches a TKey and remove the event from dictionary otherwise do nothing.
            if (Instance._events.TryGetValue(name, out Action<Dictionary<string, object>> currentEvent))
                Instance._events.Remove(name);
        }

        /// <summary>
        /// Trigger an event
        /// </summary>
        /// <param name="name">Name of the event that it should trigger</param>
        /// <param name="context">The value(s) that it sends</param>
        public void TriggerEvent(string name, Dictionary<string, object> context)
        {
            // Check if the name matches a TKey and Invoke event otherwise do nothing.
            if (Instance._events.TryGetValue(name, out Action<Dictionary<string, object>> currentEvent))
            {
                // Make sure the currentEvent is not null and then Invoke / Callx1 it
                currentEvent?.Invoke(context);
            }
            else Console.WriteLine($"Cannot trigger event: {name} since it doesn't exist!");
        }
    }
}
