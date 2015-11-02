using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BruteForce;
using System.Diagnostics;

namespace BruteForce
{
    public class Bullet
    {
        //Pistol Bullet
        public Texture2D texture;
        public Vector2 position;
        public int direction;
        public Game1 game = new Game1();
        public int bulletSpeed = 25;
        public Camera myCamera;
        public bool isEnemyBullet;
        //Rectangle bulletRectangle;
        Rectangle[] enemyRectangle = new Rectangle[100];

        public Bullet(Texture2D newTexture, Vector2 newPosition, int newDirection)
        {
            texture = newTexture;
            position = newPosition;
            direction = newDirection;
        }

        public void Update(GameTime gameTime)
        {
            if(position.X>myCamera.center.X+1280)
            {
                //#temp
                position = new Vector2(2000, 2000);
            }
            if(direction>=0)
                position.X+=bulletSpeed;
            else
                position.X -= bulletSpeed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (direction)
            {
                case 0:
                    spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y,texture.Width,texture.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1);
                    break;
                case -1:
                    spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1);
                    break;
                case 1:
                    spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1);
                    break;
            }
        }
    }
}
