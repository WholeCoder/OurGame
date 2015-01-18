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
        private int _HowFarToWalkInOneDirection;
        private bool _IsGoingRight;
        private PlayGameState _PlayGameState;
        private int _StartXOffset;

        private int _MoveRightLength;
        private int _MoveLeftLength;

        private Board _TheBoard;

        public AutomatedSprite(string configFilePathAndName, Board board, PlayGameState pState)
            : base(configFilePathAndName)
        {
            Debug.Assert(pState != null, "pState can't be null!");
            Debug.Assert(board != null, "board can't be null!");
            
            this._PlayGameState = pState;
            this._StartXOffset = pState.screenXOffset;
            this._TheBoard = board;
        }

        // This will start at the startOffset and read out it's attributes.
        protected sealed override void Load(string[] configArray, int startOffset)
        {
            Debug.Assert(configArray != null, "configArray can't be null!");
            Debug.Assert(startOffset >= 0, "startOffset must be >= 0!");

            // Nothing to Load yet!
            // TODO: Read properties starting at startOffset.
            this._HowFarToWalkInOneDirection = Convert.ToInt32(configArray[startOffset]);
            this._IsGoingRight = configArray[startOffset + 1].Equals("True");
            this._MoveLeftLength = this._HowFarToWalkInOneDirection;
            this._MoveRightLength = this._HowFarToWalkInOneDirection;
        }

        private bool _FirstTime = true;

        protected override void UpdateAfterNextFrame(GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can't be null!");

            if (this._FirstTime || this._PlayGameState.screenXOffset != this._StartXOffset)
            {
                this._MoveRightLength = (int)this._InitialPosition.X + this._PlayGameState.screenXOffset+this._HowFarToWalkInOneDirection;// Math.Max((int)(this.CurrentPosition.X - this._InitialPosition.X + this._PlayGameState.screenXOffset), this._HowFarToWalkInOneDirection);
                this._MoveLeftLength = (int)this._InitialPosition.X + this._PlayGameState.screenXOffset- this._HowFarToWalkInOneDirection;// Math.Max((int)(this._InitialPosition.X - this.CurrentPosition.X + this._PlayGameState.screenXOffset), this._HowFarToWalkInOneDirection);

                this._StartXOffset = this._PlayGameState.screenXOffset;
                this._FirstTime = false;
            }

            if (this._IsGoingRight)
            {
                this.SwitchToGoRightTexture();
                if (this.CurrentPosition.X > this._MoveRightLength)
                {
                    this._IsGoingRight = false;
                } else
                {
                    this.CurrentPosition.X += 5;
                }
            }
            else
            {
                this.SwitchToGoLeftTexture();
                if (this.CurrentPosition.X < this._MoveLeftLength)
                {
                    this._IsGoingRight = true;
                }
                else
                {
                    this.CurrentPosition.X -= 5;
                }
            }

            if (this.CurrentPosition.Y + this.BoundingRectangle.Height > this._TheBoard.BoardHeight)
            {
                this.CurrentPosition.Y = this._TheBoard.BoardHeight - this.BoundingRectangle.Height;
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
            this._HowFarToWalkInOneDirection = 50;
            AnimatedSprite.AddText(fs, this._HowFarToWalkInOneDirection+"");
            AnimatedSprite.AddText(fs, "\n");

            this._IsGoingRight = true;
            AnimatedSprite.AddText(fs, this._IsGoingRight + "");

        } // end method

    } // end class
}
