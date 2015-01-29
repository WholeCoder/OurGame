using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// My usings.
using OurGame.OurGameLibrary;
using OurGame.Sprites;
using OurGame.WindowsGame1;

namespace OurGame.GameStates
{
    public class EditSpritesState : State
    {
        private Board _board;
        
        // This is the position of the mouse "locked" onto a grid position.
        private Vector2 _mouseCursorLockedToNearestGridPositionVector;

        private SpriteManager _spriteManager;

        // This instance variable lets us scroll the board horizontally.
        private const int ScrollAmount = 5;

        // This is the name the gameboard is saved to when S is pressed.
        private const string PathToSavedGambeBoardConfigurationFile = @"MyLevel.txt";

        private MouseState _lastMouseState;
        private MouseState _currentMouseState;

        private bool _leftMouseClickOccurred = false;
        private bool _rightMouseClickOccurred = false;

        private KeyboardState _oldKeyboardState;

        private AnimatedSprite _player;
        private bool _isUserSprite;

        private int _screenXOffset = 0;

        private int _previousScrollValue;

        // Call setStateWhenUpdating on this instance variable to change to a different game state.
        public Game1 OurGame { get; set; }

        public SpriteFont _helpFont;

        // Used to reload the contend in the board for the playGameState
        private ContentManager Content { get; set; }

        public EditSpritesState()
        {
        }

        public override void Initialize(Game1 ourGame)
        {
            Debug.Assert(ourGame != null, "ourGame can't be null!");

            this.OurGame = ourGame;
            _previousScrollValue = Mouse.GetState().ScrollWheelValue;
        }

        protected override void LoadStatesContent(ContentManager Content)
        {
            Debug.Assert(Content != null," Content can't be null!");

            _board = new Board(PathToSavedGambeBoardConfigurationFile);

            this.Content = Content;

            //this._player = new UserControlledSprite("ignorthethisspritefile.txt", _board, this);
            this._player = new AutomatedSprite("ignorthethisspritefile.txt", _board, this);
            this._isUserSprite = false;
            this._spriteManager = new SpriteManager("MyLevelsEnemySpritesList.txt", _board, this);

            this._helpFont = Content.Load<SpriteFont>(@"fonts\helpfont");
        }

        public void SaveSpritesToDiskAndLoadItIntoPlayGameState(GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");

            this.SaveCurrentSprites();
            this.OurGame.PlayGameState.LoadContent(Content);
        }


        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can't be null!");

            MouseState ms = Mouse.GetState();

            // The active state from the last frame is now old
            _lastMouseState = _currentMouseState;

            // Get the mouse state relevant for this frame
            _currentMouseState = Mouse.GetState();

            if (_currentMouseState.ScrollWheelValue < _previousScrollValue)
            {
                this._player.DecrementScaleFactor();
            }
            else if (_currentMouseState.ScrollWheelValue > _previousScrollValue)
            {
                this._player.IncrementScaleFactor();
            }
            _previousScrollValue = _currentMouseState.ScrollWheelValue;


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
                    this._player.CurrentPosition.X = putX;
                    this._player.CurrentPosition.Y = putY;
                    this._player.InitialPosition.X = putX;
                    this._player.InitialPosition.Y = putY;
                    this._player.BoundingRectangle.X = putX;
                    this._player.BoundingRectangle.Y = putY;

                    this._spriteManager.AddSprite(this._player);

                    int savedSpriteScale = this._player.GetSpriteScaleFactor();
                    if (this._isUserSprite)
                    {
                        this._player = new UserControlledSprite("IgnoreThisSpriteConfig.txt", _board, this);
                    }
                    else
                    {
                        this._player = new AutomatedSprite("IgnoreThisSpriteConfig.txt", _board, this);
                    }

                    this._player.SetSpriteScaleFactor(savedSpriteScale);
                } // end if

                _leftMouseClickOccurred = false;
            }


            if (_lastMouseState.RightButton == ButtonState.Released && _currentMouseState.RightButton == ButtonState.Pressed)
            {
                // React to the click
                // ...
                _rightMouseClickOccurred = true;
            }


           if (_rightMouseClickOccurred)
            {
                this._isUserSprite = !this._isUserSprite;

                int savedSpriteScale = this._player.GetSpriteScaleFactor();

                if (this._isUserSprite)
                {
                    this._player = new UserControlledSprite("IgnoreThisSpriteConfig.txt", _board, this);
                }
                else
                {
                    this._player = new AutomatedSprite("IgnoreThisSpriteConfig.txt", _board, this);

                }
                this._player.SetSpriteScaleFactor(savedSpriteScale);
               _rightMouseClickOccurred = false;
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

            SwitchStateLogic.DoChangeGameStateFromKeyboardLogic(newKeyboardState, _oldKeyboardState, this.OurGame, gameTime);


            _oldKeyboardState = newKeyboardState;  // set the new state as the old state for next time

        }


        public void SaveBoardToDiskAndLoadItIntoPlayGameState(GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");

            this.SaveCurrentSprites();
            this.OurGame.PlayGameState.LoadContent(Content);
        }

        public void SaveCurrentSprites()
        {
            this._spriteManager.WriteOutSpritesToAfile();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Debug.Assert(gameTime!= null, "gameTime can't be null!");
            Debug.Assert(spriteBatch != null, "spriteBatch can't be null");

            this._board.DrawBoard(spriteBatch, _screenXOffset, true);  // screenXOffset scrolls the board left and right!

            this._player.Draw(spriteBatch, _mouseCursorLockedToNearestGridPositionVector);

            this._spriteManager.Draw(spriteBatch);

            spriteBatch.DrawString(this._helpFont, "Edit Sprites Mode",
                                   new Vector2(20, 10), Color.Black, 0, Vector2.Zero,
                                   1, SpriteEffects.None, 1);
            this._spriteManager.DrawSubclassName(spriteBatch, this);
            this._player.DrawSubclassName(spriteBatch, _mouseCursorLockedToNearestGridPositionVector, this);
        } // end method
    } // end class
} // end namespace