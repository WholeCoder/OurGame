using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

// My usings.
using OurGame.GameStates;
using OurGame.OurGameLibrary;

namespace OurGame.Sprites
{
    public class AutomatedSprite : AnimatedSprite
    {
        private int _howFarToWalkInOneDirection;
        private bool IsGoingRight { get; set; }
        private readonly PlayGameState _playGameState;
        private int _startXOffset;

        private int _moveRightLength;
        private int _moveLeftLength;

        private readonly Board _theBoard;

        public AutomatedSprite(string configFilePathAndName, Board board, PlayGameState pState)
            : base(configFilePathAndName)
        {
            Debug.Assert(pState != null, "pState can't be null!");
            Debug.Assert(board != null, "board can't be null!");
            
            this._playGameState = pState;
            this._startXOffset = pState.ScreenXOffset;
            this._theBoard = board;
        }

        // This will start at the startOffset and read out it's attributes.
        protected sealed override void Load(string[] configArray, int startOffset)
        {
            Debug.Assert(configArray != null, "configArray can't be null!");
            Debug.Assert(startOffset >= 0, "startOffset must be >= 0!");

            // Nothing to Load yet!
            // TODO: Read properties starting at startOffset.
            this._howFarToWalkInOneDirection = Convert.ToInt32(configArray[startOffset]);
            this.IsGoingRight = configArray[startOffset + 1].Equals("True");
            this._moveLeftLength = this._howFarToWalkInOneDirection;
            this._moveRightLength = this._howFarToWalkInOneDirection;
        }

        private bool _firstTime = true;

        protected override void UpdateAfterNextFrame(GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can't be null!");

            if (this._firstTime || this._playGameState.ScreenXOffset != this._startXOffset)
            {
                this._moveRightLength = (int)this.InitialPosition.X + this._playGameState.ScreenXOffset+this._howFarToWalkInOneDirection;// Math.Max((int)(this.CurrentPosition.X - this._InitialPosition.X + this._PlayGameState.screenXOffset), this._HowFarToWalkInOneDirection);
                this._moveLeftLength = (int)this.InitialPosition.X + this._playGameState.ScreenXOffset- this._howFarToWalkInOneDirection;// Math.Max((int)(this._InitialPosition.X - this.CurrentPosition.X + this._PlayGameState.screenXOffset), this._HowFarToWalkInOneDirection);

                this._startXOffset = this._playGameState.ScreenXOffset;
                this._firstTime = false;
            }

            if (this.IsGoingRight)
            {
                this.SwitchToGoRightTexture();
                if (this.CurrentPosition.X > this._moveRightLength)
                {
                    this.IsGoingRight = false;
                } else
                {
                    this.CurrentPosition.X += 5;
                }
            }
            else
            {
                this.SwitchToGoLeftTexture();
                if (this.CurrentPosition.X < this._moveLeftLength)
                {
                    this.IsGoingRight = true;
                }
                else
                {
                    this.CurrentPosition.X -= 5;
                }
            }

            if (this.CurrentPosition.Y + this.BoundingRectangle.Height > this._theBoard.BoardHeight)
            {
                this.CurrentPosition.Y = this._theBoard.BoardHeight - this.BoundingRectangle.Height;
            }

        }
        
        protected override string NameOfThisSubclassForWritingToConfigFile()
        {
            // This is written out to the config file and used as a "constant" in the SimpleAnimatedSpriteFactory.
            return "AutomatedSprite";
        }

        // In this method we use fs to write out the subclasses properties.
        public sealed override void Write(FileStream fs)
        {
            Debug.Assert(fs.CanWrite, "FileStream fs must be open for writing!");

            // Nothing to write yet!
            // TODO: Write out attributes if they exist for UserControlledSprite
            this._howFarToWalkInOneDirection = 50;
            AnimatedSprite.AddText(fs, this._howFarToWalkInOneDirection+"");
            AnimatedSprite.AddText(fs, "\n");

            this.IsGoingRight = true;
            AnimatedSprite.AddText(fs, this.IsGoingRight + "");

        } // end method

    } // end class
}
