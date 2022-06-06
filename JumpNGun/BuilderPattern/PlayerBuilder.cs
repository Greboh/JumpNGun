using System;
using System.Security.Cryptography;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    /// <summary>
    /// What character the player is
    /// </summary>
    public enum CharacterType
    {
        Soldier,
        Ranger,
        Wizard
    }

    public class PlayerBuilder : IBuilder
    {
        private CharacterType _character; // Reference to the CharacterType
        private GameObject _gameObject; // Player GameObject

        /// <summary>
        /// Constructor to set the CharacterType
        /// </summary>
        /// <param name="character">What Character the player is</param>
        public PlayerBuilder(CharacterType character)
        {
            _character = character;
        }

        public void BuildGameObject()
        {
            _gameObject = new GameObject();

            BuildComponents(_character);

            Animator animator = (Animator) _gameObject.GetComponent<Animator>();

            SpriteRenderer sr = (SpriteRenderer) _gameObject.GetComponent<SpriteRenderer>();

            switch (_character)
            {
                case CharacterType.Soldier:
                {
                    sr.SetSprite("1_Soldier_idle");
                    animator.AddAnimation(BuildAnimation("Idle", new string[] {"1_Soldier_idle", "2_Soldier_idle", 
                        "3_Soldier_idle", "4_Soldier_idle", "5_Soldier_idle"}));             
            
                    animator.AddAnimation(BuildAnimation("Run", new string[] {"1_Soldier_run", "2_Soldier_run", 
                        "3_Soldier_run", "4_Soldier_run", "5_Soldier_run", "6_Soldier_run"}));            
            
                    animator.AddAnimation(BuildAnimation("Jump", new string[] {"1_Soldier_jump", "2_Soldier_jump"}));
                    
                    // animator.AddAnimation(BuildAnimation("Death", new []{"1_Soldier_Death", "2_Soldier_Death", "3_Soldier_Death"}));
                    
                } break;
                case CharacterType.Ranger:
                {
                    sr.SetSprite("1_Ranger_idle");
                    
                    // animator.AddAnimation(BuildAnimation("Idle", new string[] {"1_Ranger_idle", "2__Ranger_idle", 
                    //     "3__Ranger_idle", "4_Ranger_idle", "5_Ranger_idle"}));             
                    //
                    // animator.AddAnimation(BuildAnimation("Run", new string[] {"1_Ranger_run", "2_Ranger_run", 
                    //     "3_Ranger_run", "4_Ranger_run", "5_Ranger_run", "6_Ranger_run"}));            
                    //
                    // animator.AddAnimation(BuildAnimation("Jump", new string[] {"1_Ranger_jump", "2_Ranger_jump"}));
                    
                    // animator.AddAnimation(BuildAnimation("Death", new []{"1_Soldier_Death", "2_Soldier_Death", "3_Soldier_Death"}));
                    
                } break;
                
                case CharacterType.Wizard:
                {
                    
                } break;
                

            }
            

        }

        /// <summary>
        /// Builds the player with all the required components
        /// </summary>
        /// <param name="character">What Character the player is</param>
        private void BuildComponents(CharacterType character)
        {
            switch (character)
            {
                case CharacterType.Soldier:
                    _gameObject.AddComponent(new Player(character, 100, -100, 50, 0.5f, 1f, 2, 100, 350, 15));
                    break;
                case CharacterType.Ranger:
                    _gameObject.AddComponent(new Player(character,150, -120, 75, 0.25f, 1.5f, 2, 80, 250, 20));
                    break;
                case CharacterType.Wizard:
                    break;
            }
            
            // Add all relevant components
            _gameObject.AddComponent(new SpriteRenderer());
            _gameObject.AddComponent(new Animator());
            _gameObject.AddComponent(new Input());
            _gameObject.AddComponent(new Collider());
            _gameObject.AddComponent(new LevelSystem());
            _gameObject.Tag = "player";
            
            

        }

        /// <summary>
        /// Build all animations relevant to movement 
        /// </summary>
        /// <param name="animationName">Name of the animation set</param>
        /// <param name="spriteNames">Name of the sprites in the animation set </param>
        /// <returns></returns>
        private Animation BuildAnimation(string animationName, string[] spriteNames)
        {
            Texture2D[] sprites = new Texture2D[spriteNames.Length];

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i] = GameWorld.Instance.Content.Load<Texture2D>(spriteNames[i]);
            }

            Animation anim = new Animation(animationName, sprites, 5);

            return anim;
        }


        public GameObject GetResult()
        {
            return _gameObject;
        }
    }
}
