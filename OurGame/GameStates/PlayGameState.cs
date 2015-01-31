using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OurGame.OurGameLibrary;
using OurGame.Sprites;
using OurGame.WindowsGame1;

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

            _spriteManager.Update(gameTime);

            // This next loop make sure, if the sprite hits the ground, it will not go through the ground.
            // (On the closest tile that is below the sprite).
            // SetSpritePositionIfIntersectingWithGroundOrPlatform possibly modifies the sprites sent in as parameters.
            for (var i = 0; i < _spriteManager.Sprites.Count; i++)
            {
                SetSpritePositionIfIntersectingWithGroundOrPlatform(_spriteManager.Sprites[i]);
            }

            // Ensure the game board isn't past it's ends.
            // The code to actaully scroll the board is in the UserControlledSprite class.
            if (ScreenXOffset <= -_board.BoardWidth + Board.SCREEN_WIDTH)
            {
                ScreenXOffset = -_board.BoardWidth + Board.SCREEN_WIDTH;
            }

            if (ScreenXOffset >= 0)
            {
                ScreenXOffset = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                _spriteManager.ReverseTimeForSprites();
            }
            else
            {
                // "Gravity" to pull down the player or enemies if they are in mid-air.
                _spriteManager.ApplyDownwordGravity();
                _spriteManager.SavePositionForReverseTime(this);
            }


            var newKeyboardState = Keyboard.GetState(); // get the newest state

            SwitchStateLogic.DoChangeGameStateFromKeyboardLogic(newKeyboardState, _oldKeyboardState, OurGame, gameTime);

            _oldKeyboardState = newKeyboardState; // set the new state as the old state for next time
        }

        private void SetSpritePositionIfIntersectingWithGroundOrPlatform(AnimatedSprite sSprite)
        {
            Debug.Assert(sSprite != null, "sSprite can not be null!");


            // Get all tiles, on the screen, that intersect with our sprite.
            var tilesThatHaveCollisionWithSprite = _board.RetrieveTilesThatIntersectWithThisSprite(sSprite,
                ScreenXOffset);

            // if we are not in mid air then find the tile that interesects our sprite with the least Y co-ordinate.
            if (tilesThatHaveCollisionWithSprite.Count != 0)
            {
                var leastY = _board.BoardHeight;
                leastY =
                    tilesThatHaveCollisionWithSprite.Select(tile => tile.BoundingRectangle.Y)
                        .Concat(new[] {leastY})
                        .Min();

                // Now make the sprite stay on that tile that was found.
                sSprite.CurrentPosition.Y = leastY - sSprite.BoundingRectangle.Height;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");
            Debug.Assert(spriteBatch != null, "spriteBatch can not be null!");

            _board.DrawBoard(spriteBatch, ScreenXOffset, false); // screenXOffset scrolls the board left and right!

            _spriteManager.Draw(spriteBatch);

            spriteBatch.DrawString(_helpFont, "Play Game Mode",
                new Vector2(10, 10), Color.Black, 0, Vector2.Zero,
                1, SpriteEffects.None, 1);
        }
    }
}