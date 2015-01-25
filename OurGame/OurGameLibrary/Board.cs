using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

// My usings.
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

        public Tile[,] TheBoard { get; set; }

        // The width and height of the individual tiles.
        private int TileWidth { get; set; }
        private int TileHeight { get; set; }

        // Amount in pixels that, beyond this point to the left and the right, the board won't be drawn. - used to speed up the game by not drawing
        // all the game board every cycle.
        private int BoardMarginX { get; set; }
        public const int SCREEN_WIDTH = 800;
        private const int NUMBER_OF_TILES_IN_MARGIN_X = 1;

        public Board(String pathToConfigFile)
        {
            Debug.Assert(pathToConfigFile != null && !pathToConfigFile.Equals(""), "pathToConfigFile is null or empty!");

            this.ReadInBoardConfigurationOrUseDefault(pathToConfigFile);
        } // end constructor


        public Tile GetFloorLocation(AnimatedSprite aSprite, int screenXOffset)
        {
            List<Tile> tileList = new List<Tile>();

            int startX = (int)((aSprite.CurrentPosition.X - screenXOffset - this.BoardMarginX) / this.TileWidth);
            Tile currTile = null;

            if (startX >= 0 && startX < this.TheBoard.GetLength(1))
            {
                for (int i = this.TheBoard.GetLength(0) - 1; i >= 0; i--)
                {
                    if (this.TheBoard[i, startX].TheTexture != null)
                    {
                        if (currTile == null || this.TheBoard[i, startX].BoundingRectangle.Y < currTile.BoundingRectangle.Y)
                        {
                            currTile = this.TheBoard[i, startX];
                        }
                    }
                } // End for.
            } // End if

            return currTile;
        }

        // This method gets ALL of the tiles, currently visible on the screen, that overlap with the aSprite.
        public List<Tile> RetrieveTilesThatIntersectWithThisSprite(AnimatedSprite aSprite, int screenXOffset)
        {
            Debug.Assert(aSprite != null, "AnimatedSprite aSprite can not be null!");

            List<Tile> tileList = new List<Tile>();

            for (int i = 0; i < this.TheBoard.GetLength(0); i++)
            {
                int startX;
                int endX;

                // This next method gets the array indices of the board for only the visible portion of this Board's 2D array.
                CalculateStartAndEndOfBoardToCheck(screenXOffset, out startX, out endX);

                for (int j = startX; j < endX; j++)
                {
                    if (this.TheBoard[i, j].TheTexture != null)
                    {
                        Vector2 tilePosition = this.ExtractTilePosition(screenXOffset, i, j);
                        this.TheBoard[i, j].BoundingRectangle = new Rectangle((int)tilePosition.X, (int)tilePosition.Y,
                                                                              this.TheBoard[i, j].Width, this.TheBoard[i, j].Height);
                        if (aSprite.BoundingRectangle.Intersects(this.TheBoard[i, j].BoundingRectangle))
                        {
                            tileList.Add(this.TheBoard[i, j]);
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
            this.BoardMarginX = this.TileWidth * Board.NUMBER_OF_TILES_IN_MARGIN_X;

            for (int i = 0; i < this.TheBoard.GetLength(0); i++)
            {
                int startX;
                int endX;
                CalculateStartAndEndOfBoardToCheck(screenXOffset, out startX, out endX);

                for (int j = startX; j < endX; j++)
                {
                    if (this.TheBoard[i, j].TheTexture != null)
                    {
                        Vector2 tilePosition = this.ExtractTilePosition(screenXOffset, i, j);
                        this.TheBoard[i, j].BoundingRectangle = new Rectangle((int)tilePosition.X, (int)tilePosition.Y,
                                                                              this.TheBoard[i, j].Width, this.TheBoard[i, j].Height);
                        if (aSprite.BoundingRectangle.Intersects(this.TheBoard[i, j].BoundingRectangle))
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
            Vector2 tilePosition = new Vector2(j * this.TileWidth + screenXOffset + this.BoardMarginX, i * this.TileHeight);
            return tilePosition;
        }

        public void DrawBoard(SpriteBatch spriteBatch, int screenXOffset, bool drawGrid)
        {
            Debug.Assert(spriteBatch != null, "spriteBatch can not be null!");

            // This will make the board only draw the part that is on the screen on the left side.
            this.BoardMarginX = this.TileWidth*Board.NUMBER_OF_TILES_IN_MARGIN_X;

            if (drawGrid)
            {
                for (int y = 0; y < this.BoardHeight; y += this.TileHeight)
                {
                    C3.XNA.Primitives2D.DrawLine(spriteBatch, new Vector2(0.0f + this.BoardMarginX+screenXOffset, y), new Vector2(this.BoardWidth + screenXOffset + this.BoardMarginX, y), Color.White);
                }

                for (int x = 0; x < this.BoardWidth + 1; x += this.TileWidth)
                {
                    int putItX = x / this.TileWidth * this.TileWidth + this.BoardMarginX + screenXOffset;
                    C3.XNA.Primitives2D.DrawLine(spriteBatch, new Vector2(putItX, 0.0f), new Vector2(putItX, this.BoardHeight), Color.White);
                }
            } // end if

            for (int i = 0; i < this.TheBoard.GetLength(0); i++)
            {
                int startX;
                int endX;
                CalculateStartAndEndOfBoardToCheck(screenXOffset, out startX, out endX);

                for (int j = startX; j < endX; j++)
                {
                    // ReSharper disable once InvertIf
                    if (this.TheBoard[i, j].TheTexture != null)
                    {
                        Vector2 tilePosition = this.ExtractTilePosition(screenXOffset, i, j);
                        spriteBatch.Draw(this.TheBoard[i, j].TheTexture, tilePosition, Color.White);
                    }
                }
            } // End outer for.

        } // end method DrawBoard

        // This will calculate the start and end of the board's indicies that are visible on the screen
        // at the time of the call.
        private void CalculateStartAndEndOfBoardToCheck(int screenXOffset, out int startX, out int endX)
        {
            // Transform BoardMargin on screen coordinate into an index into this Board object's 2D array of Tiles.
            startX = this.CalculateXIndex(this.BoardMarginX, screenXOffset);

            // Transform the end of the visible board on the screen into an index into this Board object's 2D array.
            endX = Math.Max(startX, this.CalculateXIndex(Board.SCREEN_WIDTH - this.BoardMarginX, screenXOffset));  // this.TheBoard.GetLength(1);
            
            // Make sure the board indicies are not greater than the column width oft he board.
            if (startX >= this.TheBoard.GetLength(1))
            {
                startX = this.TheBoard.GetLength(1) - 1;
            }
            if (endX >= this.TheBoard.GetLength(1))
            {
                endX = this.TheBoard.GetLength(1) - 1;
            }
        }

        public void PutTextureOntoBoard(Texture2D tTexture, int rowIndex, int columnIndex)
        {
            Tile t = this.TheBoard[rowIndex, columnIndex];
            t.TheTexture = tTexture;
        }

        // for calculating the indices intot he game board array.
        public int CalculateYIndex(int mouseY)
        {
            int putInGameArrayY = mouseY / this.TileHeight;
            return putInGameArrayY;
        }

        // This method takes a onScreenXCoordinate from the screen and transforms it into an index
        // into an instance of the this Board class.
        public int CalculateXIndex(int onScreenXCoordinate, int screenXOffset)
        {
            int putInGameArrayX = (onScreenXCoordinate - screenXOffset - this.BoardMarginX) / this.TileWidth;
            return putInGameArrayX;
        }

        public Texture2D GetTextureAt(int putY, int putX)
        {
            return this.TheBoard[putY, putX].TheTexture;
        }

        // For the Mouse to Screen mapping
        public int CalculateScreenCoordinateXFromMousePosition(int mouseX, int screenXOffset)
        {
            return ((mouseX - screenXOffset) / this.TileWidth) * this.TileWidth + screenXOffset;
        }

        public int CalculateScreenCoordinateYFromMousePosition(int mouseY)
        {
            return (mouseY / this.TileHeight) * this.TileHeight;
        }

        public void ReadInBoardConfigurationOrUseDefault(String path)
        {
            Debug.Assert(path != null && !path.Equals(""), "path can not be null or empty!");

            // Load the default game board configuration if the config file doesn't exist.
            if (!File.Exists(path))
            {
                this.BoardHeight = 20*24;  // Screen's height is 480 BoardHeight and Tile Height will be used to calc the number of tiles across array will be
                this.BoardWidth = 29*80;   // Screen's width is 800 BoardWidth and Tile Height will be used to calc the # of tiles across the array will be

                this.TileHeight = 20;
                this.TileWidth = 20;

                this.NumberOfVerticalTiles = this.BoardHeight / this.TileHeight;
                this.NumberOfHorizontalTiles = this.BoardWidth / this.TileWidth;

                this.TheBoard = new Tile[this.NumberOfVerticalTiles, this.NumberOfHorizontalTiles];
                
                for (int row = 0; row < this.TheBoard.GetLength(0); row++)
                {
                    for (int column = 0; column < this.TheBoard.GetLength(1); column++)
                    {
                        Tile t = new Tile(null,                  // blank tile
                        
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
                // Write out the default config of the board
                WriteOutTheGameBoard(path);
            }
            else
            {
                // A config file exists for the board so load it now!
                String[] configStringSplitRay = File.ReadAllLines(path);

                this.BoardHeight = Convert.ToInt32(configStringSplitRay[0].Split(':')[1]);  // defaults to 480
                this.BoardWidth = Convert.ToInt32(configStringSplitRay[1].Split(':')[1]);   // defaults to 800

                this.TileHeight = Convert.ToInt32(configStringSplitRay[2].Split(':')[1]);
                this.TileWidth = Convert.ToInt32(configStringSplitRay[3].Split(':')[1]);
                
                this.NumberOfVerticalTiles = this.BoardHeight / this.TileHeight;
                this.NumberOfHorizontalTiles = this.BoardWidth / this.TileWidth;

                this.TheBoard= new Tile[this.NumberOfVerticalTiles, this.NumberOfHorizontalTiles];
                for (int i = 0; i < this.TheBoard.GetLength(0); i++)
                {
                    String[] stringGameBoardRay = configStringSplitRay[4+i].Split(',');

                    for (int j = 0; j < this.TheBoard.GetLength(1); j++)
                    {
                        Tile t = new Tile(
                                      TextureCache.getInstance().GetTexture2DFromStringBoardArray(stringGameBoardRay[j]),                  // blank tile
                        
                                      j,                // remembert hese are swapped in array!!!
                                      i,

                                      this.TileWidth,        // width
                                      this.TileHeight,       // height
                                      
                                      0,                    // startBoundaryX
                                      0,                    // startBoundaryY
                                      
                                      this.TileWidth-1,     // endBoundaryX
                                      this.TileHeight-1);   // endBoundaryY
                        
                        this.TheBoard[i, j] = t;
                    }
                }

            } // end else
        }

        public void WriteOutTheGameBoard(String path)
        {
            Debug.Assert(path != null && !path.Equals(""), "path can not be null or empty!");

            using (FileStream fs = File.Create(path))
            {
                AddText(fs, "screenHeight:" + this.BoardHeight);
                AddText(fs, "\n");
                AddText(fs, "screenWidth:" + this.BoardWidth);
                AddText(fs, "\n");

                AddText(fs, "tileHeight:" + this.TileHeight);
                AddText(fs, "\n");
                AddText(fs, "tileWidth:" + this.TileWidth);
                AddText(fs, "\n");

                for (int i = 0; i < this.TheBoard.GetLength(0); i++)
                {
                    for (int j = 0; j < this.TheBoard.GetLength(1); j++)
                    {
                        Tile gBTile = this.TheBoard[i, j];
                        if (gBTile.TheTexture == null)
                        {
                            AddText(fs, "null");
                        }
                        else
                        {
                            AddText(fs, TextureCache.getInstance().GetStringFilenameFromTexture2DForBoard(gBTile.TheTexture));
                        }

                        if (j != this.TheBoard.GetLength(1) - 1)
                        {
                            AddText(fs, ",");
                        } // end if
                    } // end inner for
                    AddText(fs, "\n");
                } // end outer for
            } // end using
        } // end method


        private static void AddText(FileStream fs, string value)
        {
            Debug.Assert(fs.CanWrite, "FileStream fs nust be writable1");
            Debug.Assert(value != null, "value being written can not be null!");

            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }

        public void PutMultiTileInBoard(MultiTexture mTile, int rowIndex, int columnIndex)
        {
            Debug.Assert(mTile != null, "MultiTexture mTile can not be null!");

            for (int i = 0; i < mTile.NumberOfVerticalTiles; i++)
            {
                for (int j = 0; j < mTile.NumberOfHorizontalTiles; j++)
                {
                    if (rowIndex + i >= 0 && columnIndex >= 0 && rowIndex < this.TheBoard.GetLength(0) && columnIndex < this.TheBoard.GetLength(1))
                    {
                        Tile t = this.TheBoard[rowIndex + i, columnIndex + j];
                        t.TheTexture = mTile.TextureToRepeat;
                    }
                }
            }
        }
    } // end class Board
}

