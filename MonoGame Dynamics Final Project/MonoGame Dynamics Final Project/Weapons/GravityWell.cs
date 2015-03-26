#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Dynamics_Final_Project;
using MonoGame_Dynamics_Final_Project.Weapons;
#endregion

namespace MonoGame_Dynamics_Final_Project
{
    class GravityWell : Weapon
    {
        protected Vector2 gravityForce;
        public Vector2 GravityForce
        {
            get { return gravityForce; }
            set { gravityForce = value; }
        }

        protected float mass;
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }

        private const float G = 500.0f;

        Vector2 difference;

        public GravityWell(Texture2D textureImage, Vector2 startPosition, float velocity) 
            :base(textureImage, startPosition, velocity, 2)
        {
            gravityForce = Vector2.Zero;
        }

        public void forcePull(GameTime gameTime, List<Enemy> enemies)
        {
            float timeInterval = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            float magnitude, radius;

            foreach (Enemy enemy in enemies)
            {
                difference += position - enemy.Position;
                difference.Normalize();
                gravityForce = difference;
                radius = difference.Length();
                magnitude = G * enemy.Mass * mass / (radius * radius);
                gravityForce *= magnitude;

                enemy.Acceleration = gravityForce / enemy.Mass;
                enemy.Velocity = enemy.InitialVelocity + enemy.Acceleration * timeInterval;
                enemy.Position += enemy.InitialVelocity * timeInterval + 0.5f * enemy.Acceleration * timeInterval * timeInterval;
                enemy.InitialVelocity = enemy.Velocity;
            }

        }
    }
}
