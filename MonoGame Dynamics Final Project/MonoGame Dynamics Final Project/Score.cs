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
using MonoGame_Dynamics_Final_Project.Weapons;
#endregion

namespace MonoGame_Dynamics_Final_Project
{
    class Score
    {
        int r, g, b;
        int r2, g2, b2;
        Random rng = new Random();
        protected Vector2 position;
        public Vector2 Position 
        {
            get { return position; }
            set { position = value; }
        }
        protected int scoreAmount;
        public int ScoreAmount
        {
            get { return scoreAmount; }
            set { scoreAmount = value; }
        }
        protected float scale;
        public float elapsed;
        public bool alive;
        public Color prevColor;
        public Color newColor;
        protected Vector2 size, origin;

        public Score(Vector2 enemyPosition, int enemyScore, SpriteFont font)
        {
            position = enemyPosition;
            scoreAmount = enemyScore;
            alive = true;
            prevColor = getColor();
            scale = 0.1f;
            size = font.MeasureString(scoreAmount.ToString());
            origin = size * 0.5f;
        }
        public Color getColor()
        {
            r = rng.Next(0, 255);
            g = rng.Next(0, 255);
            b = rng.Next(0, 255);

            return new Color(r, g, b);
        }

        public Color getNewColor()
        {
            r2 = rng.Next(0, 255);
            g2 = rng.Next(0, 255);
            b2 = rng.Next(0, 255);

            return new Color(r2, g2, b2);
        }

        public Color transitionColor(int red, int green, int blue)
        {
            if (red - r2 > 0)
                red-=5;
            else
                red+=5;

            if (green - g2 > 0)
                green-=5;
            else
                green+=5;

            if (blue - b2 > 0)
                blue-=5;
            else
                blue+=5;

            r = red;
            g = green;
            b = blue;

            return new Color(red, green, blue);
        }
        public void Update(GameTime gameTime)
        {
            float timeLapse = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            elapsed += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            if (elapsed > 0.3 || (r == r2 && g == g2 && b == b2))
            {
                elapsed = 0f;
                newColor = getNewColor();
                //alive = false;

               
            }
            else
            {
                prevColor = transitionColor(r, g, b);
            }
            scale += 0.1f;
            
            if (scale > 2f)
            {
                alive = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont menuFont)
        {
            if (alive)
            {
                spriteBatch.DrawString(menuFont, scoreAmount.ToString(), position, prevColor, 0f, origin, scale, SpriteEffects.None, 1f);
            }
        }

    }
}
