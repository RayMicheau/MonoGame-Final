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

        Texture2D background; // Current Resolution 800h x 480w
        ScrollingBackground myBackground;

        Player playerShip;
        Player follower;
        Player collisionTest;

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

            myBackground = new ScrollingBackground();
            background = Content.Load<Texture2D>("Images/Animations/universe");
            myBackground.Load(GraphicsDevice, background);

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

            collisionTest = new Player(Content.Load<Texture2D>("Images/Commandunit0"),
                new Vector2(500, 100),
                new Vector2(20, 20),
                true,
                1.0f);

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
            myBackground.Update(elapsed * 100);

            UpdateInput();

            playerShip.Update(gameTime, GraphicsDevice);
            follower.Update(playerShip, offset, gameTime);

            // testing collision with other player sprite
            if (playerShip.CollisionSprite(collisionTest))
            {
                if (IntersectsPixel(playerShip.CollisionRectangle, playerShip.textureData, collisionTest.CollisionRectangle, collisionTest.textureData))
                {
                    playerShip.collisionDetected = true;
                    collisionTest.Alive = false;
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
            if (!keyPressed)
            {
                playerShip.Idle();
            }
        }

        private void AnimateBackground(Rectangle rect)
        {

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
            collisionTest.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        // Pixel Collision Detection method
        static bool IntersectsPixel(Rectangle rect1, Color[] data1, Rectangle rect2, Color[] data2)
        {
            bool collision = false;
            int top = Math.Max(rect1.Top, rect2.Top);
            int bottom = Math.Min(rect1.Bottom, rect2.Bottom);
            int left = Math.Max(rect1.Left, rect2.Left);
            int right = Math.Min(rect1.Right, rect2.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color color1 = data1[(x - rect1.Left) + (y - rect1.Top) * rect1.Width];
                    Color color2 = data2[(x - rect2.Left) + (y - rect2.Top) * rect2.Width];

                    if (color1.A != 0 && color2.A != 0) // if both colors aren't transparent, collision detected
                    {
                        return true;
                    }
                }
            }

            return collision;
        }
    }
}
