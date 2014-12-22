using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGameLibrary1
{
    // This class will hold 1-many Tile classes so that the user can place more tiles at the same time onto the game board.
    public class MultiTexture
    {
        public int NumberOfHorizontalTiles { get; set; }
        public int NumberOfVerticalTiles { get; set; }

        public Texture2D TextureToRepeat { get; set; }


        public MultiTexture(int numberOfHorizontalTiles, int numberOfVirticalTiles, Texture2D tileToRepeat)
        {
            this.NumberOfHorizontalTiles = numberOfHorizontalTiles;
            this.NumberOfVerticalTiles = numberOfVirticalTiles;

            this.TextureToRepeat = tileToRepeat;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 mouseCursorUpperLeftCorner)
        {
            for (int i = 0; i < this.NumberOfVerticalTiles; i++)
            {
                for (int j = 0; j < this.NumberOfHorizontalTiles; j++)
                {
                    int putX = (int)mouseCursorUpperLeftCorner.X+(this.TextureToRepeat.Width*j);
                    int putY = (int)mouseCursorUpperLeftCorner.Y+(this.TextureToRepeat.Height*i);

                    Vector2 alteredPosition = new Vector2(putX, putY);
                    spriteBatch.Draw(this.TextureToRepeat, alteredPosition, Color.White);
                }
            } // end for
        } // end method
    } // end class
} // end namespace
