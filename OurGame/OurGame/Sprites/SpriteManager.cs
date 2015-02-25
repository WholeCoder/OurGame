using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OurGame.GameStates;
using OurGame.OurGameLibrary;

namespace OurGame.Sprites
{
    // This class manages all the sprites.  Mainly enemy sprites but could use it for the UserControlledSprite too.
    internal class SpriteManager
    {
        private readonly string _spritesFileName;
        public List<AnimatedSprite> Sprites;
        private Board _board;
        private State _state;
        // THis constructor will read in the spritesFileName or create it with default values if it doesn't exist.
        public SpriteManager(String spritesFileName, Board board, State pState)
        {
            Debug.Assert(spritesFileName != null && !spritesFileName.Equals(""),
                "spritesFileName can not be null or empty!");
            Debug.Assert(board != null, "board can not be null!");
            Debug.Assert(pState != null, "pState can not be null!");

            _board = board;
            _state = pState;
            _spritesFileName = spritesFileName;
            LoadSpritesFromAfile(board, pState);
        }

        public override string ToString()
        {
            return "SpriteManager - Number of sprites is " + Sprites.Count;
        }

        public void AddSprite(AnimatedSprite aSprite)
        {
            Debug.Assert(aSprite != null, "aSprite can not be null!");

            Sprites.Add(aSprite);
        }

        public void RemoveSprite(AnimatedSprite aSprite)
        {
            Debug.Assert(aSprite != null, "aSprite can not be null!");

            Sprites.Remove(aSprite);
        }

        public void LoadSpritesFromAfile(Board board, State pState)
        {
            Debug.Assert(board != null, "board can not be null!");
            Debug.Assert(pState != null, "pState can not be null!");

            if (!File.Exists(_spritesFileName)) // This file, if not empty, would contain the list of sprite filenames.
            {
                using (var fs = File.Create(_spritesFileName))
                {
                    // Create empty file.
                }
            }
            var configStringSplitRay = File.ReadAllLines(_spritesFileName);

            Sprites = new List<AnimatedSprite>();
            foreach (var currentSpriteFileName in configStringSplitRay)
            {
                Sprites.Add(SimpleAnimatedSpriteFactory.CreateAnimatedSprite(currentSpriteFileName, board, pState));
            }
        } // end method

        public void WriteOutSpritesToAfile()
        {
            using (var fs = File.Create(_spritesFileName))
            {
                for (var i = 0; i < Sprites.Count; i++)
                {
                    var filename = Sprites[i].NameOfThisSubclassForWritingToConfigFile + i + ".txt";
                    Sprites[i].WritePropertiesToFile(filename);

                    Utilities.AddText(fs, filename);
                    Utilities.AddText(fs, "\n");
                }
            }
        }

/*
        public void UpdateForEditSpriteState(GameTime gameTime, int screenXOffset)
        {
            foreach (var theSprite in Sprites)
            {
                theSprite.CurrentPosition.X = theSprite.InitialPosition.X + screenXOffset;
            }
        }
*/

        public void Update(GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");

            foreach (var theSprite in Sprites)
            {
                theSprite.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Debug.Assert(spriteBatch != null, "spriteBatch can not be null!");

            foreach (var theSprite in Sprites)
            {
                theSprite.Draw(spriteBatch);
            }
        }

        public void ApplyDownwordGravity(State state)
        {
            foreach (var aSprite in Sprites)
            {
                Rectangle tempBoundingRectangle = new Rectangle(aSprite.BoundingRectangle.X,
                                                                aSprite.BoundingRectangle.Y+AnimatedSprite.GRAVITY_DOWNWARD,
                                                                aSprite.BoundingRectangle.Width,
                                                                aSprite.BoundingRectangle.Height);
                if (_board.RetrieveTilesThatIntersectWithThisSprite(tempBoundingRectangle, _state, (int)aSprite.CurrentPosition.Y).Count == 0)
                {
                    //Console.WriteLine("Some TILES Intersect!!!!   "+(new Random()).Next());
                    aSprite.ApplyDownwardGravity(_board, state);
                }
            }
        }

        public void ReverseTimeForSprites()
        {
            foreach (var aSprite in Sprites)
            {
                aSprite.ReverseTimeForThisSprite();
            }
        }

        public void SavePositionForReverseTime(PlayGameState pState)
        {
            Debug.Assert(pState != null, "pState can not equal null!");

            foreach (var aSprite in Sprites)
            {
                aSprite.SavePositionToReverseTimeStack(pState);
            }
        }

        public void DrawSubclassName(SpriteBatch sBatch, EditSpritesState pState)
        {
            Debug.Assert(sBatch != null, "sBatch can not be null!");
            Debug.Assert(pState != null, "pState can not be nul!");

            foreach (var aSprite in Sprites)
            {
                aSprite.DrawSubclassName(sBatch, pState);
            }
        }

        public void DrawSubclassName(SpriteBatch sBatch, Vector2 mouseCursorUpperLeftCorner, EditSpritesState pState)
        {
            Debug.Assert(sBatch != null, "sBatch can not be null!");
            Debug.Assert(mouseCursorUpperLeftCorner != null, "mouseCursorUpperLeftCorner can not be null!");
            Debug.Assert(pState != null, "pState can not be nul!");

            foreach (var aSprite in Sprites)
            {
                aSprite.DrawSubclassName(sBatch, mouseCursorUpperLeftCorner, pState);
            }
        }

        public List<AnimatedSprite> GetSpritesThatPlayerCollidedWith(AnimatedSprite aSprite)
        {
            Debug.Assert(aSprite != null, "aSprite can not be null!");

            return
                Sprites.Where(animatedSprite => animatedSprite.BoundingRectangle.Intersects((aSprite.BoundingRectangle)))
                    .ToList();
        } // end method
    } // end class
} // end using