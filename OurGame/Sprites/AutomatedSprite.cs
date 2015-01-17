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
        private int _StartingX;
        private PlayGameState _PlayGameState;
        private int _StartXOffset;

        public AutomatedSprite(string configFilePathAndName, PlayGameState pState)
            : base(configFilePathAndName)
        {
            //            this.Load(pathWithFile);  // Will write defaults to disk if the file isn't found.
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
        }

        private bool _FirstTime = true;

        protected override void UpdateAfterNextFrame(GameTime gameTime)
        {
            if (this._FirstTime || this._PlayGameState.screenXOffset != this._StartXOffset)
            {
                this._StartingX = (int)this._InitialPosition.X+this._PlayGameState.screenXOffset;
                this._StartXOffset = this._PlayGameState.screenXOffset;
                this._FirstTime = false;
            }

            if (this._IsGoingRight)
            {
                if (this.CurrentPosition.X + this._PlayGameState.screenXOffset > this._StartingX + this._PlayGameState.screenXOffset + this._HowFarToWalkInOneDirection)
                 {
                     this._IsGoingRight = false;
                 }
                this.CurrentPosition.X += 5;
            }
            else
            {
                if (this.CurrentPosition.X + this._PlayGameState.screenXOffset < this._StartingX + this._PlayGameState.screenXOffset - this._HowFarToWalkInOneDirection)
                {
                    this._IsGoingRight = true;
                }
                this.CurrentPosition.X -= 5;
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
