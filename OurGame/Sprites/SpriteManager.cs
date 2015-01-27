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
        public List<AnimatedSprite> Sprites;
        private readonly string _spritesFileName;

        public void AddSprite(AnimatedSprite aSprite)
        {
            this.Sprites.Add(aSprite);
        }

        public void RemoveSprite(AnimatedSprite aSprite)
        {
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
            Console.WriteLine("this.LoadSpritesFromAfile(board, pState); - this._spritesFileName == "+this._spritesFileName);
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
                    Console.WriteLine("         craeting a new "+this._spritesFileName);
                }
            }
            Console.WriteLine("***loading file "+this._spritesFileName);
            String[] configStringSplitRay = File.ReadAllLines(this._spritesFileName);

            //int numberOfSprites = Convert.ToInt32(configStringSplitRay[0].Split(':')[1]); // numberOfSprites:10
            this.Sprites = new List<AnimatedSprite>();
            for (int i = 0; i < configStringSplitRay.Length; i++)
            {
                string currentSpriteFileName = configStringSplitRay[i];
                this.Sprites.Add(SimpleAnimatedSpriteFactory.CreateAnimatedSprite(currentSpriteFileName, board, pState));
                Console.WriteLine("*********************loaded - "+currentSpriteFileName);
            } // end for

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
            for (int i = 0; i < this.Sprites.Count; i++)
            {
                this.Sprites[i].Update(gameTime);
            } // end for
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < this.Sprites.Count; i++)
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
            for (int i = 0; i < this.Sprites.Count; i++)
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
