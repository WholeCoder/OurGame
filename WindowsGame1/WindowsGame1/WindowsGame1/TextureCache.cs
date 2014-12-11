using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace WindowsGame1
{
    class TextureCache
    {
        private Texture2D[] textures;
        private int currentTextureIndex;

        private const int SLOW_DOWNMOUSE_TICKS = 1;
        private int slowDownMouseClick;

        public TextureCache(ContentManager Content)
        {
            // TODO: use this.Content to load your game content here
            textures = new Texture2D[2];

            textures[0] = Content.Load<Texture2D>(@"Images/tile");
            textures[1] = Content.Load<Texture2D>(@"Images/tile2");
            currentTextureIndex = 0;

            slowDownMouseClick = SLOW_DOWNMOUSE_TICKS;
        }

        public void NextTexture()
        {
            if (slowDownMouseClick == 0)
            {
                currentTextureIndex = (currentTextureIndex + 1) % textures.Length;
                slowDownMouseClick = SLOW_DOWNMOUSE_TICKS;
            }
            slowDownMouseClick--;
        }

        public Texture2D GetCurrentTexture()
        {
            return textures[currentTextureIndex];
        }
    }

}
