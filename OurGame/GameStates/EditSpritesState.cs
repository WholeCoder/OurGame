using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OurGame.Commands.EditBoardCommands;

// My usings.
using OurGame.WindowsGame1;
using OurGame.OurGameLibrary;
using OurGame.Sprites;

namespace OurGame.GameStates
{
    public class EditSpritesState : State
    {
        private Board _board;
        
        // This is the position of the mouse "locked" onto a grid position.
        private Vector2 _mouseCursorLockedToNearestGridPositionVector;


        private AnimatedSprite Player { get; set; }
        private SpriteManager _spriteManager;

        // This instance variable lets us scroll the board horizontally.
        private int _screenXOffset = 0;
        private const int ScrollAmount = 5;

        // This is the name the gameboard is saved to when S is pressed.
        private const string PathToSavedGambeBoardConfigurationFile = @"MyLevel.txt";

        private MultiTexture _multiTexture;
        private int _multiTextureWidthHeight = 1;

        private Stack<OurGame.Commands.ICommand> _undoStack; // Holds the executed PlaceTileOnBoardCommands to undo then if we hit z
        private Stack<OurGame.Commands.ICommand> _undoDeleteBoardStack;

        private MouseState _lastMouseState;
        private MouseState _currentMouseState;

        private bool _rightMouseClickOccurred = false;
        private bool _leftMouseClickOccurred = false;

        private int _previousScrollValue;

        private KeyboardState _oldKeyboardState;

        // Call setStateWhenUpdating on this instance variable to change to a different game state.
        public Game1 OurGame { get; set; }

        // Used to reload the contend in the board for the playGameState
        private ContentManager Content { get; set; }

        public EditSpritesState()
        {
        }

        public override void Initialize(Game1 ourGame)
        {
            Debug.Assert(ourGame != null, "ourGame can't be null!");

            this.OurGame = ourGame;
            _undoStack = new Stack<OurGame.Commands.ICommand>();
            _undoDeleteBoardStack = new Stack<OurGame.Commands.ICommand>();
            _previousScrollValue = Mouse.GetState().ScrollWheelValue;
        }

        protected override void LoadStatesContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            Debug.Assert(Content != null," Content can't be null!");

            _board = new Board(PathToSavedGambeBoardConfigurationFile);

            this.Content = Content;

            //_multiTexture = new MultiTexture(_multiTextureWidthHeight, _multiTextureWidthHeight, TextureCache.getInstance().GetCurrentTexture());
            Player = new UserControlledSprite("UserControlledSpriteConfig.txt", _board, this);

            this._spriteManager = new SpriteManager("MyLevelsEnemySpritesList.txt", _board, this);
        }

        public void SaveSpritesToDiskAndLoadItIntoPlayGameState(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");

            this.SaveCurrentSprites();
            this.OurGame.PlayGameState.LoadContent(Content);
        }


        public override void UnloadContent()
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can't be null!");

            MouseState ms = Mouse.GetState();

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
            if (_lastMouseState.RightButton == ButtonState.Released && _currentMouseState.RightButton == ButtonState.Pressed)
            {
                // React to the click
                // ...
                _rightMouseClickOccurred = true;
            }

            if (_rightMouseClickOccurred)
            {
                // Flip to the next texture under the mouse pointer.
                /*TextureCache.getInstance().NextTexture();
                _multiTexture = new MultiTexture(_multiTextureWidthHeight, _multiTextureWidthHeight, TextureCache.getInstance().GetCurrentTexture());
                 */
                _rightMouseClickOccurred = false;
            }

            // Recognize a single click of the leftmouse button
            if (_lastMouseState.LeftButton == ButtonState.Released && _currentMouseState.LeftButton == ButtonState.Pressed)
            {
                // React to the click
                // ...
                _leftMouseClickOccurred = true;
            }

            int putX = this._board.CalculateScreenCoordinateXFromMousePosition(ms.X, _screenXOffset);
            int putY = this._board.CalculateScreenCoordinateYFromMousePosition(ms.Y);

            _mouseCursorLockedToNearestGridPositionVector = new Vector2(putX, putY);

