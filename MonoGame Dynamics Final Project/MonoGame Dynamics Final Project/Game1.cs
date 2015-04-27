#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGame_Dynamics_Final_Project.Sprites;
using MonoGame_Dynamics_Final_Project.Weapons;
#endregion

namespace MonoGame_Dynamics_Final_Project
{
    public enum GameState
    {
        SplashScreen, 
        StartMenu,
        Play, 
        GameOver,
        Exit
    }
    public enum Level
    {
        Null,
        Level1,
        Level2,
        Level3
    }
    public enum Wave
    {
        Null,
        Wave1,
        Wave2,
        Wave3,
        Wave4,
        Wave5,
        Wave6,
        Wave7,
        Wave8,
        Wave9
    }
    public class Game1 : Game
    {
        #region Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Independent Resolution and Camera
        private ResolutionRenderer _irr;
        private const int VIRTUAL_RESOLUTION_WIDTH = 2160;
        private const int VIRTUAL_RESOLUTION_HEIGHT = 1440;
        private Camera2D _camera;


        GameState gameState = GameState.StartMenu;
        Wave wave = Wave.Null;
        Level level = Level.Null;
        PowerUps Powerups = PowerUps.Null;

        public static Random random;

        // background
        int windowWidth, windowHeight;
        Texture2D[] background; // Current Resolution 480w x 800h
        ScrollingBackground myBackground;
        ScrollingBackground myBGtwo;

        //POWER UP IMGs
        Texture2D AtkSpdUp;
        Texture2D MoveSpdUp;
        Texture2D HPUp;
        Texture2D AtkSpdDown;
        Texture2D MoveSpdDown;
        Texture2D HPDown;

        // menu
        Texture2D startMenuScreen;
        Menu menuScreen;
        SpriteFont menuFont;
        Color customColor;
        float score = 0;
        float timer = 0.0f;
        float damage = 0.0f; 
        bool playGame;

        // player
        Texture2D playerTexture;
        Texture2D playerMove;
        Texture2D playerRight;
        Texture2D playerLeft;
        Texture2D playerRightTurn;
        Texture2D playerLeftTurn;
        Texture2D health;
        Rectangle healthRect;
        Player playerShip;
        Player follower;
        // rail turret
        RailTurret railLeft, railRight;
        Weapon weapon;

        // enemies
        int currentWave;
        List<Enemy> Enemywave = new List<Enemy>();
        List<Enemy>[] WaveDef = new List<Enemy>[9];
        List<PowerUp> powerUpList = new List<PowerUp>();

        //PowerUps
        double spawnChance = 0.2;

       // Vector2 gravityForce = new Vector2(0.0f, 150.0f);
        Vector2 offset = new Vector2(500, 500);

        // input
        KeyboardState oldState;
        int animationResetSwitchU;
        int animationResetSwitchL;
        int animationResetSwitchR;

        //Particle Effects
        ParticleEngine Thruster1;
        ParticleEngine Thruster2;
        List<Texture2D> Thrustertextures;

        List<ParticleEngine> StingrayParticles = new List<ParticleEngine>();
        List<Texture2D> StingrayTextures;

        int EnemyParticleCounter = 0;

        List<ParticleEngine> DestructionParticles = new List<ParticleEngine>();
        List<Texture2D> DestructionTextures= new List<Texture2D>();
        List<int> DestructionRadiusCounters = new List<int>();
        List<int> DestructionAngleCounters = new List<int>();
        List<Vector2> DestructionEmmision = new List<Vector2>();

        Random randomnumber = new Random();

        #endregion

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            this.Window.IsBorderless = true;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            //set virtual screen resolution
            _irr = new ResolutionRenderer(this, VIRTUAL_RESOLUTION_WIDTH, VIRTUAL_RESOLUTION_HEIGHT, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            _camera = new Camera2D(_irr) { MaxZoom = 10f, MinZoom = .4f, Zoom = 1f };
            _camera.SetPosition(new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height/2));
            _camera.RecalculateTransformationMatrices();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            string[] menuItems = { "Launch Ship","How to Play", "Exit Cockpit" };

