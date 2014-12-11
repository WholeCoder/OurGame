using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGameLibrary1
{

    /*
     * Note: the TheBoard array stores the y coordinate first and then the X coordinate.
     */

    class Board
    {
        public int PixelWidth { get; set; }
        public int PixelHeight { get; set; }

        public int WidthOfBoardInTiles { get; set; }
        public int HeightOfBoardInTiles { get; set; }

        public Tile[,] TheBoard { get; set; }

        // The widht and height of the individual tiles.
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }


        public Board(int pWidth, int pHeight,
                     int widthOfBoardInTiles, int heightOfBoardInTiles,
                     int tWidth, int tHeight)
        {
            this.PixelHeight = pWidth;
            this.PixelHeight = pHeight;

            this.WidthOfBoardInTiles = widthOfBoardInTiles;
            this.HeightOfBoardInTiles = heightOfBoardInTiles;

            // individual tile
            this.TileWidth = tWidth;
            this.TileHeight = tHeight;

            // REMEMBER - this is stored with rows first and then columns!!!!
            this.TheBoard = new Tile[this.HeightOfBoardInTiles, this.HeightOfBoardInTiles];  // (y, x)

            // intialize board to all blank tiles
            for (int row = 0; row < this.TheBoard.GetLength(0); row++)
            {
                for (int column = 0; column < this.TheBoard.GetLength(1); column++)
                {
                    Tile t = new Tile(column*this.TileWidth, // x
                                      row*this.TileHeight,   // y
                                      
                                      column,                // remembert hese are swapped in array!!!
                                      row,

                                      this.TileWidth,        // width
                                      this.TileHeight,       // height
                                      
                                      0,                    // startBoundaryX
                                      0,                    // startBoundaryY
                                      
                                      this.TileWidth-1,     // endBoundaryX
                                      this.TileHeight-1);   // endBoundaryY
                    this.TheBoard[row, column] = t;
                }
            }
        } // end constructor
    } // end class Board
}
