﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        // ReSharper disable once InconsistentNaming
        private const int CHARACTER_SCROLL_TRIGGER_MARGIN = 300;
        // when the user is this far away from the ends of the screen, we will trigger board scrolling
        private const int MIN_LIMIT_X_LEFT_HAND_MARGIN = 10;
        // this will be the least value that the X co-ordinate will take on if the board is scrolled all the way to the left.
        private const int MAX_LIMIT_X_RIGHT_HAND_MARGIN = 40;

        public override string ToString()
        {
            return "UserControlledSprite";
        }

        public UserControlledSprite(string configFilePathAndName, Board board, State pState)
            : base(configFilePathAndName)
        {
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
                var tempBoundingRectangle = new Rectangle((int) (CurrentPosition.X + State.SCROLL_AMOUNT),
                    (int) CurrentPosition.Y,
                    BoundingRectangle.Width,
                    BoundingRectangle.Height);

                var tilesToRightAndAtOrAbove = _theBoard
                    .RetrieveTilesThatIntersectWithThisSprite(tempBoundingRectangle, _playGameState,(int)this.CurrentPosition.Y)
                    .Select(tile => tile)
                    .Where(tile => tile.BoundingRectangle.X > CurrentPosition.X
                                   && tile.BoundingRectangle.Y < CurrentPosition.Y + BoundingRectangle.Height - 5)
                    .ToList();

                if (!tilesToRightAndAtOrAbove.Any())
                {
                    if (CurrentPosition.X < Board.SCREEN_WIDTH - CHARACTER_SCROLL_TRIGGER_MARGIN)
                    {
                        CurrentPosition.X = CurrentPosition.X + State.SCROLL_AMOUNT;
                    }
                    else if (CurrentPosition.X >= Board.SCREEN_WIDTH - CHARACTER_SCROLL_TRIGGER_MARGIN
                             && _playGameState.ScreenXOffset <= -_theBoard.BoardWidth + Board.SCREEN_WIDTH)
                    {
                        if (CurrentPosition.X >= Board.SCREEN_WIDTH - MAX_LIMIT_X_RIGHT_HAND_MARGIN)
                        {
                            CurrentPosition.X = Board.SCREEN_WIDTH - MAX_LIMIT_X_RIGHT_HAND_MARGIN;
                        }
                        else
                        {
                            CurrentPosition.X += State.SCROLL_AMOUNT;
                        }
                    }
                    else
                    {
                        _playGameState.ScreenXOffset -= State.SCROLL_AMOUNT;
                    }
                }

                // This method only switches if we didn't call this method on the last Update
                SwitchToGoRightTexture();
            }
            else if (keyState.IsKeyDown(Keys.Left) && keyState.IsKeyUp(Keys.Right))
            {
                var tempBoundingRectangle = new Rectangle((int) (CurrentPosition.X - State.SCROLL_AMOUNT-1),
                    (int) CurrentPosition.Y,
                    BoundingRectangle.Width,
                    BoundingRectangle.Height);
                var tilesToLeftAndAtOrAbove = _theBoard
                    .RetrieveTilesThatIntersectWithThisSprite(tempBoundingRectangle, _playGameState, (int)this.CurrentPosition.Y)
                    .Select(tile => tile)
                    .Where(tile => tile.BoundingRectangle.X < CurrentPosition.X
                                   && tile.BoundingRectangle.Y < CurrentPosition.Y + BoundingRectangle.Height - 5)
                    .ToList();

                if (!tilesToLeftAndAtOrAbove.Any())
                {
                    Console.WriteLine("FOUND SOME!");
                    if (CurrentPosition.X > CHARACTER_SCROLL_TRIGGER_MARGIN)
                    {
                        CurrentPosition.X = CurrentPosition.X - State.SCROLL_AMOUNT;
                    }
                    else if (CurrentPosition.X <= CHARACTER_SCROLL_TRIGGER_MARGIN
                             && _playGameState.ScreenXOffset >= 0)
                    {
                        if (CurrentPosition.X <= MIN_LIMIT_X_LEFT_HAND_MARGIN)
                        {
                            CurrentPosition.X = MIN_LIMIT_X_LEFT_HAND_MARGIN;
                        }
                        else
                        {
                            CurrentPosition.X -= State.SCROLL_AMOUNT;
                        }
                    }
                    else
                    {
                        _playGameState.ScreenXOffset += State.SCROLL_AMOUNT;
                    }
                }


                // This method only switches if we didn't call this method on the last Update
                SwitchToGoLeftTexture();
            }
            else
            {
                // This method only switches if we didn't call this method on the last Update
                SwitchToAtRestTexture();
            }
            Console.WriteLine("CanJump == " + CanJump);
            if (CanJump && keyState.IsKeyDown(Keys.Space) && !_currentlyJumping)
            {

                _jumpStart = (int) CurrentPosition.Y;
                IsJumping = _currentlyJumping = true;
                IsGoingUp = true;
                IsGoingDown = false;
                CanJump = false;

                Console.WriteLine("SPACE key pressed - setting _currentlyJumping to TRUE");

                SoundSystem.getInstance().getSound("mariojumpting").Play();
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
                    BoundingRectangle.X = (int) CurrentPosition.X;
                    BoundingRectangle.Y = (int) CurrentPosition.Y;
                }
                else if (CurrentPosition.Y <= _jumpStart && IsGoingUp)
                {
                    CurrentPosition.Y += -_currentJumpIncrement - GRAVITY_DOWNWARD;
                    _currentJumpIncrement--;
                    BoundingRectangle.X = (int) CurrentPosition.X;
                    BoundingRectangle.Y = (int) CurrentPosition.Y;
                }
                else
                {
                    IsGoingDown = false;
                    IsGoingUp = false;
                    IsJumping = _currentlyJumping = false;
                }
            }


            if (
                _theBoard.RetrieveTilesThatIntersectWithThisSpriteWithBoundingBoxAdjustment(this,_playGameState).Count != 0)
            {
                CanJump = true;
                IsJumping = _currentlyJumping = false;
            }
            else
            {
                CanJump = false;
            }

            if (CurrentPosition.Y + BoundingRectangle.Height > _theBoard.BoardHeight)
            {
                CurrentPosition.Y = _theBoard.BoardHeight - BoundingRectangle.Height;
            }

            // Ensure the game board isn't past it's ends.
            if (_playGameState.ScreenXOffset <= -_theBoard.BoardWidth + Board.SCREEN_WIDTH)
            {
                _playGameState.ScreenXOffset = -_theBoard.BoardWidth + Board.SCREEN_WIDTH;
            }

            if (_playGameState.ScreenXOffset >= 0)
            {
                _playGameState.ScreenXOffset = 0;
            }

            BoundingRectangle.X = (int) CurrentPosition.X;
            BoundingRectangle.Y = (int) CurrentPosition.Y;
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

        public override string NameOfThisSubclassForWritingToConfigFile
        {
            get
            {
                // This is written out to the config file.
                return "UserControlledSprite";
            }
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

            BoundingRectangle.X = (int) CurrentPosition.X;
            BoundingRectangle.Y = (int) CurrentPosition.Y;

            C3.XNA.Primitives2D.DrawRectangle(spriteBatch, BoundingRectangle, Color.Black);
        }
    } // end class
} // end using