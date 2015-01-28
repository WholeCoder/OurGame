﻿using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// My using statements.
using OurGame.GameStates;
using OurGame.OurGameLibrary;

namespace OurGame.Sprites
{
    public class UserControlledSprite : AnimatedSprite
    {
        private int _currentJumpIncrement = 1;
        private const int JUMP_TO_THIS_RELATIVE = 200;

        private int _jumpStart;

        private bool _currentlyJumping = false;

        private readonly Board _theBoard;
        private readonly State _playGameState;

        public bool CanJump { get; set; }

        public UserControlledSprite(string configFilePathAndName, Board board, State pState)
            : base(configFilePathAndName)
        {
            this._theBoard = board;
            this._playGameState = pState;
            this.CanJump = true;
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
            Debug.Assert(gameTime != null, "gameTime can not be null!");

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

            if (this.CanJump && keyState.IsKeyDown(Keys.Space) && !this._currentlyJumping)
            {
                this._jumpStart = (int)this.CurrentPosition.Y;
                this._currentlyJumping = true;
                this.IsGoingUp = true;
                this.IsGoingDown = false;
                this.CanJump = false;

                this._currentJumpIncrement = ComputeJumpIncremnet();
            }

            if (this._currentlyJumping)
            {
                //this.CurrentPosition.Y -= 6;
                if (this.CurrentPosition.Y < this._jumpStart - UserControlledSprite.JUMP_TO_THIS_RELATIVE && this.IsGoingUp)
                {
                    this.IsGoingDown = true;
                    this.IsGoingUp = false;
                    //this.CurrentPosition.Y += UserControlledSprite.JUMP_DELTA;
                }
                
                if (this.CurrentPosition.Y < this._jumpStart && this.IsGoingDown)
                {
                    this.CurrentPosition.Y += _currentJumpIncrement - AnimatedSprite.GRAVITY_DOWNWARD;
                    this._currentJumpIncrement++;
                }
                else if (this.CurrentPosition.Y <= this._jumpStart && this.IsGoingUp)
                {
                    this.CurrentPosition.Y += -_currentJumpIncrement- AnimatedSprite.GRAVITY_DOWNWARD;
                    this._currentJumpIncrement--;
                }
                else
                {
                    this.IsGoingDown = false;
                    this.IsGoingUp = false;
                    this._currentlyJumping = false;
                }
            }


            if (this._theBoard.RetrieveTilesThatIntersectWithThisSprite(this, this._playGameState.ScreenXOffset).Count != 0)
            {
                this.CanJump = true;
                this._currentlyJumping = false;
            }
            else
            {
                this.CanJump = false;
            }

            if (this.CurrentPosition.Y + this.BoundingRectangle.Height > this._theBoard.BoardHeight)
            {
                this.CurrentPosition.Y = this._theBoard.BoardHeight - this.BoundingRectangle.Height;
            }

        }

        private int ComputeJumpIncremnet()
        {
            double a = 1;
            double b = 1;
            double c = -2*UserControlledSprite.JUMP_TO_THIS_RELATIVE;

            double sqrtpart = (b*b) - (4*a*c);

            double answer1 = ((-1)*b + Math.Sqrt(sqrtpart))/(2*a);
            double answer2 = ((-1)*b - Math.Sqrt(sqrtpart))/(2*a);

            int inc = -1;
            if (answer1 > answer2)
            {
                inc = (int) answer1;
            }
            else
            {
                inc= (int) answer2;
            }

            return inc;
        }

        public bool IsGoingDown { get; set; }

        public bool IsGoingUp { get; set; }

// end method

        public override string NameOfThisSubclassForWritingToConfigFile()
        {
            // This is written out to the config file.
            return "UserControlledSprite";
        }

        // In this method we use fs to write out the subclasses properties.
        protected override void Write(FileStream fs)
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
