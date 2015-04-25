﻿#region Using Statements
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

namespace MonoGame_Dynamics_Final_Project.Sprites
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

        protected float tileWidth;
        protected float tileHeight;
        protected float windowHeight;

        protected Vector2 distanceBetween;

        public Enemy(int width, int height, Texture2D textureImage, GraphicsDevice Device, int spotinFormation, string formationType, float scale, float damage, float health)
            : base(width,height, textureImage, new Vector2(0, 0), new Vector2(0, 0), true, scale, damage, health)
        {
            mass = 5f;

            tileWidth = Device.Adapter.CurrentDisplayMode.Width / 7f;
            tileHeight = Device.Adapter.CurrentDisplayMode.Height / 5f;
            windowHeight = Device.Adapter.CurrentDisplayMode.Height;

            setEnemy(spotinFormation, formationType);
        }

        public void ChasePlayer(Vector2 playerPos)
        {
            distanceBetween = playerPos - position;
            distanceBetween.Normalize();
            rotation = (float)Math.Atan2(distanceBetween.Y, distanceBetween.X) - MathHelper.PiOver2;
            position += distanceBetween * 1.0f; // set speed here
        }

        // This method sets the enemy position depending on it's spot in the formation            
        private void setEnemy(int spotinFormation, string formationType)
        {        
            switch(formationType)
            {
                case "delta": // formation size: 10
                    switch(spotinFormation)
                    {
                        case 1:
                            position = getGridPos(4, 5);
                            break;
                        case 2:
                            position = getGridPos(3, 4);
                            break;
                        case 3:
                            position = getGridPos(5, 4);
                            break;
                        case 4:
                            position = getGridPos(2, 3);
                            break;
                        case 5:
                            position = getGridPos(4, 3);
                            break;
                        case 6:
                            position = getGridPos(6, 3);
                            break;
                        case 7:
                            position = getGridPos(1, 2);
                            break;
                        case 8:
                            position = getGridPos(3, 2);
                            break;
                        case 9:
                            position = getGridPos(5, 2);
                            break;
                        case 10:
                            position = getGridPos(7, 2);
                            break;
                        default:
                            Console.WriteLine("Could not set position {0} in {1} formation", spotinFormation, formationType);
                            break;
                    }
                    break;

                case "v": // formation size: 7
                    switch(spotinFormation)
                    {
                        case 1:
                            position = getGridPos(4, 5);
                            break;
                        case 2:
                            position = getGridPos(3, 4);
                            break;
                        case 3:
                            position = getGridPos(5, 4);
                            break;
                        case 4:
                            position = getGridPos(2, 3);
                            break;
                        case 5:
                            position = getGridPos(6, 3);
                            break;
                        case 6:
                            position = getGridPos(1, 2);
                            break;
                        case 7:
                            position = getGridPos(7, 2);
                            break;
                        default:
                            Console.WriteLine("Could not set position {0} in {1} formation", spotinFormation, formationType);
                            break;
                    }
                    break;

                case "line": // formation size: 5
                    switch (spotinFormation)
                    {
                        case 1:
                            position = getGridPos(2, 5);
                            break;
                        case 2:
                            position = getGridPos(3, 5);
                            break;
                        case 3:
                            position = getGridPos(4, 5);
                            break;
                        case 4:
                            position = getGridPos(5, 5);
                            break;
                        case 5:
                            position = getGridPos(6, 5);
                            break;
                        default:
                            Console.WriteLine("Could not set position {0} in {1} formation", spotinFormation, formationType);
                            break;
                    }
                    break;

                case "diamond": // formation size: 9
                    switch (spotinFormation)
                    {
                        case 1:
                            position = getGridPos(4, 5);
                            break;
                        case 2:
                            position = getGridPos(3, 4);
                            break;
                        case 3:
                            position = getGridPos(5, 4);
                            break;
                        case 4:
                            position = getGridPos(2, 3);
                            break;
                        case 5:
                            position = getGridPos(4, 3);
                            break;
                        case 6:
                            position = getGridPos(6, 3);
                            break;
                        case 7:
                            position = getGridPos(3, 2);
                            break;
                        case 8:
                            position = getGridPos(5, 2);
                            break;
                        case 9:
                            position = getGridPos(4, 1);
                            break;
                        default:
                            Console.WriteLine("Could not set position {0} in {1} formation", spotinFormation, formationType);
                            break;
                    }
                    break;

                case "shockwave": // formation size: 9
                    switch (spotinFormation)
                    {
                        case 1:
                            position = getGridPos(4, 5);
                            break;
                        case 2:
                            position = getGridPos(3, 4);
                            break;
                        case 3:
                            position = getGridPos(5, 4);
                            break;
                        case 4:
                            position = getGridPos(2, 3);
                            break;
                        case 5:
                            position = getGridPos(4, 3);
                            break;
                        case 6:
                            position = getGridPos(6, 3);
                            break;
                        case 7:
                            position = getGridPos(4, 2);
                            break;
                        case 8:
                            position = getGridPos(3, 1);
                            break;
                        case 9:
                            position = getGridPos(5, 1);
                            break;
                        default:
                            Console.WriteLine("Could not set position {0} in {1} formation", spotinFormation, formationType);
                            break;
                    }
                    break;

                default:
                    Console.WriteLine("Could not load formation type");
                    break;
            }
        }

        // grid helper method
        private Vector2 getGridPos(int x, int y)
        {
            return new Vector2(x * tileWidth, (y * tileHeight) /*- windowHeight*/);
        }
    }
}