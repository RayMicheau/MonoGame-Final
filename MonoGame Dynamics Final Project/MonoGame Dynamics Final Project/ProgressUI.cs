#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Dynamics_Final_Project.Weapons;
using MonoGame_Dynamics_Final_Project;
#endregion

namespace MonoGame_Dynamics_Final_Project
{
    class ProgressUI : Score
    {
        public string progressType;
        protected int orientation;
        protected float maxScale, minScale;
        public ProgressUI(Vector2 screenPosition, int progressScore, SpriteFont font, string ProgressType)
            :base(screenPosition,progressScore,font)
        {
            progressType = ProgressType;
            orientation = 1;
            size = font.MeasureString(progressType + scoreAmount);
            origin = size * 0.5f;
            if (progressType == "Wave:")
            {
                maxScale = 1;
                minScale = 0.5f;
            }
            else if (progressType == "Level:")
            {
                maxScale = 0.4f;
                minScale = 0.2f;
            }
        }
        public override void Update(GameTime gameTime)
        {
           
            elapsed += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
           

            if (scale > maxScale)
            {
                orientation *= -1;
            }
           
            else if (scale < minScale)
            {
                orientation = 1;
            }

            scale += (0.01f * orientation);

            if (elapsed > 3)
            {
                alive = false;
            }
        }
        public void Update(Vector2 playerPos, GameTime gameTime)
        {
            Update(gameTime);
            position = playerPos;
        }
        public override void Draw(SpriteBatch spriteBatch, SpriteFont menuFont)
        {
            if (alive)
            {
                spriteBatch.DrawString(menuFont, progressType + " " + scoreAmount.ToString(), position, Color.White, 0f, origin, scale, SpriteEffects.None, 1f);
            }
        }
    }
}
