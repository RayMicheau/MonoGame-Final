#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
#endregion

namespace MonoGame_Dynamics_Final_Project
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Random random;

        // background
        Texture2D[] background; // Current Resolution 480w x 800h
        ScrollingBackground myBackground;

        // player
        Player playerShip;
        Texture2D basicShot;
        List<Shot> shots = new List<Shot>();

        Player follower;

        // enemies
        List<Enemy> Enemywave = new List<Enemy>();

       // Vector2 gravityForce = new Vector2(0.0f, 150.0f);
        Vector2 offset = new Vector2(500, 500);
        

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
          //  graphics.PreferredBackBufferWidth = 1280;
          //  graphics.PreferredBackBufferHeight = 720;
          //  graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // background
            myBackground = new ScrollingBackground();
            background = new Texture2D[3];
            for (int i = 0; i < background.Length; i++)
            {
                background[i] = Content.Load<Texture2D>("Images/Backgrounds/universe0" + (i + 1).ToString());
            }
            myBackground.Load(GraphicsDevice, background, background.Length, 0.5f); // change float to change animation speed

            // weapons
            basicShot = Content.Load<Texture2D>("Images/Animations/laser");

            // player sprites
            playerShip = new Player(Content.Load<Texture2D>("Images/Commandunit0"),
                new Vector2(100,100),
                new Vector2(10,10),
                true,
                1.0f);

            follower = new Player(Content.Load<Texture2D>("Images/Animations/synth-unit-move0"),
                new Vector2(100, 100),
                new Vector2(10, 10),
                true,
                1.0f);

            // enemy sprites
            for(int i = 0; i < 1; i++)
            {
                Enemy enemy = new Enemy(Content, GraphicsDevice);
                Enemywave.Add(enemy);
            }
            

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // updating scroll speed
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            myBackground.Update(gameTime, elapsed * 100);

            UpdateInput();

            playerShip.Update(gameTime, GraphicsDevice);
            follower.Update(playerShip, offset, gameTime);

            for (int i = 0; i < shots.Count; i++)
            {
                shots[i].Update(gameTime);
            }

            // tests for collision of shots against enemy
            for (int i = 0; i < Enemywave.Count; i++)
            {
                int collide = Enemywave[i].CollisionShot(shots);
                if (collide != -1)
                {
                    shots.RemoveAt(collide);
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

            // TODO: Add your update logic here
            base.Update(gameTime);
        }
        private void UpdateInput()
        {
            //set keyPressed to false to start.
            //if it's still false after all keys have been tested, 
            //nothing relevant has been pressed
            //and we should idle
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
            }
            if (keyState.IsKeyDown(Keys.Down)
              || keyState.IsKeyDown(Keys.S)
              || gamePadState.DPad.Down == ButtonState.Pressed
              || gamePadState.ThumbSticks.Left.Y < -0.5f)
            {
                playerShip.Down();
                keyPressed = true;
            }
            if (keyState.IsKeyDown(Keys.Left)
              || keyState.IsKeyDown(Keys.A)
              || gamePadState.DPad.Left == ButtonState.Pressed
              || gamePadState.ThumbSticks.Left.X < -0.5f)
            {
                playerShip.Left();
                keyPressed = true;
            }
            if (keyState.IsKeyDown(Keys.Right)
              || keyState.IsKeyDown(Keys.D)
              || gamePadState.DPad.Right == ButtonState.Pressed
              || gamePadState.ThumbSticks.Left.X > 0.5f)
            {
                playerShip.Right();
                keyPressed = true;
            }
            if (keyState.IsKeyDown(Keys.Space))
            {
                Shot shot = new Shot(basicShot, new Vector2(playerShip.Position.X, playerShip.Position.Y - playerShip.SpriteOrigin.Y), -600);
                shots.Add(shot);
            }
            if (!keyPressed)
            {
                playerShip.Idle();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();

            myBackground.Draw(spriteBatch);
            playerShip.Draw(spriteBatch);
            follower.Draw(spriteBatch);

            foreach (Enemy enemy in Enemywave)
            {
                enemy.Draw(spriteBatch);
            }

            foreach(Shot shot in shots)
            {
                shot.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
