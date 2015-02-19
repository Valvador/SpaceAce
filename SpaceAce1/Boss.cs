using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceAce1
{
    class Boss : Enemy
    {

        private Vector2 GunOffset;
        private Vector2 bossRestrict;
        private int relBulletSpeed = -15;
        private const int maxHealth = 50;
        private int health;

        public Boss(Vector2 position) : base(Art.Boss, position)
        {
            Position = position;
            Rows = 1;
            Columns = 3;
            currentFrame = 0;
            animated = true;
            Radius = 70;
            color = Color.Transparent;

            cooldownFrames = 20;
            base.GunOffset.X = image.Width/(Columns*2) - 150;
            base.GunOffset.Y = image.Height/(Rows*2) + 20;
            base.moveRestrict.Y -= 200;
            base.moveRestrict.X -= 200;
            bossRestrict.X = Size.X / 1.2f;
            bossRestrict.Y = Size.Y / 1.5f;
            health = maxHealth;
        }

        public override void Update()
        {
            base.Update();
            Position = Vector2.Clamp(Position, moveRestrict, GameRoot.ScreenSize - bossRestrict); 
        }

        public static Enemy CreateBoss(Vector2 position)
        {
            var boss = new Boss(position);
            boss.AddBehaviour(boss.MoveInASquare());

            return boss;
        }


        // Executed when Projectile Collision is detected with Boss.
        public void DealDamage()
        {
            if (health > 0)
                health--;
            else if (health == 0)
            {
                IsExpired = true;
                EnemySpawner.BossAlive = false;
                health = maxHealth;
            }
        }




    }
}
