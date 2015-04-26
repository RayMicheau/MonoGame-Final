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
#endregion

namespace MonoGame_Dynamics_Final_Project.Weapons
{
    class HelixMissile : Weapon
    {
        protected float time;
        protected float offsetX;
        protected float velocitySpeed;
        protected Vector2 startPosition;
        protected float radius, angle, theta;
        protected Texture2D textureImage;
        protected int orientation; // -1 left, 1 right

        public HelixMissile(ContentManager content, Vector2 startPosition, float velocity, int orientation)
            : base(content.Load<Texture2D>("Images/Animations/rocket"), startPosition, velocity, 2)
        {
            textureImage = content.Load<Texture2D>("Images/Animations/rocket");
            velocitySpeed = velocity;
            radius = 10f;
            this.startPosition = startPosition;
            time = 0;
            angle = 0;
            theta = 0;
            this.orientation = orientation;
            offsetX = 100f;
            spriteOrigin = new Vector2(0f, textureImage.Height / 2);
        }

        // Update method that curves the missile
        public override void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            float timeStep = time;

            position -= new Vector2((float)Math.Cos(theta) * orientation, (float)Math.Sin(theta)) * radius;

            position.Y -= velocitySpeed;


            if (position.Y + TextureImage.Height < 0)
            {
                offScreen = true;
            }

            angle += 5;

            if (angle > 180)
            {
                angle = 0;
                orientation *= -1;
            }

            theta = getAngle(angle);

            base.angle = -MathHelper.PiOver2 * orientation + (theta * orientation);
        }

        public float getAngle(float angle)
        {
            return angle * MathHelper.Pi / 180.0f;
        }
    }
}
