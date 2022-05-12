using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace JumpNGun
{
    public class SoundManager : GameObject
    {

        private Dictionary<string, SoundEffect> _soundEffects;
      

        private static SoundManager _instance;

        /// <summary>
        /// Property to set the SoundManager instance
        /// </summary>
        public static SoundManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Console.WriteLine("No SoundManager found, creating one!");
                    Console.WriteLine("____________________________________");
                    _instance = new SoundManager();
                    _instance.InitDictionary();
                    _instance.InitClips();
                    _instance.BuildClips();

                }
                return _instance;
            }
        }
        
        
        #region SoundEffects
        private SoundEffect test;
        private SoundEffect soundtrack;
        

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the dictionary
        /// </summary>
        private void InitDictionary()
        {

            _soundEffects ??= new Dictionary<string, SoundEffect>();
            


        }

        /// <summary>
        /// Loads soundeffects
        /// </summary>
        private void InitClips()
        {
            test = GameWorld.Instance.Content.Load<SoundEffect>("test");
            soundtrack = GameWorld.Instance.Content.Load<SoundEffect>("soundtrack");

        }

        /// <summary>
        /// adds soundeffects from InitClips into dictionary with a string attached
        /// </summary>
        private void BuildClips()
        {
            _soundEffects.Add("test", test);
            _soundEffects.Add("soundtrack", soundtrack);
        }

        /// <summary>
        /// Method recieves string parameter from other class, and looks up in _soundEffects dictionary
        /// and finds the matching string.
        /// if string matches the soundeffect will play
        /// </summary>
        /// <param name="name"></param>
        public void PlayClip(string name)
        {

            if (_instance._soundEffects.TryGetValue(name, out SoundEffect clip))
            {
                clip.Play();
            }
        }

        #endregion

    }
}
