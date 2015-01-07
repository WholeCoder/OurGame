using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;

// My usings.
using OurGame.WindowsGameLibrary1;

namespace OurGame.Sprites
{
    class SpriteManager
    {
        public AnimatedSprite[] sprites;
        public string SpritesFileName;

        public SpriteManager(String spritesFileName, ContentManager Content)
        {
            Debug.Assert(spritesFileName != null && !spritesFileName.Equals(""),"spritesFileName can not be null or empty!");
            Debug.Assert(Content != null, "Content can not be null!");

            this.SpritesFileName = spritesFileName;

            this.LoadSpritesFromAfile(Content);
        }

        private void LoadSpritesFromAfile(ContentManager Content)
        {
            Debug.Assert(Content != null, "Content can not be null!");

            String[] configStringSplitRay = File.ReadAllLines(this.SpritesFileName);

            int numberOfSprites= Convert.ToInt32(configStringSplitRay[0].Split(':')[1]);  // numberOfSprites:10
            this.sprites = new AnimatedSprite[numberOfSprites];

            for (int i = 0; i < this.sprites.Length; i++)
            {
                this.sprites[i] = SimpleAnimatedSpriteFactory.CreateAnimatedSprite(configStringSplitRay[1 + i]);

                string currentSpriteFileName = configStringSplitRay[i + 1];
            } // end for

        } // end method
    } // end class
} // end using
