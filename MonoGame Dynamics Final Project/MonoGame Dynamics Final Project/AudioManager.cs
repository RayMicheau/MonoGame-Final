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
        ///
        Dictionary<string, SoundEffect> soundDictionary;
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

        public void Initialize(ContentManager Content)
        {
            enemyDeath = Content.Load<SoundEffect>("Audio/Sound Effects/enemy dead");
            enemyDeathTwo = Content.Load<SoundEffect>("Audio/Sound Effects/Enemy dead2");
            gravityWell = Content.Load<SoundEffect>("Audio/Sound Effects/gravity well");
            powerUpSpawn = Content.Load<SoundEffect>("Audio/Sound Effects/powerup 2");
            powerUpGet = Content.Load<SoundEffect>("Audio/Sound Effects/powerup_gained something");
            playerDeath = Content.Load<SoundEffect>("Audio/Sound Effects/player dead");
            thrust = Content.Load<SoundEffect>("Audio/Sound Effects/thrust");
            rocket = Content.Load<SoundEffect>("Audio/Sound Effects/rocket shot");
            //gameplaySong = Content.Load<Song>("Audio/Songs/Catalysm Song");
            //menuSong = Content.Load<Song>("Audio/Songs/menu song");
            /*
            soundDictionary.Add("enemy dead", Content.Load<SoundEffect>("Audio/Sound Effects/enemy dead"));
            soundDictionary.Add("enemy dead2", Content.Load<SoundEffect>("Audio/Sound Effects/Enemy dead2"));
            soundDictionary.Add("gravity well", Content.Load<SoundEffect>("Audio/Sound Effects/gravity well"));
            soundDictionary.Add("powerup 2", Content.Load<SoundEffect>("Audio/Sound Effects/powerup 2"));
            soundDictionary.Add("powerup gained", Content.Load<SoundEffect>("Audio/Sound Effects/powerup_gained something"));
            soundDictionary.Add("player death", Content.Load<SoundEffect>("Audio/Sound Effects/player dead"));
            soundDictionary.Add("thrust", Content.Load<SoundEffect>("Audio/Sound Effects/thrust"));
            gameplaySong = Content.Load<Song>("Audio/Songs/Catalysm Song");
            menuSong = Content.Load<Song>("Audio/Songs/menu song");
             */
        }

        public void Play(string songName)
        {
            //will fail silently, and play nothing
            if (songName == "" || songName == null)
                return;

            if(songName == menuSong.Name)
            {
                MediaPlayer.Play(menuSong);
            }
            else if (songName == gameplaySong.Name)
            {
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

                case "enemy dead":
                    enemyDeath.Play();
                    break;

                case "enemy dead2":
                    enemyDeathTwo.Play();
                    break;
                   
                case "enemy dead3":
                    break;

                case "thrust":
                    //thrust.Play();
                    break;

                case "rocket":
                    rocket.Play();
                    break;
                case "Spawn pUp":
                    powerUpSpawn.Play();
                    break;

                case "Get pUp":
                    powerUpGet.Play();
                    break;

                default: break;
            }
        }

        public static void Stop()
        {
            MediaPlayer.Stop();
        }

    }
}
