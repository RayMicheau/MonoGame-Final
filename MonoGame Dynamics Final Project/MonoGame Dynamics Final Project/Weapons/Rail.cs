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
using MonoGame_Dynamics_Final_Project;
using MonoGame_Dynamics_Final_Project.Sprites;
#endregion

namespace MonoGame_Dynamics_Final_Project.Weapons
{
    class Rail : Weapon
    {
        protected int shotFrameIndex = 1;
        protected int shotX = 0;
        protected float shotTime, shotSpeed; 
        protected int shotFrameWidth, shotFrameHeight, shotFrames;
        protected Rectangle shotRectangle; 
        protected float scale;

        public Rail(ContentManager content, Vector2 startPosition, float velocity, int orientation) // 1 right, -1 left
            : base(content.Load<Texture2D>("Images/Animations/Plasma-Repeater-Shot"), startPosition, 1200.0f, 1)
        {
            shotTime = 0f; 
            shotSpeed = 0.1f;
            shotFrameWidth = shotFrameHeight = 20;
            shotRectangle = new Rectangle(0, 0, shotFrameWidth, shotFrameHeight);
            shotFrames = 4;
            angle = 0;

            if(orientation == -1)
            {
                spriteEffect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                spriteEffect = SpriteEffects.None;
            }
            scale = 2f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float elapsedTime = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;          
            shotRectangle = animatedSprite(shotFrames, shotSpeed, shotFrameWidth, shotFrameHeight, TextureImage, elapsedTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureImage,
                             position,
                             shotRectangle,
                             Color.White,
                             angle,
                             spriteOrigin,
                             scale,
                             spriteEffect,
                             1f);
        }

        public Rectangle animatedSprite(int frames, float frameTime, int frameWidth, int frameHeight, Texture2D image, float timeLapse)
        {
            if (shotFrameIndex == frames + 1)
            {
                shotFrameIndex = 1;
                shotX = 0;
            }
            // Calculate the source rectangle of the current frame.
            if (shotFrameIndex == (image.Width / frameWidth) + 1)
            {
                shotX = 0;
            }

            Rectangle source = new Rectangle(shotX * frameWidth, 0 , frameWidth, frameHeight);

            shotTime += timeLapse;
            while (shotTime > frameTime)
            {
                // Play the next frame in the SpriteSheet
                shotFrameIndex++;
                shotX++;

                // reset elapsed time
                shotTime = 0f;
            }
            return source;
        }
    }
}
