using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_Dynamics_Final_Project.Sprites
{
    class Follower : Player
    {
        /// <Follower AI>
        /// What needs to go here is any follower AI
        /// that relates to moving it in relation to the player.
        /// <Follower AI>

        Vector2 Offset;
        Vector2 readyCounter = new Vector2(0, 0);
        Vector2 directionVector = new Vector2(0, 0);
        bool masterSwitch = true;

        public Follower(int frameWidth, int frameHeight, ContentManager content, Player player, Vector2 offset, float scale, bool setOrigin)
            : base(frameWidth, frameHeight, content.Load<Texture2D>("Images/Animations/synth-unit"), player.Position + offset, player.Velocity / 2, setOrigin, 1.0f, 0.0f, 1000.0f)
        {
            frameNum = 4;
            frameTime = 0.1f;

            Offset = offset;
        }
        /*TextureImage = content.Load<Texture2D>("Images/Animations/synth-unit");
        Position = position;
        Scale = scale;
        if(setOrigin)
        {
            SpriteOrigin = new Vector2(TextureImage.Width / 2, TextureImage.Height / 2);
        }
        Alive = true;*/
        // Follower Update. Confirmed. 
        public override void Update(Player player, GameTime gameTime)
        {
            float timeLapse = (float)(gameTime.ElapsedGameTime.TotalSeconds);

            directionVector = Position - (player.Position + Offset);
            float vectorMagnitude = Convert.ToSingle(Math.Sqrt(Math.Pow(directionVector.X, 2) + Math.Pow(directionVector.Y, 2)));
            directionVector /= vectorMagnitude;

            float velocityMagnitude = Convert.ToSingle(Math.Sqrt(Math.Pow(player.Velocity.X, 2) + Math.Pow(player.Velocity.Y, 2)));
            if (player.isMoving && vectorMagnitude > 2)
            {

                if (readyCounter.X < 10)
                {
                    readyCounter += new Vector2(0.5f, 0.5f);
                    readyCounter += new Vector2(-20.0f, -20.0f) / velocityMagnitude;
                }
            }
            Position = player.Position + Offset + directionVector * readyCounter;

            if (!player.isMoving && readyCounter.X > 0)
            {

                readyCounter += new Vector2(-1f, -1f);
            }

        }

    }
}
