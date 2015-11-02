using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BruteForce
{
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int currentFrame;
        private int totalFrames;
        private float animSpeed;
        float spriteProgress;
        public int direction;
        public bool animComplete;
        public bool loopable;

        public bool isPlayerAttackAnim;
        public int playerWeaponType;

        public AnimatedSprite(Texture2D texture, int rows, int columns, float speed)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            animSpeed = speed;
            spriteProgress = 0;
            loopable = true;
        }

        public void Update()
        {

            if (loopable)
            {
              
                spriteProgress += animSpeed;

                if (spriteProgress > 1)
                {
                    spriteProgress = 0;
                    currentFrame++;

                    if (currentFrame == totalFrames)
                    {
                        currentFrame = 0;
                        animComplete = true;
                                               
                    }

                }
            }

            if (!loopable)
            {
                if (currentFrame < totalFrames - 1)
                {
                    spriteProgress += animSpeed;

                    if (spriteProgress > 1)
                    {
                        spriteProgress = 0;
                        currentFrame++;
                    }
                }
            }
            
        }



        public void Draw(SpriteBatch spriteBatch, Vector2 location, int dir, bool loop)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;
            direction = dir;
            if (animComplete && !loop)
            {
                if(direction == 1)
                    direction = 2;
                else if (direction == -1)
                    direction = -2;
            }

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            switch (direction)
            {
                case 0:
                    spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1);
                    break;
                case -1:
                    spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1);
                    break;
                case 1:
                    spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1);
                    break;
                case 2:
                    //Temporary case to stop death animation at frame 3 #temp
                    spriteBatch.Draw(Texture, destinationRectangle, new Rectangle(width * 2, 0, width, height), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1);
                    break;
                case -2:
                    spriteBatch.Draw(Texture, destinationRectangle, new Rectangle(width * 2, 0, width, height), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1);
                    break;
            }

            // spriteBatch.Begin();
            // spriteBatch.End();
        }
    }
}