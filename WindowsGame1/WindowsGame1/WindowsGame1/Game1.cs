using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Command;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Level editor instance variables
        int tileWidth = 20;
        int tileHeight = 20;

        //Default width X height it starts in a 800x480 resolution.
        int screenWidth;
        int screenHeight;

        int numberOfHorizontalTiles;
        int numberofVerticalTiles;

        // This is the position of the mouse "locked" onto a grid position.
        Vector2 mouseCursorLockedToNearestGridPositionVector;

        // Holds textures so they aren't re-created.
        TextureCache tCache;

        MouseState lastMouseState;
        MouseState currentMouseState;

        bool rightMouseClickOccurred = true;
        bool leftMouseClickOccurred = false;

        Texture2D[,] gameBoard;

        // This instance variable lets us scroll the board horizontally.
        int screenXOffset = 0;
        int scrollAmount = 5;

        Stack<PlaceTileOnBoardCommand> undoStack; // Holds the executed PlaceTileOnBoardCommands to undo then if we hit z

        KeyboardState oldKeyboardState;
        
        // This is the name the gameboard is saved to when S is pressed.
        string path = @"MyLevel.txt";

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public void ReadInCurrentBoardAndDimensions()
        {

            tCache = new TextureCache(Content); // Loads the texture cache with DEFAULT Textures!!!!

            // Load the default game board configuration if the config file doesn't exist.
            if (!File.Exists(path))
            {
                screenHeight = Window.ClientBounds.Height;  // defaults to 480
                screenWidth = Window.ClientBounds.Width;   // defaults to 800

                numberofVerticalTiles = screenHeight / tileHeight;
                numberOfHorizontalTiles = screenWidth / tileWidth;

                lastMouseState = Mouse.GetState();

                gameBoard = new Texture2D[numberofVerticalTiles, numberOfHorizontalTiles];
                for (int i = 0; i < gameBoard.GetLength(0); i++)
                {
                    for (int j = 0; j < gameBoard.GetLength(1); j++)
                    {
                        gameBoard[i, j] = null;
                    }
                }

                // Write out the default dimensions of the board
                WriteOutDimensionsOfTheGameBoard(path);
            }
            else
            {
                String configurationString = "";
                //Open the stream and read it back. 
                using (FileStream fs = File.OpenRead(path))
                {
                    byte[] b = new byte[1024];
                    UTF8Encoding temp = new UTF8Encoding(true);
                    while (fs.Read(b, 0, b.Length) > 0)
                    {
                        configurationString += temp.GetString(b);
                        //Console.WriteLine(temp.GetString(b));
                    }
                }

                String[] configStringSplitRay = configurationString.Split('\n');

                Console.WriteLine("------------------");

                screenHeight = Convert.ToInt32(configStringSplitRay[0].Split(':')[1]);  // defaults to 480
                screenWidth = Convert.ToInt32(configStringSplitRay[1].Split(':')[1]);   // defaults to 800

                Console.WriteLine("screenHeight == "+screenHeight);
                Console.WriteLine("screenWidth == "+screenWidth);

                tileHeight = Convert.ToInt32(configStringSplitRay[2].Split(':')[1]);
                tileWidth = Convert.ToInt32(configStringSplitRay[3].Split(':')[1]);

                Console.WriteLine("tileHeight == "+tileHeight);
                Console.WriteLine("TileWidth == "+tileWidth);

                int numberOfTileTextures = Convert.ToInt32(configStringSplitRay[4].Split(':')[1]);
                String[] texStringRay = new String[numberOfTileTextures];
                for (int i = 0; i < texStringRay.Length; i++)
                {
                    texStringRay[i] = configStringSplitRay[5+i];
                }

                tCache.loadTheseTextures(Content, texStringRay);

                numberofVerticalTiles = screenHeight / tileHeight;
                numberOfHorizontalTiles = screenWidth / tileWidth;

                lastMouseState = Mouse.GetState();

                gameBoard = new Texture2D[numberofVerticalTiles, numberOfHorizontalTiles];
                for (int i = 0; i < gameBoard.GetLength(0); i++)
                {
                    String[] stringGameBoardRay = configStringSplitRay[5+numberOfTileTextures+i].Split(',');

                    for (int j = 0; j < gameBoard.GetLength(1); j++)
                    {
                        gameBoard[i, j] = tCache.GetTexture2DFromString(stringGameBoardRay[j]);
                    }
                }

            } // end else
        }

        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }

        public void WriteOutDimensionsOfTheGameBoard(String path)
        {
            using (FileStream fs = File.Create(path))
            {
                AddText(fs, "screenHeight:" + screenHeight); 
                AddText(fs, "\n");
                AddText(fs, "screenWidth:" + screenWidth);      
                AddText(fs, "\n");

                AddText(fs, "tileHeight:" + tileHeight); 
                AddText(fs, "\n");
                AddText(fs, "tileWidth:" + tileWidth); 
                AddText(fs, "\n");

                AddText(fs, "numberOfTileTextures:" + 2); 
                AddText(fs, "\n");
                AddText(fs, "Images/tile"); 
                AddText(fs, "\n");
                AddText(fs, "Images/tile2"); 
                AddText(fs, "\n");

                for (int i = 0; i < gameBoard.GetLength(0); i++)
                {
                    for (int j = 0; j < gameBoard.GetLength(1); j++)
                    {
                        Texture2D gBTile = gameBoard[i, j];
                        if (gBTile == null)
                        {
                            AddText(fs, "null");
                        }
                        else
                        {
                            AddText(fs, tCache.GetStringFilenameFromTexture2D(gBTile));
                        }

                        if (j != gameBoard.GetLength(1) - 1)
                        {
                            AddText(fs, ",");
                        } // end if
                    } // end inner for
                    AddText(fs, "\n");
                } // end outer for
            } // end using
        } // end method

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;

            undoStack = new Stack<PlaceTileOnBoardCommand>();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ReadInCurrentBoardAndDimensions();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            MouseState ms = Mouse.GetState();


            // The active state from the last frame is now old
            lastMouseState = currentMouseState;

            // Get the mouse state relevant for this frame
            currentMouseState = Mouse.GetState();

            // Recognize a single click of the right mouse button
            if (lastMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed)
            {
                // React to the click
                // ...
                rightMouseClickOccurred = true;
            }

            if (rightMouseClickOccurred)
            {
                // Flip to the next texture under the mouse pointer.
                this.tCache.NextTexture();
                rightMouseClickOccurred = false;
            }

            // Recognize a single click of the leftmouse button
            if (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
            {
                // React to the click
                // ...
                leftMouseClickOccurred = true;
            }

            if (leftMouseClickOccurred)
            {
                int putInGameArrayY = ms.Y / tileHeight;
                int putInGameArrayX = (ms.X - screenXOffset) / tileWidth;

                if (putInGameArrayY < this.gameBoard.GetLength(0) && putInGameArrayX < this.gameBoard.GetLength(1))
                {

                    PlaceTileOnBoardCommand ptOnBoardCommand = new PlaceTileOnBoardCommand(this.gameBoard, putInGameArrayX, putInGameArrayY, this.tCache.GetCurrentTexture());
                    ptOnBoardCommand.execute();

                    this.undoStack.Push(ptOnBoardCommand);
                }

                leftMouseClickOccurred = false;
            }

            int putX = ((ms.X - screenXOffset) / tileWidth) * tileWidth + screenXOffset;
            int putY = (ms.Y / tileHeight) * tileHeight;

            mouseCursorLockedToNearestGridPositionVector = new Vector2(putX, putY);

            // Move game board.
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Right))
            {
                screenXOffset -= scrollAmount;
            }

            if (keyState.IsKeyDown(Keys.Left))
            {
                screenXOffset += scrollAmount;
            }

            if (screenXOffset <= -screenWidth)
            {
                screenXOffset = -screenWidth;
            }

            if (screenXOffset >= 0)
            {
                screenXOffset = 0;
            }


            // Do undo place tile command
            KeyboardState newKeyboardState = Keyboard.GetState();  // get the newest state

            // handle the input
            if (newKeyboardState.IsKeyDown(Keys.Z) && oldKeyboardState.IsKeyUp(Keys.Z))
            {
                if (this.undoStack.Count() != 0)
                {
                    PlaceTileOnBoardCommand ptoBoardCommandUndo = this.undoStack.Pop();
                    ptoBoardCommandUndo.undo();
                }
            }

            if (newKeyboardState.IsKeyDown(Keys.S) && oldKeyboardState.IsKeyUp(Keys.S))
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                WriteOutDimensionsOfTheGameBoard(path);
            }
            oldKeyboardState = newKeyboardState;  // set the new state as the old state for next time

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            for (int y = 0; y < screenHeight; y += tileHeight)
            {
                C3.XNA.Primitives2D.DrawLine(spriteBatch, new Vector2(0.0f + screenXOffset, y), new Vector2(screenWidth + screenXOffset, y), Color.White);
            }

            for (int x = 0; x < screenWidth+1; x += tileWidth)
            {
                C3.XNA.Primitives2D.DrawLine(spriteBatch, new Vector2(x + screenXOffset, 0.0f), new Vector2(x + screenXOffset, screenHeight), Color.White);
            }

            for (int i = 0; i < gameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < gameBoard.GetLength(1); j++)
                {
                    if (gameBoard[i, j] != null)
                    {
                        Vector2 tilePosition = new Vector2(j * tileWidth + screenXOffset, i * tileHeight);
                        spriteBatch.Draw(gameBoard[i, j], tilePosition, Color.White);
                    }
                }
            }

            spriteBatch.Draw(tCache.GetCurrentTexture(), mouseCursorLockedToNearestGridPositionVector, Color.White);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
