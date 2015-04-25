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
using MonoGame_Dynamics_Final_Project;
#endregion

namespace MonoGame_Dynamics_Final_Project.Sprites
{
    public enum PowerUps{
        Null,
        AtkSpdUp,
        MoveSpdUp,
        Shield
    }

    class PowerUp
    {
        float dTime;
        public bool removeFromScreen = false;
        public bool Alive { get; set; }
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

        public float Scale { get; set; }

        public bool setOrigin { get; set; }
        protected Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public virtual Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)(position.X - SpriteOrigin.X * Scale),((int)(position.Y - SpriteOrigin.Y * Scale)),
                   Convert.ToInt32(TextureImage.Width * Scale), Convert.ToInt32(TextureImage.Height * Scale));
            }
        }

        public Texture2D TextureImage { get; set; }

        public PowerUp(Texture2D textureImg, GraphicsDevice Device, 
            PowerUps PowerUp, Player player, Vector2 position, float scale)
        {
            Position = position;
            Alive = true;
            TextureImage = textureImg;
            SpriteOrigin = new Vector2(TextureImage.Width / 2, TextureImage.Height / 2);
            Scale = scale;
            switch (PowerUp)
            {
                case PowerUps.MoveSpdUp:
                    player.Velocity *= 1.5f;
                    break;

                case PowerUps.AtkSpdUp:
                    break;

                case PowerUps.Shield:
                    player.Health *= 2;
                    break;

                default:
                    break;
            }
        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            dTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds /1000.0f;
            position.Y += 200 * dTime;
            if (Position.Y >= graphicsDevice.Viewport.Height)
            {
                Alive = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            float timeLapse = (gameTime.ElapsedGameTime.Milliseconds/1000f);
            if (Alive)
            {
                spriteBatch.Draw(TextureImage, Position, null, Color.White, 0.0f, spriteOrigin, 1.0f, SpriteEffects.None, 0); 
            }
        }
        public bool CollisionSprite(Player player)
        {
            return CollisionRectangle.Intersects(player.CollisionRectangle);
        }
    }
}
