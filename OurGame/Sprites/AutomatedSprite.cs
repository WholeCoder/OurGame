﻿using System;
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
        private int _StartingX;
        private PlayGameState _PlayGameState;
        private int _StartXOffset;

        private int _MoveRightLength;
        private int _MoveLeftLength;

        public static int GRAVITY_DOWNWARD = 5; // This makes sure there is always downward "pressure" to keep the sprit on the ground;

        public AutomatedSprite(string configFilePathAndName, PlayGameState pState)
            : base(configFilePathAndName)
        {
            Debug.Assert(pState != null, "pState can't be null!");
            
            this._PlayGameState = pState;
            this._StartXOffset = pState.screenXOffset;
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
                this._MoveRightLength = Math.Max((int)(this.CurrentPosition.X - this._InitialPosition.X + this._PlayGameState.screenXOffset), this._HowFarToWalkInOneDirection);
                this._MoveLeftLength = Math.Max((int)(this._InitialPosition.X - this.CurrentPosition.X + this._PlayGameState.screenXOffset), this._HowFarToWalkInOneDirection);

                this._StartingX = (int)this._InitialPosition.X+this._PlayGameState.screenXOffset;
                this._StartXOffset = this._PlayGameState.screenXOffset;
                this._FirstTime = false;
            }

            if (this._IsGoingRight)
            {
                this.SwitchToGoRightTexture();
                if (this.CurrentPosition.X > this._InitialPosition.X+this._PlayGameState.screenXOffset  + this._MoveRightLength)
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
                if (this.CurrentPosition.X < this._InitialPosition.X + this._PlayGameState.screenXOffset - this._MoveLeftLength)
                {
                    this._IsGoingRight = true;
                }
                else
                {
                    this.CurrentPosition.X -= 5;
                }
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
            this._HowFarToWalkInOneDirection = 100;
            AnimatedSprite.AddText(fs, this._HowFarToWalkInOneDirection+"");
            AnimatedSprite.AddText(fs, "\n");

            this._IsGoingRight = true;
            AnimatedSprite.AddText(fs, this._IsGoingRight + "");

        } // end method

    } // end class
}
