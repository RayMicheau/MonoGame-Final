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

        public HelixMissile(Texture2D textureImage, Vector2 startPosition, float velocity)
            : base(textureImage, startPosition, velocity, 1)
        {
            offsetX = 100f;
            acceleration = new Vector2(-100, 0);
            base.velocity = new Vector2(25f, -velocity);
            time = 0;
        }

        public override void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            float timeStep = time;

            if (position.X > offsetX && timeStep == 1)
            {
                velocity.X *= -1;
                acceleration.X *= -1;
                timeStep = 0f;
            }

            position += velocity * time + 0.5f * acceleration * time * time;

            if (position.Y + TextureImage.Height < 0)
            {
                offScreen = true;
            }
        }
    }
}
