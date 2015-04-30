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
        private int horTiles;
        private int verTiles;
        private bool reset = false;
        private float DeltaY = 0;
        #endregion

        public void Load(int virtualWidth, int virtualHeight, Texture2D[] backgroundTexture, int frames, float timeBetweenFrames)
        {
            textureArray = new Texture2D[frames];

            for (int i = 0; i < frames; i++)
            {
                textureArray[i] = backgroundTexture[i];
            }

            currentFrame = 0;
            mytexture = backgroundTexture[currentFrame];
            this.timeBetweenFrames = timeBetweenFrames;
            screenheight = virtualHeight;
            screenwidth = virtualWidth;
            origin = new Vector2(0, 0);
            horTiles = virtualWidth / mytexture.Width;
            verTiles = virtualHeight / mytexture.Height;
        }

        // ScrollingBackground.Update
        public void Update(GameTime gameTime, float deltaY)
        {
            DeltaY = deltaY;
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
            if (reset == true)
            {
                screenpos.Y = 0;
                reset = false;
            }

            screenpos.Y += deltaY;
            //screenpos.Y = screenpos.Y % mytexture.Height;
        }

        // ScrollingBackground.Draw
        public void Draw(SpriteBatch batch)
        {
            // Draw the texture, if it is still onscreen.
            if (screenpos.Y< screenheight)
            {
                for (int i = 0; i <= verTiles+1; i++)
                {
                    if (DeltaY > 0)
                    {
                        for (int f = 0; f <= horTiles; f++)
                        {
                            batch.Draw(mytexture, screenpos + new Vector2(mytexture.Width * f, -mytexture.Height + mytexture.Height * i), null,
                                 Color.White, 0, origin, 1, SpriteEffects.None, 0f);
                        }
                        if (i == 0 && screenpos.Y - mytexture.Height >= 0) { reset = true; }
                    }
                    else if (DeltaY < 0)
                    {
                        for (int f = 0; f <= horTiles; f++)
                        {
                            batch.Draw(mytexture, screenpos + new Vector2(mytexture.Width * f, mytexture.Height * i), null,
                                 Color.White, 0, origin, 1, SpriteEffects.None, 0f);
                        }
                        if (i == verTiles+1 && screenpos.Y + mytexture.Height*i <= screenheight) { reset = true; }
                    }
                }
            }
            // Draw the texture a second time, behind the first, to create the scrolling illusion.
            batch.Draw(mytexture, screenpos - texturesize, null,
                 Color.White, 0, origin, 1, SpriteEffects.None, 0f);
        }
    }
}
