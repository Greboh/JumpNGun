using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace JumpNGun
{
    public class SoundManager
    {
        /*
           [Description]
           Manager for loading, handling and playback of soundclips
           uses dictionaries to couple a string to a SoundEffectInstance to be used in other classes
       */

        private Random _rnd = new Random();

        #region Menu soundeffects
        private static SoundEffectInstance _menuHover1;
        private static SoundEffectInstance _menuHover2;
        private static SoundEffectInstance _menuHover3;
        private static SoundEffectInstance _menuClick;


        #endregion

        #region Player soundeffects
        private static SoundEffectInstance _jump;
        private static SoundEffectInstance _footstep1;
        private static SoundEffectInstance _footstep2;
        private static SoundEffectInstance _footstep3;
        private static SoundEffectInstance _footstep4;

        private static SoundEffectInstance _jump1;
        private static SoundEffectInstance _jump2;

        private static SoundEffectInstance _pickup;

        private static SoundEffectInstance _enter;
        private static SoundEffectInstance _exit;

        #endregion

        private static SoundEffectInstance _soundtrack1;
        private static SoundEffectInstance _soundtrack2;


        #region Lists
        private Dictionary<string, SoundEffectInstance> _soundEffects;
        private Dictionary<string, SoundEffectInstance> _soundtracks;
        
        #endregion

        public bool SfxDisabled { get; set; }
        public bool MusicDisabled { get; set; }


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
        private void InitDictionary()
        {

            _soundEffects ??= new Dictionary<string, SoundEffectInstance>();    // SFX
            _soundtracks ??= new Dictionary<string, SoundEffectInstance>();     // MUSIC
        }

        /// <summary>
        /// Loads soundeffects
        /// </summary>
        private void InitClips()
        {
            _jump = GameWorld.Instance.Content.Load<SoundEffect>("jump").CreateInstance();
            _menuHover1 = GameWorld.Instance.Content.Load<SoundEffect>("menu_hover_1").CreateInstance();
            _menuHover2 = GameWorld.Instance.Content.Load<SoundEffect>("menu_hover_2").CreateInstance();
            _menuHover3 = GameWorld.Instance.Content.Load<SoundEffect>("menu_hover_3").CreateInstance();
            _menuClick = GameWorld.Instance.Content.Load<SoundEffect>("menu_click").CreateInstance();

            _soundtrack1 = GameWorld.Instance.Content.Load<SoundEffect>("soundtrack_1").CreateInstance();
            _soundtrack2 = GameWorld.Instance.Content.Load<SoundEffect>("soundtrack_2").CreateInstance();

            _footstep1 = GameWorld.Instance.Content.Load<SoundEffect>("footstep_1").CreateInstance();
            _footstep2 = GameWorld.Instance.Content.Load<SoundEffect>("footstep_2").CreateInstance();
            _footstep3 = GameWorld.Instance.Content.Load<SoundEffect>("footstep_3").CreateInstance();
            _footstep4 = GameWorld.Instance.Content.Load<SoundEffect>("footstep_4").CreateInstance();

            _jump1 = GameWorld.Instance.Content.Load<SoundEffect>("jump_1").CreateInstance();
            _jump2 = GameWorld.Instance.Content.Load<SoundEffect>("jump_2").CreateInstance();

            _pickup = GameWorld.Instance.Content.Load<SoundEffect>("pickup").CreateInstance();

            _enter = GameWorld.Instance.Content.Load<SoundEffect>("enter").CreateInstance();
            _exit = GameWorld.Instance.Content.Load<SoundEffect>("exit").CreateInstance();




        }

        /// <summary>
        /// adds soundeffects from InitClips into dictionary with a string attached.
        /// makes it easier to control parameters like volume for all of them at once.
        /// </summary>
        private void BuildClips()
        {
            _soundEffects.Add("jump", _jump);
            _soundEffects.Add("menu_hover_1", _menuHover1);
            _soundEffects.Add("menu_hover_2", _menuHover2);
            _soundEffects.Add("menu_hover_3", _menuHover3);
            _soundEffects.Add("menu_click", _menuClick);

            _soundEffects.Add("footstep_1", _footstep1);
            _soundEffects.Add("footstep_2", _footstep2);
            _soundEffects.Add("footstep_3", _footstep3);
            _soundEffects.Add("footstep_4", _footstep4);

            _soundEffects.Add("jump_1", _jump1);
            _soundEffects.Add("jump_2", _jump2);

            _soundEffects.Add("pickup", _pickup);

            _soundEffects.Add("enter", _enter);
            _soundEffects.Add("exit", _exit);
            
            _soundtracks.Add("soundtrack_1", _soundtrack1); // Main game soundtrack
            _soundtracks.Add("soundtrack_2", _soundtrack2); // Menu soundtrack
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
            int rdm = _rnd.Next(1, 4);

            switch (rdm)
            {
                case 1:
                    _menuHover1.Play();
                    break;
                case 2:
                    _menuHover2.Play();
                    break;
                case 3:
                    _menuHover3.Play();
                    break;
            }
        }

        /// <summary>
        /// Play a random footstep from 4 different sounds
        /// </summary>
        public void PlayRandomFootstep()
        {
            int rdm = _rnd.Next(1, 5);

            switch (rdm)
            {
                case 1:
                    _footstep1.Play();
                    break;
                case 2:
                    _footstep2.Play();
                    break;
                case 3:
                    _footstep3.Play();
                    break;
                case 4:
                    _footstep4.Play();
                    break;
            }
        }

        /// <summary>
        /// Play a random footstep from 4 different sounds
        /// </summary>
        public void PlayRandomJump()
        {
            int rdm = _rnd.Next(1, 3);

            switch (rdm)
            {
                case 1:
                    _jump1.Play();
                    break;
                case 2:
                    _jump2.Play();
                    break;
            }
        }


        /// <summary>
        /// Toggles SoundEffect volume to 0 (off)
        /// </summary>
        public void ToggleSfxOff()
        {
            foreach (KeyValuePair<string, SoundEffectInstance> s in _soundEffects)
            {
                s.Value.Volume = 0;

            }
            SfxDisabled = true;
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
            SfxDisabled = false;

        }

        /// <summary>
        /// Toggles Soundtrack volume to 0 (off)
        /// </summary>
        public void ToggleSoundtrackOff()
        {
            foreach (KeyValuePair<string, SoundEffectInstance> s in _soundtracks)
            {
                s.Value.Volume = 0;
            }
            MusicDisabled = true;
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
            MusicDisabled = false;
        }

        #endregion

    }
}