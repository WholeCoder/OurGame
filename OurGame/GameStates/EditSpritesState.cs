﻿using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OurGame.OurGameLibrary;
using OurGame.Sprites;

namespace OurGame.GameStates
{
    public class EditSpritesState : State
    {
        // This instance variable lets us scroll the board horizontally.
        private const int ScrollAmount = 5;
        // This is the name the gameboard is saved to when S is pressed.
        private const string PathToSavedGambeBoardConfigurationFile = @"MyLevel.txt";
        private Board _board;
        private MouseState _currentMouseState;
        public SpriteFont _helpFont;
        private bool _isUserSprite;
        private MouseState _lastMouseState;
        private bool _leftMouseClickOccurred;
        // This is the position of the mouse "locked" onto a grid position.
        private Vector2 _mouseCursorLockedToNearestGridPositionVector;
        private KeyboardState _oldKeyboardState;
        private AnimatedSprite _player;
        private int _previousScrollValue;
        private bool _rightMouseClickOccurred;
        
        private SpriteManager _spriteManager;
        // Call setStateWhenUpdating on this instance variable to change to a different game state.
        public Game1 OurGame { get; set; }
        // Used to reload the contend in the board for the playGameState
        private ContentManager Content { get; set; }

        public override void Initialize(Game1 ourGame)
        {
            Debug.Assert(ourGame != null, "ourGame can't be null!");

            OurGame = ourGame;
            _previousScrollValue = Mouse.GetState().ScrollWheelValue;
        }

        protected override void LoadStatesContent(ContentManager Content)
        {
            Debug.Assert(Content != null, " Content can't be null!");

            _board = new Board(PathToSavedGambeBoardConfigurationFile);

            this.Content = Content;

            //this._player = new UserControlledSprite("ignorthethisspritefile.txt", _board, this);
            _player = new AutomatedSprite("ignorthethisspritefile.txt", _board, this);
            _isUserSprite = false;
            _spriteManager = new SpriteManager("MyLevelsEnemySpritesList.txt", _board, this);

            _helpFont = Content.Load<SpriteFont>(@"fonts\helpfont");
        }

        public void SaveSpritesToDiskAndLoadItIntoPlayGameState(GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");
            Console.WriteLine("----- Saving Sprites in - SaveSpritesToDiskAndLoadItIntoPlayGameState");
            _spriteManager.WriteOutSpritesToAfile(); 
            //SaveCurrentSprites();
            Console.WriteLine("----- Saving Sprites in - Calling PLayGameState.LoadContent");
            //OurGame.PlayGameState.LoadContent(Content);
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
                _player.DecrementScaleFactor();
            }
            else if (_currentMouseState.ScrollWheelValue > _previousScrollValue)
            {
                _player.IncrementScaleFactor();
            }
            _previousScrollValue = _currentMouseState.ScrollWheelValue;

            _spriteManager.UpdateForEditSpriteState(gameTime, ScreenXOffset);

            // Recognize a single click of the leftmouse button
            if (_lastMouseState.LeftButton == ButtonState.Released &&
                _currentMouseState.LeftButton == ButtonState.Pressed)
            {
                // React to the click
                // ...
                _leftMouseClickOccurred = true;
            }

            var putX = _board.CalculateScreenCoordinateXFromMousePosition(ms.X, ScreenXOffset);
            var putY = _board.CalculateScreenCoordinateYFromMousePosition(ms.Y);

            _mouseCursorLockedToNearestGridPositionVector = new Vector2(putX, putY);

