using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using OurGame.GameStates;

namespace OurGame.OurGameLibrary.TemplateMethod
{
    public class RetrieveTilesTemplateMethod
    {
        public List<Tile> RetrieveTilesThatIntersectWithRectangle(Board aBoardObject, State state, Rectangle theBoundingRectangle, int spritesCurrentPositionY)
        {
            Tile[,] TheBoard = aBoardObject.TheBoard;

            var tileList = new List<Tile>();

            for (var i = 0; i < TheBoard.GetLength(0); i++)
            {
                int startX;
                int endX;

                // This next method gets the array indices of the board for only the visible portion of this Board's 2D array.
                CalculateStartAndEndOfBoardToCheck(state.ScreenXOffset, out startX, out endX, aBoardObject);

                for (var j = startX; j < endX; j++)
                {
                    if (TheBoard[i, j].TheTexture != null)
                    {
                        var tilePosition = ExtractTilePosition(state.ScreenXOffset, i, j, aBoardObject);
                        TheBoard[i, j].BoundingRectangle = new Rectangle((int)tilePosition.X, (int)tilePosition.Y,
                            TheBoard[i, j].Width, TheBoard[i, j].Height);
                        if (theBoundingRectangle.Intersects(TheBoard[i, j].BoundingRectangle))
                        {
                            tileList.Add(TheBoard[i, j]);
                        }
                    }
                }
            } // End outer for.

            if (ShouldKeepOnlyTilesBelowCurrentPositionY())
            {
                tileList = tileList.Where(tile => tile.BoundingRectangle.Y > spritesCurrentPositionY).ToList();
            }

            return tileList;
        }

        public virtual bool ShouldKeepOnlyTilesBelowCurrentPositionY()
        {
            return false;
        }

        private Vector2 ExtractTilePosition(int screenXOffset, int i, int j, Board board)
        {
            var tilePosition = new Vector2(j * board.TileWidth + screenXOffset + board.BoardMarginX, i * board.TileHeight);
            return tilePosition;
        }

        private void CalculateStartAndEndOfBoardToCheck(int screenXOffset, out int startX, out int endX, Board board)
        {
            Tile[,] TheBoard = board.TheBoard;

            // Transform BoardMargin on screen coordinate into an index into this Board object's 2D array of Tiles.
            startX = CalculateXIndex(board.BoardMarginX, screenXOffset, board);

            // Transform the end of the visible board on the screen into an index into this Board object's 2D array.
            endX = Math.Max(startX, CalculateXIndex(Board.SCREEN_WIDTH - board.BoardMarginX, screenXOffset, board));
            // this.TheBoard.GetLength(1);

            // Make sure the board indicies are not greater than the column width oft he board.
            if (startX >= TheBoard.GetLength(1))
            {
                startX = TheBoard.GetLength(1) - 1;
            }
            if (endX >= TheBoard.GetLength(1))
            {
                endX = TheBoard.GetLength(1) - 1;
            }
        } // end method

        // This method takes a onScreenXCoordinate from the screen and transforms it into an index
        // into an instance of the this Board class.
        public int CalculateXIndex(int onScreenXCoordinate, int screenXOffset,Board board)
        {
            var putInGameArrayX = (onScreenXCoordinate - screenXOffset - board.BoardMarginX) / board.TileWidth;
            return putInGameArrayX;
        }

    } // end class
}
