using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Collections;
using BruteForce;
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace BruteForce
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Character player;
       //private SoundEffect effect;
        Texture2D Bg;
        Vector2 BgPosition;
        //Texture2D Bg1;
        //Vector2 Bg1Position;
        Texture2D Bg2;
        Vector2 Bg2Position;
        Texture2D bulletTexture;
        Texture2D rect;
        Texture2D rectEnem;
        Texture2D brut;
        Vector2 brutpos;
        public bool brutality = false;
        int brutalitytimer = 0;

        public bool numbersGood, numbersBad;
        int numberstimerGood = 0;
        int numberstimerBad = 0;

        //bool blockcamera;
        Camera myCamera;
        //int ey = 200; //Variable to change y position of enemy due to different sprite heights. Argh!
        public Rectangle charRectangle = new Rectangle(0,0,128,200);
        public Rectangle[] enemyRectangle = new Rectangle[100];
        Rectangle[] bulletRectangle = new Rectangle[100];
        public AI[] AIPool = new AI[100];
        AiAnimation AiAnims;
        public int AiInGame;
        Random random = new Random();
        Vector2 cameraPosition = Vector2.Zero;

        Texture2D[] inventory = new Texture2D[10]; 

        //LoadLevel() Variables
        Texture2D trashBag;
        Vector2 trashBagPosition;
        Rectangle trashBagRec;


        SpriteFont scoreTexture;
        Vector2 scoreFontPos;
        public int Score = 0;

        Texture2D Civilians;
        Vector2 CivilianPos;
        SpriteFont civil;
        Vector2 civilfontpos;
        int civilcount = 0;

        Texture2D Bad;
        Vector2 BadPos;
        SpriteFont badFont;
        Vector2 badFontpos;
        int badCount = 0;

        Texture2D Hundred;
        Vector2 HundredPos;
        Texture2D Twohundred;
        Vector2 TwohundredPos;

        // counters
        public int whitesKilled;
        public int blacksKilled;
        public int brownsKilled;
        public int civiliansKilled;
        public int thugsKilled;
        public float aiSpawnCounter;
        public double aiSpawnTime;
        public bool allowSpawn;

        Random randomTime = new Random();
        Random randPos = new Random();
        Random randSide = new Random();

        float animSpeed = 0.25f; //Of Player Shooting

        bool Gameover = false;

        Texture2D Thug1;
        Vector2 Thug1pos;
        Texture2D Thug2;
        Vector2 Thug2pos;
        Texture2D Thug3;
        Vector2 Thug3pos;

        Texture2D civ1;
        Vector2 civ1pos;
        Texture2D civ2;
        Vector2 civ2pos;
        Texture2D civ3;
        Vector2 civ3pos;

        bool ShowGameOver;
        //bool randomimage;

        Song BackgroundSong;
        Song FinalSong;
        //MouseState mouseState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
            myCamera = new Camera(GraphicsDevice.Viewport);
            AiInGame = 0;
            for (int i = 0; i < AIPool.Length; i++)
            {
                enemyRectangle[i] = new Rectangle(2000, 2000, 50, 250);
            }
            this.IsMouseVisible = true;

            base.Initialize();
        }

        AI.aiAttackType tempAttackType;
        AI.aiType tempAiType;
        AI.aiOrientation tempAiOrientation;
        AI.AiWeapon tempWeapon;
        AI tempAI;

        void startGame()
        {
            for (int i = 0; i < AIPool.Length; i++)
            {
                Array values = Enum.GetValues(typeof(AI.aiType));
                tempAiType = (AI.aiType)values.GetValue(random.Next(values.Length));

                //Random num = new Random();
                int nextOri = random.Next(0, 100);

                // 40 / 60
                if (nextOri > 40)
                {
                    tempAiOrientation = AI.aiOrientation.Bad;


                    // 50 / 50
                    // Random num2 = new Random();
                    int next2Value = random.Next(0, 100);

                    if (next2Value > 50)
                    {
                        tempAttackType = AI.aiAttackType.Melee;
                        int nextWeapon = random.Next(0, 3);
                        switch (nextWeapon)
                        {
                            case 0:
                                tempWeapon = AI.AiWeapon.knife;
                                break;
                            case 1:
                                tempWeapon = AI.AiWeapon.bat;
                                break;
                            case 2:
                                tempWeapon = AI.AiWeapon.pipe;
                                break;
                        }
                    }
                    else
                    {
                        tempAttackType = AI.aiAttackType.Range;
                        int nextWeapon = random.Next(0, 2);
                        switch (nextWeapon)
                        {
                            case 0:
                                tempWeapon = AI.AiWeapon.pistol;
                                break;
                            case 1:
                                tempWeapon = AI.AiWeapon.uzi;
                                break;
                        }
                    }
                }
                else
                {
                    tempAiOrientation = AI.aiOrientation.Good;
                    tempWeapon = AI.AiWeapon.none;
                }

                int nextCostume = random.Next(0, 3);
                Debug.WriteLine("type " + tempAiType + " attack " + tempAttackType + " orient " + tempAiOrientation + " weapon " + tempWeapon + " costume " + nextCostume);
                AIPool[i] = new AI(tempAiType, tempAttackType, tempAiOrientation, tempWeapon, nextCostume, AiAnims);
                //AIPool[i] = new AI(AI.aiType.walkingCivilian, AI.aiAttackType.Range, AI.aiOrientation.Bad, AI.AiWeapon.uzi, 0, AiAnims);

                AIPool[i].myPlayer = player;
                AIPool[i].meleeRadius = random.Next(150, 200); //215

                AIPool[i].myGame1 = this;

                if (AIPool[i].myPos.X < player.position.X)
                {
                    AIPool[i].direction = -1;
                }
                else
                {
                    AIPool[i].direction = 1;
                }
            }

            for (int i = 0; i < 100; i++)
            {
                player.bullet[i] = new Bullet(bulletTexture, new Vector2(2000, 2000), 1);
                player.bullet[i].myCamera = myCamera;
            }


            for (int i = 0; i < 100; i++)
            {
                bulletRectangle[i] = new Rectangle((int)player.bullet[i].position.X, (int)player.bullet[i].position.Y, 15, 5);
                //bulletRectangle[i] = new Rectangle(2000, 2000, 15, 5);
            }

            for (int i = 0; i < AIPool.Length; i++)
            {
                enemyRectangle[i] = new Rectangle((int)AIPool[i].myPos.X, (int)AIPool[i].myPos.Y, 15, 200); //x=75
                                                                                                            //enemyRectangle[i] = new Rectangle(2000, 2000, 15, 200); //x=75

            }

            foreach (AI ai in AIPool)
            {
                //if (ai.myAnimations[0] == null)
                //{
                //    Debug.WriteLine("type " + ai.myAiType + " attack " + ai.myAiAttackType + " orient " + ai.myAiOrientation + " weapon " + ai.myAiWeapon + " costume " + ai.costumeType + " HAS NULL 0");
                //}
                //if (ai.myAnimations[1] == null)
                //{
                //    Debug.WriteLine("type " + ai.myAiType + " attack " + ai.myAiAttackType + " orient " + ai.myAiOrientation + " weapon " + ai.myAiWeapon + " costume " + ai.costumeType + " HAS NULL 1");
                //}
                //if (ai.myAnimations[2] == null)
                //{
                //    Debug.WriteLine("type " + ai.myAiType + " attack " + ai.myAiAttackType + " orient " + ai.myAiOrientation + " weapon " + ai.myAiWeapon + " costume " + ai.costumeType + " HAS NULL 2");
                //}
                if (ai.myAnimations[3] == null)
                {
                    Debug.WriteLine("type " + ai.myAiType + " attack " + ai.myAiAttackType + " orient " + ai.myAiOrientation + " weapon " + ai.myAiWeapon + " costume " + ai.costumeType + " HAS NULL 3");
                }
                if (ai.myAnimations[4] == null)
                {
                    Debug.WriteLine("type " + ai.myAiType + " attack " + ai.myAiAttackType + " orient " + ai.myAiOrientation + " weapon " + ai.myAiWeapon + " costume " + ai.costumeType + " HAS NULL 4");
                }
            }

            spawnAI();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            BackgroundSong = Content.Load<Song>("City_Music");
            MediaPlayer.Play(BackgroundSong);
            MediaPlayer.IsRepeating = true;

            FinalSong = Content.Load<Song>("Final_Scene");

            AiAnims = new AiAnimation();

            LoadLevel();

            player = new Character(Content.Load<Texture2D>("breathingAnim"), new Vector2(-630, 720 - 250));
            player.chainSaw = Content.Load<Texture2D>("ChainSaw");
            bulletTexture = Content.Load<Texture2D>("Bullet");
            player.sndPistol = Content.Load<SoundEffect>("Desert_Eagle_Firing_01");
            player.sndHit = Content.Load<SoundEffect>("killhit");
            player.myGame1 = this;

            myCamera.myPlayer = player;
            myCamera.start();

            player.walkingTexture = Content.Load<Texture2D>("walkingAnim");
            player.walkingAnim = new AnimatedSprite(player.walkingTexture, 1, 6, .1f);

            player.playerCrouchTexture = Content.Load<Texture2D>("playerCrouch");
            player.playerCrouchAnim = new AnimatedSprite(player.playerCrouchTexture, 1, 3, .1f);
            player.playerCrouchAnim.loopable = false;

            player.breathingTexture = Content.Load<Texture2D>("breathingAnim");
            player.breathingAnim = new AnimatedSprite(player.breathingTexture, 1, 3, .05f);

            player.smokingTexture = Content.Load<Texture2D>("smokingAnim");
            player.smokingAnim = new AnimatedSprite(player.smokingTexture, 1, 11, .05f);

            Thug1 = Content.Load<Texture2D>("Thug1");
            Thug2 = Content.Load<Texture2D>("Thug2");
            Thug3 = Content.Load<Texture2D>("Thug3");

            civ1 = Content.Load<Texture2D>("Civ1");
            civ2 = Content.Load<Texture2D>("Civ2");
            civ3 = Content.Load<Texture2D>("Civ3");


            // 0 bat
            // 1 nightstick
            // 2 pipe
            // 3 chainsaw
            // 4 knife
            player.meleeWeaponStandTextures[0] = Content.Load<Texture2D>("pipeMelee");
            player.meleeWeaponStandAnim[0] = new AnimatedSprite(player.meleeWeaponStandTextures[0], 1, 4, animSpeed * 2);

            player.meleeWeaponStandTextures[1] = Content.Load<Texture2D>("pipeMelee");
            player.meleeWeaponStandAnim[1] = new AnimatedSprite(player.meleeWeaponStandTextures[1], 1, 4, animSpeed * 2);

            player.meleeWeaponStandTextures[2] = Content.Load<Texture2D>("pipeMelee");
            player.meleeWeaponStandAnim[2] = new AnimatedSprite(player.meleeWeaponStandTextures[2], 1, 4, animSpeed * 2);

            player.meleeWeaponStandTextures[3] = Content.Load<Texture2D>("pipeMelee");
            player.meleeWeaponStandAnim[3] = new AnimatedSprite(player.meleeWeaponStandTextures[3], 1, 4, animSpeed * 2);

            player.meleeWeaponStandTextures[4] = Content.Load<Texture2D>("pipeMelee");
            player.meleeWeaponStandAnim[4] = new AnimatedSprite(player.meleeWeaponStandTextures[4], 1, 4, animSpeed * 2);
            /////////
            //player.meleeWeaponWalkTextures[0] = Content.Load<Texture2D>("pipeMelee");
            //player.meleeWeaponWalkAnim[0] = new AnimatedSprite(player.meleeWeaponWalkTextures[0], 1, 4, animSpeed);

            //player.meleeWeaponWalkTextures[1] = Content.Load<Texture2D>("pipeMelee");
            //player.meleeWeaponWalkAnim[1] = new AnimatedSprite(player.meleeWeaponWalkTextures[1], 1, 4, animSpeed);

            //player.meleeWeaponWalkTextures[2] = Content.Load<Texture2D>("pipeMelee");
            //player.meleeWeaponWalkAnim[2] = new AnimatedSprite(player.meleeWeaponWalkTextures[2], 1, 4, animSpeed);

            //player.meleeWeaponWalkTextures[3] = Content.Load<Texture2D>("pipeMelee");
            //player.meleeWeaponWalkAnim[3] = new AnimatedSprite(player.meleeWeaponWalkTextures[3], 1, 4, animSpeed);

            //player.meleeWeaponWalkTextures[4] = Content.Load<Texture2D>("pipeMelee");
            //player.meleeWeaponWalkAnim[4] = new AnimatedSprite(player.meleeWeaponWalkTextures[4], 1, 4, animSpeed);

            player.playerDeathTexture = Content.Load<Texture2D>("playerDeathTexture");
            player.playerDeathAnim = new AnimatedSprite(player.playerDeathTexture, 1, 4, 0.3f);

            

            player.playerHandcuffTexture = Content.Load<Texture2D>("playerHandcuffTexture");
            player.playerHandcuffAnim = new AnimatedSprite(player.playerHandcuffTexture, 1, 7, animSpeed);

            player.playerStompTexture = Content.Load<Texture2D>("playerStompTexture");
            player.playerStompAnim = new AnimatedSprite(player.playerStompTexture, 1, 3, 0.1f);


            // 0 pistol
            // 1 uzi
            // 2 shotgun
            // 3 rifle
            player.rangeWeaponStandTextures[0] = Content.Load<Texture2D>("shootingAnim");
            player.rangeWeaponStandAnim[0] = new AnimatedSprite(player.rangeWeaponStandTextures[0], 1, 6, animSpeed);
            player.rangeWeaponStandAnim[0].isPlayerAttackAnim = true;

            player.rangeWeaponStandTextures[1] = Content.Load<Texture2D>("shootingAnim");
            player.rangeWeaponStandAnim[1] = new AnimatedSprite(player.rangeWeaponStandTextures[1], 1, 6, animSpeed);
            player.rangeWeaponStandAnim[1].isPlayerAttackAnim = true;

            player.rangeWeaponStandTextures[2] = Content.Load<Texture2D>("shotgunStand");
            player.rangeWeaponStandAnim[2] = new AnimatedSprite(player.rangeWeaponStandTextures[2], 1, 6, animSpeed);
            player.rangeWeaponStandAnim[2].isPlayerAttackAnim = true;

            player.rangeWeaponStandTextures[3] = Content.Load<Texture2D>("shootingAnim");
            player.rangeWeaponStandAnim[3] = new AnimatedSprite(player.rangeWeaponStandTextures[3], 1, 6, animSpeed);
            player.rangeWeaponStandAnim[3].isPlayerAttackAnim = true;
            ///////
            player.rangeWeaponWalkTextures[0] = Content.Load<Texture2D>("shootingAnim");
            player.rangeWeaponWalkAnim[0] = new AnimatedSprite(player.rangeWeaponWalkTextures[0], 1, 6, animSpeed);
            player.rangeWeaponWalkAnim[0].isPlayerAttackAnim = true;

            player.rangeWeaponWalkTextures[1] = Content.Load<Texture2D>("shotgunWalk");
            player.rangeWeaponWalkAnim[1] = new AnimatedSprite(player.rangeWeaponWalkTextures[1], 1, 6, animSpeed);
            player.rangeWeaponWalkAnim[1].isPlayerAttackAnim = true;

            player.rangeWeaponWalkTextures[2] = Content.Load<Texture2D>("shotgunWalk");
            player.rangeWeaponWalkAnim[2] = new AnimatedSprite(player.rangeWeaponWalkTextures[2], 1, 6, animSpeed);
            player.rangeWeaponWalkAnim[2].isPlayerAttackAnim = true;

            player.rangeWeaponWalkTextures[3] = Content.Load<Texture2D>("shootingAnim");
            player.rangeWeaponWalkAnim[3] = new AnimatedSprite(player.rangeWeaponWalkTextures[3], 1, 6, animSpeed);
            player.rangeWeaponWalkAnim[3].isPlayerAttackAnim = true;

            //AI
            //-------------------------------
            
            for (int j = 0; j < 3; j++)
            {
                AiAnims.civIdleSprites[j] = Content.Load<Texture2D>("civIdleSprites" + j);
                AiAnims.civWalkSprites[j] = Content.Load<Texture2D>("civWalkSprites" + j);
                AiAnims.civBatSprites[j] = Content.Load<Texture2D>("civBatSprites" + j);
                AiAnims.civKnifeSprites[j] = Content.Load<Texture2D>("civKnifeSprites" + j);
                AiAnims.civPipeSprites[j] = Content.Load<Texture2D>("civPipeSprites" + j);
                AiAnims.civStandPistolSprites[j] = Content.Load<Texture2D>("civStandPistolSprites" + j);
                AiAnims.civStandUziSprites[j] = Content.Load<Texture2D>("civStandUziSprites" + j);
                AiAnims.civWalkPistolSprites[j] = Content.Load<Texture2D>("civWalkPistolSprites" + j);
                AiAnims.civWalkUziSprites[j] = Content.Load<Texture2D>("civWalkUziSprites" + j);
                // switch range for all
                AiAnims.thugIdleSprites[j] = Content.Load<Texture2D>("thugIdleSprites" + j);
                AiAnims.thugWalkSprites[j] = Content.Load<Texture2D>("thugWalkSprites" + j);
                AiAnims.thugBatSprites[j] = Content.Load<Texture2D>("thugBatSprites" + j);
                AiAnims.thugKnifeSprites[j] = Content.Load<Texture2D>("thugKnifeSprites" + j);
                AiAnims.thugPipeSprites[j] = Content.Load<Texture2D>("thugPipeSprites" + j);
                AiAnims.thugStandPistolSprites[j] = Content.Load<Texture2D>("thugStandPistolSprites" + j);
                AiAnims.thugStandUziSprites[j] = Content.Load<Texture2D>("thugStandUziSprites" + j);
                AiAnims.thugWalkPistolSprites[j] = Content.Load<Texture2D>("thugWalkPistolSprites" + j);
                AiAnims.thugWalkUziSprites[j] = Content.Load<Texture2D>("thugWalkUziSprites" + j);
              

                //Death Animations
                AiAnims.civDieSprites[j] = Content.Load<Texture2D>("civDismemberSprites" + j);
                AiAnims.thugDieSprites[j] = Content.Load<Texture2D>("thugDismemberSprites" + j);

                AiAnims.civFloorSprites[j] = Content.Load<Texture2D>("civDieSprites" + j);
                AiAnims.thugFloorSprites[j] = Content.Load<Texture2D>("thugDieSprites" + j);
                AiAnims.civArrestSprites[j] = Content.Load<Texture2D>("civCuffSprites" + j);
                AiAnims.thugArrestSprites[j] = Content.Load<Texture2D>("thugCuffSprites" + j);
            }


            startGame();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
                        
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            Bg2Position = new Vector2(myCamera.center.X + 530, 10);
            scoreFontPos = new Vector2(myCamera.center.X + 730, 10);
            CivilianPos = new Vector2(myCamera.center.X, 10);
            civilfontpos = new Vector2(myCamera.center.X + 240, 10);
            BadPos = new Vector2(myCamera.center.X + 900, 10);
            badFontpos = new Vector2(myCamera.center.X + 1150, 10);
            brutpos = new Vector2(myCamera.center.X + 235, 150);

            Hundred = Content.Load<Texture2D>("+100");
            Twohundred = Content.Load<Texture2D>("-200");
            HundredPos = new Vector2(myCamera.center.X + 550, 350);
            TwohundredPos = new Vector2(myCamera.center.X + 550, 350);

            KeyboardState resetbutton = Keyboard.GetState();

            if (player.position.X > 0 && player.position.X < 6415) //Extend Level 8980?
            {
                myCamera.Update(gameTime);
                allowSpawn = true;
            }
            //Debug.WriteLine("Player Pos " + player.position.X);
            //Debug.WriteLine("Cam Pos " + myCamera.center.X);


            player.Update(gameTime);
            if (!Gameover)
            {
               
                for (int i = 0; i < AIPool.Length; i++)
                {
                    if (AIPool[i] != null && AIPool[i].isInGame)
                        AIPool[i].updateAI(gameTime);
                }
                CollisionDetection();

                aiSpawnCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (allowSpawn)
                {
                    if (aiSpawnCounter > aiSpawnTime)
                    {
                        spawnAI();
                    }

                }
            }

            LoadLevelUpdate();


            if (resetbutton.IsKeyDown(Keys.R))
            {
                Reset();
            }

            /*MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Debug.WriteLine("Clicked");
            }*/

            base.Update(gameTime);
        }
        Texture2D tempText;
        public void Endscreen()
        {
            ShowGameOver = true;
            MediaPlayer.Stop();

            MediaPlayer.Play(FinalSong);
            MediaPlayer.IsRepeating = true;

            Thug1pos = new Vector2(myCamera.center.X, 0);
            Thug2pos = new Vector2(myCamera.center.X, 0);
            Thug3pos = new Vector2(myCamera.center.X, 0);

            civ1pos = new Vector2(myCamera.center.X, 0);
            civ2pos = new Vector2(myCamera.center.X, 0);
            civ3pos = new Vector2(myCamera.center.X, 0);


            int rand = random.Next(0, 6);
            switch (rand)
            {
                case 0:
                    tempText = Thug1;
                    break;
                case 1:
                    tempText = Thug2;
                    break;
                case 2:
                    tempText = Thug3;
                    break;
                case 3:
                    tempText = civ1;
                    break;
                case 4:
                    tempText = civ2;
                    break;
                case 5:
                    tempText = civ3;
                    break;
            }
            for (int k = 0; k < 100; k++)
            {
                //AIPool[k].isDead = false;
                AIPool[k] = null;
                enemyRectangle[k].X = 10000;
                enemyRectangle[k].Y = 10000;

            }
        }

        public void CollisionDetection()
        {

            //Rectangle charRectangle = new Rectangle((int)player.position.X, (int)player.position.Y, /*player.defaultTexture.Width, player.defaultTexture.Height*/);
            if(player.usingPipe)
            {
                charRectangle.Width = 130;
            }
            else
            {
                charRectangle.Width = 50;
            }
            
            charRectangle.Height = 200;

            //#temp
            rect = new Texture2D(graphics.GraphicsDevice, charRectangle.Width, charRectangle.Height);
            Color[] data = new Color[charRectangle.Width * charRectangle.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Chocolate;
            rect.SetData(data);
            rectEnem = new Texture2D(graphics.GraphicsDevice, charRectangle.Width, charRectangle.Height);
            //Color[] data = new Color[charRectangle.Width * charRectangle.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Red;
            rectEnem.SetData(data);

            if(charRectangle.Width == 130)
            {
                if (player.direction == 1)
                    charRectangle.X = (int)player.position.X + 69;
                else
                    charRectangle.X = (int)player.position.X + 9;
            }
            else
            {
                charRectangle.X = (int)player.position.X + 69;
            }

            charRectangle.Y = (int)player.position.Y + 50;

            if (charRectangle.Intersects(trashBagRec))
            {
                inventory[0] = trashBag;
                trashBag = null;
                trashBagRec.X = 5000;
                trashBagRec.Y = 5000;
                //Debug.WriteLine("Trash Collision");
            }

            for (int i = 0; i < 100; i++)
            {
                if (player.bullet[i] != null)
                {
                    bulletRectangle[i].X = (int)player.bullet[i].position.X;
                    bulletRectangle[i].Y = (int)player.bullet[i].position.Y;
                }
            }

            for (int i = 0; i < AIPool.Length; i++)
            {
                if (AIPool[i].isInGame)
                {
                    if (AIPool[i].enemyState != 3)
                    {
                        if (!AIPool[i].isDead)
                        {
                            enemyRectangle[i].X = (int)AIPool[i].myPos.X + AIPool[i].colliderXPos;

                            //todo Shfit to AI onFrame()
                            /*if(AIPool[i].direction == -1)
                                enemyRectangle[i].X = (int)AIPool[i].myPos.X + 72;
                            else
                                enemyRectangle[i].X = (int)AIPool[i].myPos.X + 28; //58;*/

                            enemyRectangle[i].Y = (int)AIPool[i].myPos.Y + 54;
                        }
                      

                        if(AIPool[i].myAiAttackType == AI.aiAttackType.Melee)
                        {
                            if(AIPool[i].changeCollider == true)
                                enemyRectangle[i].Width = 100;
                            else
                                enemyRectangle[i].Width = 50;
                        }
                    }
                
                }

                if (charRectangle.Intersects(enemyRectangle[i]))
                {
                   

                    if (player.playerState == 2 || player.playerState == 3)
                    {
                        // add weapon specific actions
                        
                      

                        if (AIPool[i].isFallen == false)
                        {
                            if (AIPool[i].myAiOrientation == AI.aiOrientation.Bad)
                            {
                                Score += 100;
                                badCount += 1;
                                numbersBad = true;
                                //enemyRectangle[i].X = -2000;
                                //enemyRectangle[i].Y = -2000;
                            }

                            if (AIPool[i].myAiOrientation == AI.aiOrientation.Good)
                            {
                                Score -= 200;
                                civilcount += 1;
                                brutality = true;
                                numbersGood = true;

                                //enemyRectangle[i].X = -2000;
                                //enemyRectangle[i].Y = -2000;
                            }
                        }

                        AIPool[i].isFallen = true;
                        enemyRectangle[i].Y = (int)AIPool[i].myPos.Y - 54;
                    }
                     else 
                    {
                        if (AIPool[i].myAiOrientation == AI.aiOrientation.Bad)
                        {
                            //enemyRectangle[i].X = 8000;
                            //enemyRectangle[i].Y = 8000;
                            if (AIPool[i].isDead || AIPool[i].isFallen || AIPool[i].isCuffed)// || AIPool[i].myAiAttackType == AI.aiAttackType.Range)
                            {
                                //enemyRectangle[i].X = 8000;
                                //enemyRectangle[i].Y = 8000;
                            }
                            else
                            {
                                Debug.WriteLine("YOU DIED!!!!!!!");
                                player.isDead = true;
                                charRectangle.X = 6000;
                                charRectangle.Y = 6000;
                                Gameover = true;
                                Endscreen();
                                i = 10000;
                                return;
                           }
                                
                         }

                    }
                }

                for (int j = 0; j < 100; j++)
                {
                    
                    if (!AIPool[i].isFallen && bulletRectangle[j].Intersects(enemyRectangle[i]))
                    {
                        //Debug.WriteLine("YOU KILLED SOMEONE!");
                        player.bullet[j] = null;
                        bulletRectangle[j].X = 2000;
                        bulletRectangle[j].Y = 2000;
                        enemyRectangle[i].X = -2000;
                        enemyRectangle[i].Y = -2000;
                        //AIPool[i].enemyState = 3;
                        AIPool[i].isDead = true;
                        //AIPool[i].assignCounter();
                        if (AIPool[i].myAiOrientation == AI.aiOrientation.Bad)
                        {
                            Score += 100;
                            badCount += 1;
                            numbersBad = true;
                        }

                        if (AIPool[i].myAiOrientation == AI.aiOrientation.Good)
                        {
                            Score -= 200;
                            civilcount += 1;
                            brutality = true;
                            numbersGood = true;
                        }

                    }
                }
                if (brutality == true)
                {
                    brutalitytimer++;
                    brutpos.Y -= 0.5f;
                    if (brutalitytimer > 7000)
                    {
                        brutality = false;

                        brutalitytimer = 0;

                    }
                }
                if (numbersGood)
                {
                    numberstimerGood++;
                    if (numberstimerGood > 7000)
                    {
                        numbersGood = false;
                        numberstimerGood = 0;
                    }
                }

                if (numbersBad)
                {
                    numberstimerBad++;
                    if (numberstimerBad > 7000)
                    {
                        numbersBad = false;
                        numberstimerBad = 0;
                    }
                }
            }
        }

        void spawnAI()
        {
            //int temp = 0 ;
            //while (AiInGame < 15)
            //{
             for (int i = 0; i < AIPool.Length; i++)
	         {
                    if (!AIPool[i].isInGame)
                    {
                        tempAI = AIPool[i];
                        i = 2000;
                    }
            }
            tempAI.isInGame = true;


            switch (tempAI.myAiType)
            {
                case AI.aiType.standingCivilian:
                case AI.aiType.standingThug:
                    float randSpot = randPos.Next(640, 1000);
                    tempAI.myPos = new Vector2(player.position.X + randSpot, player.position.Y);
                    break;
                case AI.aiType.walkingCivilian:
                case AI.aiType.walkingThug:

                    int tempSide = randSide.Next(0, 2);
                    int tempPos = randPos.Next(640, 1000);
                    if (tempSide == 0)
                    {
                        // spawn to left
                        tempAI.myPos = new Vector2(player.position.X - tempPos, player.position.Y);
                    }
                    else
                    {
                        // spawn to right
                        tempAI.myPos = new Vector2(player.position.X + tempPos, player.position.Y);
                    }
                    

                    break;

            }

            AiInGame++;
            if (tempAI.myPos.X < player.position.X)
            {
                tempAI.direction = -1;
            }
            else
            {
                tempAI.direction = 1;
            }
            tempAI = null;

            aiSpawnCounter = 0;
            aiSpawnTime = 2 * (0.5f + randomTime.NextDouble()); // between 1 and 3 seconds
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, myCamera.Transform);

            

            DrawLevel();


            player.Draw(spriteBatch);
            //DrawRectangle(new Rectangle(charRectangle.X, charRectangle.Y, charRectangle.Width, charRectangle.Height), Color.Blue * 0.5f);
            //for (int i = 0; i < 100; i++)
            //{
            //    DrawRectangle(new Rectangle(enemyRectangle[i].X, enemyRectangle[i].Y, enemyRectangle[i].Width, enemyRectangle[i].Height), Color.Red * 0.5f);
            //}
            spriteBatch.End();

            base.Draw(gameTime);
        }



