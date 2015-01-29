using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.GameStates;
using OurGame.OurGameLibrary;

namespace OurGame.Sprites
{
    // This class manages all the sprites.  Mainly enemy sprites but could use it for the UserControlledSprite too.
    internal class SpriteManager
    {
        public List<AnimatedSprite> Sprites;
        private readonly string _spritesFileName;

        public void AddSprite(AnimatedSprite aSprite)
        {
            Debug.Assert(aSprite != null, "aSprite can not be null!");

            this.Sprites.Add(aSprite);
        }

        public void RemoveSprite(AnimatedSprite aSprite)
        {
            Debug.Assert(aSprite != null, "aSprite can not be null!");

            this.Sprites.Remove(aSprite);
        }

        // THis constructor will read in the spritesFileName or create it with default values if it doesn't exist.
        public SpriteManager(String spritesFileName, Board board, State pState)
        {
            Debug.Assert(spritesFileName != null && !spritesFileName.Equals(""),
                "spritesFileName can not be null or empty!");
            Debug.Assert(board != null, "board can not be null!");
            Debug.Assert(pState != null, "pState can not be null!");

            this._spritesFileName = spritesFileName;
            this.LoadSpritesFromAfile(board, pState);
        }

        public void LoadSpritesFromAfile(Board board, State pState)
        {
            Debug.Assert(board != null, "board can not be null!");
            Debug.Assert(pState != null, "pState can not be null!");

            if (!File.Exists(this._spritesFileName)) // This file, if not empty, would contain the list of sprite filenames.
            {
                using (FileStream fs = File.Create(this._spritesFileName))
                {
                    // Create empty file.
                }
            }
            String[] configStringSplitRay = File.ReadAllLines(this._spritesFileName);

            this.Sprites = new List<AnimatedSprite>();
            foreach (string currentSpriteFileName in configStringSplitRay)
            {
                this.Sprites.Add(SimpleAnimatedSpriteFactory.CreateAnimatedSprite(currentSpriteFileName, board, pState));
            }

        } // end method

        public void WriteOutSpritesToAfile()
        {
            using (FileStream fs = File.Create(this._spritesFileName))
            {
                for (int i = 0; i < this.Sprites.Count; i++)
                {
                    string filename = this.Sprites[i].NameOfThisSubclassForWritingToConfigFile() + i + ".txt";
                    this.Sprites[i].WritePropertiesToFile(filename);

                    AddText(fs, filename);
                    AddText(fs, "\n");
                }
            }
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");

            foreach (AnimatedSprite theSprite in this.Sprites)
            {
                theSprite.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Debug.Assert(spriteBatch != null, "spriteBatch can not be null!");

            foreach (AnimatedSprite theSprite in this.Sprites)
            {
                theSprite.Draw(spriteBatch);
            }
        }

        private static void AddText(FileStream fs, string value)
        {
            Debug.Assert(fs != null, "FileStream fs can't be null!");
            Debug.Assert(value != null, "value can't be null!");
            Debug.Assert(fs.CanWrite, "FileStream fs must be open for writing!");

            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        } // end method

        public void ApplyDownwordGravity()
        {
            foreach (AnimatedSprite aSprite in this.Sprites)
            {
                aSprite.ApplyDownwardGravity();
            }
        }

        public void ReverseTimeForSprites()
        {
            foreach (AnimatedSprite aSprite in this.Sprites)
            {
                aSprite.ReverseTimeForThisSprite();
            }
        }

        public void SavePositionForReverseTime(PlayGameState pState)
        {
            Debug.Assert(pState != null, "pState can not equal null!");

            foreach (AnimatedSprite aSprite in this.Sprites)
            {
                aSprite.SavePositionToReverseTimeStack(pState);
            }
        }

        public void DrawSubclassName(SpriteBatch sBatch, EditSpritesState pState)
        {
            Debug.Assert(sBatch != null, "sBatch can not be null!");
            Debug.Assert(pState != null, "pState can not be nul!");

            foreach (AnimatedSprite aSprite in this.Sprites)
            {
                aSprite.DrawSubclassName(sBatch, pState);
            }
        }

        public void DrawSubclassName(SpriteBatch sBatch, Vector2 mouseCursorUpperLeftCorner, EditSpritesState pState)
        {
            Debug.Assert(sBatch != null, "sBatch can not be null!");
            Debug.Assert(mouseCursorUpperLeftCorner != null, "mouseCursorUpperLeftCorner can not be null!");
            Debug.Assert(pState != null, "pState can not be nul!");
            
            foreach (AnimatedSprite aSprite in this.Sprites)
            {
                aSprite.DrawSubclassName(sBatch, mouseCursorUpperLeftCorner, pState);
            }
        }

        public List<AnimatedSprite> GetSpritesThatPlayerCollidedWith(AnimatedSprite aSprite)
        {
            Debug.Assert(aSprite != null, "aSprite can not be null!");

            return this.Sprites.Where(animatedSprite => animatedSprite.BoundingRectangle.Intersects((aSprite.BoundingRectangle))).ToList();
        } // end method
    } // end class
} // end using
