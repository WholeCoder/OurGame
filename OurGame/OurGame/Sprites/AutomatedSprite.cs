using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OurGame.GameStates;
using OurGame.OurGameLibrary;

namespace OurGame.Sprites
{
    public class AutomatedSprite : AnimatedSprite
    {
        private readonly State _playGameState;
        private readonly Board _theBoard;
        private int _howFarToWalkInOneDirection;
        private int _howFarWalkedSoFarInDirection = 0;

        private bool _onScreen = true;

        public AutomatedSprite(string configFilePathAndName, Board board, State pState)
            : base(configFilePathAndName)
        {
            Debug.Assert(pState != null, "pState can't be null!");
            Debug.Assert(board != null, "board can't be null!");

            _playGameState = pState;
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

        public override string ToString()
        {
            return "AutomatedSprite";
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
        }

        protected override void UpdateAfterNextFrame(GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can't be null!");

            if (CurrentPosition.X+_playGameState.ScreenXOffset > -BoundingRectangle.Width+10 && CurrentPosition.X+_playGameState.ScreenXOffset < Board.SCREEN_WIDTH-20)
            {
                _onScreen = true;
            }
            else
            {
                _onScreen = false;
                return;
            }

            if (IsGoingRight)
            {

                SwitchToGoRightTexture();

                if (_howFarWalkedSoFarInDirection > _howFarToWalkInOneDirection)
                {
                    IsGoingRight = false;
                    _howFarWalkedSoFarInDirection = 0;
                }
                else
                {
                    // This fixes when the sprite goes offscreen and reappears in some blocks that appear from scrolling.
                    EnsureThatSpriteDoesNotGetStuckInBlocks(true); // going right

                    var tempBoundingRectangle = new Rectangle((int)(CurrentPosition.X+_playGameState.ScreenXOffset + State.SCROLL_AMOUNT),
                                                                (int)CurrentPosition.Y,
                                                                BoundingRectangle.Width,
                                                                BoundingRectangle.Height);

                    var tilesToRightAndAtOrAbove = _theBoard
                        .RetrieveTilesThatIntersectWithThisSprite(tempBoundingRectangle, _playGameState, (int)this.CurrentPosition.Y)
                        .Select(tile => tile)
                        .Where(tile => tile.BoundingRectangle.X > CurrentPosition.X + _playGameState.ScreenXOffset
                                       && tile.BoundingRectangle.Y < CurrentPosition.Y + BoundingRectangle.Height - 5)
                        .ToList();

                    if (!tilesToRightAndAtOrAbove.Any())
                    {
                        CurrentPosition.X += 5;
                    }
                    
                    _howFarWalkedSoFarInDirection += 5;
                }
            }
            else
            {

                SwitchToGoLeftTexture();

                if (_howFarWalkedSoFarInDirection > _howFarToWalkInOneDirection)
                {
                    IsGoingRight = true;
                    _howFarWalkedSoFarInDirection = 0;
                }
                else
                {
                    // This fixes when the sprite goes offscreen and reappears in some blocks that appear from scrolling.
                    EnsureThatSpriteDoesNotGetStuckInBlocks(false); // going left == false

                    var tempBoundingRectangle = new Rectangle((int)(CurrentPosition.X+_playGameState.ScreenXOffset - State.SCROLL_AMOUNT - 1),
                                                                (int)CurrentPosition.Y,
                                                                BoundingRectangle.Width,
                                                                BoundingRectangle.Height);
                    var tilesToLeftAndAtOrAbove = _theBoard
                        .RetrieveTilesThatIntersectWithThisSprite(tempBoundingRectangle, _playGameState, (int)this.CurrentPosition.Y)
                        .Select(tile => tile)
                        .Where(tile => tile.BoundingRectangle.X < CurrentPosition.X + _playGameState.ScreenXOffset
                                       && tile.BoundingRectangle.Y < CurrentPosition.Y + BoundingRectangle.Height - 5)
                        .ToList();

                    if (!tilesToLeftAndAtOrAbove.Any())
                    {
                        CurrentPosition.X -= 5;
                    }
                    _howFarWalkedSoFarInDirection += 5;
                }
            }


            if (CurrentPosition.Y + BoundingRectangle.Height > _theBoard.BoardHeight)
            {
                CurrentPosition.Y = _theBoard.BoardHeight - BoundingRectangle.Height;
            }


        }

        // This fixes when the sprite goes offscreen and reappears in some blocks that appear from scrolling.
        private void EnsureThatSpriteDoesNotGetStuckInBlocks(bool goingRight)
        {
            var tilesToRightAndAtOrAbove = TilesToRightAndAtOrAbove();

            while (tilesToRightAndAtOrAbove.Any())
            {
                if (goingRight)
                {
                    CurrentPosition.X += 5;
                }
                else
                {
                    CurrentPosition.X -= 5;
                }
                tilesToRightAndAtOrAbove = TilesToRightAndAtOrAbove();
            } 

        }

        private List<Tile> TilesToRightAndAtOrAbove()
        {
            var tempBoundingRectangle = new Rectangle((int) (CurrentPosition.X + _playGameState.ScreenXOffset),
                (int) CurrentPosition.Y,
                BoundingRectangle.Width,
                BoundingRectangle.Height);

            var tilesToRightAndAtOrAbove = _theBoard
                .RetrieveTilesThatIntersectWithThisSprite(tempBoundingRectangle, _playGameState, (int) this.CurrentPosition.Y)
                .Select(tile => tile)
                .Where(tile => tile.BoundingRectangle.X > CurrentPosition.X + _playGameState.ScreenXOffset
                               && tile.BoundingRectangle.Y < CurrentPosition.Y + BoundingRectangle.Height - 5)
                .ToList();
            return tilesToRightAndAtOrAbove;
        }

// end method

        public override void ApplyDownwardGravity(Board theBoard,State state)
        {
            if (_onScreen)
            {
                base.ApplyDownwardGravity(theBoard, _playGameState);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 realPosition = CurrentPosition;
            realPosition.X += _playGameState.ScreenXOffset;
            realPosition.Y += _playGameState.ScreenYOffset;
            BoundingRectangle.X = (int)realPosition.X;
            BoundingRectangle.Y = (int)realPosition.Y;


            spriteBatch.Draw(TextureCache.getInstance().GetTexture2DFromStringSpriteArray(CurrentTextureFilename),
                realPosition,
                new Rectangle(CurrentFrame.X * CurrentFrameSize.X + CurrentFrame.X + 1,
                // CurrentFrame.X+1 is an offset for pixel boundaries in image
                    CurrentFrame.Y * CurrentFrameSize.Y,
                    CurrentFrameSize.X,
                    CurrentFrameSize.Y),
                Color.White,
                0,
                Vector2.Zero,
                ScaleUpThisSpriteFactor, // scale
                CurrentSpriteEffect,
                0);

            C3.XNA.Primitives2D.DrawRectangle(spriteBatch, BoundingRectangle, Color.White);
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