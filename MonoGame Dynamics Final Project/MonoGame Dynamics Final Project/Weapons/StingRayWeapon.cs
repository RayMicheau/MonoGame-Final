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
using MonoGame_Dynamics_Final_Project;
using MonoGame_Dynamics_Final_Project.Sprites;
#endregion

namespace MonoGame_Dynamics_Final_Project.Weapons
{
    class StingRayWeapon : EnemyWeapon
    {
        public StingRayWeapon(ContentManager content, Vector2 startPosition)
            :base(content.Load<Texture2D>("Images/Animations/Sting-Ray-shot"),startPosition, 1f)
        {
            shotTime = 0f;
            shotSpeed = 0.1f;
            shotFrameWidth = 10;
            shotFrameHeight = 11;
            shotRectangle = new Rectangle(0, 0, shotFrameWidth, shotFrameHeight);
            shotFrames = 3;
            velocitySpeed = 25f;
            velocity = new Vector2(0, 10) * velocitySpeed;
            scale = 2;
        }



    }
}
