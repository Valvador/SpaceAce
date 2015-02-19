using System;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceAce1
{


    static class Input
    {
        private static KeyboardState keyboardState, lastKeyboardState;


        /// <summary>
        /// Update keyboardState so that we can keep track of which
        /// commands are being handled.
        /// </summary>
        public static void Update()
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
        }

        public static Vector2 GetMovementDirection()
        {

            Vector2 direction;
            direction = new Vector2(0, 0);
            direction.Y *= -1;  // invert the y-axis

            if (keyboardState.IsKeyDown(Keys.A))
                direction.X -= 1;
            if (keyboardState.IsKeyDown(Keys.D))
                direction.X += 1;
            if (keyboardState.IsKeyDown(Keys.W))
                direction.Y -= 1;
            if (keyboardState.IsKeyDown(Keys.S))
                direction.Y += 1;

            // Clamp the length of the vector to a maximum of 1.
            if (direction.LengthSquared() > 1)
                direction.Normalize();

            return direction;
        }

        /// <summary>
        /// The following are animation Processing Bool Functions.
        /// They are utilized in PlayShip in order to figure out which
        /// frame to display.
        /// 
        /// Firing_Primary and Firing_Secondary will also be used
        /// for projectile generation.
        /// </summary>
        /// <returns></returns>

        public static bool Firing_Primary()
        {
            // Check if player is attempting to fire.
            if (keyboardState.IsKeyDown(Keys.Space))
                return true;
            else
                return false;
        }

        public static bool Firing_Secondary()
        {
            // Check if player is attempting to fire secondary.
            if (keyboardState.IsKeyDown(Keys.LeftControl))
                return true;
            else
                return false;
        }

        public static bool Moving_Left()
        {
            // Checks if the ship is moving backwards.
            if (keyboardState.IsKeyDown(Keys.A))
                return true;
            if (!keyboardState.IsKeyDown(Keys.A) && !keyboardState.IsKeyDown(Keys.S) &&
                !keyboardState.IsKeyDown(Keys.W) && !keyboardState.IsKeyDown(Keys.D))
                return false;
            else
                return false;
        }

        public static bool Moving_Right()
        {
            if (keyboardState.IsKeyDown(Keys.D))
                return true;
            if (keyboardState.IsKeyDown(Keys.D) && keyboardState.IsKeyDown(Keys.W))
                return true;
            if (keyboardState.IsKeyDown(Keys.D) && keyboardState.IsKeyDown(Keys.S))
                return true;
            if (keyboardState.IsKeyDown(Keys.S) && !keyboardState.IsKeyDown(Keys.A))
                return true;
            if (keyboardState.IsKeyDown(Keys.W) && !keyboardState.IsKeyDown(Keys.A))
                return true;
            if (!keyboardState.IsKeyDown(Keys.A) && !keyboardState.IsKeyDown(Keys.S) &&
                !keyboardState.IsKeyDown(Keys.W) && !keyboardState.IsKeyDown(Keys.D))
                return false;
            else
                return false;
        }
    }
}
