using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.Sprites;
using OurGame.Commands;
using OurGame.WindowsGame1;
using OurGame.OurGameLibrary;
using OurGame.Commands.ReverseTimeCommands;

// Created by someone else.
using ParticleEffects;

namespace OurGame.GameStates
{
    public class PlayGameState : State
    {
        Board _board;

        // This is the name the gameboard is saved to when S is pressed.
        readonly string pathToSavedGambeBoardConfigurationFile = @"MyLevel.txt";

        KeyboardState _oldKeyboardState;

        // Call setStateWhenUpdating on this instance variable to change to a different game state.
        private Game1 OurGame { get; set; }

        private SpriteManager _spriteManager;

        private SpriteFont _helpFont;

        public PlayGameState()
        {
        }

        public override void Initialize(Game1 ourGame)
        {
            Debug.Assert(ourGame != null, "ourGame can not be null!");

            this.OurGame = ourGame;
        }

        protected override void LoadStatesContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            Debug.Assert(Content != null, "Content can not be null!");

            _board = new Board(pathToSavedGambeBoardConfigurationFile);

            this._spriteManager = new SpriteManager("MyLevelsEnemySpritesList.txt", _board, this);

            new UserControlledSprite("UserControlledSpriteConfig.txt", _board, this);

            this._helpFont = Content.Load<SpriteFont>(@"fonts\helpfont");
        }

        public override void UnloadContent()
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");

            this._spriteManager.Update(gameTime);

            // This next loop make sure, if the sprite hits the ground, it will not go through the ground.
            // (On the closest tile that is below the sprite).
            // SetSpritePositionIfIntersectingWithGroundOrPlatform possibly modifies the sprites sent in as parameters.
            for (int i = 0; i < this._spriteManager.Sprites.Count; i++)
            {
                SetSpritePositionIfIntersectingWithGroundOrPlatform(this._spriteManager.Sprites[i]);
            }
            
            // Move game board.
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Right))
            {
                ScreenXOffset -= SCROLL_AMOUNT;
            }

            if (keyState.IsKeyDown(Keys.Left))
            {
                ScreenXOffset += SCROLL_AMOUNT;
            }
            


            if (ScreenXOffset <= -this._board.BoardWidth+Board.SCREEN_WIDTH)
            {
                ScreenXOffset = -this._board.BoardWidth+Board.SCREEN_WIDTH;
            }

            if (ScreenXOffset >= 0)
            {
                ScreenXOffset = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                this._spriteManager.ReverseTimeForSprites();
            }
            else
            {
                // "Gravity" to pull down the player or enemies if they are in mid-air.
                this._spriteManager.ApplyDownwordGravity();
                this._spriteManager.SavePositionForReverseTime(this);
            }


            KeyboardState newKeyboardState = Keyboard.GetState();  // get the newest state

            SwitchStateLogic.DoChangeGameStateFromKeyboardLogic(newKeyboardState, _oldKeyboardState, this.OurGame, gameTime);

            _oldKeyboardState = newKeyboardState;  // set the new state as the old state for next time

        }

        private void SetSpritePositionIfIntersectingWithGroundOrPlatform(AnimatedSprite sSprite)
        {
            Debug.Assert(sSprite != null, "sSprite can not be null!");

            
            // Get all tiles, on the screen, that intersect with our sprite.
            List<Tile> tilesThatHaveCollisionWithSprite = this._board.RetrieveTilesThatIntersectWithThisSprite(sSprite, ScreenXOffset);
            
            // if we are not in mid air then find the tile that interesects our sprite with the least Y co-ordinate.
            if (tilesThatHaveCollisionWithSprite.Count != 0)
            {
                int leastY = this._board.BoardHeight;
                leastY = tilesThatHaveCollisionWithSprite.Select(tile => tile.BoundingRectangle.Y).Concat(new[] {leastY}).Min();

                // Now make the sprite stay on that tile that was found.
                sSprite.CurrentPosition.Y = leastY - sSprite.BoundingRectangle.Height;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, SpriteBatch spriteBatch)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");
            Debug.Assert(spriteBatch != null, "spriteBatch can not be null!");

            this._board.DrawBoard(spriteBatch, ScreenXOffset, false);  // screenXOffset scrolls the board left and right!

            this._spriteManager.Draw(spriteBatch);

            spriteBatch.DrawString(this._helpFont, "Play Game Mode",
                            new Vector2(10, 10), Color.Black, 0, Vector2.Zero,
                            1, SpriteEffects.None, 1);
        }
    }
}
