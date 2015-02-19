using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceAce1
{
    static class EnemySpawner
    {
        public const int xPosLimit = 350;
        static Random rand = new Random();
        static int maxEnemies = 8;
        static float inverseSpawnChance = 100;
        static int enemiesSpawned = 0;
        const int enemiesTillBoss = 20;
        public static bool BossAlive = false;


        public static void Update()
        {
            if (!PlayerShip.Instance.IsDead && EntityManager.numberOfEnemies() < maxEnemies && !BossAlive)
            {
                if (rand.Next((int)inverseSpawnChance) == 0)
                {
                    if (enemiesSpawned == enemiesTillBoss)
                    {
                        EntityManager.Add(Boss.CreateBoss(GetBossSpawnPosition()));
                        BossAlive = true;
                        enemiesSpawned = 0;
                    }

                    if (rand.Next(1,11) > 7)
                        EntityManager.Add(Enemy.CreateSeeker(GetSpawnPosition()));
                    else
                        EntityManager.Add(Enemy.CreatePatroller(GetSpawnPosition()));

                    enemiesSpawned++;
                }
            }

            // slowly increase the spawn rate as time progresses
            if (inverseSpawnChance > 20)
                inverseSpawnChance -= 0.005f;
        }

        private static Vector2 GetSpawnPosition()
        {
            Vector2 pos;
            do
            {
                pos = new Vector2(rand.Next(xPosLimit, (int)GameRoot.ScreenSize.X), rand.Next((int)GameRoot.ScreenSize.Y));
            }
            while (Vector2.DistanceSquared(pos, PlayerShip.Instance.Position) < 250 * 250);

            return pos;
        }

        private static Vector2 GetBossSpawnPosition()
        {
            Vector2 pos;

            pos = new Vector2(350, 0);


            return pos;
            
        }

        public static void Reset()
        {
            inverseSpawnChance = 60;
            EntityManager.ClearEnemies();
            EntityManager.ClearBosses();
            enemiesSpawned = 0;
            BossAlive = false;
        }
    }
}
