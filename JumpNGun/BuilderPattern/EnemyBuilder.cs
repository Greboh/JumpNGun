using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    /// <summary>
    /// What character the enemy is
    /// </summary>
    public enum EnemyType
    {
        Slug,
    }

    public class EnemyBuilder : IBuilder
    {

        private EnemyType _enemy; // Reference to the EnemyType
        private GameObject _gameObject; // Player GameObject

        /// <summary>
        /// Constructor to set the CharacterType
        /// </summary>
        /// <param name="character">What Character the player is</param>
        public EnemyBuilder(EnemyType enemy)
        {
            _enemy = enemy;
        }


        public void BuildGameObject()
        {
            _gameObject = new GameObject();

            BuildComponents(_enemy);

            Animator animator = (Animator)_gameObject.GetComponent<Animator>();
        }

        /// <summary>
        /// Builds the enemy with all the required components
        /// </summary>
        /// <param name="enemy">What type of enemy it ts</param>
        private void BuildComponents(EnemyType enemy)
        {
            // Switch depending on what type of enemy it is
            switch (enemy)
            {
                //TODO Add different slug logic etc!
                case EnemyType.Slug:
                    _gameObject.AddComponent(new SlugEnemy(100));
                    break;
            }

            // Add all relevant components
            _gameObject.AddComponent(new SpriteRenderer());
            _gameObject.AddComponent(new Animator());
            _gameObject.AddComponent(new Collider());
            _gameObject.Tag = "Slug";

        }

        /// <summary>
        /// Build all animations relevant to movement 
        /// </summary>
        /// <param name="animationName">Name of the animation set</param>
        /// <param name="spriteNames">Name of the sprites in the animation set </param>
        /// <returns></returns>
        private Animation BuildMoveAnimations(string animationName, string[] spriteNames)
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
