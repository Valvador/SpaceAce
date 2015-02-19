using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


namespace SpaceAce1
{
    static class Art
    {
        public static Texture2D PlayerTexture { get; private set; }
        public static Texture2D Projectile { get; private set; }
        public static Texture2D Background1 { get; private set; }
        public static Texture2D Enemy { get; private set; }
        public static Texture2D Boss { get; private set; }
        public static Texture2D enemyProjectile { get; private set; }
        public static Texture2D healthBar { get; private set; }
        public static Texture2D gameOver { get; private set; }


        public static void Load(ContentManager content)
        {
            PlayerTexture = content.Load<Texture2D>("EventGrid2");
            Projectile = content.Load<Texture2D>("Projectile");
            Background1 = content.Load<Texture2D>("space2");
            Enemy = content.Load<Texture2D>("EnemyGrid");
            Boss = content.Load<Texture2D>("BossGrid");
            enemyProjectile = content.Load<Texture2D>("EnemySmallFire");
            healthBar = content.Load<Texture2D>("HealthGrid");
            gameOver = content.Load<Texture2D>("GameOver");


        }
    }
}
