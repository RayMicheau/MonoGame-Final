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
    class VoidAngelWeapon : EnemyWeapon
    {
        public VoidAngelWeapon(ContentManager content, Vector2 startPosition)
            : base(content.Load<Texture2D>("Images/Animations/VoidAngel-Shot"), startPosition, 50f)
        {
            shotTime = 0f;
            shotSpeed = 0.1f;
            shotFrameWidth = 50;
            shotFrameHeight = 50;
            shotRectangle = new Rectangle(0, 0, shotFrameWidth, shotFrameHeight);
            shotFrames = 8;
            velocitySpeed = 50f;
            velocity *= velocitySpeed;
            scale = 1f;
            damage = 500;
        }
    }
}
