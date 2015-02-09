using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using OurGame.GameStates;
using OurGame.OurGameLibrary;

namespace OurGame.Sprites
{
    public class AutomatedSprite : AnimatedSprite
    {
        private readonly State _playGameState;
        private readonly Board _theBoard;
        private bool _firstTime = true;
        private int _howFarToWalkInOneDirection;
        private int _moveLeftLength;
        private int _moveRightLength;
        private int _startXOffset;

        public AutomatedSprite(string configFilePathAndName, Board board, State pState)
            : base(configFilePathAndName)
        {
            Debug.Assert(pState != null, "pState can't be null!");
            Debug.Assert(board != null, "board can't be null!");

            _playGameState = pState;
            _startXOffset = pState.ScreenXOffset;
            _theBoard = board;
        }

        private bool IsGoingRight { get; set; }

        public override string NameOfThisSubclassForWritingToConfigFile
        {
            get
            {
                // This is written out to the config file and used as a "constant" in the SimpleAnimatedSpriteFactory.
                return "AutomatedSprite";
            }
        }

        // This will start at the startOffset and read out it's attributes.
        protected override sealed void Load(string[] configArray, int startOffset)
        {
            Debug.Assert(configArray != null, "configArray can't be null!");
            Debug.Assert(startOffset >= 0, "startOffset must be >= 0!");

            // Nothing to Load yet!
            // TODO: Read properties starting at startOffset.
            _howFarToWalkInOneDirection = Convert.ToInt32(configArray[startOffset]);
            IsGoingRight = configArray[startOffset + 1].Equals("True");
            _moveLeftLength = _howFarToWalkInOneDirection;
            _moveRightLength = _howFarToWalkInOneDirection;
        }

        protected override void UpdateAfterNextFrame(GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can't be null!");

            if (_firstTime || _playGameState.ScreenXOffset != _startXOffset)
            {
                _moveRightLength = (int) InitialPosition.X + _playGameState.ScreenXOffset + _howFarToWalkInOneDirection;
                // Math.Max((int)(this.CurrentPosition.X - this._InitialPosition.X + this._PlayGameState.screenXOffset), this._HowFarToWalkInOneDirection);
                _moveLeftLength = (int) InitialPosition.X + _playGameState.ScreenXOffset - _howFarToWalkInOneDirection;
                // Math.Max((int)(this._InitialPosition.X - this.CurrentPosition.X + this._PlayGameState.screenXOffset), this._HowFarToWalkInOneDirection);

                _startXOffset = _playGameState.ScreenXOffset;
                _firstTime = false;
            }

            if (IsGoingRight)
            {
                SwitchToGoRightTexture();
                if (CurrentPosition.X > _moveRightLength)
                {
                    IsGoingRight = false;
                }
                else
                {
                    CurrentPosition.X += 5;
                }
            }
            else
            {
                SwitchToGoLeftTexture();
                if (CurrentPosition.X < _moveLeftLength)
                {
                    IsGoingRight = true;
                }
                else
                {
                    CurrentPosition.X -= 5;
                }
            }

            if (CurrentPosition.Y + BoundingRectangle.Height > _theBoard.BoardHeight)
            {
                CurrentPosition.Y = _theBoard.BoardHeight - BoundingRectangle.Height;
            }
        }

        // In this method we use fs to write out the subclasses properties.
        protected override sealed void Write(FileStream fs)
        {
            Debug.Assert(fs.CanWrite, "FileStream fs must be open for writing!");

            // Nothing to write yet!
            // TODO: Write out attributes if they exist for UserControlledSprite
            _howFarToWalkInOneDirection = GetSpriteScaleFactor()*BoundingRectangle.Width;
            Utilities.AddText(fs, _howFarToWalkInOneDirection + "");
            Utilities.AddText(fs, "\n");

            IsGoingRight = true;
            Utilities.AddText(fs, IsGoingRight + "");
        }
    } // end class
}