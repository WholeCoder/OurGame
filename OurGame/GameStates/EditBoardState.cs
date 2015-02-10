using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OurGame.Commands;
using OurGame.Commands.EditBoardCommands;
using OurGame.OurGameLibrary;

namespace OurGame.GameStates
{
    public class EditBoardState : State
    {
        private const int ScrollAmount = 5;
        // This is the name the gameboard is saved to when S is pressed.
        private const string PathToSavedGambeBoardConfigurationFile = @"MyLevel.txt";
        private Board _board;
        private MouseState _currentMouseState;
        private SpriteFont _helpFont;
        private MouseState _lastMouseState;
        private bool _leftMouseClickOccurred;
        // This is the position of the mouse "locked" onto a grid position.
        private Vector2 _mouseCursorLockedToNearestGridPositionVector;
        private MultiTexture _multiTexture;
        private int _multiTextureWidthHeight = 1;
        private KeyboardState _oldKeyboardState;
        private int _previousScrollValue;
        private bool _rightMouseClickOccurred;
        // This instance variable lets us scroll the board horizontally.
        private int _screenXOffset;
        private Stack<ICommand> _undoDeleteBoardStack;
        private Stack<ICommand> _undoStack; // Holds the executed PlaceTileOnBoardCommands to undo then if we hit z
        // Call setStateWhenUpdating on this instance variable to change to a different game state.
        private Game1 OurGame { get; set; }
        // Used to reload the contend in the board for the playGameState
        private ContentManager Content { get; set; }

        public override string ToString()
        {
            return "EditBoardState";
        }

        public override void Initialize(Game1 ourGame)
        {
            Debug.Assert(ourGame != null, "ourGame can't be null!");

            OurGame = ourGame;
            _undoStack = new Stack<ICommand>();
            _undoDeleteBoardStack = new Stack<ICommand>();
            _previousScrollValue = Mouse.GetState().ScrollWheelValue;
        }

        protected override void LoadStatesContent(ContentManager Content)
        {
            Debug.Assert(Content != null, " Content can't be null!");

            _board = new Board(PathToSavedGambeBoardConfigurationFile);

            this.Content = Content;

            _multiTexture = new MultiTexture(_multiTextureWidthHeight, _multiTextureWidthHeight,
                TextureCache.getInstance().GetCurrentTexture());

            _helpFont = Content.Load<SpriteFont>(@"fonts\helpfont");
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can't be null!");

            var ms = Mouse.GetState();

            // The active state from the last frame is now old
            _lastMouseState = _currentMouseState;

            // Get the mouse state relevant for this frame
            _currentMouseState = Mouse.GetState();

            if (_currentMouseState.ScrollWheelValue < _previousScrollValue)
            {
                _multiTextureWidthHeight--;
            }
            else if (_currentMouseState.ScrollWheelValue > _previousScrollValue)
            {
                _multiTextureWidthHeight++;
            }
            _previousScrollValue = _currentMouseState.ScrollWheelValue;

            // Recognize a single click of the right mouse button
            if (_lastMouseState.RightButton == ButtonState.Released &&
                _currentMouseState.RightButton == ButtonState.Pressed)
            {
                // React to the click
                // ...
                _rightMouseClickOccurred = true;
            }

            if (_rightMouseClickOccurred)
            {
                // Flip to the next texture under the mouse pointer.
                TextureCache.getInstance().NextTexture();
                _multiTexture = new MultiTexture(_multiTextureWidthHeight, _multiTextureWidthHeight,
                    TextureCache.getInstance().GetCurrentTexture());
                _rightMouseClickOccurred = false;
            }

            // Recognize a single click of the leftmouse button
            if (_lastMouseState.LeftButton == ButtonState.Released &&
                _currentMouseState.LeftButton == ButtonState.Pressed)
            {
                // React to the click
                // ...
                _leftMouseClickOccurred = true;
            }

            if (_leftMouseClickOccurred)
            {
                if (_board.CalculateYIndex(ms.Y) < _board.TheBoard.GetLength(0) &&
                    _board.CalculateXIndex(ms.X, _screenXOffset) < _board.TheBoard.GetLength(1)
                    && _board.CalculateYIndex(ms.Y) >= 0 && _board.CalculateXIndex(ms.X, _screenXOffset) >= 0)
                {
                    ICommand ptMultiOnBoardCommand = new PlaceMultiTextureOnBoardCommand(_board, ms.X, ms.Y,
                        _multiTexture.TextureToRepeat, _screenXOffset, _multiTexture.NumberOfHorizontalTiles,
                        _multiTexture.NumberOfVerticalTiles);
                    ptMultiOnBoardCommand.Execute();

                    _undoStack.Push(ptMultiOnBoardCommand);
                }

                _leftMouseClickOccurred = false;
            }

            var putX = _board.CalculateScreenCoordinateXFromMousePosition(ms.X, _screenXOffset);
            var putY = _board.CalculateScreenCoordinateYFromMousePosition(ms.Y);

            _mouseCursorLockedToNearestGridPositionVector = new Vector2(putX, putY);

            // Move game board.
            var keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Right))
            {
                _screenXOffset -= ScrollAmount;
            }

