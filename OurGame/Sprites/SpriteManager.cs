using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.GameStates;
using OurGame.OurGameLibrary;

namespace OurGame.Sprites
{
    class SpriteManager
    {
        public AnimatedSprite[] Sprites;
        public string SpritesFileName;

        public SpriteManager(String spritesFileName, Board board, PlayGameState pState)
        {
            Debug.Assert(spritesFileName != null && !spritesFileName.Equals(""),"spritesFileName can not be null or empty!");
            Debug.Assert(board != null, "board can not be null!");
            Debug.Assert(pState != null, "pState can not be null!");

            this.SpritesFileName = spritesFileName;

            this.LoadSpritesFromAfile(board, pState);
        }

        private void LoadSpritesFromAfile(Board board, PlayGameState pState)
        {
            Debug.Assert(board!= null, "board can not be null!");
            Debug.Assert(pState != null, "pState can not be null!");

            String[] configStringSplitRay = File.ReadAllLines(this.SpritesFileName);

            int numberOfSprites= Convert.ToInt32(configStringSplitRay[0].Split(':')[1]);  // numberOfSprites:10
            this.Sprites = new AnimatedSprite[numberOfSprites];

            for (int i = 0; i < this.Sprites.Length; i++)
            {
                string currentSpriteFileName = configStringSplitRay[i + 1];
                this.Sprites[i] = SimpleAnimatedSpriteFactory.CreateAnimatedSprite(currentSpriteFileName, board, pState);
            } // end for
        } // end method

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            for (int i = 0; i < this.Sprites.Length; i++)
            {
                this.Sprites[i].Update(gameTime);
            } // end for
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.Sprites.Length; i++)
            {
                this.Sprites[i].Draw(spriteBatch);
            } // end for
        }
    } // end class
} // end using
