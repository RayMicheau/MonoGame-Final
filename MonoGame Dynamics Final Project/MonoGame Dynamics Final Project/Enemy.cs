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
#endregion

namespace MonoGame_Dynamics_Final_Project
{
    class Enemy : Player
    {
        /// Enemies
        /// Enemy logic goes in here
        /// Multiple types of enemies spawn, that logic is placed here 
        /// to control enemy movements
        /// 

        protected float mass;
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }

        public Enemy(ContentManager content, GraphicsDevice Device)
            : base(content.Load<Texture2D>("Images/Commandunit0"), new Vector2(500, 100), new Vector2(20, 20), true, 1.0f)
        {
            mass = 5f;
        }

    }
}