using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace JumpNGun
{
    class Reaper : Enemy
    {
        private int _spawnTimer = 20;
        private Thread _t1;
        private Reaper _reaper;
        

        public Reaper(Vector2 position)
        {
            this.position = position;
            health = 100;
            damage = 20;
            speed = 40;
            isColliding = false;
        }

        public override void Awake()
        {
            sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            _reaper = GameObject.GetComponent<Reaper>() as Reaper;
            enemyCollider = GameObject.GetComponent<Collider>() as Collider;
            animator = GameObject.GetComponent<Animator>() as Animator;
        }

        public override void Start()
        {
            _t1 = new Thread(FindPlayerObject);
            _t1.IsBackground = true;
            _t1.Start();
            
        }

        public override void Update(GameTime gameTime)
        {
            _t1 = new Thread(ChasePlayer);
            _t1.Start();
            Move();
            Death();
            Flipsprite();
            UpdatePositionReference();
            HandleAnimations();
            CheckCollision();
            
        }

        public override void Attack()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Chase Player 
        /// </summary>
        public override void ChasePlayer()
        {
            Vector2 sourceToTarget = Vector2.Subtract(player.Position, _reaper.position);
            sourceToTarget.Normalize();
            sourceToTarget = Vector2.Multiply(sourceToTarget, player.Speed);

            velocity = sourceToTarget;
        }

        public override void CheckCollision()
        {
            foreach (Collider col in GameWorld.Instance.Colliders)
            {
                if (col.GameObject.Tag == "Player" && enemyCollider.CollisionBox.Intersects(col.CollisionBox))
                {
                    isColliding = true;
                }
                if (col.GameObject.Tag == "Player" && !enemyCollider.CollisionBox.Intersects(col.CollisionBox))
                {
                    isColliding = false;
                }
            }
        }

        public override void FindPlayerObject()
        {
            foreach (GameObject go in GameWorld.Instance.GameObjects)
            {
                if (go.HasComponent<Player>())
                {
                    player =  go.GetComponent<Player>() as Player;
                }
            }
        }

        public override void HandleAnimations()
        {
            if (!isColliding)
            {
                animator.PlayAnimation("reaper_idle");
            }
            if (isColliding)
            {
                animator.PlayAnimation("reaper_attack");
            }
        }

    }
}
