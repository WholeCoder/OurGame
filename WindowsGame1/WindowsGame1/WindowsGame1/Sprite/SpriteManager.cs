using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Sprite
{
    class SpriteManager
    {
        public AnimatedSprite[] sprites;
        public string SpritesFileName;

        public SpriteManager(String spritesFileName, ContentManager Content)
        {
            this.SpritesFileName = spritesFileName;

            this.LoadSpritesFromAfile(Content);
        }

        private void LoadSpritesFromAfile(ContentManager Content)
        {
            String configurationString = "";  // Holds the entire configuration file.

            // Open the stream and read it back. 
            using (FileStream fs = File.OpenRead(this.SpritesFileName))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    configurationString += temp.GetString(b);
                }
            }

            String[] configStringSplitRay = configurationString.Split('\n');

            int numberOfSprites= Convert.ToInt32(configStringSplitRay[0].Split(':')[1]);  // defaults to 480
            this.sprites = new AnimatedSprite[numberOfSprites];

            for (int i = 0; i < this.sprites.Length; i++)
            {
                string currentSpriteFileName = configStringSplitRay[i + 1];
            } // end for

        } // end method
    } // end class
} // end using
