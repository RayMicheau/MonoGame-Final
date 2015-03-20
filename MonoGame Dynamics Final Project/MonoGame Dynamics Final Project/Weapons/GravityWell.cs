﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Dynamics_Final_Project;
using MonoGame_Dynamics_Final_Project.Weapons;

namespace MonoGame_Dynamics_Final_Project
{
    class GravityWell : Weapon
    {
        public GravityWell(Texture2D textureImage, Vector2 startPosition, float velocity) 
            :base(textureImage, startPosition, velocity)
        {

            ammo = 1;

        }
    }
}