            if (keyState.IsKeyDown(Keys.Left))
            {
                _screenXOffset += ScrollAmount;
            }

            if (_screenXOffset <= -_board.BoardWidth + Board.SCREEN_WIDTH)
            {
                _screenXOffset = -_board.BoardWidth + Board.SCREEN_WIDTH;
            }

            if (_screenXOffset >= 0)
            {
                _screenXOffset = 0;
            }

            var newKeyboardState = Keyboard.GetState(); // get the newest state

            if (newKeyboardState.IsKeyDown(Keys.PageUp) && _oldKeyboardState.IsKeyUp(Keys.PageUp))
            {
                _multiTextureWidthHeight++;
            }

            if (newKeyboardState.IsKeyDown(Keys.PageDown) && _oldKeyboardState.IsKeyUp(Keys.PageDown))
            {
                _multiTextureWidthHeight--;
            }

            if (_multiTextureWidthHeight <= 0)
            {
                _multiTextureWidthHeight = 1;
            }

            _multiTexture = new MultiTexture(_multiTextureWidthHeight, _multiTextureWidthHeight,
                TextureCache.getInstance().GetCurrentTexture());

            // Do undo place tile command
            if (newKeyboardState.IsKeyDown(Keys.Z) && _oldKeyboardState.IsKeyUp(Keys.Z))
            {
                if (_undoStack.Count() != 0)
                {
                    var ptoBoardCommandUndo = _undoStack.Pop();
                    ptoBoardCommandUndo.Undo();
                }
            }

            // Save to MyLevel.txt.
            if (newKeyboardState.IsKeyDown(Keys.S) && _oldKeyboardState.IsKeyUp(Keys.S))
            {
                SaveCurrentBoard();
            }

            // Delete MyLevel.txt.
            if (newKeyboardState.IsKeyDown(Keys.D) && _oldKeyboardState.IsKeyUp(Keys.D))
            {
                ICommand dbCommand = new DeleteBoardCommand(PathToSavedGambeBoardConfigurationFile, _board, this);
                dbCommand.Execute();

                // Add this delete to the undo history.
                _undoDeleteBoardStack.Push(dbCommand);

                // Make sure we reset the undo history.
                _undoStack = new Stack<ICommand>();
            }

            // Press U to undo a board deletion.
            if (newKeyboardState.IsKeyDown(Keys.U) && _oldKeyboardState.IsKeyUp(Keys.U))
            {
                if (_undoDeleteBoardStack.Count() != 0)
                {
                    var dbCommand = _undoDeleteBoardStack.Pop();
                    dbCommand.Undo();
                }
            }

/*            // Press B for the blank state.  Just for testing.
            if (newKeyboardState.IsKeyDown(Keys.B) && _oldKeyboardState.IsKeyUp(Keys.B))
            {
                this.SaveBoardToDiskAndLoadItIntoPlayGameState(gameTime);
                this.OurGame.SetStateWhenUpdating(this.OurGame.blankState, gameTime);
            }
*/
            SwitchStateLogic.DoChangeGameStateFromKeyboardLogic(newKeyboardState, _oldKeyboardState, OurGame, gameTime);


            _oldKeyboardState = newKeyboardState; // set the new state as the old state for next time
        }

        public void SaveBoardToDiskAndLoadItIntoPlayGameState(GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");

            SaveCurrentBoard();
            //OurGame.PlayGameState.LoadContent(Content);
        }

        public void SaveBoardToDiskAndLoadItIntoEditSpritesState(GameTime gameTi)
        {
            SaveCurrentBoard();
            OurGame.EditSpritesState.LoadContent(Content);
        }

        public void SaveCurrentBoard()
        {
            if (File.Exists(PathToSavedGambeBoardConfigurationFile))
            {
                File.Delete(PathToSavedGambeBoardConfigurationFile);
            }

            Console.WriteLine("Saving to " + PathToSavedGambeBoardConfigurationFile);

            _board.WriteOutTheGameBoard(PathToSavedGambeBoardConfigurationFile);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Debug.Assert(gameTime != null, "gameTime can't be null!");
            Debug.Assert(spriteBatch != null, "spriteBatch can't be null");

            _board.DrawBoard(spriteBatch, _screenXOffset, true); // screenXOffset scrolls the board left and right!

            _multiTexture.Draw(spriteBatch, _mouseCursorLockedToNearestGridPositionVector);

            spriteBatch.DrawString(_helpFont, "Edit Board Mode",
                new Vector2(20, 10), Color.Black, 0, Vector2.Zero,
                1, SpriteEffects.None, 1);
        }
    } // end class
} // end namespace