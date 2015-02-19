using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceAce1
{
    class PlayerShip : Entity
    {
        private Vector2 limClamp;
        private Vector2 frontGunOffset;
        private Vector2 backGunOffset;
        private Random rnd;
        private static PlayerShip instance;

        const int maximum_health = 3;
        int current_health = 3;
        int framesUntilRespawn = 0;
        public bool IsDead = false;

        const int primary_cooldownFrames = 15;
        int primary_cooldownRemaining = 0;
        const int primary_energyCost = 90;
        const int maximum_energy = 100;
        int energy_remaining = 100;
        const int secondary_cooldownFrames = 300;
        int secondaryCooldownRemaining = 0;
        const int relBulletSpeed = 30;




        public static PlayerShip Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlayerShip();

                return instance;
    
            }
        }

        private PlayerShip()
        {
            image = Art.PlayerTexture;
            Rows = 3;
            Columns = 4;
            Position.X = 100;
            Position.Y = (GameRoot.Viewport.Height / 2);
            Radius = 13;
            animated = true;        // Is an Animated Character
            currentFrame = 0;       // Current frame being shown on ship.

            // NON-ENTITY CONSTRUCTIONS
            rnd = new Random();
            limClamp.X = -25;       // Ship "Center" at top left of Texture. 
            limClamp.Y = -40;       // These values allow offsets to compensate.
            frontGunOffset.X = 70;
            frontGunOffset.Y = 50;
            backGunOffset.X = 85;
            backGunOffset.Y = 40;

        }

        public override void Update()
        {
            // Control scheme derived from input. Literal copy from:
            // http://gamedev.tutsplus.com/tutorials/implementation/make-a-neon-vector-shooter-in-xna-basic-gameplay/
            const float speed = 8;
            Velocity = speed * Input.GetMovementDirection();
            Position += Velocity;
            Position = Vector2.Clamp(Position, limClamp, GameRoot.ScreenSize - Size/2);

            if (Velocity.LengthSquared() > 0)
                Orientation = Extensions.ToAngle(Velocity);

            // Based on current inputs and movement, generates
            // animation texture from Texture Atlas.
            currentFrame = Random_Allowed_Frame();

            if (Input.Firing_Primary() && primary_cooldownRemaining <= 0 &&
                ((energy_remaining-primary_energyCost) > 0))
            {
                // Check primary weapon cooldown.
                primary_cooldownRemaining = primary_cooldownFrames;
                // Create projectile velocity based on movement of ship.
                Vector2 vel;
                vel.X = Velocity.X + relBulletSpeed;
                vel.Y = 0;

                // Shoot Projectile from front gun.
                EntityManager.Add(new Projectile(Position + frontGunOffset, vel));
                // Shot Projectile from back gun.
                EntityManager.Add(new Projectile(Position + backGunOffset, vel));
            }

            if (primary_cooldownRemaining > 0)
                primary_cooldownRemaining--;


            // TODO: REFINE ENERGY RECHARGE MECHANIC.
            if (energy_remaining < 100)
                energy_remaining++;

            // Death Logic
            if (current_health == 0)
            {
                IsDead = true;
                EnemySpawner.Reset();
                if (Input.Firing_Primary())
                {
                    current_health = maximum_health;
                    IsDead = false;
                }
            }
            // ship logic goes here
        }

        // Function to be called if ship takes damage.
        public void DamageShip()
        {
            if (current_health > 0)
            {
                current_health--;
                if (current_health == 0)
                    Kill();
            }
        }

        public void Kill()
        {
            Position.X = 100;
            Position.Y = (GameRoot.Viewport.Height / 2);
        }
        
        public int currentHealth()
        {
            return current_health;
        }



        // Overrides the Draw Function in Entity if it finds out the the ship is dead.
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDead)
                base.Draw(spriteBatch);
                
        }




        private int Random_Allowed_Frame()
        {
            // Neutral, not shooting or moving Object.
            if (!Input.Firing_Primary() && !Input.Firing_Secondary() &&
                !Input.Moving_Left() && !Input.Moving_Right())
                return 0;

            // Moving right, but not firing.
            if (!Input.Firing_Primary() && !Input.Firing_Secondary() &&
                !Input.Moving_Left() && Input.Moving_Right())
                return rnd.Next(1, 4);

            // Moving left, but not firing.
            if (!Input.Firing_Primary() && !Input.Firing_Secondary() &&
                Input.Moving_Left() && !Input.Moving_Right())
                return rnd.Next(8, 10);

            // Not moving, but firing.
            if ((Input.Firing_Primary() || Input.Firing_Secondary()) &&
                !Input.Moving_Left() && !Input.Moving_Right())
                if (rnd.Next(1, 11) > 5)
                    return 0;
                else
                    return 4;

            // Moving right, AND firing.
            if ((Input.Firing_Primary() || Input.Firing_Secondary()) &&
                !Input.Moving_Left() && Input.Moving_Right())
                return rnd.Next(5, 8);

            // Moving left, AND firing.
            if ((Input.Firing_Primary() || Input.Firing_Secondary()) &&
                Input.Moving_Left() && !Input.Moving_Right())
                return rnd.Next(10, 12);
            else
                return 0;
        }


        
    }
}
