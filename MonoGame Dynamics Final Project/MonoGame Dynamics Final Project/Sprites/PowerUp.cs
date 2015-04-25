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
using MonoGame_Dynamics_Final_Project.Sprites;
using MonoGame_Dynamics_Final_Project;
#endregion

namespace MonoGame_Dynamics_Final_Project.Sprites
{
    public enum PowerUps{
        Null,
        AtkSpdUp,
        MoveSpdUp,
        Shield
    }

    class PowerUp : Enemy
    {
        float dTime;
        public bool removeFromScreen = false;
        PowerUps powerUps = PowerUps.Null;
        public PowerUp(int width, int height, Texture2D textureImg, GraphicsDevice Device, int spotInFormation, string formationType, float scale, float damage, float health, PowerUps PowerUp)
            : base(width, height, textureImg, Device, spotInFormation, formationType, scale, damage, health)
        {
            if(powerUps != PowerUps.Null){
                switch(powerUps){
                    case PowerUps.MoveSpdUp:
                        break;

                    case PowerUps.AtkSpdUp:
                        break;

                    case PowerUps.Shield:
                        break;

                    default:
                        break;
                }
            }
        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            dTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds /1000.0f;
            position.Y = Velocity.Y * dTime;
            if (Position.Y >= graphicsDevice.Viewport.Height)
            {
                removeFromScreen = true;
            }
        }
    }
}
