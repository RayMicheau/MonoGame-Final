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
#endregion

namespace MonoGame_Dynamics_Final_Project
{
    class Weapon : Player
    {
        public Weapon(Texture2D textureImage, Vector2 startPosition, float velocity)
            : base(textureImage, startPosition, new Vector2(0, velocity), true, 1.0f)
        {
        }
    }
}
