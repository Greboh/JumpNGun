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
        /*
           [Description]
           Manager for loading, handling and playback of soundclips
           uses dictionaries to couple a string to a SoundEffectInstance to be used in other classes
       */

        protected Random myRandom = new Random();

        #region Menu soundeffects
        private static SoundEffectInstance menu_hover_1;
        private static SoundEffectInstance menu_hover_2;
        private static SoundEffectInstance menu_hover_3;
        private static SoundEffectInstance menu_click;


        #endregion

        #region Player soundeffects
        private static SoundEffectInstance jump;
        private static SoundEffectInstance footstep_1;
        private static SoundEffectInstance footstep_2;
        private static SoundEffectInstance footstep_3;
        private static SoundEffectInstance footstep_4;

        private static SoundEffectInstance jump_1;
        private static SoundEffectInstance jump_2;

        private static SoundEffectInstance pickup;

        private static SoundEffectInstance enter;
        private static SoundEffectInstance exit;




        #endregion

        private static SoundEffectInstance soundtrack_1;
        private static SoundEffectInstance soundtrack_2;


        #region Lists
        private Dictionary<string, SoundEffectInstance> _soundEffects;
        private Dictionary<string, SoundEffectInstance> _soundtracks;
        private List<SoundEffectInstance> soundEffects = new List<SoundEffectInstance>();//list for soundeffects to easier control volume for all soundeffects
        private List<SoundEffectInstance> soundtracks = new List<SoundEffectInstance>();//list for soundeffects to easier control volume for all soundeffects


        #endregion

        public bool _sfxDisabled = false; // bool for controlling sfx output
        public bool _musicDisabled = false; // bool for controlling music output



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

            _soundEffects ??= new Dictionary<string, SoundEffectInstance>();    // SFX
            _soundtracks ??= new Dictionary<string, SoundEffectInstance>();     // MUSIC



        }

        /// <summary>
        /// Loads soundeffects
        /// </summary>
        public void InitClips()
        {
            jump = GameWorld.Instance.Content.Load<SoundEffect>("jump").CreateInstance();
            menu_hover_1 = GameWorld.Instance.Content.Load<SoundEffect>("menu_hover_1").CreateInstance();
            menu_hover_2 = GameWorld.Instance.Content.Load<SoundEffect>("menu_hover_2").CreateInstance();
            menu_hover_3 = GameWorld.Instance.Content.Load<SoundEffect>("menu_hover_3").CreateInstance();
            menu_click = GameWorld.Instance.Content.Load<SoundEffect>("menu_click").CreateInstance();

            soundtrack_1 = GameWorld.Instance.Content.Load<SoundEffect>("soundtrack_1").CreateInstance();
            soundtrack_2 = GameWorld.Instance.Content.Load<SoundEffect>("soundtrack_2").CreateInstance();

            footstep_1 = GameWorld.Instance.Content.Load<SoundEffect>("footstep_1").CreateInstance();
            footstep_2 = GameWorld.Instance.Content.Load<SoundEffect>("footstep_2").CreateInstance();
            footstep_3 = GameWorld.Instance.Content.Load<SoundEffect>("footstep_3").CreateInstance();
            footstep_4 = GameWorld.Instance.Content.Load<SoundEffect>("footstep_4").CreateInstance();

            jump_1 = GameWorld.Instance.Content.Load<SoundEffect>("jump_1").CreateInstance();
            jump_2 = GameWorld.Instance.Content.Load<SoundEffect>("jump_2").CreateInstance();

            pickup = GameWorld.Instance.Content.Load<SoundEffect>("pickup").CreateInstance();

            enter = GameWorld.Instance.Content.Load<SoundEffect>("enter").CreateInstance();
            exit = GameWorld.Instance.Content.Load<SoundEffect>("exit").CreateInstance();




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

            _soundEffects.Add("footstep_1", footstep_1);
            _soundEffects.Add("footstep_2", footstep_2);
            _soundEffects.Add("footstep_3", footstep_3);
            _soundEffects.Add("footstep_4", footstep_4);

            _soundEffects.Add("jump_1", jump_1);
            _soundEffects.Add("jump_2", jump_2);

            _soundEffects.Add("pickup", pickup);

            _soundEffects.Add("enter", enter);
            _soundEffects.Add("exit", exit);






            _soundtracks.Add("soundtrack_1", soundtrack_1); // Main game soundtrack
            _soundtracks.Add("soundtrack_2", soundtrack_2); // Menu soundtrack





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
        /// Play a random footstep from 4 different sounds
        /// </summary>
        public void PlayRandomFootstep()
        {
            int rdm = myRandom.Next(1, 5);

            if (rdm == 1)
            {
                footstep_1.Play();

            }
            else if (rdm == 2)
            {
                footstep_2.Play();

            }
            else if (rdm == 3)
            {
                footstep_3.Play();

            }
            else if (rdm == 4)
            {
                footstep_4.Play();

            }
        }

        /// <summary>
        /// Play a random footstep from 4 different sounds
        /// </summary>
        public void PlayRandomJump()
        {
            int rdm = myRandom.Next(1, 3);

            if (rdm == 1)
            {
                jump_1.Play();

            }
            else if (rdm == 2)
            {
                jump_2.Play();

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