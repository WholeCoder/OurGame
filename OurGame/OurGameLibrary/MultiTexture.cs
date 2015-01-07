﻿using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OurGame.WindowsGameLibrary1
{
    // This class will hold 1-many Tile classes so that the user can place more tiles at the same time onto the game board.
    public class MultiTexture
    {
        TextureCache tCache;

        public int NumberOfHorizontalTiles { get; set; }
        public int NumberOfVerticalTiles { get; set; }

        public Texture2D TextureToRepeat { get; set; }


        public MultiTexture(int numberOfHorizontalTiles, int numberOfVirticalTiles, Texture2D tileToRepeat)
        {
            this.NumberOfHorizontalTiles = numberOfHorizontalTiles;
            this.NumberOfVerticalTiles = numberOfVirticalTiles;

            this.TextureToRepeat = tileToRepeat;

            this.tCache = TextureCache.getInstance();
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 mouseCursorUpperLeftCorner)
        {
            Debug.Assert(spriteBatch != null, "spriteBatch can not be null!");
            Debug.Assert(mouseCursorUpperLeftCorner != null, "mouseCursorUpperLeftCorner can not be null!");

            for (int i = 0; i < this.NumberOfVerticalTiles; i++)
            {
                for (int j = 0; j < this.NumberOfHorizontalTiles; j++)
                {
                    int width = tCache.GetFromTexture2DBoardArray(0).Width;
                    int height = tCache.GetFromTexture2DBoardArray(0).Height;

                    if (this.TextureToRepeat != null)
                    {
                        width = tCache.GetCurrentTexture().Width;
                        height = tCache.GetCurrentTexture().Height;
                    }

                    int putX = (int)mouseCursorUpperLeftCorner.X+(width*j);
                    int putY = (int)mouseCursorUpperLeftCorner.Y+(height*i);
                    
                    Vector2 alteredPosition = new Vector2(putX, putY);
                    spriteBatch.Draw(this.SelectDeleteBrushPossibly(this.TextureToRepeat), alteredPosition, Color.White);
                }
            } // end for
        } // end method

        public Texture2D SelectDeleteBrushPossibly(Texture2D t)
        {
            if (this.TextureToRepeat == null)
            {
                return tCache.GetTexture2DFromStringBoardArray("Images/DeleteBrush");
            }
            return t;
        } // end method
    } // end class
} // end namespace