            if (_leftMouseClickOccurred)
            {
                if (_board.CalculateYIndex(ms.Y) < _board.TheBoard.GetLength(0) &&
                    _board.CalculateXIndex(ms.X, ScreenXOffset) < _board.TheBoard.GetLength(1)
                    && _board.CalculateYIndex(ms.Y) >= 0 && _board.CalculateXIndex(ms.X, ScreenXOffset) >= 0)
                {
                    /* OurGame.Commands.ICommand ptMultiOnBoardCommand = new PlaceMultiTextureOnBoardCommand(this._board, ms.X, ms.Y, this._multiTexture.TextureToRepeat, _screenXOffset, this._multiTexture.NumberOfHorizontalTiles, this._multiTexture.NumberOfVerticalTiles);
                    ptMultiOnBoardCommand.Execute();

                    this._undoStack.Push(ptMultiOnBoardCommand);
                    */
                    _player.CurrentPosition.X = putX;
                    _player.CurrentPosition.Y = putY;
                    _player.InitialPosition.X = putX - ScreenXOffset;
                    _player.InitialPosition.Y = putY;
                    _player.BoundingRectangle.X = putX;
                    _player.BoundingRectangle.Y = putY;

                    _spriteManager.AddSprite(_player);

                    var savedSpriteScale = _player.GetSpriteScaleFactor();
                    if (_isUserSprite)
                    {
                        _player = new UserControlledSprite("IgnoreThisSpriteConfig.txt", _board, this);
                    }
                    else
                    {
                        _player = new AutomatedSprite("IgnoreThisSpriteConfig.txt", _board, this);
                    }

                    _player.SetSpriteScaleFactor(savedSpriteScale);
                } // end if

                _leftMouseClickOccurred = false;
            }


            if (_lastMouseState.RightButton == ButtonState.Released &&
                _currentMouseState.RightButton == ButtonState.Pressed)
            {
                // React to the click
                // ...
                _rightMouseClickOccurred = true;
            }


            if (_rightMouseClickOccurred)
            {
                _isUserSprite = !_isUserSprite;

                var savedSpriteScale = _player.GetSpriteScaleFactor();

                if (_isUserSprite)
                {
                    _player = new UserControlledSprite("IgnoreThisSpriteConfig.txt", _board, this);
                }
                else
                {
                    _player = new AutomatedSprite("IgnoreThisSpriteConfig.txt", _board, this);
                }
                _player.SetSpriteScaleFactor(savedSpriteScale);
                _rightMouseClickOccurred = false;
            }


            // Move game board.
            var keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Right))
            {
                ScreenXOffset -= ScrollAmount;
            }

            if (keyState.IsKeyDown(Keys.Left))
            {
                ScreenXOffset += ScrollAmount;
            }

            if (ScreenXOffset <= -_board.BoardWidth + Board.SCREEN_WIDTH)
            {
                ScreenXOffset = -_board.BoardWidth + Board.SCREEN_WIDTH;
            }

            if (ScreenXOffset >= 0)
            {
                ScreenXOffset = 0;
            }

            var newKeyboardState = Keyboard.GetState(); // get the newest state

            if (newKeyboardState.IsKeyDown(Keys.S) && _oldKeyboardState.IsKeyUp(Keys.S))
            {
                _spriteManager.WriteOutSpritesToAfile();
                Console.WriteLine(("Writing out SPRITES to config fiile"));
            }

            SwitchStateLogic.DoChangeGameStateFromKeyboardLogic(newKeyboardState, _oldKeyboardState, OurGame, gameTime);


            _oldKeyboardState = newKeyboardState; // set the new state as the old state for next time
        }

        // This next method is used to load in the sprites and board in the SwitchStateLogic class.
        public void LoadContentForRefresh()
        {
            LoadContent(this.OurGame.Content);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Debug.Assert(gameTime != null, "gameTime can't be null!");
            Debug.Assert(spriteBatch != null, "spriteBatch can't be null");

            _board.DrawBoard(spriteBatch, ScreenXOffset, true); // screenXOffset scrolls the board left and right!

            _player.Draw(spriteBatch, _mouseCursorLockedToNearestGridPositionVector);

            _spriteManager.Draw(spriteBatch);

            spriteBatch.DrawString(_helpFont, "Edit Sprites Mode",
                new Vector2(20, 10), Color.Black, 0, Vector2.Zero,
                1, SpriteEffects.None, 1);
            _spriteManager.DrawSubclassName(spriteBatch, this);
            _player.DrawSubclassName(spriteBatch, _mouseCursorLockedToNearestGridPositionVector, this);
        } // end method
    } // end class
} // end namespace