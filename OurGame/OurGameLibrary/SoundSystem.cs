﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace OurGame.OurGameLibrary
{
    public class SoundSystem
    {
        private static SoundSystem _soundSystem;

        private SoundEffect soundEffect;
        private static ContentManager _content;

        public static SoundSystem getInstance()
        {
            if (_content == null)
            {
                throw new MustCallSetContentMethodFirst(
                    "SoundSystem.SetContent(...) must be called (usually in LoadContent(..) method of the State subclass) before SoundSystem.getInstance() is called!");
            }

            Debug.Assert(_content != null, "TextureCache._Content must not be nulll!");

            if (_soundSystem == null)
            {
                _soundSystem = new SoundSystem(_content);
            }

            return _soundSystem;
        }

        private SoundSystem(ContentManager Content)
        {
            // For wavs.
            soundEffect = Content.Load<SoundEffect>(@"audio\mario_jump");
        }

        // This method needs to be called before the TextureCache.getInstance() is called!
        public static void SetContent(ContentManager Content)
        {
            Debug.Assert(Content != null, "Content can not be null!");

            _content = Content;
        }

        // For wav files.
        public SoundEffectInstance getSound(String nameOfSoundEffect)
        {
            return soundEffect.CreateInstance();
        }

        // For mp3 files.
        public void playMusic(String nameOfMusic)
        {
            Song song = _content.Load<Song>("meat1");  // Put the name of your song in instead of "song_title"
            MediaPlayer.Play(song);
        }
    }
}
