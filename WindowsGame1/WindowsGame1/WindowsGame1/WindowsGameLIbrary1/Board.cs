using System;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGameLibrary1
{

    /*
     * Note: the TheBoard array stores the y coordinate first and then the X coordinate.
     */

    public class Board
    {
        // In pixels.
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }

        public int NumberOfHorizontalTiles { get; set; }
        public int NumberOfVerticalTiles { get; set; }

        public Tile[,] TheBoard { get; set; }

        // The width and height of the individual tiles.
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }


        public Board(String pathToConfigFile, TextureCache tCache)
        {
            this.ReadInBoardConfigurationOrUseDefault(pathToConfigFile, tCache); // tCache must fully loaded to use here!!!!
        } // end constructor

        public void DrawBoard(SpriteBatch spriteBatch, int screenXOffset)
        {
            for (int y = 0; y < this.BoardHeight; y += this.TileHeight)
            {
                C3.XNA.Primitives2D.DrawLine(spriteBatch, new Vector2(0.0f + screenXOffset, y), new Vector2(this.BoardWidth + screenXOffset, y), Color.White);
            }

            for (int x = 0; x < this.BoardWidth + 1; x += this.TileWidth)
            {
                C3.XNA.Primitives2D.DrawLine(spriteBatch, new Vector2(x + screenXOffset, 0.0f), new Vector2(x + screenXOffset, this.BoardHeight), Color.White);
            }

            for (int i = 0; i < this.TheBoard.GetLength(0); i++)
            {
                for (int j = 0; j < this.TheBoard.GetLength(1); j++)
                {
                    if (this.TheBoard[i, j].TheTexture != null)
                    {
                        Vector2 tilePosition = new Vector2(j * this.TileWidth + screenXOffset, i * this.TileHeight);
                        spriteBatch.Draw(this.TheBoard[i, j].TheTexture, tilePosition, Color.White);
                    }
                }
            } // End outer for.

        } // end method DrawBoard

        public void putTextureOntoBoard(Texture2D tTexture, int rowIndex, int columnIndex)
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

        public int CalculateXIndex(int mouseX, int screenXOffset)
        {
            int putInGameArrayX = (mouseX - screenXOffset) / this.TileWidth;
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

        public void ReadInBoardConfigurationOrUseDefault(String path, TextureCache tCache) // tCache must fully loaded to use here!!!!
        {
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
                WriteOutDimensionsOfTheGameBoard(path, tCache);
            }
            else
            {
                // A config file exists for the board so load it now!

                String configurationString = "";  // Holds the entire configuration file.

                // Open the stream and read it back. 
                using (FileStream fs = File.OpenRead(path))
                {
                    byte[] b = new byte[1024];
                    UTF8Encoding temp = new UTF8Encoding(true);
                    while (fs.Read(b, 0, b.Length) > 0)
                    {
                        configurationString += temp.GetString(b);
                    }
                }

                String[] configStringSplitRay = configurationString.Split('\n');

                this.BoardHeight = Convert.ToInt32(configStringSplitRay[0].Split(':')[1]);  // defaults to 480
                this.BoardWidth = Convert.ToInt32(configStringSplitRay[1].Split(':')[1]);   // defaults to 800

                Console.WriteLine("screenHeight == " + this.BoardHeight);
                Console.WriteLine("screenWidth == " + this.BoardWidth);

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
                                      tCache.GetTexture2DFromString(stringGameBoardRay[j]),                  // blank tile
                        
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

        public void WriteOutDimensionsOfTheGameBoard(String path, TextureCache tCache)
        {
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
                            AddText(fs, tCache.GetStringFilenameFromTexture2D(gBTile.TheTexture));
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
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }

        public void PutMultiTileInBoard(MultiTexture mTile, int rowIndex, int columnIndex)
        {
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

