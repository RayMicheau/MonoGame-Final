using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGame_Dynamics_Final_Project.Sprites;
using MonoGame_Dynamics_Final_Project.Weapons;


namespace MonoGame_Dynamics_Final_Project
{
    class animateSprite
    {
        int frameIndex = 1;
        int y = 0;
        int x = 0;
        float time;
        public int FrameWidth;
        public int FrameHeight;

        public Rectangle animatedSprite(int frames, float frameTime,int frameWidth, int frameHeight, Texture2D image, float timeLapse) 
        {
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;

   
            if (frameIndex == frames + 1)
            {
                frameIndex = 1;
                y = 0;
                x = 0;
            }
            // Calculate the source rectangle of the current frame.
            if (frameIndex == (image.Width/frameWidth)+1)
            {
                y ++;
                x = 0;
            }

            Rectangle source = new Rectangle(x * frameWidth, y* frameHeight, frameWidth, frameHeight);
            time += timeLapse;
            while (time > frameTime)
            {
                // Play the next frame in the SpriteSheet
                frameIndex++;
                x++;

                // reset elapsed time
                time = 0f;
            }
            return source;
        }

    }
}
