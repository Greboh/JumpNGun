using System;
using Microsoft.Xna.Framework.Graphics;

namespace JumpNGun
{
    /// <summary>
    /// What character the player is
    /// </summary>
    public enum CharacterType
    {
        Soldier
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
            
            animator.AddAnimation(BuildAnimations("Idle", new string[] {"1_Soldier_idle", "2_Soldier_idle", 
                                                                                                        "3_Soldier_idle", "4_Soldier_idle", "5_Soldier_idle"}));             
            
            animator.AddAnimation(BuildAnimations("Run", new string[] {"1_Soldier_run", "2_Soldier_run", 
                "3_Soldier_run", "4_Soldier_run", "5_Soldier_run", "6_Soldier_run"}));            
            
            animator.AddAnimation(BuildAnimations("Jump", new string[] {"1_Soldier_jump", "2_Soldier_jump"}));
        }

        /// <summary>
        /// Builds the player with all the required components
        /// </summary>
        /// <param name="character">What Character the player is</param>
        private void BuildComponents(CharacterType character)
        {
            // Switch depending on what character the player is
            switch (character)
            {
                //TODO Add different character logic etc!
                case CharacterType.Soldier:
                    _gameObject.AddComponent(new Player(character));
                    break;
            }
            
            // Add all relevant components
            _gameObject.AddComponent(new SpriteRenderer());
            _gameObject.AddComponent(new Animator());
            _gameObject.AddComponent(new Input());
            _gameObject.AddComponent(new Collider());
            _gameObject.Tag = "Player";

        }

        /// <summary>
        /// Build all animations relevant to movement 
        /// </summary>
        /// <param name="animationName">Name of the animation set</param>
        /// <param name="spriteNames">Name of the sprites in the animation set </param>
        /// <returns></returns>
        private Animation BuildAnimations(string animationName, string[] spriteNames)
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