           // try
            //{
                //Load Particle textures
                Thrustertextures = new List<Texture2D>();
                Thrustertextures.Add(Content.Load<Texture2D>("Images/Particles/smokepoof"));
                //textures.Add(Content.Load<Texture2D>("Images/Particles/starpoof1"));
                //textures.Add(Content.Load<Texture2D>("Images/Particles/starpoof2"));
                Thrustertextures.Add(Content.Load<Texture2D>("Images/Particles/poofparticle"));
                Thruster1 = new ParticleEngine(Thrustertextures, new Vector2(400, 240));
                Thruster2 = new ParticleEngine(Thrustertextures, new Vector2(400, 240));

                StingrayTextures = new List<Texture2D>();
                StingrayTextures.Add(Content.Load<Texture2D>("Images/Particles/starpoof1"));
                StingrayTextures.Add(Content.Load<Texture2D>("Images/Particles/starpoof2"));

                //Destruction Particles
                DestructionTextures.Add(Content.Load<Texture2D>("Images/Particles/meow"));
                DestructionTextures.Add(Content.Load<Texture2D>("Images/Particles/meow1"));
                DestructionTextures.Add(Content.Load<Texture2D>("Images/Particles/meow2"));
                DestructionTextures.Add(Content.Load<Texture2D>("Images/Particles/meow3"));
                DestructionTextures.Add(Content.Load<Texture2D>("Images/Particles/meow4"));

                /*Song song = Content.Load<Song>("TellMe");
                MediaPlayer.Play(song);*/

                

                menuFont = Content.Load<SpriteFont>("Fonts/titleFont");
                menuScreen = new Menu(GraphicsDevice, menuFont, menuItems);
                startMenuScreen = Content.Load<Texture2D>("Images/Backgrounds/MenuTwo");
                customColor.A = 1;
                customColor.R = 200;
                customColor.G = 0;
                customColor.B = 255;

                windowWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                windowHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                random = new Random();
                playGame = false;

                //PwrUpTextures
                AtkSpdUp = Content.Load<Texture2D>("Images/PowerUps/AtkSpdUp");
                MoveSpdUp = Content.Load<Texture2D>("Images/PowerUps/MoveSpdUp");
                HPUp = Content.Load<Texture2D>("Images/PowerUps/HPUp");
                AtkSpdDown = Content.Load<Texture2D>("Images/PowerUps/AtkSpdDown");
                MoveSpdDown = Content.Load<Texture2D>("Images/PowerUps/MoveSpdDown");
                HPDown = Content.Load<Texture2D>("Images/PowerUps/HPDown");


                // background
                myBackground = new ScrollingBackground();
                myBGtwo = new ScrollingBackground();
                background = new Texture2D[3];
                for (int i = 0; i < background.Length; i++)
                {
                    background[i] = Content.Load<Texture2D>("Images/Backgrounds/universe0" + (i+1).ToString());
                }
                myBackground.Load(GraphicsDevice, background, background.Length, 0.5f, 1); // change float to change animation speed           
                myBGtwo.Load(GraphicsDevice, background, background.Length, 0.5f, 2); // change float to change animation speed 
               
                // player sprites
                playerTexture = Content.Load<Texture2D>("Images/Animations/Commandunit-idle");
                playerMove = Content.Load<Texture2D>("Images/Animations/Commandunit-move");
                playerRight = Content.Load<Texture2D>("Images/Animations/Commandunit-right");
                playerLeft = Content.Load<Texture2D>("Images/Animations/Commandunit-left");
                playerRightTurn = Content.Load<Texture2D>("Images/Animations/Commandunit-Turn");
                playerLeftTurn = Content.Load<Texture2D>("Images/Animations/Commandunit-Turn-left");
                health = Content.Load<Texture2D>("Images/playerhealth");
 
                playerShip = new Player(64,70,playerTexture,
                    new Vector2(windowWidth / 2, windowHeight - 70),
                    new Vector2(10, 10),
                    true,
                    1.0f,
                    5000.0f
                    );

