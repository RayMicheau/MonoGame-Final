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

namespace MonoGame_Dynamics_Final_Project.Sprites
{
    class RailTurret : Player
    {
        #region variables
        protected SpriteEffects spriteEffect;
        public SpriteEffects SpriteEffect
        {
            get { return spriteEffect; }
            set { spriteEffect = value; }
        }

        protected float angle;
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        // rail
        protected float turretTime, turretSpeed;
        public Vector2 turretPosition, turretOrigin;
        protected int turretFrameWidth, turretFrameHeight, turretFrames;
        #endregion
    
        public RailTurret(ContentManager content, Vector2 position, Vector2 velocity, int orientation)
            :base(32, 64, content.Load<Texture2D>("Images/Animations/Plasma-Repeater"), position, velocity, false, 1f, 100, 100)
        {
            turretTime = 0f;
            turretSpeed = 0.1f;
            turretFrameWidth = 32;
            turretFrameHeight = 64;
            turretFrames = 6;
            source = new Rectangle(0, 0, turretFrameWidth, turretFrameHeight);
            turretOrigin = new Vector2(turretFrameWidth / 2, turretFrameHeight / 2);

            if (orientation == -1)
            {
                spriteEffect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                spriteEffect = SpriteEffects.None;
            }
        }

        public override void Update(GameTime gameTime, Vector2 playerPosition)
        {
          //  base.Update(gameTime, Device, enemyWave);
            float timeLapse = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            source = animatedSprite(turretFrames, turretSpeed, turretFrameWidth, turretFrameHeight, TextureImage, timeLapse);
            turretPosition = playerPosition;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //base.Draw(spriteBatch, gameTime);

            spriteBatch.Draw(TextureImage,
                             position,
                             source,
                             Color.White,
                             angle,
                             spriteOrigin,
                             1f,
                             SpriteEffect,
                             1f);
        }
    }
}
