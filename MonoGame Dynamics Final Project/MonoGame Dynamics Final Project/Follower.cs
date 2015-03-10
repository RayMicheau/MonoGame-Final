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

namespace MonoGame_Dynamics_Final_Project
{
    class Follower
    {
        /// <Follower AI>
        /// What needs to go here is any follower AI
        /// that relates to moving it in relation to the player.
        /// <Follower AI>

        #region Variables

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

        protected Vector2 initialVelocity;
        public Vector2 InitialVelocity
        {
            get { return initialVelocity; }
            set { initialVelocity = value; }
        }

        public bool SetOrigin { get; set; }
        public float Scale { get; set; }

        protected SpriteEffects Spriteeffect { get; set; }

        //public float mass;
        public bool Alive { get; set; }

        public Texture2D TextureImage { get; set; }

        #endregion


        public Follower(Vector2 playerPosition, Vector2 position, Vector2 offset, Texture2D sprite, Vector2 velocity, float scale, bool setOrigin)
        {
            TextureImage = sprite;
            Position = position;
            Velocity = velocity;
            Scale = scale;
            if (setOrigin)
            {
                SpriteOrigin = new Vector2(TextureImage.Width / 2, TextureImage.Height / 2);
            }
            Alive = true;
        }
    }
}
