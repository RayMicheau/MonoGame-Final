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
        float time;
        float offsetX;
        float velocitySpeed;
        Vector2 startPosition;
        int orientation; // -1 left, 1 right

        public HelixMissile(Texture2D textureImage, Vector2 startPosition, float velocity, int orientation)
            : base(textureImage, startPosition, velocity, 1)
        {
            velocitySpeed = velocity;
            this.startPosition = startPosition;
            time = 0;
            this.orientation = orientation;
            acceleration = new Vector2(-100 * orientation, 0);
            base.Velocity = new Vector2(30f * orientation, -velocity);
            offsetX = 100f;
            spriteOrigin = new Vector2(0f, textureImage.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            float timeStep = time;

            if (position.X > offsetX && timeStep == 1)
            {
                velocity.X *= -1f;
                acceleration.X *= -1f;
            }

            angle = getAngle(position);

            position += velocity * time + 0.5f * acceleration * time * time;

            if (position.Y + TextureImage.Height < 0)
            {
                offScreen = true;
            }
        }

        public float getAngle(Vector2 pos)
        {
            float angle = (float)Math.Atan2(pos.Y, pos.X) * 20f; // adjust rotation here
            return angle;
        }
    }
}
