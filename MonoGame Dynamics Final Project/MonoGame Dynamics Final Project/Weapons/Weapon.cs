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
using MonoGame_Dynamics_Final_Project;
#endregion

namespace MonoGame_Dynamics_Final_Project.Weapons
{
    class Weapon
    {
        #region Variables
        protected int ammo;
<<<<<<< Updated upstream
        public int Ammo
        {
            get { return ammo; }
            set { ammo = value; }
        }

=======
        public int Ammo 
        {
            get { return ammo; }
            set { ammo = value; }
        }
>>>>>>> Stashed changes
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

        // pixel collision
        public Color[] textureData;
        public bool collisionDetected;

        //Texture object and a collision rectangle
        public Texture2D TextureImage { get; set; }

        public virtual Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)(position.X - SpriteOrigin.X), (int)(position.Y - SpriteOrigin.Y),
                   Convert.ToInt32(TextureImage.Width), Convert.ToInt32(TextureImage.Height));
            }
        }
        #endregion

        public Weapon(Texture2D textureImage, Vector2 startPoisition, float velocity)
        {
            this.TextureImage = textureImage;
            this.position = startPoisition;
            this.velocity = new Vector2(0, velocity * -1);
            textureData = new Color[TextureImage.Width * TextureImage.Height];
            textureImage.GetData(textureData);
        }

        public virtual void Update(GameTime gameTime)
        {
            //Time between the frames
            float timeLapse = (float)(gameTime.ElapsedGameTime.TotalSeconds);

            //Move the sprite
            position += Velocity * timeLapse;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureImage,
                position,
                null,
                Microsoft.Xna.Framework.Color.White,
                0f,
                SpriteOrigin,
                1f, // scale
                SpriteEffects.None,
                0);
        }

        public bool CollisionSprite(Player sprite)
        {
            return CollisionRectangle.Intersects(sprite.CollisionRectangle);
        }

        // Pixel Collision Detection method
        public bool IntersectsPixel(Rectangle rect1, Color[] data1, Rectangle rect2, Color[] data2)
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