using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceAce1
{

    /// <summary>
    /// Use this class for collision detection and any enemy,
    /// bullet, or player vessel.
    /// </summary>
    static class EntityManager
    {
        static List<Entity> entities = new List<Entity>();

        static bool isUpdating;
        static List<Entity> addedEntities = new List<Entity>();
        static List<Boss> bosses = new List<Boss>();
        static List<Enemy> enemies = new List<Enemy>();
        static List<Projectile> projectiles = new List<Projectile>();
        static List<EnemyProjectile> enemyprojectiles = new List<EnemyProjectile>();

        public static int Count { get { return entities.Count; } }


        // When creating new bullets or enemies, use this function.
        // 
        public static void Add(Entity entity)
        {
            if (!isUpdating)
                entities.Add(entity);

            else
                addedEntities.Add(entity);

            // The below lists are not itterated, and do not need to be updating.
            if (entity is Boss)
                bosses.Add(entity as Boss);
            else if (entity is Enemy)
                enemies.Add(entity as Enemy);
            else if (entity is Projectile && !(entity is EnemyProjectile))
                projectiles.Add(entity as Projectile);
            else if (entity is EnemyProjectile)
                enemyprojectiles.Add(entity as EnemyProjectile);
        }

        // Returns number of enemies currently on the Screen.
        public static int numberOfEnemies()
        {
            return (enemies.Count() + bosses.Count());
        }
        public static void Update()
        {
            isUpdating = true;

            // Collision and Damage Check
            HandleCollisions();

            // Consistently update every entity on the screen.
            foreach (var entity in entities)
                entity.Update();

            // Finish Updating entities.
            isUpdating = false;

            // Add entities that are waiting to be added.
            foreach (var entity in addedEntities)
                entities.Add(entity);

            addedEntities.Clear();

            // remove any expired entities.
            entities = entities.Where(x => !x.IsExpired).ToList();
            projectiles = projectiles.Where(x => !x.IsExpired).ToList();
            enemies = enemies.Where(x => !x.IsExpired).ToList();
            enemyprojectiles = enemyprojectiles.Where(x => !x.IsExpired).ToList();
        }

        private static bool IsColliding(Entity a, Entity b)
        {
            Vector2 TruePositionA, TruePositionB;

            if (a.animated)
                TruePositionA = a.collisionPosition;
            else 
                TruePositionA = a.Position;

            if (b.animated)
                TruePositionB = b.collisionPosition;
            else
                TruePositionB = b.Position;

            float radius = a.Radius + b.Radius;
            return !a.IsExpired && !b.IsExpired && Vector2.DistanceSquared(TruePositionA, TruePositionB) < radius * radius;
        }


        /// <summary>
        /// Handles drawing every object in the field.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
                entity.Draw(spriteBatch);
        }




        // This Method takes care of Enemy Enemy collisions, Enemy-Projectile Collisions
        // and Player-Enemy Projectile Collisions.
        static void HandleCollisions()
        {
            // handle collisions between enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                for (int j = i + 1; j < enemies.Count; j++)
                {
                    if (IsColliding(enemies[i], enemies[j]))
                    {
                        enemies[i].HandleCollision(enemies[j]);
                        enemies[j].HandleCollision(enemies[i]);
                    }
                }
            }

            // handle collisions between bullets and enemies
            for (int i = 0; i < enemies.Count; i++)
                for (int j = 0; j < projectiles.Count; j++)
                {
                    if (IsColliding(enemies[i], projectiles[j]))
                    {
                        enemies[i].WasShot();
                        projectiles[j].IsExpired = true;
                    }
                }

            // handle collisions between the player and enemies
            for (int i = 0; i < enemyprojectiles.Count; i++)
            {
                if (IsColliding(PlayerShip.Instance, enemyprojectiles[i]))
                {
                    PlayerShip.Instance.DamageShip();
                    enemyprojectiles[i].IsExpired = true;
                    break;
                }
            }

            // handle collisions between player shots and boss.
            for (int i = 0; i < bosses.Count; i++)
                for (int j = 0; j < projectiles.Count; j++)
                {
                    if (IsColliding(bosses[i], projectiles[j]))
                    {
                        bosses[i].DealDamage();
                        projectiles[j].IsExpired = true;
                    }
                }

        }


        // Used by the enemy manager to remove all enemies when Reset is hit!
        public static void ClearEnemies()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].IsExpired = true;
            }
        }

        public static void ClearBosses()
        {
            for (int i = 0; i < bosses.Count; i++)
                bosses[i].IsExpired = true;
        }
    }
}
