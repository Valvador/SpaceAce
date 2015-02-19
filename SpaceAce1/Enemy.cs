using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceAce1
{
    class Enemy : Entity
    {
        private List<IEnumerator<int>> behaviours = new List<IEnumerator<int>>();
        private int timeUntilStart = 60;
        private Random rnd;
        public bool IsActive { get { return timeUntilStart <= 0; } }
        public static int cooldownFrames = 100;
        public int cooldownRemaining = 0;
        protected Vector2 GunOffset;
        protected Vector2 moveRestrict;
        private int relBulletSpeed = -15;
        private static int animatedFrames = 15;
        private int animateFor = 0;

        public Enemy(Texture2D image, Vector2 position)
        {
            this.image = image;
            Position = position;
            Rows = 1;
            Columns = 3;
            currentFrame = 0;
            animated = true;
            Radius = 15;
            color = Color.Transparent;

            GunOffset.X = 40;
            GunOffset.Y = 40;
            moveRestrict.X = EnemySpawner.xPosLimit;
            moveRestrict.Y = 0;
            rnd = new Random();
        }



        public override void Update()
        {
            if (timeUntilStart <= 0)
            {
                if (timeUntilStart <= 0)
                    ApplyBehaviours();


                if (cooldownRemaining <= 0)
                {
                    // Check primary weapon cooldown.
                    cooldownRemaining = cooldownFrames;
                    animateFor = animatedFrames;
                    // Create projectile velocity based on movement of ship.
                    Vector2 vel;
                    vel.X = Velocity.X + relBulletSpeed;
                    vel.Y = 0;

                    // Shoot Projectile from gun.
                    EntityManager.Add(new EnemyProjectile(Position + GunOffset, vel));
                    
                    // Play firing animation.
           
                }

                // Manages weapon cooldown if they fire.
                if (cooldownRemaining > 0)
                    cooldownRemaining--;

                if (animateFor > 0)
                {
                    currentFrame = rnd.Next(1, 3);
                    animateFor--;
                }

                if (animateFor == 0)
                    currentFrame = 0;
                // enemy behaviour logic goes here.
            }
            else
            {
                timeUntilStart--;
                color = Color.White * (1 - timeUntilStart / 60f);
            }

            Position += Velocity;
            Position = Vector2.Clamp(Position, moveRestrict , GameRoot.ScreenSize - Size/2); 

            Velocity *= 0.8f;


        }

        private void Animate()
        {
            animateFor = animatedFrames;
            while (animateFor > 0);

        }

        public void WasShot()
        {
            IsExpired = true;
        }

        // Creates enemy that Seeks general direction of player.
        public static Enemy CreateSeeker(Vector2 position)
        {
            var enemy = new Enemy(Art.Enemy, position);
            enemy.AddBehaviour(enemy.FollowPlayer());
            return enemy;
        }

        // Creates enemy that patrols around it's spawn location.
        public static Enemy CreatePatroller(Vector2 position)
        {
            var enemy = new Enemy(Art.Enemy, position);
            enemy.AddBehaviour(enemy.MoveInASquare());
            return enemy;
        }

        /// <summary>
        /// NEED BETTER COLLISION HANDLING, THEY SEEM TO STACK UP NEXT TO ME.
        /// </summary>
        /// <param name="other"></param>
        public void HandleCollision(Enemy other)
        {
            var d = collisionPosition - other.collisionPosition;
            Velocity += 10 * d / (d.LengthSquared() + 1);
        }


        protected void AddBehaviour(IEnumerable<int> behaviour)
        {
            behaviours.Add(behaviour.GetEnumerator());
        }

        private void ApplyBehaviours()
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (!behaviours[i].MoveNext())
                    behaviours.RemoveAt(i--);
            }
        }

        protected IEnumerable<int> FollowPlayer(float acceleration = 1f)
        {
            int setVel = 0;
            int setVelX = 0;
            const int defaultVel = 5;
            bool switchTime = true;
            bool switchTimeX = true;
            const int switchDelay = 20;
            int currentDelay = 0;
            int currentDelayX = 0;
            while (true)
            {
                
                Velocity += (PlayerShip.Instance.Position - Position);
                if ((EnemySpawner.xPosLimit - Position.X) < 100 && (EnemySpawner.xPosLimit - Position.X) > -100)
                {
                    if (rnd.Next(1, 11) > 5 && switchTime)
                    {
                        setVelX = defaultVel;
                        switchTimeX = false;
                        currentDelayX = switchDelay;
                    }
                    else if (switchTime)
                    {
                        setVelX = -defaultVel;
                        switchTimeX = false;
                        currentDelayX = switchDelay;
                    }
                }
                // Counts Down delay frames before directional change is allowed.
                if (!switchTimeX && currentDelayX > 0)
                {
                    currentDelayX--;
                    Velocity.X = setVelX;
                }

                // Allows switch direction once delay frames reach 0.
                else if (!switchTimeX && currentDelayX == 0)
                {
                    switchTimeX = true;
                    Velocity.X = setVelX;
                }


                // Normal Speed Limiting behavior for the X-Axis.
                else
                {
                    if (Velocity.X > defaultVel)
                        Velocity.X = defaultVel;
                    if (Velocity.X < -defaultVel)
                        Velocity.X = -defaultVel;
                
                }
                 
                // Makes it so many enemies do not stack and fly back and forth near players view.
                if ((PlayerShip.Instance.Position.Y - Position.Y) < 15 &&
                    (PlayerShip.Instance.Position.Y - Position.Y) > -15)
                {
                    if (rnd.Next(1, 11) > 5 && switchTime)
                    {
                        setVel = defaultVel;
                        switchTime = false;
                        currentDelay = switchDelay;
                    }
                    else if (switchTime)
                    {
                        setVel = -defaultVel;
                        switchTime = false;
                        currentDelay = switchDelay;
                    }
                }

                // Counts Down delay frames before directional change is allowed.
                if (!switchTime && currentDelay > 0)
                {
                    currentDelay--;
                    Velocity.Y = setVel;
                }

                // Allows switch direction once delay frames reach 0.
                else if (!switchTime && currentDelay == 0)
                {
                    switchTime = true;
                    Velocity.Y = setVel;
                }

                // Normal speed limiting behavior for the Y-Axis.
                else
                {
                    if (Velocity.Y > defaultVel)
                        Velocity.Y = defaultVel;
                    else if (Velocity.Y < -defaultVel)
                        Velocity.Y = -defaultVel;
                    else if (Velocity != Vector2.Zero)
                        Orientation = Velocity.ToAngle();

                }


                yield return 0;


            }
        }

        protected IEnumerable<int> MoveInASquare()
        {
            const int framesPerSide = 100;
            const int framesPerSideY = 4 * framesPerSide;
            while (true)
            {
                // move right for 30 frames
                for (int i = 0; i < framesPerSide; i++)
                {
                    Velocity = Vector2.UnitX;
                    yield return 0;
                }

                // move down
                for (int i = 0; i < framesPerSideY; i++)
                {
                    Velocity = Vector2.UnitY;
                    yield return 0;
                }

                // move left
                for (int i = 0; i < framesPerSide; i++)
                {
                    Velocity = -Vector2.UnitX;
                    yield return 0;
                }

                // move up
                for (int i = 0; i < framesPerSideY; i++)
                {
                    Velocity = -Vector2.UnitY;
                    yield return 0;
                }
            }
        }
    }
}
