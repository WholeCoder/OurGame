using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.GameStates;
using OurGame.OurGameLibrary;

namespace OurGame.Sprites
{
    // This class manages all the sprites.  Mainly enemy sprites but could use it for the UserControlledSprite too.
    internal class SpriteManager
    {
        public AnimatedSprite[] Sprites;
        private readonly string _spritesFileName;

        public SpriteManager(String spritesFileName, Board board, PlayGameState pState)
        {
            Debug.Assert(spritesFileName != null && !spritesFileName.Equals(""),
                "spritesFileName can not be null or empty!");
            Debug.Assert(board != null, "board can not be null!");
            Debug.Assert(pState != null, "pState can not be null!");

            this._spritesFileName = spritesFileName;

            this.LoadSpritesFromAfile(board, pState);
        }

        private void LoadSpritesFromAfile(Board board, PlayGameState pState)
        {
            Debug.Assert(board != null, "board can not be null!");
            Debug.Assert(pState != null, "pState can not be null!");

            if (!File.Exists(this._spritesFileName))
            {
                using (FileStream fs = File.Create(this._spritesFileName))
                {
                    AddText(fs, "numberOfSprites:1"); // ex) "UserControlledSprite"
                    AddText(fs, "\n");

                    AddText(fs, "AutomatedSpriteConfig.txt"); // ex) "UserControlledSprite"
                    AddText(fs, "\n");
                }
            }
            if (!File.Exists("AutomatedSpriteConfig.txt"))
            {
                // This will create the AutomatedSpriteConfig.txt file
                AnimatedSprite aAnimatedSprite = new AutomatedSprite("AutomatedSpriteConfig.txt", board, pState);
            }

            String[] configStringSplitRay = File.ReadAllLines(this._spritesFileName);

            int numberOfSprites = Convert.ToInt32(configStringSplitRay[0].Split(':')[1]); // numberOfSprites:10
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

        public static void AddText(FileStream fs, string value)
        {
            Debug.Assert(fs != null, "FileStream fs can't be null!");
            Debug.Assert(value != null, "value can't be null!");
            Debug.Assert(fs.CanWrite, "FileStream fs must be open for writing!");

            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        } // end method

        public void ApplyDownwordGravity()
        {
            for (int i = 0; i < this.Sprites.Length; i++)
            {
                this.Sprites[i].ApplyDownwardGravity();
            } // end for
        }

        public List<AnimatedSprite> GetSpritesThatPlayerCollidedWith(AnimatedSprite aSprite)
        {
            List<AnimatedSprite> sList = new List<AnimatedSprite>();

            foreach (AnimatedSprite animatedSprite in this.Sprites)
            {
                if (animatedSprite.BoundingRectangle.Intersects((aSprite.BoundingRectangle)))
                {
                    sList.Add(animatedSprite);
                }
            }

            return sList;
        } // end method
    } // end class
} // end using
