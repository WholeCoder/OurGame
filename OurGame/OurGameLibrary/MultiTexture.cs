using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OurGame.OurGameLibrary
{
    // This class will hold 1-many Tile classes so that the user can place more tiles at the same time onto the game board.
    public class MultiTexture
    {
        public MultiTexture(int numberOfHorizontalTiles, int numberOfVirticalTiles, Texture2D tileToRepeat)
        {
            // tileToRepeat can be null!

            NumberOfHorizontalTiles = numberOfHorizontalTiles;
            NumberOfVerticalTiles = numberOfVirticalTiles;

            TextureToRepeat = tileToRepeat;
        }

        public int NumberOfHorizontalTiles { get; set; }
        public int NumberOfVerticalTiles { get; set; }
        public Texture2D TextureToRepeat { get; set; }

        public void Draw(SpriteBatch spriteBatch, Vector2 mouseCursorUpperLeftCorner)
        {
            Debug.Assert(spriteBatch != null, "spriteBatch can not be null!");
            Debug.Assert(mouseCursorUpperLeftCorner != null, "mouseCursorUpperLeftCorner can not be null!");

            for (var i = 0; i < NumberOfVerticalTiles; i++)
            {
                for (var j = 0; j < NumberOfHorizontalTiles; j++)
                {
                    var width = TextureCache.getInstance().GetFromTexture2DBoardArray(0).Width;
                    var height = TextureCache.getInstance().GetFromTexture2DBoardArray(0).Height;

                    if (TextureToRepeat != null)
                    {
                        width = TextureCache.getInstance().GetCurrentTexture().Width;
                        height = TextureCache.getInstance().GetCurrentTexture().Height;
                    }

                    var putX = (int) mouseCursorUpperLeftCorner.X + (width*j);
                    var putY = (int) mouseCursorUpperLeftCorner.Y + (height*i);

                    var alteredPosition = new Vector2(putX, putY);
                    spriteBatch.Draw(SelectDeleteBrushPossibly(TextureToRepeat), alteredPosition, Color.White);
                }
            } // end for
        } // end method

        public Texture2D SelectDeleteBrushPossibly(Texture2D t)
        {
            // t can be null!

            if (TextureToRepeat == null)
            {
                return TextureCache.getInstance().GetTexture2DFromStringBoardArray("Images/DeleteBrush");
            }
            return t;
        } // end method
    } // end class
} // end namespace