using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BruteForce;
using Microsoft.Xna.Framework.Audio;

namespace BruteForce
{
    public class Character
    {


        public Texture2D chainSaw;
        private AnimatedSprite myAnimatedSprite;
        private AnimatedSprite oldAnimatedSprite;
        public SoundEffect sndPistol;
        public SoundEffect sndHit;
        int hitTimer = 0;

        //Animations / Textures
        public Texture2D defaultTexture;
        public Texture2D smokingTexture;
        public Texture2D breathingTexture;
        public Texture2D walkingTexture;

        public AnimatedSprite defaultAnim;
        public AnimatedSprite smokingAnim;
        public AnimatedSprite breathingAnim;
        public AnimatedSprite walkingAnim;

        public Texture2D[] meleeWeaponStandTextures = new Texture2D[5];
        public AnimatedSprite[] meleeWeaponStandAnim = new AnimatedSprite[5];
        //public Texture2D[] meleeWeaponWalkTextures = new Texture2D[5];
        //public AnimatedSprite[] meleeWeaponWalkAnim = new AnimatedSprite[5];
        public int meleeWeaponType;
        // 0 bat
        // 1 nightstick
        // 2 pipe
        // 3 chainsaw
        // 4 knife
        
        public Texture2D[] rangeWeaponStandTextures = new Texture2D[4];
        public AnimatedSprite[] rangeWeaponStandAnim = new AnimatedSprite[4];
        public Texture2D[] rangeWeaponWalkTextures = new Texture2D[4];
        public AnimatedSprite[] rangeWeaponWalkAnim = new AnimatedSprite[4];
        public Texture2D playerDeathTexture;
        public AnimatedSprite playerDeathAnim;
        public Texture2D playerHandcuffTexture;
        public AnimatedSprite playerHandcuffAnim;
        public Texture2D playerCrouchTexture;
        public AnimatedSprite playerCrouchAnim;
        public Texture2D playerStompTexture;
        public AnimatedSprite playerStompAnim;
        public int rangeWeaponType;
        // 0 pistol
        // 1 uzi
        // 2 shotgun
        // 3 rifle
        public int ammoLeft;
        //MouseState mouseState;

        public int previousState;
        public int playerState;
        // playerStates
        // 0 Idle
        // 1 Walking
        // 2 Stand Melee
        // 3 Walk Melee
        // 4 Stand Shoot
        // 5 Walk Shoot
        // 6 Stomping space  
        // 7 Crouch 
        // 8 HandCuffing e
        // 9 Dead

        public int direction;
        //-1 left, 1 right;
        
        // variables
        float idleTimer;
        public float speed = 7.5f;
        public bool isCrouching;
        public bool usingPipe = false;
        public bool loop = true;
        public bool meleeBlocked;
        public int meleeTimer;

        //Bullet Variables
        public Bullet[] bullet = new Bullet[100];
        public int bulletCount = 0;
        public Texture2D bulletTexture;
        public int bulletTimer = 0;
        public bool isDead;

        //"Physics" Variables
        public Vector2 position;
        Vector2 velocity;
        bool hasJumped;

        public Game1 myGame1;
      
        public Character(Texture2D newTexture, Vector2 newPosition)
        {
            defaultTexture = newTexture;
            position = newPosition;
            hasJumped = true;
            playerState = 0;
            previousState = 0;
            defaultAnim = new AnimatedSprite(defaultTexture, 1, 1, .1f);
            myAnimatedSprite = defaultAnim;
            oldAnimatedSprite = defaultAnim;
            ammoLeft = 10;
            meleeWeaponType = 0;
            rangeWeaponType = 0;
        }

        public void Update(GameTime gameTime)
        {
            //position += velocity;
            
            if(!isDead)
             inputUpdate();

            bulletTimer++;
            for (int i = 0; i < 100; i++)
            {
                if (bullet[i] != null)
                    bullet[i].Update(gameTime);
            }

            switch (playerState)
            {
                case 0:
                    idleTimer += 1;
                    myAnimatedSprite = defaultAnim;
                    // make idle timer zero after any input
                    if (idleTimer > 300)
                    {
                        myAnimatedSprite = smokingAnim;
                    }
                    else
                    {
                        myAnimatedSprite = breathingAnim;
                    }
                    break;
                case 1:
                    myAnimatedSprite = walkingAnim;
                    myAnimatedSprite.animComplete = true;
                    break;
                case 2:
                    myAnimatedSprite = meleeWeaponStandAnim[meleeWeaponType];
                    break;
                case 3:
                    myAnimatedSprite = meleeWeaponStandAnim[meleeWeaponType];
                    break;
                case 4:
                    myAnimatedSprite = rangeWeaponStandAnim[rangeWeaponType];
                    break;
                case 5:
                    myAnimatedSprite = rangeWeaponWalkAnim[rangeWeaponType];
                    break;
                case 6:
                    myAnimatedSprite = playerStompAnim;
                    break;
                case 7:
                    myAnimatedSprite = playerCrouchAnim;
                    break;
                case 8:
                    myAnimatedSprite = playerHandcuffAnim;
                    break;
                case 9:
                    myAnimatedSprite = playerDeathAnim;
                    break;
            }

            if(playerState==2)
            {
                usingPipe = true;
            }
            else
            {
                usingPipe = false;
            }

            if (playerState == 9 || playerState == 7)
            {
                myAnimatedSprite.loopable = false;
            }
            else
            {
                myAnimatedSprite.loopable = true;
            }

            if (isDead)
            {
                playerState = 9;
            }

            if (position.Y + defaultTexture.Height >= 720)
            {
                hasJumped = false;
            }

            if (hasJumped == false)
            {
                velocity.Y = 0f;
            }
            myAnimatedSprite.Update();
            
        }

                
        

