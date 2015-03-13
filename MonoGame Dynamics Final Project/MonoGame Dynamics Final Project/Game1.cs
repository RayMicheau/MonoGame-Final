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
        Texture2D basicWeapon;
        List<Weapon> weapon = new List<Weapon>();

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

        protected override void Initialize()
        {
            base.Initialize();
        }

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
            basicWeapon = Content.Load<Texture2D>("Images/Animations/laser");

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

            UpdateInput();

            playerShip.Update(gameTime, GraphicsDevice);
            follower.Update(playerShip, offset, gameTime);

            for (int i = 0; i < weapon.Count; i++)
            {
                weapon[i].Update(gameTime);
            }

            // tests for collision of shots against enemy
            for (int i = 0; i < Enemywave.Count; i++)
            {
                int collide = Enemywave[i].CollisionShot(weapon);
                if (collide != -1)
                {
                    weapon.RemoveAt(collide);
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

            base.Update(gameTime);
        }
        private void UpdateInput()
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
                Weapon shot = new Weapon(basicWeapon, new Vector2(playerShip.Position.X, playerShip.Position.Y - playerShip.SpriteOrigin.Y), -600);
                weapon.Add(shot);
            }
            if (!keyPressed)
            {
                playerShip.Idle();
            }
        }

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

            foreach(Weapon shot in weapon)
            {
                shot.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
