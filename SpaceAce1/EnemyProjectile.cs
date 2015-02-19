using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace SpaceAce1
{
    class EnemyProjectile : Projectile
    {
        public EnemyProjectile(Vector2 position, Vector2 velocity) : base(position, velocity)
        {
            image = Art.enemyProjectile;

        }
    }
}
