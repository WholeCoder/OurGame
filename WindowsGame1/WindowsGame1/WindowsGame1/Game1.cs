using System;
using System.IO;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using Command;

namespace WindowsGameLibrary1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        Board board;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // This is the position of the mouse "locked" onto a grid position.
        Vector2 mouseCursorLockedToNearestGridPositionVector;

        // Holds textures so they aren't re-created.
        TextureCache tCache;

        MouseState lastMouseState;
        MouseState currentMouseState;

        bool rightMouseClickOccurred = true;
        bool leftMouseClickOccurred = false;

        // This instance variable lets us scroll the board horizontally.
        int screenXOffset = 0;
        int scrollAmount = 5;

        Stack<Command.Command> undoStack; // Holds the executed PlaceTileOnBoardCommands to undo then if we hit z

        KeyboardState oldKeyboardState;
        
        // This is the name the gameboard is saved to when S is pressed.
        string pathToSavedGambeBoardConfigurationFile = @"MyLevel.txt";

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


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

            undoStack = new Stack<Command.Command>();

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
            tCache = new TextureCache("TextureCache.txt",Content);
            board = new Board("MyLevel.txt", tCache);
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
                if (this.board.CalculateYIndex(ms.Y) < this.board.TheBoard.GetLength(0) && this.board.CalculateXIndex(ms.X,screenXOffset) < this.board.TheBoard.GetLength(1))
                {

                    Command.Command ptOnBoardCommand = new PlaceTileOnBoardCommand(this.board, ms.X, ms.Y, this.tCache.GetCurrentTexture(),screenXOffset);
                    ptOnBoardCommand.execute();

                    this.undoStack.Push(ptOnBoardCommand);
                }

                leftMouseClickOccurred = false;
            }

            int putX = this.board.CalculateScreenCoordinateXFromMousePosition(ms.X, screenXOffset);
            int putY = this.board.CalculateScreenCoordinateYFromMousePosition(ms.Y);

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

            if (screenXOffset <= -this.board.ScreenWidth)
            {
                screenXOffset = -this.board.ScreenWidth;
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
                    Command.Command ptoBoardCommandUndo = this.undoStack.Pop();
                    ptoBoardCommandUndo.undo();
                }
            }

            // Save to MyLevel.txt.
            if (newKeyboardState.IsKeyDown(Keys.S) && oldKeyboardState.IsKeyUp(Keys.S))
            {
                if (File.Exists(pathToSavedGambeBoardConfigurationFile))
                {
                    File.Delete(pathToSavedGambeBoardConfigurationFile);
                }

                Console.WriteLine("Saving to "+pathToSavedGambeBoardConfigurationFile);

                this.board.WriteOutDimensionsOfTheGameBoard(pathToSavedGambeBoardConfigurationFile, tCache);
            }

            // Delete MyLevel.txt.
            if (newKeyboardState.IsKeyDown(Keys.D) && oldKeyboardState.IsKeyUp(Keys.D))
            {
                if (File.Exists(pathToSavedGambeBoardConfigurationFile))
                {
                    File.Delete(pathToSavedGambeBoardConfigurationFile);
                }

                this.board.ReadInBoardConfigurationOrUseDefault("MyLevel.txt", tCache);
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

            this.board.DrawBoard(spriteBatch, screenXOffset);

            spriteBatch.Draw(tCache.GetCurrentTexture(), mouseCursorLockedToNearestGridPositionVector, Color.White);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