                //rail turret
                railLeft = new RailTurret(Content, playerShip.Position, playerShip.Velocity, 1);
                railRight = new RailTurret(Content, playerShip.Position, playerShip.Velocity, -1);

                follower = new Follower(32,32,Content, playerShip,new Vector2(0, playerShip.frameHeight+20),1.0f, true);

                LoadWaves();
                LoadLevel(1, 1);

                foreach (Enemy enemy in Enemywave)
                {
                    if (enemy.enemyType == "stingRay")
                    {
                        StingrayParticles.Add(new ParticleEngine(StingrayTextures, new Vector2(400, 240)));
                    }
                }
            //}
            //catch (ContentLoadException e)
            //{
                //Will properly display error messages soon
                //Console.WriteLine("Could not load " + e.Source);
                //Console.ReadLine();
            //}
            
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float elapsed = (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f; 
            myBackground.Update(gameTime, elapsed * 100);
            myBGtwo.Update(gameTime, elapsed * 100);
            UpdateInput(gameTime); 

            //If Game is Running
            if (gameState == GameState.Play)
            {
                healthRect = new Rectangle(100, GraphicsDevice.Viewport.Height - 50, (int)playerShip.Health / 5, health.Height);

                // updating scroll speed
               
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (playerShip.Alive)
                {
                    playerShip.Update(gameTime, GraphicsDevice, Enemywave);
                    railLeft.Update(gameTime, playerShip.Position);
                    railRight.Update(gameTime, playerShip.Position);
                    follower.Update(playerShip, gameTime);
                   
                    Thruster1.EmitterLocation = playerShip.Position + new Vector2(15, playerShip.frameHeight - 30);
                    Thruster2.EmitterLocation = playerShip.Position + new Vector2(-15, playerShip.frameHeight - 30);
                }

                Thruster1.Update(playerShip.Alive, playerShip.Velocity, 100f, Color.SlateGray, 1);
                Thruster2.Update(playerShip.Alive, playerShip.Velocity, 100f, Color.SlateGray, 1);

                foreach (Enemy enemy in Enemywave)
                {
                    enemy.Update(gameTime, playerShip);
                    if (enemy.enemyType == "stingRay")
                    {
                        enemy.Update(gameTime, playerShip);

                        //Stingray Particles

                        if (EnemyParticleCounter < StingrayParticles.Count)
                        enemy.Update(Content, gameTime, playerShip);
                    
                    //Stingray Particles
                    
                    if (EnemyParticleCounter < StingrayParticles.Count)
                        {
                            StingrayParticles[EnemyParticleCounter].EmitterLocation = enemy.Position + new Vector2(Convert.ToSingle(Math.Cos(enemy.rotation) * enemy.frameWidth / 4), Convert.ToSingle(Math.Sin(enemy.rotation) * enemy.frameWidth / 4));
                            StingrayParticles[EnemyParticleCounter].Update(enemy.Alive, enemy.Velocity, 9f, Color.Plum, 1);
                            EnemyParticleCounter++;
                        }
                        else
                        {
                            EnemyParticleCounter = 0;
                        }
                    }
                    if (enemy.enemyType == "voidVulture")
                    {
                        enemy.Update(gameTime, playerShip);
                    }
                    if (enemy.enemyType == "voidAngel")
                    {

                        enemy.Update(gameTime, playerShip);
                    }
                }

                foreach (PowerUp pUp in powerUpList)
                {
                    pUp.Update(gameTime, GraphicsDevice);
                    if (pUp.removeFromScreen)
                    {
                        pUp.Alive = false;
                    }
                }

                // tests for collision of primary shots against enemy
                if (Enemywave.Count > 0)
                {
                    for (int i = 0; i < Enemywave.Count; i++)
                    {

                        if (Enemywave[i].painSwitch == true)
                        {
                            Enemywave[i].flashCounter++;
                            Enemywave[i].hurtFlash = new Color(random.Next(0, 255), random.Next(0, 10), random.Next(0, 100));
                            if (Enemywave[i].flashCounter >= 100)
                            {
                                Enemywave[i].flashCounter = 0;
                                Enemywave[i].painSwitch = false;
                            }
                        }
                        else
                        {
                            Enemywave[i].hurtFlash = Color.White;
                        }

                        int collide = Enemywave[i].CollisionShot(playerShip.Primary);
                        if (collide != -1)
                        {
                            Enemywave[i].painSwitch = true;
                            Enemywave[i].flashCounter = 0;
                            Enemywave[i].Health -= playerShip.Primary[collide].Damage;
                            playerShip.Primary.RemoveAt(collide);

                            Enemywave[i].Position += Vector2.Normalize(Enemywave[i].Position - playerShip.Position) * 2000 / Enemywave[i].frameHeight;

                            playerShip.CurrentPrimaryAmmo++;

                            if (Enemywave[i].Health <= 0f)
                            {
                                if (Enemywave[i].enemyType == "stingRay")
                                {
                                    score += 100;
                                    StingrayParticles.RemoveAt(StingrayParticles.Count - 1);
                                }
                                if (Enemywave[i].enemyType == "voidVulture")
                                {
                                    score += 1000;

                                    DestructionParticles.Add(new ParticleEngine(DestructionTextures, new Vector2(400, 240)));
                                    DestructionRadiusCounters.Add(10);
                                    DestructionAngleCounters.Add(0);
                                    DestructionEmmision.Add(Enemywave[i].Position);
                                }
                                if (Enemywave[i].enemyType == "voidAngel")
                                {
                                    score += 500;
                                }

                                double rand = random.NextDouble();
                                if (rand < spawnChance)
                                    SpawnPowerUp(Enemywave[i].Position);
                                Enemywave[i].Alive = false;
                                Enemywave.RemoveAt(i);
                            }
                        }
                    }


                    // tests for collision of secondary shots against enemy
                    for (int i = 0; i < Enemywave.Count; i++)
                    {
                        int collide = Enemywave[i].CollisionShot(playerShip.Secondary);
                        if (collide != -1)
                        {
                            if (playerShip.SecondaryType != "gravityWell")
                            {
                                playerShip.CurrentSecondaryAmmo++;
                                playerShip.Secondary.RemoveAt(collide);
                                if (random.NextDouble() <= spawnChance)
                                    SpawnPowerUp(Enemywave[i].Position);
                                Enemywave[i].Alive = false;
                                Enemywave.RemoveAt(i);

                            }
                        }
                    }
                }

                //Destrcution Update
                for (int i = 0; i < DestructionParticles.Count; i++)
                {
                    DestructionParticles[i].EmitterLocation = DestructionEmmision[i];
                    DestructionEmmision[i] += new Vector2(Convert.ToSingle(Math.Cos(DestructionAngleCounters[i]) * DestructionRadiusCounters[i]), Convert.ToSingle(Math.Sin(DestructionAngleCounters[i]) * DestructionRadiusCounters[i]));
                    DestructionRadiusCounters[i] += 10;
                    DestructionAngleCounters[i] += 10;
                    DestructionParticles[i].Update((DestructionRadiusCounters[i] < 1000), new Vector2(10, 10), 0f, new Color(random.Next(0, 255), random.Next(0, 50), random.Next(0, 100)), 10);

                }
                // testing collision of playership with enemy

                for (int i = 0; i < Enemywave.Count; i++)
                {
                    if (playerShip.CollisionSprite(Enemywave[i]))
                    {
                        //if (playerShip.IntersectsPixel(playerShip.source, playerShip.textureData, Enemywave[i].source, Enemywave[i].textureData))
                        //{
                       
                        playerShip.collisionDetected = true;
                        if (playerShip.collisionDetected == true)
                        {
                            damage += Enemywave[i].Damage;
                            Console.WriteLine("Health:" + playerShip.Health);
                            if (playerShip.Health <= 0.0f)
                            {

                                gameState = GameState.GameOver;
                                playerShip.Alive = false;
                                follower.Alive = false;
                            }
                        }

                        //Enemywave.RemoveAt(i);
                        //}
                    }
                }
                playerShip.Health -= damage;
                damage = 0f;


                // gravity well
                if (playerShip.ForcePull)
                {
                    playerShip.Secondary[0].forcePull(gameTime, Enemywave);
                    playerShip.Secondary[0].ElapsedTime += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

                    if (playerShip.Secondary[0].ElapsedTime > 5.0f)
                    {
                        playerShip.ForcePull = false;
                        playerShip.Secondary.RemoveAt(0);
                    }
                }

                for (int i = powerUpList.Count - 1; i >= 0; i--)
                {
                    if (powerUpList[i].removeFromScreen)
                    {
                        powerUpList.RemoveAt(i);
                    }
                    else if (CheckForPowerUps(playerShip.CollisionRectangle, powerUpList[i].CollisionRectangle))
                    {
                        powerUpList[i].ActivatePowerUp(Powerups, playerShip);
                        powerUpList.RemoveAt(i);
                    }
                }

                _camera.Update(gameTime);
                base.Update(gameTime);
                
            }
        }
        //public void ResetGame
        public void UpdateInput(GameTime gameTime)
        {
            bool keyPressed = false;
            KeyboardState keyState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (gameState == GameState.StartMenu)
            {
                Rectangle exit = new Rectangle(572, 384, 100, 50);
                Rectangle start = new Rectangle(572, 274, 200, 50);
                Rectangle instr = new Rectangle(572, 344, 150, 50);
                menuScreen.Update();

                if (menuScreen.ItemSelected == 3)
                {
                    this.Exit();
                }
                else if (menuScreen.ItemSelected == 1)
                {
                    gameState = GameState.Play;
                }
            }
            if (gameState == GameState.GameOver)
            {
                if (keyState.IsKeyDown(Keys.Enter))
                {
                    gameState = GameState.StartMenu;
                }
            }
            if (gameState == GameState.Play)
            {
                if (keyState.IsKeyDown(Keys.Up)
               || keyState.IsKeyDown(Keys.W)
               || gamePadState.DPad.Up == ButtonState.Pressed
               || gamePadState.ThumbSticks.Left.Y > 0)
                {
                    playerShip.Up();
                    keyPressed = true;

                    if (animationResetSwitchU == 0 && animationResetSwitchR == 0 && animationResetSwitchL == 0)
                    {
                        playerShip.resetAnimation();
                        playerShip.TextureImage = playerMove;
                        animationResetSwitchU++;
                        keyPressed = true;
                    }


                }
                else if (keyState.IsKeyUp(Keys.Up)
                  || keyState.IsKeyUp(Keys.W)
                  || gamePadState.DPad.Up == ButtonState.Released
                  || gamePadState.ThumbSticks.Left.Y == 0)
                {
                    if (animationResetSwitchU > 0)
                    {
                        playerShip.resetAnimation();
                        animationResetSwitchU = 0;
                        playerShip.TextureImage = playerTexture;
                    }

                }
                if (keyState.IsKeyDown(Keys.Down)
                  || keyState.IsKeyDown(Keys.S)
                  || gamePadState.DPad.Down == ButtonState.Pressed
                  || gamePadState.ThumbSticks.Left.Y < -0.5f)
                {
                    playerShip.Down();
                    keyPressed = true;
                }
                else if (keyState.IsKeyUp(Keys.Down)
                  || keyState.IsKeyUp(Keys.S)
                  || gamePadState.DPad.Down == ButtonState.Released
                  || gamePadState.ThumbSticks.Left.Y == 0)
                { }

                if (keyState.IsKeyDown(Keys.Left)
                  || keyState.IsKeyDown(Keys.A)
                  || gamePadState.DPad.Left == ButtonState.Pressed
                  || gamePadState.ThumbSticks.Left.X < -0.5f)
                {
                    playerShip.Left();
                    keyPressed = true;

                    if (animationResetSwitchL == 0)
                    {
                        playerShip.resetAnimation();
                        animationResetSwitchL++;
                        playerShip.framesOverride = 6;
                        playerShip.TextureImage = playerLeftTurn;
                    }
                    if (animationResetSwitchL == 1)
                    {

                        if (playerShip.frameIndex > 6) { animationResetSwitchL++; }
                    }
                    if (animationResetSwitchL == 2)
                    {
                        playerShip.resetAnimation();
                        playerShip.framesOverride = 0;
                        playerShip.TextureImage = playerLeft;
                        animationResetSwitchL++;
                    }
                }
                else if (keyState.IsKeyUp(Keys.Left)
                  || keyState.IsKeyUp(Keys.A)
                  || gamePadState.DPad.Left == ButtonState.Released
                  || gamePadState.ThumbSticks.Left.X == 0)
                {
                    if (animationResetSwitchL > 0)
                    {
                        playerShip.resetAnimation();
                        playerShip.framesOverride = 0;
                        animationResetSwitchL = 0;
                        animationResetSwitchR = 0;
                        playerShip.TextureImage = playerTexture;
                    }

                }
                if (keyState.IsKeyDown(Keys.Right)
                  || keyState.IsKeyDown(Keys.D)
                  || gamePadState.DPad.Right == ButtonState.Pressed
                  || gamePadState.ThumbSticks.Left.X > 0.5f)
                {
                    playerShip.Right();
                    keyPressed = true;

                    if (animationResetSwitchR == 0)
                    {
                        playerShip.resetAnimation();
                        animationResetSwitchR++;
                        playerShip.framesOverride = 6;
                        playerShip.TextureImage = playerRightTurn;
                    }
                    if (animationResetSwitchR == 1)
                    {
                        if (playerShip.frameIndex > 6) { animationResetSwitchR++; }
                    }
                    if (animationResetSwitchR == 2)
                    {
                        playerShip.resetAnimation();
                        playerShip.framesOverride = 0;
                        playerShip.TextureImage = playerRight;
                        animationResetSwitchR++;
                    }
                }
                else if (keyState.IsKeyUp(Keys.Right)
                  || keyState.IsKeyUp(Keys.D)
                  || gamePadState.DPad.Right == ButtonState.Released
                  || gamePadState.ThumbSticks.Left.X == 0)
                {
                    if (animationResetSwitchR > 0)
                    {
                        playerShip.resetAnimation();
                        playerShip.framesOverride = 0;
                        animationResetSwitchR = 0;
                        animationResetSwitchL = 0;
                        playerShip.TextureImage = playerTexture;
                    }

                }
                // Primary Weapon
                if (oldState.IsKeyUp(Keys.Space) && keyState.IsKeyDown(Keys.Space))
                {
                    playerShip.HasShotPrim = true;
                    if (playerShip.HasShotPrim)
                    {
                        playerShip.shootPrimary(Content, gameTime);
                    }
                    else
                    {
                        playerShip.HasShotPrim = false;
                    }

                }
                // Secondary Weapon
                if (oldState.IsKeyUp(Keys.B) && keyState.IsKeyDown(Keys.B))
                //|| gamePadState.IsButtonDown(Buttons.LeftTrigger))
                {
                    if (playerShip.HasShot)
                    {
                        playerShip.HasShot = false;
                        playerShip.ForcePull = true;
                        playerShip.Secondary[0].ElapsedTime = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                    }
                    else if (!playerShip.HasShot)
                    {
                        playerShip.shootSecondary(Content);
                    }
                }
                if (!keyPressed)
                {
                    playerShip.Idle();
                }

                oldState = keyState;
            }
        }
        private void drawRect(Rectangle coords, Color color)
        {
            var rect = new Texture2D(GraphicsDevice, 1, 1);
            rect.SetData(new[] { color });
        }
  
