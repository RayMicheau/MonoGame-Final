#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGame_Dynamics_Final_Project;
#endregion

namespace MonoGame_Dynamics_Final_Project.Weapons
{
    class HelixMissile : Weapon
    {
        Vector2 endPosition;
        float time;

        public HelixMissile(Texture2D textureImage, Vector2 startPosition, float velocity)
            : base(textureImage, startPosition, velocity, 1)
        {
            endPosition = new Vector2(startPosition.X, startPosition.Y - 200);
            time = 0;
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            time += gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            position += velocity * time + 0.5f * acceleration * time * time;
        }
    }
}
