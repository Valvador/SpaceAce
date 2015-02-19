using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceAce1
{
    class Projectile : Entity
    {

        Rectangle bounds;

        public Projectile(Vector2 position, Vector2 velocity)
        {
            image = Art.Projectile;
            Position = position;
            Velocity = velocity;
            Orientation = Velocity.ToAngle();
            Radius = 8;

            
        }

        public override void Update()
        {
            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();

            Position += Velocity;

            // delete Projectiles that go off-screen
            if (!GameRoot.Viewport.Bounds.Contains((int)Position.X, (int)Position.Y))
                IsExpired = true;
        }
    }
}
