using System;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework.Content;

// My usings.
using OurGame.WindowsGameLibrary1;

namespace OurGame.Sprites
{
    class SpriteManager
    {
        public AnimatedSprite[] sprites;
        public string SpritesFileName;
        public TextureCache tCache { get; set; }

        public SpriteManager(String spritesFileName, ContentManager Content, TextureCache tCache)
        {
            this.SpritesFileName = spritesFileName;
            this.tCache = tCache;

            this.LoadSpritesFromAfile(Content);
        }

        private void LoadSpritesFromAfile(ContentManager Content)
        {
            String[] configStringSplitRay = File.ReadAllLines(this.SpritesFileName);

            int numberOfSprites= Convert.ToInt32(configStringSplitRay[0].Split(':')[1]);  // numberOfSprites:10
            this.sprites = new AnimatedSprite[numberOfSprites];

            for (int i = 0; i < this.sprites.Length; i++)
            {
                this.sprites[i] = SimpleAnimatedSpriteFactory.CreateAnimatedSprite(configStringSplitRay[1 + i], tCache);

                string currentSpriteFileName = configStringSplitRay[i + 1];
            } // end for

        } // end method
    } // end class
} // end using
