﻿#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace MonoGame_Dynamics_Final_Project
{
    class Player
    {
        
        //IDEAL CLASS SET UP
        //Set up all player variables to be used
        //Create Player constructor
        //An Update function for keeping track of gametime
        //Another update for checking screenwidths
        //And a draw method
        //As well as collision and movement methods.

        #region Variables

        protected Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        protected Vector2 spriteOrigin;
        public Vector2 SpriteOrigin
        {
            get { return spriteOrigin; }
            set { spriteOrigin = value; }
        }

        protected Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        protected Vector2 acceleration;
        public Vector2 Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }

        protected Vector2 initialVelocity;
        public Vector2 InitialVelocity
        {
            get { return initialVelocity; }
            set { initialVelocity = value; }
        }

        public bool SetOrigin { get; set; }
        public float Scale { get; set; }

        protected SpriteEffects Spriteeffect { get; set; }
        
        //public float mass;
        public bool Alive { get; set; }

        #endregion

        //Texture object and a collision rectangle
        public Texture2D TextureImage { get; set; }

        public virtual Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)(position.X - SpriteOrigin.X * Scale), (int)(position.Y - SpriteOrigin.Y * Scale),
                   Convert.ToInt32(TextureImage.Width * Scale), Convert.ToInt32(TextureImage.Height * Scale));
            }
        }

        public Player(Texture2D textureImage, Vector2 position, Vector2 velocity, bool setOrig, float scale)
        {
            Position = position;
            TextureImage = textureImage;
            InitialVelocity = velocity;
            Velocity = velocity;
            SetOrigin = setOrig;
            Acceleration = acceleration;
            if (SetOrigin)
            {
                SpriteOrigin = new Vector2(TextureImage.Width / 2, TextureImage.Height / 2);
            }
            Scale = scale;
            Alive = true;
        }

        //Update that doesn't keep sprites onscreen
        public virtual void Update(GameTime gameTime)
        {
            if (Alive)
            {
                //Time between the frames
                float timeLapse = (float)(gameTime.ElapsedGameTime.TotalSeconds);

                //Move the sprite
                position += Velocity * timeLapse;
            }
        }
        public virtual void Update(Player player, Vector2 offset, GameTime gameTime)
        {
            offset = player.position + offset;
            float timeLapse = (float)(gameTime.ElapsedGameTime.TotalSeconds);
            if (position !=  offset)
            {
                Vector2 vel = player.Position - position;
                vel.Normalize();
                position += vel * 250 * timeLapse;
            }
        }

        public virtual void Update(GameTime gameTime, GraphicsDevice Device)
        {
            if (Alive)
            {
                //Keep the sprite onscreen
                Update(gameTime);

                if (Position.X > Device.Viewport.Width + SpriteOrigin.X * Scale - SpriteOrigin.X)
                {
                    position.X = 0 + SpriteOrigin.X * Scale;
                }
                else if (Position.X < SpriteOrigin.X * Scale - spriteOrigin.X)
                {
                    position.X = Device.Viewport.Width - SpriteOrigin.X * Scale;
                }

                if (Position.Y >= Device.Viewport.Height - SpriteOrigin.Y * Scale)
                {
                    position.Y = Device.Viewport.Height - SpriteOrigin.Y * Scale;
                    if (velocity.Y >= 0)
                    {
                        velocity.Y *= -0.95f;
                    }
                }
                else if (Position.Y < SpriteOrigin.Y * Scale)
                { 
                   velocity.Y *= -0.15f;
                }
                /*
                 If the velocity is less than 1000, increase it.
                 */
                 

                Console.WriteLine("{0}, {1}, {2}", position.Y, velocity.Y, velocity.X);
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Alive)
            {
                spriteBatch.Draw(TextureImage, 
                    position, 
                    null, 
                    Microsoft.Xna.Framework.Color.White, 
                    0f, 
                    SpriteOrigin, 
                    Scale, 
                    Spriteeffect,
                    0);
            }
        }

        public bool CollisionSprite(Player sprite)
        {
            return CollisionRectangle.Intersects(sprite.CollisionRectangle);
        }

        public void Up()
        {
            velocity.Y -= InitialVelocity.Y;;
        }

        public void Down()
        {
            velocity.Y += InitialVelocity.Y;;
        }

        public virtual void Right() 
        {
            if (velocity.X <= 1000)
            {
                velocity.X += InitialVelocity.X;
            }
        }

        public virtual void Left()
        {
            if (velocity.X >= -1000)
            {
                velocity.X -= InitialVelocity.X;
            }
            
        }
        public virtual void Idle()
        {
            Velocity = Velocity * .98f;
        }
    }
}
