using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using WindowsGame1;
using WindowsGameLibrary1;


namespace GameState
{
    class PlayGameState : State
    {
        Board board;
        
        // Holds textures so they aren't re-created.
        TextureCache tCache;

        // This instance variable lets us scroll the board horizontally.
        int screenXOffset = 0;
        int scrollAmount = 5;

        // This is the name the gameboard is saved to when S is pressed.
        string pathToSavedGambeBoardConfigurationFile = @"MyLevel.txt";
        string pathToTextureCacheConfig = @"TextureCache.txt";

        KeyboardState oldKeyboardState;

        // Call setStateWhenUpdating on this instance variable to change to a different game state.
        public Game1 OurGame { get; set; }

        public PlayGameState()
        {
        }

        public override void Initialize(Game1 ourGame)
        {
            this.OurGame = ourGame;
        }

        public override void LoadContent(ContentManager Content)
        {
            tCache = new TextureCache(pathToTextureCacheConfig, Content);
            board = new Board(pathToSavedGambeBoardConfigurationFile, tCache); // MUST have tCache created before calling this!
        }

        public override void UnloadContent()
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {

            // Move game board.
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Right))
            {
                screenXOffset -= scrollAmount;
            }

            if (keyState.IsKeyDown(Keys.Left))
            {
                screenXOffset += scrollAmount;
            }

            if (screenXOffset <= -this.board.ScreenWidth)
            {
                screenXOffset = -this.board.ScreenWidth;
            }

            if (screenXOffset >= 0)
            {
                screenXOffset = 0;
            }

            KeyboardState newKeyboardState = Keyboard.GetState();  // get the newest state

            if (newKeyboardState.IsKeyDown(Keys.Q) && oldKeyboardState.IsKeyUp(Keys.Q))
            {
                this.OurGame.Exit();
            }

            // Press B for the blank state.  Just for testing.
            if (newKeyboardState.IsKeyDown(Keys.B) && oldKeyboardState.IsKeyUp(Keys.B))
            {
                this.OurGame.setStateWhenUpdating(this.OurGame.blankState, gameTime);
            }

            // Press E for editor state.
            if (newKeyboardState.IsKeyDown(Keys.E) && oldKeyboardState.IsKeyUp(Keys.E))
            {
                this.OurGame.setStateWhenUpdating(this.OurGame.editBoardState, gameTime);
            }

            oldKeyboardState = newKeyboardState;  // set the new state as the old state for next time

        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.board.DrawBoard(spriteBatch, screenXOffset);  // screenXOffset scrolls the board left and right!
        }
    }
}
