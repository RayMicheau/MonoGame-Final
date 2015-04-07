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
        Play, 
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

        GameState gameState = GameState.Play;
        Wave wave = Wave.Null;
        Level level = Level.Null;

        public static Random random;

        // background
        int windowWidth, windowHeight;
        Texture2D[] background; // Current Resolution 480w x 800h
        ScrollingBackground myBackground;

        // player
        Texture2D playerTexture;
        Texture2D playerMove;
        Texture2D playerRight;
        Texture2D playerLeft;
        Texture2D playerRightTurn;
        Texture2D playerLeftTurn;
        Player playerShip;
        Player follower;

        // enemies
        int currentWave;
        List<Enemy> Enemywave = new List<Enemy>();
        List<Enemy>[] WaveDef = new List<Enemy>[9];

       // Vector2 gravityForce = new Vector2(0.0f, 150.0f);
        Vector2 offset = new Vector2(500, 500);

        // input
        KeyboardState oldState;
        int animationResetSwitchU;
        int animationResetSwitchL;
        int animationResetSwitchR;
        
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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            try
            {
                /*Song song = Content.Load<Song>("TellMe");
                MediaPlayer.Play(song);*/

                windowWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                windowHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                random = new Random();

                // background
                myBackground = new ScrollingBackground();
                background = new Texture2D[1];
                for (int i = 0; i < background.Length; i++)
                {
                    background[i] = Content.Load<Texture2D>("Images/Backgrounds/background-level-1");
                }
                myBackground.Load(GraphicsDevice, background, background.Length, 0.5f); // change float to change animation speed           

                // player sprites
                playerTexture = Content.Load<Texture2D>("Images/Animations/Commandunit-idle");
                playerMove = Content.Load<Texture2D>("Images/Animations/Commandunit-move");
                playerRight = Content.Load<Texture2D>("Images/Animations/Commandunit-right");
                playerLeft = Content.Load<Texture2D>("Images/Animations/Commandunit-left");
                playerRightTurn = Content.Load<Texture2D>("Images/Animations/Commandunit-Turn");
                playerLeftTurn = Content.Load<Texture2D>("Images/Animations/Commandunit-Turn-left");
                playerShip = new Player(64,70,playerTexture,
                    new Vector2(windowWidth / 2, windowHeight - 70),
                    new Vector2(10, 10),
                    true,
                    1.0f
                    );

                follower = new Follower(32,32,Content, playerShip,new Vector2(0, playerShip.frameHeight+20),1.0f, true);

<<<<<<< HEAD
                animationResetSwitchU = 0;
                animationResetSwitchL = 0;
                animationResetSwitchR = 0;
                // enemy sprites
                maxEnemies = 10;
                for (int i = 0; i < maxEnemies; i++)
                {
                    Vector2 position = new Vector2(random.Next(windowWidth), 150);
                    Enemy enemy = new Enemy(Content, GraphicsDevice, position);
                    Enemywave.Add(enemy);
                }
=======
                LoadWaves();
                LoadLevel(1, 1);
>>>>>>> master
            }
            catch (ContentLoadException)
            {
                //Will properly display error messages soon
                Console.WriteLine("Could not load a thing!");
            }
            
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // updating scroll speed
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            myBackground.Update(gameTime, elapsed * 100);

            UpdateInput(gameTime);

            playerShip.Update(gameTime, GraphicsDevice, Enemywave);
            follower.Update(playerShip, gameTime);

            // tests for collision of primary shots against enemy
            for (int i = 0; i < Enemywave.Count; i++)
            {
                int collide = Enemywave[i].CollisionShot(playerShip.Primary);
                if (collide != -1)
                {
                    playerShip.Primary.RemoveAt(collide);
                    Enemywave[i].Alive = false;
                    Enemywave.RemoveAt(i);
                }
            }
            // tests for collision of secondary shots against enemy
            for (int i = 0; i < Enemywave.Count; i++)
            {
                int collide = Enemywave[i].CollisionShot(playerShip.Secondary);
                if (collide != -1)
                {
                    playerShip.Secondary.RemoveAt(collide);
                    Enemywave[i].Alive = false;
                    Enemywave.RemoveAt(i);
                }
            }

            // testing collision of playership with enemy
            for (int i = 0; i < Enemywave.Count; i++)
            {
                if (playerShip.CollisionSprite(Enemywave[i]))
                {
                    if (playerShip.IntersectsPixel(playerShip.CollisionRectangle, playerShip.textureData, Enemywave[i].CollisionRectangle, Enemywave[i].textureData))
                    {
                        playerShip.collisionDetected = true;
                        Enemywave.RemoveAt(i);
                    }
                }
            }

            // gravity well
            if (playerShip.ForcePull)
            {
                playerShip.Secondary[0].forcePull(gameTime, Enemywave);

                if ((elapsed - playerShip.Secondary[0].ElapsedTime) > 1f)
                {
                    playerShip.ForcePull = false;
                    playerShip.Secondary.RemoveAt(0);
                }
            }

            base.Update(gameTime);
        }
        private void UpdateInput(GameTime gameTime)
        {
            bool keyPressed = false;

            KeyboardState keyState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (keyState.IsKeyDown(Keys.Up)
              || keyState.IsKeyDown(Keys.W)
              || gamePadState.DPad.Up == ButtonState.Pressed
              || gamePadState.ThumbSticks.Left.Y > 0)
            {
                playerShip.Up();
                keyPressed = true;

                if (animationResetSwitchU == 0)
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
            {}
             
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
                    animationResetSwitchL ++;
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
                    animationResetSwitchR ++;
                    playerShip.framesOverride = 6;
                    playerShip.TextureImage = playerRightTurn;
                }
                if (animationResetSwitchR == 1)
                {
                    if(playerShip.frameIndex > 6){animationResetSwitchR++;}
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
                    playerShip.TextureImage = playerTexture;
                }
                
            }
            // Primary Weapon
            if (keyState.IsKeyDown(Keys.Space)
              || gamePadState.IsButtonDown(Buttons.RightTrigger))
            {
                playerShip.shootPrimary(Content);

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

        protected override void Draw(GameTime gameTime)
        {
            if (gameState == GameState.Play)
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
                myBackground.Draw(spriteBatch);
                playerShip.Draw(4, 0.2f, 64, 70,spriteBatch, gameTime);
                follower.Draw(4, 0.1f, 32, 32, spriteBatch, gameTime);
                foreach (Enemy enemy in Enemywave)
                {
                    enemy.Draw(4, 0.2f, 64, 70,spriteBatch, gameTime);
                }
                spriteBatch.End();
                base.Draw(gameTime);
            }
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
            enemy = new Enemy(Content, GraphicsDevice, 6, "delta");
            WaveDef[0].Add(enemy);

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
    }
}