        protected override void Draw(GameTime gameTime)
        {
            switch (gameState)
            {
                case GameState.SplashScreen:
                    
                    break;

                case GameState.StartMenu:

                    _irr.Draw();
                    spriteBatch.BeginCamera(_camera, BlendState.NonPremultiplied);
                    
                    //spriteBatch.Begin();

                    myBackground.Draw(spriteBatch);
                    myBGtwo.Draw(spriteBatch);
                    spriteBatch.Draw(startMenuScreen, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    spriteBatch.DrawString(menuFont, "Cataclysm", new Vector2(GraphicsDevice.Viewport.Width / 8, GraphicsDevice.Viewport.Height / 9), customColor);
                    menuScreen.Draw(spriteBatch);

                    spriteBatch.End();
                    
                    _irr.SetupVirtualScreenViewport();

                    break;

                case GameState.Play:
                    GraphicsDevice.Clear(Color.Black);
                    
                    _irr.Draw();

                    spriteBatch.BeginCamera(_camera, BlendState.NonPremultiplied);
                    
                    //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
                    myBackground.Draw(spriteBatch);


                    spriteBatch.Draw(health, healthRect, Color.White);
                    spriteBatch.DrawString(menuFont, "Score:" + (Math.Round(timer,2) * score), new Vector2(0.0f, 50.0f), Color.White, 0.0f,Vector2.Zero,0.25f,SpriteEffects.None,0.0f);
                    spriteBatch.DrawString(menuFont, "Primary:" + playerShip.PrimaryType, new Vector2(100.0f, GraphicsDevice.Viewport.Height - 25.0f), Color.White, 0.0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0.0f);
                    spriteBatch.DrawString(menuFont, "Secondary:" + playerShip.SecondaryType, new Vector2(GraphicsDevice.Viewport.Width - 550.0f, GraphicsDevice.Viewport.Height - 25.0f), Color.White, 0.0f,Vector2.Zero,0.15f,SpriteEffects.None,0.0f);
                    railLeft.Draw(spriteBatch, gameTime, Color.White);
                    railRight.Draw(spriteBatch, gameTime, Color.White);
                    playerShip.Draw(spriteBatch, gameTime, Color.White);

                    //follower.Draw(spriteBatch, gameTime, Color.White);
                    foreach (Enemy enemy in Enemywave)
                    {
                        enemy.Draw(spriteBatch, gameTime, enemy.hurtFlash);
                    }
                    foreach (PowerUp pUp in powerUpList)
                    {
                        pUp.Draw(spriteBatch, gameTime);
                    }

                    foreach (ParticleEngine explosion in DestructionParticles)
                    {
                        explosion.Draw(spriteBatch);
                    }

                    Thruster1.Draw(spriteBatch);
                    Thruster2.Draw(spriteBatch);

                    foreach (ParticleEngine particle in StingrayParticles) { particle.Draw(spriteBatch); }
              

                    //Draw Explosion
                    for(int i = 0; i < DestructionParticles.Count; i++)
                    {
                        DestructionParticles[i].Draw(spriteBatch);
                        if (DestructionParticles[i].particles.Count == 0)
                        {
                            DestructionRadiusCounters.RemoveAt(i);
                            DestructionAngleCounters.RemoveAt(i);
                            DestructionEmmision.RemoveAt(i);
                            DestructionParticles.RemoveAt(i);
                        }
                    }
                    
                    spriteBatch.End(); 
                    _irr.SetupVirtualScreenViewport();
                 //   base.Draw(gameTime);
                    break;

                case GameState.GameOver:
                    GraphicsDevice.Clear(Color.Black);
                    _irr.Draw();
                    spriteBatch.BeginCamera(_camera, BlendState.NonPremultiplied);
                    //spriteBatch.Begin();
                    drawRect(new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.Black);
                    spriteBatch.DrawString(menuFont, "Game Over\n", new Vector2(GraphicsDevice.Viewport.Width / 4, 100.0f), customColor,0f,Vector2.Zero,0.5f,SpriteEffects.None,0f);
                    spriteBatch.DrawString(menuFont, "You Scored:" + (Math.Round(timer, 2) * score), new Vector2(GraphicsDevice.Viewport.Width / 6, 200.0f), customColor, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
                    
                    spriteBatch.End();
                    _irr.SetupVirtualScreenViewport();
                    
                    break;

                case GameState.Exit:

                    break;
            }

            
            //spriteBatch.End();
        }

        // Load the enemy level/waves here
        public void LoadLevel(int levelNum, int waveNum)
        {
            switch(levelNum)
            {
                case 1:
                    level = Level.Level1;
                    switch(waveNum)
                    {
                        case 1:
                            wave = Wave.Wave1;
                            Enemywave = WaveDef[0];
                            break;
                        case 2:
                            wave = Wave.Wave2;
                            break;
                        case 3:
                            wave = Wave.Wave3;
                            break;
                        default:
                            Console.WriteLine("Could not load wave (1-3)");
                            break;
                    }
                    break;
                case 2:
                    level = Level.Level2;
                    switch (levelNum)
                    {
                        case 1:
                            wave = Wave.Wave4;
                            break;
                        case 2:
                            wave = Wave.Wave5;
                            break;
                        case 3:
                            wave = Wave.Wave6;
                            break;
                        default:
                            Console.WriteLine("Could not load wave (4-6)");
                            break;
                    }
                    break;
                case 3:
                    level = Level.Level3;
                    switch (levelNum)
                    {
                        case 1:
                            wave = Wave.Wave7;
                            break;
                        case 2:
                            wave = Wave.Wave8;
                            break;
                        case 3:
                            wave = Wave.Wave9;
                            break;
                        default:
                            Console.WriteLine("Could not load wave (7-9)");
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("Could not load Level");
                    break;
            }
        }

        // Define the enemy waves here
        public void LoadWaves()
        {
            Enemy enemy;

            // Wave 1
            WaveDef[0] = new List<Enemy>();
            //formationSize = 10;          

            for (int i = 0; i < 10; i++ )
            {
                enemy = new Stingray(Content, GraphicsDevice, i + 1, "delta");
                WaveDef[0].Add(enemy);
            }
            VoidVulture voidVulture = new VoidVulture(Content, GraphicsDevice, 3, "line");
            WaveDef[0].Add(voidVulture);
            VoidAngel voidAngel = new VoidAngel(Content, GraphicsDevice, 3, "line");
            WaveDef[0].Add(voidAngel); 
            // Wave 2
            WaveDef[1] = new List<Enemy>();

            // Wave 3
            WaveDef[2] = new List<Enemy>();

            // Wave 4
            WaveDef[3] = new List<Enemy>();

            // Wave 5
            WaveDef[4] = new List<Enemy>();

            // Wave 6
            WaveDef[5] = new List<Enemy>();

            // Wave 7
            WaveDef[6] = new List<Enemy>();

            // Wave 8
            WaveDef[7] = new List<Enemy>();

            // Wave 9
            WaveDef[8] = new List<Enemy>();
        }
        public void SpawnPowerUp(Vector2 Position)
        {
            switch (random.Next(6))
            {
                case 0:
                    Powerups = PowerUps.AtkSpdUp;
                    powerUpList.Add(new PowerUp(AtkSpdUp, GraphicsDevice, Powerups, playerShip, Position, 2.0f));
                    break;
                case 1:
                    Powerups = PowerUps.MoveSpdUp;
                    powerUpList.Add(new PowerUp(MoveSpdUp, GraphicsDevice, Powerups, playerShip, Position, 1.0f));
                    break;
                case 2:
                    Powerups = PowerUps.HealthUp;
                    powerUpList.Add(new PowerUp(HPUp, GraphicsDevice, Powerups, playerShip, Position, 1.0f));
                    break;
                case 3:
                    Powerups = PowerUps.AtkSpdDown;
                    powerUpList.Add(new PowerUp(AtkSpdDown, GraphicsDevice, Powerups, playerShip, Position, 2.0f));
                    break;
                case 4:
                    Powerups = PowerUps.MoveSpdDown;
                    powerUpList.Add(new PowerUp(MoveSpdDown, GraphicsDevice, Powerups, playerShip, Position, 2.0f));
                    break;
                case 5:
                    Powerups = PowerUps.HealthDown;
                    powerUpList.Add(new PowerUp(HPDown, GraphicsDevice, Powerups, playerShip, Position, 2.0f));
                    break;

                default: break;
            }
        }
        public bool CheckForPowerUps(Rectangle player, Rectangle pwerUp)
        {
            return player.Intersects(pwerUp);
        }

    }
}
