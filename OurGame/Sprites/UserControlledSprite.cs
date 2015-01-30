﻿using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OurGame.GameStates;
using OurGame.OurGameLibrary;

namespace OurGame.Sprites
{
    public class UserControlledSprite : AnimatedSprite
    {
        private int _currentJumpIncrement = 1;
        // ReSharper disable once InconsistentNaming
        private const int JUMP_TO_THIS_RELATIVE = 200;

        private int _jumpStart;

        private bool _currentlyJumping;

        private readonly Board _theBoard;
        private readonly State _playGameState;

        private bool CanJump { get; set; }

        public UserControlledSprite(string configFilePathAndName, Board board, State pState)
            : base(configFilePathAndName)
        {
            Debug.Assert(configFilePathAndName != null && !configFilePathAndName.Equals(""),
                "configFilePathAndName can't be null and can't be the empty string!");
            Debug.Assert(board != null, "board can not be null!");
            Debug.Assert(pState != null, "pState can not be null!");

            _theBoard = board;
            _playGameState = pState;
            CanJump = true;
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

            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Right) && keyState.IsKeyUp(Keys.Left))
            {
                if (CurrentPosition.X < Board.SCREEN_WIDTH - 100)
                {
                    CurrentPosition.X = CurrentPosition.X + 5;
                }

                // This method only switches if we didn't call this method on the last Update
                SwitchToGoRightTexture();
            }
            else if (keyState.IsKeyDown(Keys.Left) && keyState.IsKeyUp(Keys.Right))
            {
                if (CurrentPosition.X > 100)
                {
                    CurrentPosition.X = CurrentPosition.X - 5;
                }

                // This method only switches if we didn't call this method on the last Update
                SwitchToGoLeftTexture();
            }
            else
            {
                // This method only switches if we didn't call this method on the last Update
                SwitchToAtRestTexture();
            } 

            if (CanJump && keyState.IsKeyDown(Keys.Space) && !_currentlyJumping)
            {
                _jumpStart = (int) CurrentPosition.Y;
                _currentlyJumping = true;
                IsGoingUp = true;
                IsGoingDown = false;
                CanJump = false;

                _currentJumpIncrement = ComputeJumpIncremnet();
            }

            if (_currentlyJumping)
            {
                //this.CurrentPosition.Y -= 6;
                if (CurrentPosition.Y < _jumpStart - JUMP_TO_THIS_RELATIVE && IsGoingUp)
                {
                    IsGoingDown = true;
                    IsGoingUp = false;
                    //this.CurrentPosition.Y += UserControlledSprite.JUMP_DELTA;
                }

                if (CurrentPosition.Y < _jumpStart && IsGoingDown)
                {
                    CurrentPosition.Y += _currentJumpIncrement - GRAVITY_DOWNWARD;
                    _currentJumpIncrement++;
                }
                else if (CurrentPosition.Y <= _jumpStart && IsGoingUp)
                {
                    CurrentPosition.Y += -_currentJumpIncrement - GRAVITY_DOWNWARD;
                    _currentJumpIncrement--;
                }
                else
                {
                    IsGoingDown = false;
                    IsGoingUp = false;
                    _currentlyJumping = false;
                }
            }


            if (_theBoard.RetrieveTilesThatIntersectWithThisSprite(this, _playGameState.ScreenXOffset).Count != 0)
            {
                CanJump = true;
                _currentlyJumping = false;
            }
            else
            {
                CanJump = false;
            }

            if (CurrentPosition.Y + BoundingRectangle.Height > _theBoard.BoardHeight)
            {
                CurrentPosition.Y = _theBoard.BoardHeight - BoundingRectangle.Height;
            }
        }

        private static int ComputeJumpIncremnet()
        {
            double a = 1;
            double b = 1;
            double c = -2*JUMP_TO_THIS_RELATIVE;

            var sqrtpart = (b*b) - (4*a*c);

            Debug.Assert(sqrtpart >= 0, "the discriminant, sqrtpart, can not be less than 0!");

            var answer1 = ((-1)*b + Math.Sqrt(sqrtpart))/(2*a);
            var answer2 = ((-1)*b - Math.Sqrt(sqrtpart))/(2*a);

            var inc = -1;
            if (answer1 > answer2)
            {
                inc = (int) answer1;
            }
            else
            {
                inc = (int) answer2;
            }

            return inc;
        }

        private bool IsGoingDown { get; set; }

        private bool IsGoingUp { get; set; }

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            Debug.Assert(spriteBatch != null, "spriteBatch can not be null!");

            base.Draw(spriteBatch);
            C3.XNA.Primitives2D.DrawRectangle(spriteBatch, BoundingRectangle, Color.Red);
        }
    } // end class
} // end using