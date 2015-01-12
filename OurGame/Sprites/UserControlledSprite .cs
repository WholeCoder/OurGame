﻿using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// My using statements.
using OurGame.OurGameLibrary;

namespace OurGame.Sprites
{
    public class UserControlledSprite : AnimatedSprite
    {
        private int STARTING_DELTA = 20;
        private int _StartyingYCoordinateForJumping;
        private int _JumpDelta = 0;
        private bool _CurrentlyJumpting = false;

        public UserControlledSprite(string configFilePathAndName)
            : base(configFilePathAndName)
        {

        }

        // This will start at the startOffset and read out it's attributes.
        protected override void Load(string[] configArray, int startOffset)
        {
            Debug.Assert(startOffset >= 0);
            Debug.Assert(configArray != null, "configArray can not be null!");

            // Nothing to Load yet!
            // TODO: Read properties starting at startOffset.
        }

        protected override void UpdateAfterNextFrame(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Right) && keyState.IsKeyUp(Keys.Left))
            {
                this.CurrentPosition.X = this.CurrentPosition.X + 5;

                // This method only switches if we didn't call this method on the last Update
                this.SwitchToGoRightTexture();
            }
            else if (keyState.IsKeyDown(Keys.Left) && keyState.IsKeyUp(Keys.Right))
            {
                this.CurrentPosition.X = this.CurrentPosition.X - 5;

                // This method only switches if we didn't call this method on the last Update
                this.SwitchToGoLeftTexture();
            } else
            {
                // This method only switches if we didn't call this method on the last Update
                this.SwitchToAtRestTexture();
            } 

            if (keyState.IsKeyDown(Keys.Space) && !this._CurrentlyJumpting)
            {
                this._JumpDelta = -this.STARTING_DELTA;
                this._CurrentlyJumpting = true;
                this._StartyingYCoordinateForJumping = (int)this.CurrentPosition.Y;
            }

            if (this._CurrentlyJumpting && this.CurrentPosition.Y <= this._StartyingYCoordinateForJumping)
            {
                this.CurrentPosition.Y += this._JumpDelta;
                this._JumpDelta += 1;
            }

            if (this.CurrentPosition.Y > this._StartyingYCoordinateForJumping && this._CurrentlyJumpting)
            {
                this._CurrentlyJumpting = false;
                this._JumpDelta = 0;
                this.CurrentPosition.Y = this._StartyingYCoordinateForJumping;
            }
        } // end method

        protected override string NameOfThisSubclassForWritingToConfigFile()
        {
            // This is written out to the config file.
            return "UserControlledSprite";
        }

        // In this method we use fs to write out the subclasses properties.
        public override void Write(FileStream fs)
        {
            Debug.Assert(fs.CanWrite, "FileStream fs is not Writable!");

            // Nothing to write yet!
            // TODO: Write out attributes if they exist for UserCotnrolledSprite

        } // end method

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            C3.XNA.Primitives2D.DrawRectangle(spriteBatch, this.BoundingRectangle, Color.Red);
        }
    } // end class
} // end using
