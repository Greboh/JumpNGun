using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace JumpNGun
{
    public class SoundManager
    {
        protected Random myRandom = new Random();

        #region SoundEffects
        private static SoundEffectInstance jump;
        private static SoundEffectInstance menu_hover_1;
        private static SoundEffectInstance menu_hover_2;
        private static SoundEffectInstance menu_hover_3;
        private static SoundEffectInstance menu_click;


        #endregion

        private static SoundEffectInstance soundtrack_1;
        private static SoundEffectInstance soundtrack_2;


        #region Lists
        private Dictionary<string, SoundEffectInstance> _soundEffects;
        private Dictionary<string, SoundEffectInstance> _soundtracks;
        private List<SoundEffectInstance> soundEffects = new List<SoundEffectInstance>();//list for soundeffects to easier control volume for all soundeffects
        private List<SoundEffectInstance> soundtracks = new List<SoundEffectInstance>();//list for soundeffects to easier control volume for all soundeffects


        #endregion

        public bool _sfxDisabled = false;
        public bool _musicDisabled = false;



        #region Instance
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
        
        #endregion

        #region Methods

        /// <summary>
        /// Initializes the dictionary
        /// </summary>
        public void InitDictionary()
        {

            _soundEffects ??= new Dictionary<string, SoundEffectInstance>();
            _soundtracks ??= new Dictionary<string, SoundEffectInstance>();



        }

        /// <summary>
        /// Loads soundeffects
        /// </summary>
        public void InitClips()
        {
            //soundtrack = GameWorld.Instance.Content.Load<SoundEffect>("soundtrack").CreateInstance;
            jump = GameWorld.Instance.Content.Load<SoundEffect>("jump").CreateInstance();
            menu_hover_1 = GameWorld.Instance.Content.Load<SoundEffect>("menu_hover_1").CreateInstance();
            menu_hover_2 = GameWorld.Instance.Content.Load<SoundEffect>("menu_hover_2").CreateInstance();
            menu_hover_3 = GameWorld.Instance.Content.Load<SoundEffect>("menu_hover_3").CreateInstance();
            menu_click = GameWorld.Instance.Content.Load<SoundEffect>("menu_click").CreateInstance();

            soundtrack_1 = GameWorld.Instance.Content.Load<SoundEffect>("soundtrack_1").CreateInstance();
            soundtrack_2 = GameWorld.Instance.Content.Load<SoundEffect>("soundtrack_2").CreateInstance();



        }

        /// <summary>
        /// adds soundeffects from InitClips into dictionary with a string attached.
        /// makes it easier to control parameters like volume for all of them at once.
        /// </summary>
        public void BuildClips()
        {
            _soundEffects.Add("jump", jump);
            _soundEffects.Add("menu_hover_1", menu_hover_1);
            _soundEffects.Add("menu_hover_2", menu_hover_2);
            _soundEffects.Add("menu_hover_3", menu_hover_3);
            _soundEffects.Add("menu_click", menu_click);

            _soundtracks.Add("soundtrack_1", soundtrack_1);
            _soundtracks.Add("soundtrack_2", soundtrack_2);





        }

        /// <summary>
        /// Method recieves string parameter from other class, and looks up in _soundEffects dictionary
        /// and finds the matching string.
        /// if string matches the soundeffect will play
        /// </summary>
        /// <param name="name"></param>
        public void PlayClip(string name)
        {

            if (_instance._soundEffects.TryGetValue(name, out SoundEffectInstance clip))
            {
                clip.Play();
            }
            if (_instance._soundtracks.TryGetValue(name, out SoundEffectInstance music))
            {
                music.Play();
            }
        }

        public void StopClip(string name)
        {

            if (_instance._soundEffects.TryGetValue(name, out SoundEffectInstance clip))
            {
                clip.Stop();
            }
            if (_instance._soundtracks.TryGetValue(name, out SoundEffectInstance music))
            {
                music.Stop();
            }
        }


        /// <summary>
        /// Play a random ui click from 3 different clicks
        /// </summary>
        public void PlayRandomClick()
        {
            int rdm = myRandom.Next(1, 4);

            //TODO: Fix this garbage
            if (rdm == 1)
            {
                menu_hover_1.Play();

            }
            else if (rdm == 2)
            {
                menu_hover_2.Play();

            }
            else if (rdm == 3)
            {
                menu_hover_3.Play();

            }


        }

      

        /// <summary>
        /// Toggles SoundEffect volume to 0 (off)
        /// </summary>
        public void toggleSFXOff()
        {
            foreach (KeyValuePair<string, SoundEffectInstance> s in _soundEffects)
            {
                s.Value.Volume = 0;

            }
            _sfxDisabled = true;
        }

        /// <summary>
        /// Toggles SoundEffect volume to 1 (on)
        /// </summary>
        public void toggleSFXOn()
        {
            foreach (KeyValuePair<string, SoundEffectInstance> s in _soundEffects)
            {
                s.Value.Volume = 1;
            }
            _sfxDisabled = false;

        }

        /// <summary>
        /// Toggles Soundtrack volume to 0 (off)
        /// </summary>
        public void toggleSoundtrackOff()
        {
            foreach (KeyValuePair<string, SoundEffectInstance> s in _soundtracks)
            {
                s.Value.Volume = 0;
            }
            _musicDisabled = true;
        }

        /// <summary>
        /// Toggles Soundtrack volume to 1 (on)
        /// </summary>
        public void toggleSoundtrackOn()
        {
            foreach (KeyValuePair<string,SoundEffectInstance> s in _soundtracks)
            {
                s.Value.Volume = 1;
            }
            _musicDisabled = false;
        }

        #endregion

    }
}