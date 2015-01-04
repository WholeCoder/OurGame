using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.Commands;
using WindowsGame1;
using OurGame.WindowsGameLibrary1;

namespace OurGame.GameStates
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
        string pathToTextureCacheConfig = @"BoardTextureCache.txt";
        string pathToSpriteTextureCacheConfig = @"SpriteTextureCache.txt";

        MultiTexture multiTexture;
        int multiTextureWidthHeight = 1;

        Stack<OurGame.Commands.ICommand> undoStack; // Holds the executed PlaceTileOnBoardCommands to undo then if we hit z
        Stack<OurGame.Commands.ICommand> undoDeleteBoardStack;

        MouseState lastMouseState;
        MouseState currentMouseState;

        bool rightMouseClickOccurred = false;
        bool leftMouseClickOccurred = false;

        private int previousScrollValue;

        KeyboardState oldKeyboardState;

        // Call setStateWhenUpdating on this instance variable to change to a different game state.
        public Game1 OurGame { get; set; }

        // Used to reload the contend in the board for the playGameState
        public ContentManager Content { get; set; }
        
        public EditBoardState()
        {
        }

        public override void Initialize(Game1 ourGame)
        {
            this.OurGame = ourGame;
            undoStack = new Stack<OurGame.Commands.ICommand>();
            undoDeleteBoardStack = new Stack<OurGame.Commands.ICommand>();
            previousScrollValue = Mouse.GetState().ScrollWheelValue;
        }

        public override void LoadContent(ContentManager Content)
        {
            tCache = new TextureCache(pathToTextureCacheConfig, pathToSpriteTextureCacheConfig, Content);
            board = new Board(pathToSavedGambeBoardConfigurationFile, tCache); // MUST have tCache created before calling this!

            this.Content = Content;

            multiTexture = new MultiTexture(multiTextureWidthHeight, multiTextureWidthHeight, tCache.GetCurrentTexture(), tCache);
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

            if (currentMouseState.ScrollWheelValue < previousScrollValue)
            {
                multiTextureWidthHeight--;
            }
            else if (currentMouseState.ScrollWheelValue > previousScrollValue)
            {
                multiTextureWidthHeight++;
            }
            previousScrollValue = currentMouseState.ScrollWheelValue;

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
                multiTexture = new MultiTexture(multiTextureWidthHeight, multiTextureWidthHeight, tCache.GetCurrentTexture(), tCache);
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
                    OurGame.Commands.ICommand ptMultiOnBoardCommand = new PlaceMultiTextureOnBoardCommand(this.board, ms.X, ms.Y, this.multiTexture.TextureToRepeat, screenXOffset, this.multiTexture.NumberOfHorizontalTiles, this.multiTexture.NumberOfVerticalTiles);
                    ptMultiOnBoardCommand.Execute();

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

            if (screenXOffset <= -this.board.BoardWidth)
            {
                screenXOffset = -this.board.BoardWidth;
            }

            if (screenXOffset >= 0)
            {
                screenXOffset = 0;
            }

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

            multiTexture = new MultiTexture(multiTextureWidthHeight, multiTextureWidthHeight, tCache.GetCurrentTexture(), tCache);

            // Do undo place tile command
            if (newKeyboardState.IsKeyDown(Keys.Z) && oldKeyboardState.IsKeyUp(Keys.Z))
            {
                if (this.undoStack.Count() != 0)
                {
                    OurGame.Commands.ICommand ptoBoardCommandUndo = this.undoStack.Pop();
                    ptoBoardCommandUndo.Undo();
                }
            }

            // Save to MyLevel.txt.
            if (newKeyboardState.IsKeyDown(Keys.S) && oldKeyboardState.IsKeyUp(Keys.S))
            {
                SaveCurrentBoard();
            }

            // Delete MyLevel.txt.
            if (newKeyboardState.IsKeyDown(Keys.D) && oldKeyboardState.IsKeyUp(Keys.D))
            {
                OurGame.Commands.ICommand dbCommand = new DeleteBoardCommand(pathToSavedGambeBoardConfigurationFile, tCache, this.board, this);
                dbCommand.Execute();                
                
                // Add this delete to the undo history.
                undoDeleteBoardStack.Push(dbCommand);

                // Make sure we reset the undo history.
                undoStack = new Stack<OurGame.Commands.ICommand>();
            }

            // Press U to undo a board deletion.
            if (newKeyboardState.IsKeyDown(Keys.U) && oldKeyboardState.IsKeyUp(Keys.U))
            {
                if (this.undoDeleteBoardStack.Count() != 0)
                {
                    OurGame.Commands.ICommand dbCommand = this.undoDeleteBoardStack.Pop();
                    dbCommand.Undo();
                }
            }

            if (newKeyboardState.IsKeyDown(Keys.Q) && oldKeyboardState.IsKeyUp(Keys.Q))
            {
                this.OurGame.Exit();
            }

            // Press B for the blank state.  Just for testing.
            if (newKeyboardState.IsKeyDown(Keys.B) && oldKeyboardState.IsKeyUp(Keys.B))
            {
                this.SaveBoardToDiskAndReloadPlayGameState(gameTime);
                this.OurGame.SetStateWhenUpdating(this.OurGame.blankState, gameTime);
            }

            // Press P for play game state.
            if (newKeyboardState.IsKeyDown(Keys.P) && oldKeyboardState.IsKeyUp(Keys.P))
            {
                this.SaveBoardToDiskAndReloadPlayGameState(gameTime);
            }

            oldKeyboardState = newKeyboardState;  // set the new state as the old state for next time

        }

        private void SaveBoardToDiskAndReloadPlayGameState(Microsoft.Xna.Framework.GameTime gameTime)
        {
            this.SaveCurrentBoard();
            this.OurGame.playGameState.LoadContent(Content);
            this.OurGame.SetStateWhenUpdating(this.OurGame.playGameState, gameTime);
        }

        public void SaveCurrentBoard()
        {
            if (File.Exists(pathToSavedGambeBoardConfigurationFile))
            {
                File.Delete(pathToSavedGambeBoardConfigurationFile);
            }

            Console.WriteLine("Saving to " + pathToSavedGambeBoardConfigurationFile);

            this.board.WriteOutDimensionsOfTheGameBoard(pathToSavedGambeBoardConfigurationFile, tCache);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.board.DrawBoard(spriteBatch, screenXOffset, true);  // screenXOffset scrolls the board left and right!

            this.multiTexture.Draw(spriteBatch, mouseCursorLockedToNearestGridPositionVector);
        }
    } // end class
} // end namespace
