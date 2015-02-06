using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OurGame.Sprites;

namespace OurGame.OurGameLibrary
{
    /*
     * Note: the TheBoard array stores the y coordinate first and then the X coordinate.
     */

    public class Board
    {
        // In pixels.
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }
        private int NumberOfHorizontalTiles { get; set; }
        private int NumberOfVerticalTiles { get; set; }
        
        // The elements of this 2D array can be NULL!  This will signify that the tile is empty.
        public Tile[,] TheBoard { get; set; }

        // The width and height of the individual tiles.
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }

        // Amount in pixels that, beyond this point to the left and the right, the board won't be drawn. - used to speed up the game by not drawing
        // all the game board every cycle.
        private int BoardMarginX { get; set; }
        // ReSharper disable once InconsistentNaming
        public const int SCREEN_WIDTH = 800;
        // ReSharper disable once InconsistentNaming
        private const int NUMBER_OF_TILES_IN_MARGIN_X = 1;

        public const int DEFAULT_TILE_WIDTH = 20;
        public const int DEFAULT_TILE_HEIGHT = 20;

        public Board(String pathToConfigFile)
        {
            Debug.Assert(pathToConfigFile != null && !pathToConfigFile.Equals(""), "pathToConfigFile is null or empty!");

            ReadInBoardConfigurationOrUseDefault(pathToConfigFile);
        } // end constructor

        // This method gets ALL of the tiles, currently visible on the screen, that overlap with the aSprite.
        public List<Tile> RetrieveTilesThatIntersectWithThisSprite(AnimatedSprite aSprite, int screenXOffset)
        {
            Debug.Assert(aSprite != null, "AnimatedSprite aSprite can not be null!");

            var tileList = new List<Tile>();

            for (var i = 0; i < TheBoard.GetLength(0); i++)
            {
                int startX;
                int endX;

                // This next method gets the array indices of the board for only the visible portion of this Board's 2D array.
                CalculateStartAndEndOfBoardToCheck(screenXOffset, out startX, out endX);

                for (var j = startX; j < endX; j++)
                {
                    if (TheBoard[i, j].TheTexture != null)
                    {
                        var tilePosition = ExtractTilePosition(screenXOffset, i, j);
                        TheBoard[i, j].BoundingRectangle = new Rectangle((int) tilePosition.X, (int) tilePosition.Y,
                            TheBoard[i, j].Width, TheBoard[i, j].Height);
                        if (aSprite.BoundingRectangle.Intersects(TheBoard[i, j].BoundingRectangle))
                        {
                            tileList.Add(TheBoard[i, j]);
                        }
                    }
                }
            } // End outer for.
            return tileList;
        } // end method

        // This method gets ALL of the tiles, currently visible on the screen, that overlap with the aSprite.
        public List<Tile> RetrieveTilesThatIntersectWithThisSpriteWithBoundingBoxAdjustment(AnimatedSprite aSprite, int screenXOffset)
        {
            Debug.Assert(aSprite != null, "AnimatedSprite aSprite can not be null!");

            var tileList = new List<Tile>();

            for (var i = 0; i < TheBoard.GetLength(0); i++)
            {
                int startX;
                int endX;

                // This next method gets the array indices of the board for only the visible portion of this Board's 2D array.
                CalculateStartAndEndOfBoardToCheck(screenXOffset, out startX, out endX);

                for (var j = startX; j < endX; j++)
                {
                    if (TheBoard[i, j].TheTexture != null)
                    {
                        var tilePosition = ExtractTilePosition(screenXOffset, i, j);
                        TheBoard[i, j].BoundingRectangle = new Rectangle((int)tilePosition.X, (int)tilePosition.Y,
                            TheBoard[i, j].Width, TheBoard[i, j].Height);

                        var aSpritesBoundingBoxModified = new Rectangle(aSprite.BoundingRectangle.X,
                            aSprite.BoundingRectangle.Y,
                            aSprite.BoundingRectangle.Width,
                            aSprite.BoundingRectangle.Height+1);
                        if (aSpritesBoundingBoxModified.Intersects(TheBoard[i, j].BoundingRectangle))
                        {
                            tileList.Add(TheBoard[i, j]);
                        }
                    }
                }
            } // End outer for.
            return tileList;
        } // end method

        public bool IsThereACollisionWith(AnimatedSprite aSprite, int screenXOffset)
        {
            Debug.Assert(aSprite != null, "AnimatedSprite aSprite can not be null!");

            // This will make the board only draw the part that is on the screen on the left side.
            BoardMarginX = TileWidth*NUMBER_OF_TILES_IN_MARGIN_X;

            for (var i = 0; i < TheBoard.GetLength(0); i++)
            {
                int startX;
                int endX;
                CalculateStartAndEndOfBoardToCheck(screenXOffset, out startX, out endX);

                for (var j = startX; j < endX; j++)
                {
                    if (TheBoard[i, j].TheTexture != null)
                    {
                        var tilePosition = ExtractTilePosition(screenXOffset, i, j);
                        TheBoard[i, j].BoundingRectangle = new Rectangle((int) tilePosition.X, (int) tilePosition.Y,
                            TheBoard[i, j].Width, TheBoard[i, j].Height);
                        if (aSprite.BoundingRectangle.Intersects(TheBoard[i, j].BoundingRectangle))
                        {
                            return true;
                        }
                    }
                }
            } // End outer for.
            return false;
        } // end method

        private Vector2 ExtractTilePosition(int screenXOffset, int i, int j)
        {
            var tilePosition = new Vector2(j*TileWidth + screenXOffset + BoardMarginX, i*TileHeight);
            return tilePosition;
        }

        public void DrawBoard(SpriteBatch spriteBatch, int screenXOffset, bool drawGrid)
        {
            Debug.Assert(spriteBatch != null, "spriteBatch can not be null!");

            // This will make the board only draw the part that is on the screen on the left side.
            BoardMarginX = TileWidth*NUMBER_OF_TILES_IN_MARGIN_X;

            if (drawGrid)
            {
                for (var y = 0; y < BoardHeight; y += TileHeight)
                {
                    C3.XNA.Primitives2D.DrawLine(spriteBatch, new Vector2(0.0f + BoardMarginX + screenXOffset, y),
                        new Vector2(BoardWidth + screenXOffset + BoardMarginX, y), Color.White);
                }

                for (var x = 0; x < BoardWidth + 1; x += TileWidth)
                {
                    var putItX = x/TileWidth*TileWidth + BoardMarginX + screenXOffset;
                    C3.XNA.Primitives2D.DrawLine(spriteBatch, new Vector2(putItX, 0.0f),
                        new Vector2(putItX, BoardHeight), Color.White);
                }
            } // end if

            for (var i = 0; i < TheBoard.GetLength(0); i++)
            {
                int startX;
                int endX;
                CalculateStartAndEndOfBoardToCheck(screenXOffset, out startX, out endX);

                for (var j = startX; j < endX; j++)
                {
                    // ReSharper disable once InvertIf
                    if (TheBoard[i, j].TheTexture != null)
                    {
                        var tilePosition = ExtractTilePosition(screenXOffset, i, j);
                        spriteBatch.Draw(TheBoard[i, j].TheTexture, tilePosition, Color.White);

                        TheBoard[i, j].BoundingRectangle.X = (int)tilePosition.X;
                        TheBoard[i, j].BoundingRectangle.Y = (int)tilePosition.Y;

                        C3.XNA.Primitives2D.DrawRectangle(spriteBatch, TheBoard[i, j].BoundingRectangle, Color.Black);
                    }
                }
            } // End outer for.
        } // end method DrawBoard

        // This will calculate the start and end of the board's indicies that are visible on the screen
        // at the time of the call.
        private void CalculateStartAndEndOfBoardToCheck(int screenXOffset, out int startX, out int endX)
        {
            // Transform BoardMargin on screen coordinate into an index into this Board object's 2D array of Tiles.
            startX = CalculateXIndex(BoardMarginX, screenXOffset);

            // Transform the end of the visible board on the screen into an index into this Board object's 2D array.
            endX = Math.Max(startX, CalculateXIndex(SCREEN_WIDTH - BoardMarginX, screenXOffset));
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
        }

        public void PutTextureOntoBoard(Texture2D tTexture, int rowIndex, int columnIndex)
        {
            // tTexture can be null!*****************************************************
            //      - this will signify that the board position is empty on the screen

            var t = TheBoard[rowIndex, columnIndex];
            t.TheTexture = tTexture;
        }

        // for calculating the indices intot he game board array.
        public int CalculateYIndex(int mouseY)
        {
            var putInGameArrayY = mouseY/TileHeight;
            return putInGameArrayY;
        }

        // This method takes a onScreenXCoordinate from the screen and transforms it into an index
        // into an instance of the this Board class.
        public int CalculateXIndex(int onScreenXCoordinate, int screenXOffset)
        {
            var putInGameArrayX = (onScreenXCoordinate - screenXOffset - BoardMarginX)/TileWidth;
            return putInGameArrayX;
        }

        public Texture2D GetTextureAt(int putY, int putX)
        {
            return TheBoard[putY, putX].TheTexture;
        }

        // For the Mouse to Screen mapping
        public int CalculateScreenCoordinateXFromMousePosition(int mouseX, int screenXOffset)
        {
            return ((mouseX - screenXOffset)/TileWidth)*TileWidth + screenXOffset;
        }

        public int CalculateScreenCoordinateYFromMousePosition(int mouseY)
        {
            return (mouseY/TileHeight)*TileHeight;
        }

        public void ReadInBoardConfigurationOrUseDefault(String path)
        {
            Debug.Assert(path != null && !path.Equals(""), "path can not be null or empty!");

            // Load the default game board configuration if the config file doesn't exist.
            if (!File.Exists(path))
            {
                BoardHeight = 20*24;
                    // Screen's height is 480 BoardHeight and Tile Height will be used to calc the number of tiles across array will be
                BoardWidth = 29*80;
                    // Screen's width is 800 BoardWidth and Tile Height will be used to calc the # of tiles across the array will be

                TileHeight = DEFAULT_TILE_WIDTH;
                TileWidth = DEFAULT_TILE_HEIGHT;

                NumberOfVerticalTiles = BoardHeight/TileHeight;
                NumberOfHorizontalTiles = BoardWidth/TileWidth;

                TheBoard = new Tile[NumberOfVerticalTiles, NumberOfHorizontalTiles];

                for (var row = 0; row < TheBoard.GetLength(0); row++)
                {
                    for (var column = 0; column < TheBoard.GetLength(1); column++)
                    {
                        var t = new Tile(null, // blank tile
                        
                            column, // remembert hese are swapped in array!!!
                            row,
                            TileWidth, // width
                            TileHeight, // height
                                      
                            0, // startBoundaryX
                            0, // startBoundaryY
                                      
                            TileWidth - 1, // endBoundaryX
                            TileHeight - 1); // endBoundaryY
                        TheBoard[row, column] = t;
                    }
                }
                // Write out the default config of the board
                WriteOutTheGameBoard(path);
            }
            else
            {
                // A config file exists for the board so load it now!
                var configStringSplitRay = File.ReadAllLines(path);

                BoardHeight = Convert.ToInt32(configStringSplitRay[0].Split(':')[1]); // defaults to 480
                BoardWidth = Convert.ToInt32(configStringSplitRay[1].Split(':')[1]); // defaults to 800

                TileHeight = Convert.ToInt32(configStringSplitRay[2].Split(':')[1]);
                TileWidth = Convert.ToInt32(configStringSplitRay[3].Split(':')[1]);

                NumberOfVerticalTiles = BoardHeight/TileHeight;
                NumberOfHorizontalTiles = BoardWidth/TileWidth;

                TheBoard = new Tile[NumberOfVerticalTiles, NumberOfHorizontalTiles];
                for (var i = 0; i < TheBoard.GetLength(0); i++)
                {
                    var stringGameBoardRay = configStringSplitRay[4 + i].Split(',');

                    for (var j = 0; j < TheBoard.GetLength(1); j++)
                    {
                        var t = new Tile(
                            TextureCache.getInstance().GetTexture2DFromStringBoardArray(stringGameBoardRay[j]),
                            // blank tile
                        
                            j, // remembert hese are swapped in array!!!
                            i,
                            TileWidth, // width
                            TileHeight, // height
                                      
                            0, // startBoundaryX
                            0, // startBoundaryY
                                      
                            TileWidth - 1, // endBoundaryX
                            TileHeight - 1); // endBoundaryY

                        TheBoard[i, j] = t;
                    }
                }
            } // end else
        }

        public void WriteOutTheGameBoard(String path)
        {
            Debug.Assert(path != null && !path.Equals(""), "path can not be null or empty!");

            using (var fs = File.Create(path))
            {
                Utilities.AddText(fs, "screenHeight:" + BoardHeight);
                Utilities.AddText(fs, "\n");
                Utilities.AddText(fs, "screenWidth:" + BoardWidth);
                Utilities.AddText(fs, "\n");

                Utilities.AddText(fs, "tileHeight:" + TileHeight);
                Utilities.AddText(fs, "\n");
                Utilities.AddText(fs, "tileWidth:" + TileWidth);
                Utilities.AddText(fs, "\n");

                for (var i = 0; i < TheBoard.GetLength(0); i++)
                {
                    for (var j = 0; j < TheBoard.GetLength(1); j++)
                    {
                        var gBTile = TheBoard[i, j];
                        if (gBTile.TheTexture == null)
                        {
                            Utilities.AddText(fs, "null");
                        }
                        else
                        {
                            Utilities.AddText(fs,
                                TextureCache.getInstance().GetStringFilenameFromTexture2DForBoard(gBTile.TheTexture));
                        }

                        if (j != TheBoard.GetLength(1) - 1)
                        {
                            Utilities.AddText(fs, ",");
                        } // end if
                    } // end inner for
                    Utilities.AddText(fs, "\n");
                } // end outer for
            } // end using
        } // end method

        public void PutMultiTileInBoard(MultiTexture mTile, int rowIndex, int columnIndex)
        {
            Debug.Assert(mTile != null, "MultiTexture mTile can not be null!");

            for (var i = 0; i < mTile.NumberOfVerticalTiles; i++)
            {
                for (var j = 0; j < mTile.NumberOfHorizontalTiles; j++)
                {
                    if (rowIndex + i >= 0 && columnIndex >= 0 && rowIndex < TheBoard.GetLength(0) &&
                        columnIndex < TheBoard.GetLength(1))
                    {
                        var t = TheBoard[rowIndex + i, columnIndex + j];
                        t.TheTexture = mTile.TextureToRepeat;
                    }
                }
            }
        }
    } // end class Board
}