using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OurGame.OurGameLibrary;
using OurGame.Sprites;

namespace OurGame.GameStates
{
    public class PlayGameState : State
    {
        // This is the name the gameboard is saved to when S is pressed.
        private readonly string pathToSavedGambeBoardConfigurationFile = @"MyLevel.txt";
        private Board _board;
        private SpriteFont _helpFont;
        private KeyboardState _oldKeyboardState;
        private SpriteManager _spriteManager;
        // Call setStateWhenUpdating on this instance variable to change to a different game state.
        private Game1 OurGame { get; set; }

        private bool DrawRanAlready = true;
        
        private readonly Vector2 _positionOfTitle = new Vector2(10, 10);

        public override string ToString()
        {
            return "PlayGameState - number of sprites on board == " + _spriteManager.Sprites.Count;
        }

        public override void Initialize(Game1 ourGame)
        {
            Debug.Assert(ourGame != null, "ourGame can not be null!");

            OurGame = ourGame;
        }

        protected override void LoadStatesContent(ContentManager Content)
        {
            Debug.Assert(Content != null, "Content can not be null!");

            _board = new Board(pathToSavedGambeBoardConfigurationFile);

            _spriteManager = new SpriteManager("MyLevelsEnemySpritesList.txt", _board, this);

            //new UserControlledSprite("UserControlledSpriteConfig.txt", _board, this);

            _helpFont = Content.Load<SpriteFont>(@"fonts\helpfont");
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");

            if (DrawRanAlready)
            {
                DrawRanAlready = false; 
            }
            else
            {
                return;
            }

            Console.WriteLine("PlayGameState.Update(..) - "+(new Random()).Next());

            //ScreenXOffset = -_board.BoardWidth + Board.SCREEN_WIDTH;
            _board.UpdateBoard(ScreenXOffset);

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                _spriteManager.ReverseTimeForSprites();
            }
            else
            {
                // "Gravity" to pull down the player or enemies if they are in mid-air.
                _spriteManager.ApplyDownwordGravity(this);
                _spriteManager.SavePositionForReverseTime(this);

                _spriteManager.Update(gameTime);

                // Make sure a jumping sprite doesn't go thorugh a platform in mid air.
                // SetSpritePositionIfIntersectingWithGroundOrPlatform possibly modifies the sprites sent in as parameters.
                foreach (var aSprite in _spriteManager.Sprites)
                {
                    SetSpritePositonIfIntersectingUnderneathAPlatform(aSprite);
                }

                // This next loop make sure, if the sprite hits the ground, it will not go through the ground.
                // (On the closest tile that is below the sprite).
                // SetSpritePositionIfIntersectingWithGroundOrPlatform possibly modifies the sprites sent in as parameters.
                foreach (var aSprite in _spriteManager.Sprites)
                {
                    SetSpritePositionIfIntersectingWithGroundOrPlatform(aSprite);
                }

                // Need this!  Changed my mind.  If this isn't in then the user can jump through "sandwiched" platform gaps.
                foreach (var aSprite in _spriteManager.Sprites)
                {
                    MakeSureThatSpriteCanNotGoThroughSideOfGroundOrPlatform(aSprite);
                }
            }
            var newKeyboardState = Keyboard.GetState(); // get the newest state

            SwitchStateLogic.DoChangeGameStateFromKeyboardLogic(newKeyboardState, _oldKeyboardState, OurGame,
                gameTime);

            _oldKeyboardState = newKeyboardState; // set the new state as the old state for next time
        } // end methods

        private void MakeSureThatSpriteCanNotGoThroughSideOfGroundOrPlatform(AnimatedSprite aSprite)
        {
            if (aSprite.CurrentPosition.Y < aSprite.GetLastY() &&
                !aSprite.IsJumping)
            {
                aSprite.CurrentPosition.Y = aSprite.GetLastY();

                if (aSprite.CurrentPosition.X > aSprite.GetLastX())
                {
                    aSprite.CurrentPosition.X = aSprite.GetLastX();
                }
                else
                {
                    aSprite.CurrentPosition.X = aSprite.GetLastX();
                }

                if (aSprite is UserControlledSprite)
                {
                    ScreenXOffset = aSprite.GetLastScreenXOffset();
                }
            }
            else
            {
                aSprite.SetLastXAndY((int) aSprite.CurrentPosition.X,
                    (int) aSprite.CurrentPosition.Y, ScreenXOffset);
            } // end else

            aSprite.BoundingRectangle.X = (int) aSprite.CurrentPosition.X;
            aSprite.BoundingRectangle.Y = (int) aSprite.CurrentPosition.Y;
        }

        private void SetSpritePositonIfIntersectingUnderneathAPlatform(AnimatedSprite aSprite)
        {
            var tilesThatHaveCollisionWithSprite = _board.RetrieveTilesThatIntersectWithThisSprite(aSprite.BoundingRectangle,
                this, (int)aSprite.CurrentPosition.Y).Where(tile => aSprite.CurrentPosition.Y > tile.BoundingRectangle.Y
                                             &&
                                             aSprite.CurrentPosition.Y <
                                             tile.BoundingRectangle.Y + tile.BoundingRectangle.Height);

            if (tilesThatHaveCollisionWithSprite.Count() != 0)
            {
                var firstTile = tilesThatHaveCollisionWithSprite.OrderBy(tile => tile.BoundingRectangle.Y).Last();
                aSprite.CurrentPosition.Y = firstTile.BoundingRectangle.Y + firstTile.BoundingRectangle.Height;
            }
        }

        // This next method is used to load in the sprites and board in the SwitchStateLogic class.
        public void LoadContentForRefresh()
        {
            LoadContent(OurGame.Content);
        }

        private void SetSpritePositionIfIntersectingWithGroundOrPlatform(AnimatedSprite sSprite)
        {
            Debug.Assert(sSprite != null, "sSprite can not be null!");


            // Get all tiles, on the screen, that intersect with our sprite.
            var tilesThatHaveCollisionWithSprite = _board.RetrieveTilesThatIntersectWithThisSprite(sSprite.BoundingRectangle, this, (int)sSprite.CurrentPosition.Y)
                .Where(tile => sSprite.BoundingRectangle.Y < tile.BoundingRectangle.Y);
            //var firstTile = tilesThatHaveCollisionWithSprite.OrderBy(tile => tile.BoundingRectangle.Y).Last();

            // if we are not in mid air then find the tile that interesects our sprite with the least Y co-ordinate.
            if (tilesThatHaveCollisionWithSprite.Count() != 0)
            {
                var leastY = _board.BoardHeight;
                leastY =
                    tilesThatHaveCollisionWithSprite.Select(tile => tile.BoundingRectangle.Y)
                        .Concat(new[] {leastY})
                        .Min();

                // Now make the sprite stay on that tile that was found.
                sSprite.CurrentPosition.Y = leastY - sSprite.BoundingRectangle.Height;
                //Console.WriteLine("COLLISION WITH TILE(S) - "+new Random().Next());
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");
            Debug.Assert(spriteBatch != null, "spriteBatch can not be null!");

            if (!DrawRanAlready)
            {
                DrawRanAlready = true;
            }
            else
            {
                return;
            }

            Console.WriteLine("PlayGameState.Draw(..) - " + (new Random()).Next());

            _board.DrawBoard(spriteBatch, ScreenXOffset, false); // screenXOffset scrolls the board left and right!

            _spriteManager.Draw(spriteBatch);

            spriteBatch.DrawString(_helpFont, "Play Game Mode",
                _positionOfTitle, Color.Black, 0, Vector2.Zero,
                1, SpriteEffects.None, 1);
        }
    }
}