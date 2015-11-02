using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BruteForce
{
    public class AI
    {
        public AnimatedSprite currentAnimation;

        //public AnimatedSprite stMelee, stRange, stIdle, wtMelee, wtRange, wtIdle, scMelee, scRange, scIdle, wcMelee, wcRange, wcIdle;

        public enum aiType { standingThug, walkingThug, standingCivilian, walkingCivilian };
        // 10 / 25 / 20 / 45
        public enum aiAttackType { Melee, Range, Idle };
        // 50 / 50
        public enum aiOrientation { Good, Bad };
        // 40 / 60
        public enum AiWeapon { none, knife, bat, pipe, pistol, uzi };

        public aiType myAiType;
        public aiAttackType myAiAttackType;
        public aiOrientation myAiOrientation;
        public AiWeapon myAiWeapon;
        public int costumeType;
        public int meleeRadius;
        public int rangeRadius = 575;
        public float mySpeed;
        public bool isInGame = false;
        public Vector2 myPos;
        public int direction;
        private float animSpeed = 0.2f;
        public int enemyState = 0;
        // enemyStates
        // 0 Idle or Walking
        // 1 Attack
        // 2 Dead

        public bool isDead;
        public bool isFallen;
        public bool isCuffed;
        public bool loop = true;
        public Character myPlayer;
        public int ey = 0;
        public bool changeCollider = false;
        public int colliderXPos = 72;
        public Camera myCamera;

        public float idleTimer;

        //Bullets
        public Bullet[] bullet = new Bullet[10];
        public int bulletCount = 0;
        public Texture2D bulletTexture;
        public int bulletTimer = 0;

        public AiAnimation myAiAnims;// = new AiAnimation();
        public AnimatedSprite[] myAnimations = new AnimatedSprite[5];
        public Game1 myGame1;
        
        public AI(aiType aiType, aiAttackType aiAttackType, aiOrientation aiOrientation, AiWeapon aiWeapon,  int costumeNum, AiAnimation aiAnim)
        {
           
            myAiType = aiType;
            myAiAttackType = aiAttackType;
            myAiOrientation = aiOrientation;
            costumeType = costumeNum;
            myAiAnims = aiAnim;
            myAiWeapon = aiWeapon;
            mySpeed = 5;

            //for (int i = 0; i < 10; i++)
            //{
            //    bullet[i] = new Bullet(bulletTexture, new Vector2(2000, 2000), 1);
            //    bullet[i].myCamera = myCamera;
            //    bullet[i].isEnemyBullet = true;
            //}

                //randomize speed
                //isDead = false;
                // change frame numbers

                //myAnimations[0] = new AnimatedSprite(myAiAnims.thugIdleSprites[costumeType], 1, 4, animSpeed);
                //myAnimations[1] = new AnimatedSprite(myAiAnims.civKnifeSprites[costumeType], 1, 3, animSpeed);
                //myAnimations[2] = new AnimatedSprite(myAiAnims.thugDieSprites[costumeType], 1, 3, animSpeed);

                switch (myAiType)
            {
                case aiType.standingThug:
                    if (myAiOrientation == aiOrientation.Bad)
                    {
                        if (myAiAttackType == aiAttackType.Melee)
                        {
                            myAnimations[0] = new AnimatedSprite(myAiAnims.thugIdleSprites[costumeType], 1, 4, animSpeed);
                            myAnimations[2] = new AnimatedSprite(myAiAnims.thugFloorSprites[costumeType], 1, 3, animSpeed);
                        }
                        else if (myAiAttackType == aiAttackType.Range)
                        {
                            myAnimations[0] = new AnimatedSprite(myAiAnims.thugIdleSprites[costumeType], 1, 4, animSpeed);
                            myAnimations[2] = new AnimatedSprite(myAiAnims.thugFloorSprites[costumeType], 1, 3, animSpeed);
                        }
                    }
                    else if (myAiOrientation == aiOrientation.Good)
                    {
                      //  if (myAiAttackType == aiAttackType.Idle)
                            myAnimations[0] = new AnimatedSprite(myAiAnims.thugIdleSprites[costumeType], 1, 4, animSpeed);
                            myAnimations[2] = new AnimatedSprite(myAiAnims.thugFloorSprites[costumeType], 1, 3, animSpeed);
                    }
                    break;
                case aiType.standingCivilian:
                    if (myAiOrientation == aiOrientation.Bad)
                    {
                        if (myAiAttackType == aiAttackType.Melee)
                        {
                            myAnimations[0] = new AnimatedSprite(myAiAnims.civIdleSprites[costumeType], 1, 4, animSpeed);
                            myAnimations[2] = new AnimatedSprite(myAiAnims.civFloorSprites[costumeType], 1, 3, animSpeed);
                        }
                        else if (myAiAttackType == aiAttackType.Range)
                        {
                            myAnimations[0] = new AnimatedSprite(myAiAnims.civIdleSprites[costumeType], 1, 4, animSpeed);
                            myAnimations[2] = new AnimatedSprite(myAiAnims.civFloorSprites[costumeType], 1, 3, animSpeed);
                        }
                    }
                    else if (myAiOrientation == aiOrientation.Good)
                    {
                    //    if (myAiAttackType == aiAttackType.Idle)
                    //    {
                            myAnimations[0] = new AnimatedSprite(myAiAnims.civIdleSprites[costumeType], 1, 4, animSpeed);
                            myAnimations[2] = new AnimatedSprite(myAiAnims.civFloorSprites[costumeType], 1, 3, animSpeed);
                        //     }
                    }
                    break;
                case aiType.walkingThug:
                    if (myAiOrientation == aiOrientation.Bad)
                    {
                        if (myAiAttackType == aiAttackType.Melee)
                        {
                            myAnimations[0] = new AnimatedSprite(myAiAnims.thugWalkSprites[costumeType], 1, 8, animSpeed);
                            myAnimations[2] = new AnimatedSprite(myAiAnims.thugFloorSprites[costumeType], 1, 3, animSpeed);
                        }
                        else if (myAiAttackType == aiAttackType.Range)
                        {
                            myAnimations[0] = new AnimatedSprite(myAiAnims.thugWalkSprites[costumeType], 1, 8, animSpeed);
                            myAnimations[2] = new AnimatedSprite(myAiAnims.thugFloorSprites[costumeType], 1, 3, animSpeed);
                        }
                    }
                    else if (myAiOrientation == aiOrientation.Good)
                    {
                     //   if (myAiAttackType == aiAttackType.Idle)
                    //    {
                            myAnimations[0] = new AnimatedSprite(myAiAnims.thugWalkSprites[costumeType], 1, 8, animSpeed);
                            myAnimations[2] = new AnimatedSprite(myAiAnims.thugFloorSprites[costumeType], 1, 3, animSpeed);
                        //   }
                    }
                    break;

                case aiType.walkingCivilian:
                    if (myAiOrientation == aiOrientation.Bad)
                    {
                        if (myAiAttackType == aiAttackType.Melee)
                        {
                            myAnimations[0] = new AnimatedSprite(myAiAnims.civWalkSprites[costumeType], 1, 8, animSpeed);
                            myAnimations[2] = new AnimatedSprite(myAiAnims.civFloorSprites[costumeType], 1, 3, animSpeed);
                        }
                        else if (myAiAttackType == aiAttackType.Range)
                        {
                            myAnimations[0] = new AnimatedSprite(myAiAnims.civWalkSprites[costumeType], 1, 8, animSpeed);
                            myAnimations[2] = new AnimatedSprite(myAiAnims.civFloorSprites[costumeType], 1, 3, animSpeed);
                        }
                    }
                    else if (myAiOrientation == aiOrientation.Good)
                    {
                     //   if (myAiAttackType == aiAttackType.Idle)
                            myAnimations[0] = new AnimatedSprite(myAiAnims.civWalkSprites[costumeType], 1, 8, animSpeed);
                            myAnimations[2] = new AnimatedSprite(myAiAnims.civFloorSprites[costumeType], 1, 3, animSpeed);
                    }
                    break;
            }

            assignWeapon();

            currentAnimation = myAnimations[0];
        }

        void assignWeapon()
        {
            if (myAiAttackType == aiAttackType.Melee)
            {
                switch (myAiType)
                {
                    case aiType.walkingCivilian:
                    case aiType.standingCivilian:

                        myAnimations[3] = new AnimatedSprite(myAiAnims.civDieSprites[costumeType], 1, 1, animSpeed);
                        myAnimations[4] = new AnimatedSprite(myAiAnims.civArrestSprites[costumeType], 1, 1, animSpeed);
                        myAnimations[3].loopable = false;
                        myAnimations[4].loopable = false;


                        switch (myAiWeapon)
                        {
                            case AiWeapon.knife:
                                myAnimations[1] = new AnimatedSprite(myAiAnims.civKnifeSprites[costumeType], 1, 3, animSpeed);
                                break;
                            case AiWeapon.bat:
                                myAnimations[1] = new AnimatedSprite(myAiAnims.civBatSprites[costumeType], 1, 4, animSpeed);
                                break;
                            case AiWeapon.pipe:
                                myAnimations[1] = new AnimatedSprite(myAiAnims.civPipeSprites[costumeType], 1, 4, animSpeed);
                                break;
                        }
                        break;
                    case aiType.walkingThug:
                    case aiType.standingThug:

                        myAnimations[3] = new AnimatedSprite(myAiAnims.thugDieSprites[costumeType], 1, 1, animSpeed);
                        myAnimations[4] = new AnimatedSprite(myAiAnims.thugArrestSprites[costumeType], 1, 1, animSpeed);
                        myAnimations[3].loopable = false;
                        myAnimations[4].loopable = false;

                        switch (myAiWeapon)
                        {
                            case AiWeapon.knife:
                                myAnimations[1] = new AnimatedSprite(myAiAnims.thugKnifeSprites[costumeType], 1, 3, animSpeed);
                                break;
                            case AiWeapon.bat:
                                myAnimations[1] = new AnimatedSprite(myAiAnims.thugBatSprites[costumeType], 1, 4, animSpeed);
                                break;
                            case AiWeapon.pipe:
                                myAnimations[1] = new AnimatedSprite(myAiAnims.thugPipeSprites[costumeType], 1, 4, animSpeed);
                                break;
                        }
                        break;
                }
            }
            else if (myAiAttackType == aiAttackType.Range)
            {
               
                switch (myAiType)
                {
                    case aiType.standingCivilian:
                        myAnimations[3] = new AnimatedSprite(myAiAnims.civDieSprites[costumeType], 1, 1, animSpeed);
                        myAnimations[4] = new AnimatedSprite(myAiAnims.civArrestSprites[costumeType], 1, 1, animSpeed);
                        myAnimations[3].loopable = false;
                        myAnimations[4].loopable = false;
                        switch (myAiWeapon)
                        {
                            case AiWeapon.pistol:
                                myAnimations[1] = new AnimatedSprite(myAiAnims.civStandPistolSprites[costumeType], 1, 2, animSpeed);
                                break;
                            case AiWeapon.uzi:
                                myAnimations[1] = new AnimatedSprite(myAiAnims.civStandUziSprites[costumeType], 1, 2, animSpeed);
                                break;

                        }
                        break;
                    case aiType.standingThug:
                        myAnimations[3] = new AnimatedSprite(myAiAnims.thugDieSprites[costumeType], 1, 1, animSpeed);
                        myAnimations[4] = new AnimatedSprite(myAiAnims.thugArrestSprites[costumeType], 1, 1, animSpeed);
                        myAnimations[3].loopable = false;
                        myAnimations[4].loopable = false;
                        switch (myAiWeapon)
                        {
                            case AiWeapon.pistol:
                                myAnimations[1] = new AnimatedSprite(myAiAnims.thugStandPistolSprites[costumeType], 1, 2, animSpeed);
                                break;
                            case AiWeapon.uzi:
                                myAnimations[1] = new AnimatedSprite(myAiAnims.thugStandUziSprites[costumeType], 1, 2, animSpeed);
                                break;

                        }
                        break;
                    case aiType.walkingCivilian:
                        myAnimations[3] = new AnimatedSprite(myAiAnims.civDieSprites[costumeType], 1, 1, animSpeed);
                        myAnimations[4] = new AnimatedSprite(myAiAnims.civArrestSprites[costumeType], 1, 1, animSpeed);
                        myAnimations[3].loopable = false;
                        myAnimations[4].loopable = false;
                        switch (myAiWeapon)
                        {
                            case AiWeapon.pistol:
                                myAnimations[1] = new AnimatedSprite(myAiAnims.civWalkPistolSprites[costumeType], 1, 8, animSpeed);
                                break;
                            case AiWeapon.uzi:
                                myAnimations[1] = new AnimatedSprite(myAiAnims.civWalkUziSprites[costumeType], 1, 8, animSpeed);
                                break;

                        }
                        break;
                    case aiType.walkingThug:
                        myAnimations[3] = new AnimatedSprite(myAiAnims.thugDieSprites[costumeType], 1, 1, animSpeed);
                        myAnimations[4] = new AnimatedSprite(myAiAnims.thugArrestSprites[costumeType], 1, 1, animSpeed);
                        myAnimations[3].loopable = false;
                        myAnimations[4].loopable = false;
                        switch (myAiWeapon)
                        {
                            case AiWeapon.pistol:
                                myAnimations[1] = new AnimatedSprite(myAiAnims.thugWalkPistolSprites[costumeType], 1, 8, animSpeed);
                                break;
                            case AiWeapon.uzi:
                                myAnimations[1] = new AnimatedSprite(myAiAnims.thugWalkUziSprites[costumeType], 1, 8, animSpeed);
                                break;

                        }
                        break;
                }
            }

        }

        //void shootNow()
        //{
        //    if (bulletCount < bullet.Length - 1)
        //    {
        //        if (bulletTimer > 50)
        //        {
        //            //sndPistol.Play(); //#sound
        //            if (direction == -1)
        //            {
        //                bullet[bulletCount].position.X = myPos.X + 136; //310
        //                bullet[bulletCount].position.Y = myPos.Y + 38 + 50;
        //                bullet[bulletCount].direction = 1;
        //            }
        //            else
        //            {
        //                bullet[bulletCount].position.X = myPos.X + 84;
        //                bullet[bulletCount].position.Y = myPos.Y + 38 + 50;
        //                bullet[bulletCount].direction = -1;
        //            }
        //            bulletCount++;
        //            bulletTimer = 0;
        //        }

        //    }
        //}


        public void updateAI(GameTime gameTime)
        {
            
            if (isInGame)
            {

                //if (enemyState != 3)
                //{
                    if (myAiType == aiType.standingCivilian || myAiType == aiType.standingThug)
                    {
                        if (myAiOrientation == aiOrientation.Bad)
                        {
                            if (myAiAttackType == aiAttackType.Melee)
                            {
                                if (Math.Abs(myPlayer.position.X - myPos.X) > meleeRadius)
                                {
                                    enemyState = 0;
                                }
                                else
                                {
                                    enemyState = 1;
                                }
                            }
                            else if (myAiAttackType == aiAttackType.Range)
                            {
                                if (Math.Abs(myPlayer.position.X - myPos.X) > rangeRadius)
                                {
                                    enemyState = 0;
                                }
                                else
                                {
                                    enemyState = 1;
                                }
                            }
                        }
                        else if (myAiOrientation == aiOrientation.Good)
                        {
                            //if (myAiAttackType == aiAttackType.Idle)
                            //{
                            enemyState = 0;
                            //}
                        }
                    }
                    else if (myAiType == aiType.walkingCivilian || myAiType == aiType.walkingThug)
                    {

                        if (myAiOrientation == aiOrientation.Bad)
                        {
                            if (myAiAttackType == aiAttackType.Melee)
                            {
                                if (Math.Abs(myPlayer.position.X - myPos.X) > meleeRadius)
                                {
                                    enemyState = 0;
                                }
                                else
                                {
                                    enemyState = 1;
                                }
                            }
                            else if (myAiAttackType == aiAttackType.Range)
                            {
                                if (Math.Abs(myPlayer.position.X - myPos.X) > rangeRadius)
                                {
                                    enemyState = 0;
                                }
                                else
                                {
                                    enemyState = 1;
                                }
                            }
                        }
                        else if (myAiOrientation == aiOrientation.Good)
                        {
                            //if (myAiAttackType == aiAttackType.Idle)
                            //{
                            enemyState = 0;
                            //}
                        }
                    }
                //}


                if (isDead)
                {
                    enemyState = 3;               
                }

                if(isFallen)
                {
                    enemyState = 2;
                }

                if (isCuffed)
                {
                    enemyState = 4;
                }

                //State Based Movement
                switch (enemyState)
                {
                    case 0:
                        if (myAiType == aiType.walkingCivilian || myAiType == aiType.walkingThug)
                        {
                            currentAnimation = myAnimations[0];
                            if (direction == -1)//goRight
                            {
                                myPos.X += mySpeed;
                            }
                            else if (direction == 1)//goLeft
                            {
                                myPos.X -= mySpeed;
                            }
                        }
                        else if(myAiType == aiType.standingCivilian || myAiType == aiType.standingThug)
                        {
                            currentAnimation = myAnimations[0];
                            if (myPlayer.position.X < myPos.X - 100)//error in postions
                            {
                                direction = 1;
                            }
                            else if (myPlayer.position.X >= myPos.X - 100)
                            {
                                direction = -1;
                            }
                        }
                        break;
                    case 1:
                        if (myAiType == aiType.walkingCivilian || myAiType == aiType.walkingThug)
                        {
                            if(myAiOrientation == aiOrientation.Bad)                            
                                currentAnimation = myAnimations[1];
                            if (myAiAttackType == aiAttackType.Melee)
                            {

                            }
                            else if (myAiAttackType == aiAttackType.Range)
                            {

                                //shootNow();

                                if (direction == -1)//goRight
                                {
                                    myPos.X += mySpeed;
                                }
                                else if (direction == 1)//goLeft
                                {
                                    myPos.X -= mySpeed;
                                }
                            }
                           
                           
                        }
                        else if (myAiType == aiType.standingCivilian || myAiType == aiType.standingThug)
                        {
                            if (myAiOrientation == aiOrientation.Bad)
                            {
                                currentAnimation = myAnimations[1];

                                //Shoot Bullets
                                /*    if (bulletCount < bullet.Length - 1)
                                    {
                                        if (bulletTimer > 20)
                                        {
                                        new Bullet(bulletTexture, new Vector2(2000, 2000), 1);
                                        //sndPistol.Play(); //#sound
                                        if (direction >= 0)
                                            {
                                                bullet[bulletCount].position.X = myPos.X + 136; //310
                                                bullet[bulletCount].position.Y = myPos.Y + 38;
                                                bullet[bulletCount].direction = 1;
                                            }
                                            else
                                            {
                                                bullet[bulletCount].position.X = myPos.X + 84;
                                                bullet[bulletCount].position.Y = myPos.Y + 38;
                                                bullet[bulletCount].direction = -1;
                                            }
                                            bulletCount++;
                                            bulletTimer = 0;
                                        }

                                    }
                                 */
                            }
                                
                            if (myPlayer.position.X < myPos.X - 100)//error in postions
                            {
                                direction = 1;
                            }
                            else if (myPlayer.position.X >= myPos.X - 100)
                            {
                                direction = -1;
                            }
                        }
                       
                        break;
                    case 2:
                        currentAnimation = myAnimations[2];
                        loop = false;
                        break;
						
					case 3:
                        currentAnimation = myAnimations[3];
                        //loop = false;
                        break;
                    case 4:
                        currentAnimation = myAnimations[4];
                        loop = false;
                        break;
                }

                if (currentAnimation == myAnimations[1])
                {
                    changeCollider = true;
                    if (direction == -1)
                        colliderXPos = 72;
                    else
                        colliderXPos = 28;
                }
                else
                {
                    changeCollider = false;
                    colliderXPos = 72;
                }

                //Debug.WriteLine(myCamera.center.X);
                //if (currentAnimation!=null && myPos.X > myCamera.center.X && myPos.X < myCamera.center.X + 1280)
                    currentAnimation.Update();

                //bulletTimer++;
                //for (int i = 0; i < 100; i++)
                //{
                //    if (bullet[i] != null)
                //        bullet[i].Update(gameTime);
                //}
                
            }


        }

        public void assignCounter()
        {
            switch (myAiOrientation)
            {
                case aiOrientation.Good:
                    myGame1.civiliansKilled++;
                    break;
                case aiOrientation.Bad:
                    myGame1.thugsKilled++;
                    break;
            }

            switch (costumeType)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, position, Color.White);
            //currentAnimation.direction = direction;

            //currentAnimation.Draw(spriteBatch, myPos + new Vector2(0, ey), direction, loop);
            //if (currentAnimation != null && myPos.X > myCamera.center.X - 250 && myPos.X < myCamera.center.X + 1280 + 250) //#error
            //{
            //for (int i = 0; i < 100; i++)
            //{
            //    if (bullet[i] != null)
            //        bullet[i].Draw(spriteBatch);
            //}
            //Debug.WriteLine(currentAnimation.Texture);
            currentAnimation.Draw(spriteBatch, myPos, direction, loop);
            //}
            //currentAnimation.Draw(spriteBatch, myPos+new Vector2(0,0/*-23ey*/), direction, loop);
            //spriteBatch.Draw(chainSaw, position+ new Vector2(defaultTexture.Width/4-15,0), Color.White);
        }

    }
}