        void inputUpdate()
        {

            //if (meleeBlocked)
            //{
            //    meleeTimer++;
            //    if (meleeTimer > 100)
            //    {
            //        meleeBlocked = false;
            //    }
            //}




            KeyboardState newState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();


            if (newState.Equals(new KeyboardState()))
            {
                //No keys are pressed. 
                if(playerState != 9)
                    playerState = 0;
                if (previousState != playerState)
                {
                    myAnimatedSprite.currentFrame = 0;
                    myAnimatedSprite.animComplete = false;
                    previousState = playerState;
                }
                if (hasJumped)
                {
                    float i = 1;
                    velocity.Y += 0.15f * i;
                    position.Y += velocity.Y;
                }
                oldAnimatedSprite = myAnimatedSprite;
            }

           

            if (newState.IsKeyDown(Keys.Right) || newState.IsKeyDown(Keys.D))
            {
                idleTimer = 0;
                direction = 1;
                if (newState.IsKeyDown(Keys.X))
                {
                    meleeTimer = 0;
                    meleeBlocked = true;
                    playerState = 3;
                    position.X += 30;
                    if (previousState != playerState)
                    {
                        myAnimatedSprite.currentFrame = 0;
                        myAnimatedSprite.animComplete = false;
                        previousState = playerState;
                    }
                                       
                   
                    oldAnimatedSprite = myAnimatedSprite;

                }
                else
                if ( (newState.IsKeyDown(Keys.C) ||  mouseState.RightButton == ButtonState.Pressed ) && ammoLeft > 0)
                {
                    playerState = 5;
                    if (previousState != playerState)
                    {
                        myAnimatedSprite.currentFrame = 0;
                        myAnimatedSprite.animComplete = false;
                        previousState = playerState;
                    }
                    shootBullet();
                    if (position.X < 6415) //Extend Level
                    {
                        velocity.X = speed;
                        position += velocity;

                    }
                    oldAnimatedSprite = myAnimatedSprite;

                }
                else
                {
                    playerState = 1;
                    if (previousState != playerState)
                    {
                        myAnimatedSprite.currentFrame = 0;
                        myAnimatedSprite.animComplete = false;
                        previousState = playerState;
                    }
                    if (position.X < 6800 ) //Extend Level
                    {
                        velocity.X = speed;
                        position += velocity;

                    }
                    oldAnimatedSprite = myAnimatedSprite;

                }



            }

            else if (newState.IsKeyDown(Keys.Left) || newState.IsKeyDown(Keys.A))
            {
                idleTimer = 0;
                direction = -1;
                if (newState.IsKeyDown(Keys.X) ||  mouseState.LeftButton == ButtonState.Pressed )
                {
                    meleeTimer = 0;
                    meleeBlocked = true;

                    playerState = 3;
                    if (previousState != playerState)
                    {
                        myAnimatedSprite.currentFrame = 0;
                        myAnimatedSprite.animComplete = false;
                        previousState = playerState;
                    }
                    position.X -= 30;
                    oldAnimatedSprite = myAnimatedSprite;

                }
                else
                    if (newState.IsKeyDown(Keys.C) && ammoLeft > 0)
                {
                    playerState = 5;
                    if (previousState != playerState)
                    {
                        myAnimatedSprite.currentFrame = 0;
                        myAnimatedSprite.animComplete = false;
                        previousState = playerState;
                    }
                    shootBullet();
                    if (position.X > -630 )
                    {
                        velocity.X = -speed;
                        position += velocity;
                    }
                    oldAnimatedSprite = myAnimatedSprite;

                }
                else
                {
                    playerState = 1;
                    if (previousState != playerState)
                    {
                        myAnimatedSprite.currentFrame = 0;
                        myAnimatedSprite.animComplete = false;
                        previousState = playerState;
                    }
                    if (position.X > -630 )
                    {
                        velocity.X = -speed;
                        position += velocity;
                    }
                    oldAnimatedSprite = myAnimatedSprite;

                }
            }
            else if(newState.IsKeyDown(Keys.Down) || newState.IsKeyDown(Keys.S))
            {
                playerState = 7;
                if (previousState != playerState)
                {
                    myAnimatedSprite.currentFrame = 0;
                    myAnimatedSprite.animComplete = false;
                    previousState = playerState;
                }
                oldAnimatedSprite = myAnimatedSprite;

            }
            else if (newState.IsKeyDown(Keys.E))
            {
                playerState = 8;
                if (previousState != playerState)
                {
                    myAnimatedSprite.currentFrame = 0;
                    myAnimatedSprite.animComplete = false;
                    previousState = playerState;
                }


                for (int i = 0; i < 100; i++)
                {
                    if (myGame1.enemyRectangle[i].Intersects(myGame1.charRectangle))
                    {
                        if (myGame1.AIPool[i].isFallen)
                        {
                            myGame1.AIPool[i].isCuffed = true;
                            myGame1.AIPool[i].isFallen = false;
                            myGame1.enemyRectangle[i].X = 20000;
                            myGame1.enemyRectangle[i].Y = 20000;

                            if (myGame1.AIPool[i].myAiOrientation == AI.aiOrientation.Bad)
                            {
                                myGame1.Score += 100;

                                myGame1.numbersBad = true;
                            }

                            if (myGame1.AIPool[i].myAiOrientation == AI.aiOrientation.Good)
                            {
                                myGame1.Score -= 200;


                                myGame1.numbersGood = true;
                            }

                        }
                    }
                }

                oldAnimatedSprite = myAnimatedSprite;

            }
            else if (newState.IsKeyDown(Keys.Space))
            {
                
                if (hitTimer > 60)
                {
                    sndHit.Play(); //#sound
                    hitTimer = 0;
                }
                playerState = 6;
                if (previousState != playerState)
                {
                    myAnimatedSprite.currentFrame = 0;
                    myAnimatedSprite.animComplete = false;
                    previousState = playerState;
                }



                for (int i = 0; i < 100; i++)
                {
                    if (myGame1.enemyRectangle[i].Intersects(myGame1.charRectangle))
                    {
                        if (myGame1.AIPool[i].isFallen)
                        {
                            myGame1.AIPool[i].isDead = true;
                            myGame1.AIPool[i].isFallen = false;
                            myGame1.brutality = true;
                            myGame1.enemyRectangle[i].X = 20000;
                            myGame1.enemyRectangle[i].Y = 20000;
                        }
                    }
                }

                oldAnimatedSprite = myAnimatedSprite;

            }
            else
            {
                velocity.X = 0;
                if (newState.IsKeyDown(Keys.X))
                {
                    meleeTimer = 0;
                    meleeBlocked = true;
                    playerState = 2;
                    if (previousState != playerState)
                    {
                        myAnimatedSprite.currentFrame = 0;
                        myAnimatedSprite.animComplete = false;
                        previousState = playerState;
                    }
                    if (direction == -1)
                    {
                        position.X -= 30;
                    }
                    else
                    {
                        position.X += 30;
                    }
                    idleTimer = 0;
                    oldAnimatedSprite = myAnimatedSprite;

                }
                else
                if (newState.IsKeyDown(Keys.C) && ammoLeft > 0)
                {
                    playerState = 4;
                    if (previousState != playerState)
                    {
                        myAnimatedSprite.currentFrame = 0;
                        myAnimatedSprite.animComplete = false;
                        previousState = playerState;
                    }
                    shootBullet();
                    idleTimer = 0;
                    oldAnimatedSprite = myAnimatedSprite;

                }
            }

            hitTimer++;
            /*if ((newState.IsKeyDown(Keys.Up) || newState.IsKeyDown(Keys.Space)) && hasJumped == false)
            {
                position.Y -= 10f;
                velocity.Y = -5f;
                hasJumped = true;
                idleTimer = 0;
            }*/
        }

        void shootBullet()
        {
            if (bulletCount < bullet.Length - 1)
            {
                if (bulletTimer > 20)
                {
                    sndPistol.Play(); //#sound
                    if (direction >= 0)
                    {
                        bullet[bulletCount].position.X = position.X + 136; //310
                        bullet[bulletCount].position.Y = position.Y + 38 + 50;
                        bullet[bulletCount].direction = 1;
                    }
                    else
                    {
                        bullet[bulletCount].position.X = position.X + 84;
                        bullet[bulletCount].position.Y = position.Y + 38 + 50;
                        bullet[bulletCount].direction = -1;
                    }
                    bulletCount++;
                    bulletTimer = 0;
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, position, Color.White);
            //myAnimatedSprite.direction = direction;
            for (int i = 0; i < 10; i++)
            {
                if (bullet[i] != null)
                    bullet[i].Draw(spriteBatch);
            }
            myAnimatedSprite.Draw(spriteBatch, position, direction, loop);
           
            //spriteBatch.Draw(chainSaw, position+ new Vector2(defaultTexture.Width/4-15,0), Color.White);
        }
    }
}
