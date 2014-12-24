using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using Command;
using WindowsGameLibrary1;

namespace GameState
{
    public class EditBoardState : State
    {
        Board board;

        // This is the position of the mouse "locked" onto a grid position.
        Vector2 mouseCursorLockedToNearestGridPositionVector;

        // Holds textures so they aren't re-created.
        TextureCache tCache;

        // This instance variable lets us scroll the board horizontally.
        int screenXOffset = 0;
        int scrollAmount = 5;

        // This is the name the gameboard is saved to when S is pressed.
        string pathToSavedGambeBoardConfigurationFile = @"MyLevel.txt";
        string pathToTextureCacheConfig = @"TextureCache.txt";

        MultiTexture multiTexture;
        int multiTextureWidthHeight = 1;

        Stack<Command.Command> undoStack; // Holds the executed PlaceTileOnBoardCommands to undo then if we hit z

        MouseState lastMouseState;
        MouseState currentMouseState;

        bool rightMouseClickOccurred = true;
        bool leftMouseClickOccurred = false;

        KeyboardState oldKeyboardState;

        // Call setState on this instance variable to change to a different game state.
        // We might want to call Update() on the new state too.
        public Game OurGame { get; set; }

        public EditBoardState()
        {
        }

        public override void Initialize(Game ourGame)
        {
            this.OurGame = ourGame;
            undoStack = new Stack<Command.Command>();
        }

        public override void LoadContent(ContentManager Content)
        {
            tCache = new TextureCache(pathToTextureCacheConfig, Content);
            board = new Board(pathToSavedGambeBoardConfigurationFile, tCache); // MUST have tCache created before calling this!

            multiTexture = new MultiTexture(multiTextureWidthHeight, multiTextureWidthHeight, tCache.GetCurrentTexture());
        }

        public override void UnloadContent()
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
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
                multiTexture = new MultiTexture(multiTextureWidthHeight, multiTextureWidthHeight, tCache.GetCurrentTexture());
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
                if (this.board.CalculateYIndex(ms.Y) < this.board.TheBoard.GetLength(0) && this.board.CalculateXIndex(ms.X, screenXOffset) < this.board.TheBoard.GetLength(1)
                    && this.board.CalculateYIndex(ms.Y) >= 0 && this.board.CalculateXIndex(ms.X, screenXOffset) >= 0)
                {
                    Command.Command ptMultiOnBoardCommand = new PlaceMultiTextureOnBoardCommand(this.board, ms.X, ms.Y, this.multiTexture.TextureToRepeat, screenXOffset, this.multiTexture.NumberOfHorizontalTiles, this.multiTexture.NumberOfVerticalTiles);
                    ptMultiOnBoardCommand.execute();

                    this.undoStack.Push(ptMultiOnBoardCommand);
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

            if (newKeyboardState.IsKeyDown(Keys.PageUp) && oldKeyboardState.IsKeyUp(Keys.PageUp))
            {
                multiTextureWidthHeight++;
            }

            if (newKeyboardState.IsKeyDown(Keys.PageDown) && oldKeyboardState.IsKeyUp(Keys.PageDown))
            {
                multiTextureWidthHeight--;
            }

            if (multiTextureWidthHeight <= 0)
            {
                multiTextureWidthHeight = 1;
            }

            multiTexture = new MultiTexture(multiTextureWidthHeight, multiTextureWidthHeight, tCache.GetCurrentTexture());

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

                Console.WriteLine("Saving to " + pathToSavedGambeBoardConfigurationFile);

                this.board.WriteOutDimensionsOfTheGameBoard(pathToSavedGambeBoardConfigurationFile, tCache);
            }

            // Delete MyLevel.txt.
            if (newKeyboardState.IsKeyDown(Keys.D) && oldKeyboardState.IsKeyUp(Keys.D))
            {
                if (File.Exists(pathToSavedGambeBoardConfigurationFile))
                {
                    File.Delete(pathToSavedGambeBoardConfigurationFile);
                }

                this.board.ReadInBoardConfigurationOrUseDefault(pathToSavedGambeBoardConfigurationFile, tCache);

                // Make sure we reset the undo history.
                undoStack = new Stack<Command.Command>();
            }

            if (newKeyboardState.IsKeyDown(Keys.Q) && oldKeyboardState.IsKeyUp(Keys.Q))
            {
                this.OurGame.Exit();
            }

            oldKeyboardState = newKeyboardState;  // set the new state as the old state for next time

        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.board.DrawBoard(spriteBatch, screenXOffset);  // screenXOffset scrolls the board left and right!

            this.multiTexture.Draw(spriteBatch, mouseCursorLockedToNearestGridPositionVector);
        }
    }
}
