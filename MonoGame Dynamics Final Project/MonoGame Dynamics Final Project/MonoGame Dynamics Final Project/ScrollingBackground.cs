using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_Dynamics_Final_Project
{
    public class ScrollingBackground
    {
        #region Class Variables
        private Vector2 screenpos, origin, texturesize;
        private Texture2D[] textureArray;
        private Texture2D mytexture;
        private int screenheight, screenwidth;
        private float timeBetweenFrames { get; set; }
        private float timeSinceLastFrame;
        private int currentFrame;
        #endregion

        public void Load(GraphicsDevice device, Texture2D[] backgroundTexture, int frames, float timeBetweenFrames, int posFlag)
        {
            textureArray = new Texture2D[frames];

            for (int i = 0; i < frames; i++)
            {
                textureArray[i] = backgroundTexture[i];
            }

            currentFrame = 0;
            mytexture = backgroundTexture[currentFrame];
            this.timeBetweenFrames = timeBetweenFrames;
            screenheight = device.Viewport.Height;
            screenwidth = device.Viewport.Width;

            // Set the origin so that we're drawing from the center of the top edge.
            origin = new Vector2(mytexture.Width / 2, 0);

            switch (posFlag)
            {
                case 1:
                    // Set the screen position to the left of the screen.
                    screenpos = new Vector2(screenwidth / 4, screenheight / 1.3f);
                    break;
                case 2:
                    // Set the screen position to the right of the screen.
                    screenpos = new Vector2(screenwidth / 1.3f, screenheight / 1.3f);
                    break;

                default: break;
            }
        }

        // ScrollingBackground.Update
        public void Update(GameTime gameTime, float deltaY)
        {
            // animation
            timeSinceLastFrame += (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            if (timeSinceLastFrame >= timeBetweenFrames)
            {
                timeSinceLastFrame = 0f;

                if (currentFrame < textureArray.Length - 1)
                    currentFrame++;
                else
                    currentFrame = 0;

                mytexture = textureArray[currentFrame];
            }

            // scrolling
            screenpos.Y += deltaY;
            screenpos.Y = screenpos.Y % mytexture.Height;
        }

        // ScrollingBackground.Draw
        public void Draw(SpriteBatch batch)
        {
            // Draw the texture, if it is still onscreen.
            if (screenpos.Y < screenheight)
            {
                batch.Draw(mytexture, screenpos, null,
                     Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            }
            // Draw the texture a second time, behind the first, to create the scrolling illusion.
            batch.Draw(mytexture, screenpos - texturesize, null,
                 Color.White, 0, origin, 1, SpriteEffects.None, 0f);
        }
    }
}
