using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceAce1
{
    class PlayerStatus
    {
        public int currentFrame = 0;
        private Texture2D image;
        private Texture2D gameOver;
        private int Rows = 1;
        private int Columns = 3;
        private Vector2 Position;
        private Vector2 gameOverSize;


        public PlayerStatus()
        {
            image = Art.healthBar;
            gameOver = Art.gameOver;
            gameOverSize.X = gameOver.Width;
            gameOverSize.Y = gameOver.Height;
        }

        public void Update()
        {
            currentFrame = 3-PlayerShip.Instance.currentHealth();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Position.X = 0;
            Position.Y = 375;
            int width = image.Width / Columns;
            int height = image.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, width, height);

            
            spriteBatch.Draw(image, destinationRectangle, sourceRectangle, Color.White);
            if (PlayerShip.Instance.IsDead)
            {
                spriteBatch.Draw(gameOver, GameRoot.ScreenSize / 2 - gameOverSize / 2, null, Color.White);

            }
        }


    }
}
