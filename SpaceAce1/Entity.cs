using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceAce1
{
    abstract class Entity
    {
        protected Texture2D image;
        // The tint of the image. This will also allow us to change the transparency.
        protected Color color = Color.White;

        public Vector2 Position, Velocity;
        public Vector2 collisionPosition; //Texture Atlas position is top left corner, not center.
        public float Orientation;
        public float Radius = 20;   // used for circular collision detection
        public bool IsExpired;      // true if the entity was destroyed and should be deleted.
        public bool animated = false;       // true if Object is animated with Texture Atlas
        public int Rows = 0;       // Used if Entity is a Cloud Atlas
        public int Columns = 0;    // Used if Entity is a Cloud Atlas
        public int currentFrame;
        public int health;

        public Vector2 Size
        {
            get
            {
                // If Texture is a cloud Atlas, use individual squares for size comparison.
                if (animated)
                {
                    collisionPosition.X = Position.X + ((image.Width / Columns) / 2);
                    collisionPosition.Y = Position.Y + ((image.Height / Rows) / 2);
                    return image == null ? Vector2.Zero : new Vector2(image.Width / (Columns), image.Height / (Rows));
                }
                else
                {
                    collisionPosition = Position;
                    return image == null ? Vector2.Zero : new Vector2(image.Width, image.Height);
                }
            }
        }

        public abstract void Update();

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (animated)
            {
                // Draws based on the fact that Entity is a Texture Atlas.
                // Allows the modification of what texture to use.
                int width = image.Width / Columns;
                int height = image.Height / Rows;
                int row = (int)((float)currentFrame / (float)Columns);
                int column = currentFrame % Columns;

                Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
                Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, width, height);

                
                spriteBatch.Draw(image, destinationRectangle, sourceRectangle, Color.White);
            }

            else
                spriteBatch.Draw(image, Position, null, color, Orientation, Size / 2f, 1f, 0, 0);
        }
    }
}