/// Methods to Load Level


        public void LoadLevel()
        {
            Bg = Content.Load<Texture2D>("bkg");
            BgPosition = new Vector2(-640, 0);
            //Bg1 = Content.Load<Texture2D>("Backdrop");
            //Bg1Position = new Vector2(+640, 0);
            Bg2 = Content.Load<Texture2D>("square");
            brut = Content.Load<Texture2D>("Brutality");
            Civilians = Content.Load<Texture2D>("civilians");
            Bad = Content.Load<Texture2D>("badpeople");
            scoreTexture = Content.Load<SpriteFont>("scoreFont");
            civil = Content.Load<SpriteFont>("civilian");
            badFont = Content.Load<SpriteFont>("bad");
            trashBag = Content.Load<Texture2D>("TrashBag");
            trashBagPosition = new Vector2(640 + 708, 557 + 65);
            trashBagRec = new Rectangle((int)trashBagPosition.X, (int)trashBagPosition.Y, 65, 65);

        }

        public void Reset()
        {


            Gameover = false;
            myCamera.myPlayer = player;
            player.position = new Vector2(-630, 720 - 250);
            myCamera.start();
            badCount = 0;
            civilcount = 0;
            Score = 0;

            for (int k = 0; k < 100; k++)
            {
                //AIPool[k].isDead = false;
                AIPool[k] = null;
                enemyRectangle[k].X = 10000;
                enemyRectangle[k].Y = 10000;

            }

            foreach (Bullet b in player.bullet)
            {
                if (b != null)
                {
                    b.position.X = 2000;
                    b.position.Y = 2000;
                }
            }
            startGame();

            Debug.WriteLine(Score);

        }

        private void LoadLevelUpdate()
        {
            if (trashBag != null)
            {
                trashBagRec.X = (int)trashBagPosition.X;
                trashBagRec.Y = (int)trashBagPosition.Y;
            }


        }


        private void DrawLevel()
        {
            spriteBatch.Draw(Bg, BgPosition);
            //spriteBatch.Draw(Bg1, Bg1Position);
            spriteBatch.Draw(Bg2, Bg2Position);
            spriteBatch.Draw(Civilians, CivilianPos);
            spriteBatch.Draw(Bad, BadPos);
            spriteBatch.DrawString(scoreTexture, Score.ToString(), scoreFontPos, Color.White);
            spriteBatch.DrawString(civil, civilcount.ToString(), civilfontpos, Color.White);
            spriteBatch.DrawString(badFont, badCount.ToString(), badFontpos, Color.White);


            if (!Gameover)
            {
                for (int i = 0; i < AIPool.Length; i++)
                {
                    if (AIPool[i].isInGame)
                        AIPool[i].Draw(spriteBatch);
                }
            }

            if (brutality == true)
            {
                spriteBatch.Draw(brut, brutpos);
            }

            if (numbersGood)
            {
                spriteBatch.Draw(Twohundred, TwohundredPos);
            }

            if (numbersBad)
            {
                spriteBatch.Draw(Hundred, HundredPos);
            }


           /* if (trashBag != null)
                spriteBatch.Draw(trashBag, trashBagPosition);

            if (inventory.Length > 0)
            {
                for (int i = 0; i < inventory.Length; i++)
                {
                    if (inventory[i] != null)
                        spriteBatch.Draw(inventory[i], new Vector2(myCamera.center.X + 10, 10 + 75 * i));
                }
            }*/

            if (ShowGameOver)
            {

                spriteBatch.Draw(tempText, new Vector2(myCamera.center.X, 0));

                Bg2Position = new Vector2(myCamera.center.X + 430, 10);
                scoreFontPos = new Vector2(myCamera.center.X + 630, 10);
                CivilianPos = new Vector2(myCamera.center.X, 10);
                civilfontpos = new Vector2(myCamera.center.X + 240, 10);
                BadPos = new Vector2(myCamera.center.X + 900, 10);
                badFontpos = new Vector2(myCamera.center.X + 1150, 10);
                brutpos = new Vector2(myCamera.center.X + 235, 150);

                spriteBatch.Draw(Bg2, Bg2Position);
                spriteBatch.Draw(Civilians, CivilianPos);
                spriteBatch.Draw(Bad, BadPos);

                spriteBatch.DrawString(scoreTexture, Score.ToString(), scoreFontPos, Color.White);
                spriteBatch.DrawString(civil, civilcount.ToString(), civilfontpos, Color.White);
                spriteBatch.DrawString(badFont, badCount.ToString(), badFontpos, Color.White);


            }
        }

        private void DrawRectangle(Rectangle coords, Color color)
        {
            var rect = new Texture2D(graphics.GraphicsDevice, 1, 1);
            rect.SetData(new[] { color });
            spriteBatch.Draw(rect, coords, color);
        }

    }
}
