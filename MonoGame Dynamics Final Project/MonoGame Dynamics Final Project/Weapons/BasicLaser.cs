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
    class BasicLaser : Weapon
    {
        public BasicLaser(ContentManager content, Vector2 startPosition, float velocity)
            : base(content.Load<Texture2D>("Images/Animations/laser"), startPosition, velocity, 1)
        {
            
        }
    }
}