            if (_leftMouseClickOccurred)
            {
                if (this._board.CalculateYIndex(ms.Y) < this._board.TheBoard.GetLength(0) && this._board.CalculateXIndex(ms.X, _screenXOffset) < this._board.TheBoard.GetLength(1)
                    && this._board.CalculateYIndex(ms.Y) >= 0 && this._board.CalculateXIndex(ms.X, _screenXOffset) >= 0)
                {
                   /* OurGame.Commands.ICommand ptMultiOnBoardCommand = new PlaceMultiTextureOnBoardCommand(this._board, ms.X, ms.Y, this._multiTexture.TextureToRepeat, _screenXOffset, this._multiTexture.NumberOfHorizontalTiles, this._multiTexture.NumberOfVerticalTiles);
                    ptMultiOnBoardCommand.Execute();

                    this._undoStack.Push(ptMultiOnBoardCommand);
                    */
                    this.Player.CurrentPosition.X = putX;
                    this.Player.CurrentPosition.Y = putY;
                    this.Player.InitialPosition.X = putX;
                    this.Player.InitialPosition.Y = putY;
                    this.Player.BoundingRectangle.X = putX;
                    this.Player.BoundingRectangle.Y = putY;

                    this._spriteManager.AddSprite(this.Player);
                    this.Player = new UserControlledSprite("UserControlledSpriteConfig.txt", _board, this);
                }

                _leftMouseClickOccurred = false;
            }

            // Move game board.
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Right))
            {
                _screenXOffset -= ScrollAmount;
            }

            if (keyState.IsKeyDown(Keys.Left))
            {
                _screenXOffset += ScrollAmount;
            }

            if (_screenXOffset <= -this._board.BoardWidth+Board.SCREEN_WIDTH)
            {
                _screenXOffset = -this._board.BoardWidth+Board.SCREEN_WIDTH;
            }

            if (_screenXOffset >= 0)
            {
                _screenXOffset = 0;
            }

            KeyboardState newKeyboardState = Keyboard.GetState();  // get the newest state

            if (newKeyboardState.IsKeyDown(Keys.S) && _oldKeyboardState.IsKeyUp(Keys.S))
            {
                this._spriteManager.WriteOutSpritesToAfile();
                Console.WriteLine(("Writing out SPRITES to config fiile"));
            }

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

            _multiTexture = new MultiTexture(_multiTextureWidthHeight, _multiTextureWidthHeight, TextureCache.getInstance().GetCurrentTexture());

            // Do undo place tile command
            if (newKeyboardState.IsKeyDown(Keys.Z) && _oldKeyboardState.IsKeyUp(Keys.Z))
            {
                if (this._undoStack.Count() != 0)
                {
                    OurGame.Commands.ICommand ptoBoardCommandUndo = this._undoStack.Pop();
                    ptoBoardCommandUndo.Undo();
                }
            }

            // Delete MyLevel.txt.
            if (newKeyboardState.IsKeyDown(Keys.D) && _oldKeyboardState.IsKeyUp(Keys.D))
            {
               /* OurGame.Commands.ICommand dbCommand = new DeleteBoardCommand(PathToSavedGambeBoardConfigurationFile, this._board, this);
                dbCommand.Execute();                
                
                // Add this delete to the undo history.
                _undoDeleteBoardStack.Push(dbCommand);

                // Make sure we reset the undo history.
                _undoStack = new Stack<OurGame.Commands.ICommand>();
                */
            }

            // Press U to undo a board deletion.
            if (newKeyboardState.IsKeyDown(Keys.U) && _oldKeyboardState.IsKeyUp(Keys.U))
            {
                if (this._undoDeleteBoardStack.Count() != 0)
                {
                    OurGame.Commands.ICommand dbCommand = this._undoDeleteBoardStack.Pop();
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
            SwitchStateLogic.DoChangeGameStateFromKeyboardLogic(newKeyboardState, _oldKeyboardState, this.OurGame, gameTime);


            _oldKeyboardState = newKeyboardState;  // set the new state as the old state for next time

        }

        public void SaveBoardToDiskAndLoadItIntoPlayGameState(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");

            this.SaveCurrentSprites();
            this.OurGame.PlayGameState.LoadContent(Content);
        }

        public void SaveCurrentSprites()
        {
            this._spriteManager.WriteOutSpritesToAfile();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, SpriteBatch spriteBatch)
        {
            Debug.Assert(gameTime!= null, "gameTime can't be null!");
            Debug.Assert(spriteBatch != null, "spriteBatch can't be null");

            this._board.DrawBoard(spriteBatch, _screenXOffset, true);  // screenXOffset scrolls the board left and right!

            this.Player.Draw(spriteBatch, _mouseCursorLockedToNearestGridPositionVector);

            this._spriteManager.Draw(spriteBatch);
            //this._multiTexture.Draw(spriteBatch, _mouseCursorLockedToNearestGridPositionVector);
        } // end method
    } // end class
} // end namespace