using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpNGun
{
    class ReaperMinion : Enemy
    {
        private bool _spawningDone;
        private float liveTime = 10;
        private float timer;

        public ReaperMinion(Vector2 position)
        {
            this.position = position;
            health = 10;
            speed = 1f;
            damage = 10;
        }

        public override void Awake()
        {
            base.Awake();
            GameObject.Transform.Position = position;
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Update(GameTime gameTime)
        {
            Death();
            ChasePlayer();
            CheckCollision();
            HandleAnimations();
            base.Update(gameTime);
        }

        public override void Attack()
        {
            throw new NotImplementedException();
        }

        protected override void ChasePlayer()
        {
            Vector2 sourceToTarget = Vector2.Subtract(Player.Position, GameObject.Transform.Position);
            sourceToTarget.Normalize();
            sourceToTarget = Vector2.Multiply(sourceToTarget, Player.Speed);
            Velocity = sourceToTarget;
        }

        public override void CheckCollision()
        {
            foreach (Collider col in GameWorld.Instance.Colliders)
            {
                if (collider.CollisionBox.Intersects(col.CollisionBox))
                {
                    //deal damage to the player if reaperminion and player collides
                    if (col.GameObject.HasComponent<Player>())
                    {
                        //get player component and attack player
                        GameWorld.Instance.Destroy(GameObject);
                    }
                }
            }
        }

        private void Death()
        {
            timer += GameWorld.DeltaTime;
            if (liveTime < GameWorld.DeltaTime)
            {
                GameWorld.Instance.Destroy(GameObject);
            }
        }

        public override void HandleAnimations()
        {
            if(!_spawningDone)Animator.PlayAnimation("minion_spawn");

            if (Animator.IsAnimationDone && !_spawningDone)
            {
                _spawningDone = true;
                Animator.PlayAnimation("minion_idle");
            }
        }
    }
}
