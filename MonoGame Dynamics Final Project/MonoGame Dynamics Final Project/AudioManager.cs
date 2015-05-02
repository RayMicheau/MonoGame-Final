using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace MonoGame_Dynamics_Final_Project
{
    class AudioManager
    {
        /// <summary>
        /// Sound effects to load in init
        /// </summary>
        Song menuSong;
        Song gameplaySong;
        SoundEffect enemyDeath;
        SoundEffect enemyDeathTwo;
        SoundEffect gravityWell;
        SoundEffect powerUpSpawn;
        SoundEffect powerUpGet;
        SoundEffect playerDeath;
        SoundEffect thrust;
        SoundEffect rocket;
        SoundEffect hit;
        SoundEffect shot;
        SoundEffectInstance thruster;
        SoundEffectInstance laser;
        public void Initialize(ContentManager Content)
        {

            shot = Content.Load<SoundEffect>("Audio Files/Sound Effects/basic laser");
            enemyDeath = Content.Load<SoundEffect>("Audio Files/Sound Effects/enemy dead");
            enemyDeathTwo = Content.Load<SoundEffect>("Audio Files/Sound Effects/Enemy dead2");
            gravityWell = Content.Load<SoundEffect>("Audio Files/Sound Effects/gravity well");
            powerUpSpawn = Content.Load<SoundEffect>("Audio Files/Sound Effects/powerup 2");
            powerUpGet = Content.Load<SoundEffect>("Audio Files/Sound Effects/powerup_gained something");
            playerDeath = Content.Load<SoundEffect>("Audio Files/Sound Effects/player dead");
            thrust = Content.Load<SoundEffect>("Audio Files/Sound Effects/thrust");
            rocket = Content.Load<SoundEffect>("Audio Files/Sound Effects/rocket shot");
            hit = Content.Load<SoundEffect>("Audio Files/Sound Effects/hit noise");
            gameplaySong = Content.Load<Song>("Audio Files/Songs/Catalysm Song");
            menuSong = Content.Load<Song>("Audio Files/Songs/menu song");
            
            laser = shot.CreateInstance();
            laser.IsLooped = false;
            thruster = thrust.CreateInstance();
            thruster.IsLooped = false;
        }

        public void Play(string songName)
        {
            //will fail silently, and play nothing
            if (songName == "" || songName == null)
                return;

            if(songName == menuSong.Name)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(menuSong);
            }
            else if (songName == gameplaySong.Name)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(gameplaySong);
            }
        }

        //Not the most elegant solution, but trying to get more into it.
        //Easy enough to add sounds to this
        public void PlaySoundEffect(string effectName)
        {
            //will fail silently and play nothing
            if (effectName == null || effectName == "")
                return;

            switch (effectName)
            {
                case "gravity well":
                    gravityWell.Play(0.1f, 0.0f, 0.0f);
                    break;

                case "enemy dead":
                    enemyDeath.Play(0.1f, 0.0f, 0.0f);
                    break;

                case "enemy dead2":
                    enemyDeathTwo.Play(0.1f, 0.0f, 0.0f);
                    break;

                case "thrust":
                    thrust.Play(0.1f, 0.0f, 0.0f);
                    break;

                case "rocket":
                    rocket.Play(0.1f, 0.0f, 0.0f);
                    break;

                case "Spawn pUp":
                    powerUpSpawn.Play(0.1f, 0.0f, 0.0f);
                    break;

                case "Get pUp":
                    powerUpGet.Play(0.1f, 0.0f, 0.0f);
                    break;

                case "hit":
                    hit.Play(0.1f, 0.0f, 0.0f);
                    break;

                case "shot":
                    shot.Play(0.1f, 0.0f, 0.0f);
                    break;
                default: break;
            }
        }

        public static void Stop()
        {
            MediaPlayer.Stop();
        }

        public void StopThrust()
        {
            thruster.Stop(true);
        }
        public void StopLaser()
        {
            laser.Stop(true);
        }

        public void setLaserLooping(bool set)
        {
            if (set)
            {
                laser.IsLooped = true;

            }
            else if (!set)
            {
                laser.IsLooped = false;
            }
        }

        public bool isLaserLooping()
        {
            if (laser.IsLooped)
            {
                return true;
            }
            if (!laser.IsLooped)
            {
                return false;
            }
            else { return false; }
        }

        public void setThrustLooping(bool set)
        {
            if (set)
            {
                thruster.IsLooped = true;
                
            }
            else if (!set)
            {
                thruster.IsLooped = false;
            }
        }

        public bool isThrustLooping()
        {
            if (thruster.IsLooped)
            {
                return true;
            }
            if (!thruster.IsLooped)
            {
                return false;
            }
            else { return false; }
        }
    }
}
