using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.Sprites;
using WindowsGame1;
using OurGame.WindowsGameLibrary1;

// Created by someone else.
using ParticleEffects;

namespace OurGame.GameStates
{
    class PlayGameState : State
    {
        cEffectManager myEffectsManager;
        int keyboardDelayCounter = 0;

        bool fireIsRunning = false;
        bool fireworksAreRunning = false;
        bool snowIsFalling = false;
        bool smokeIsRunning = false;
        bool spiralIsRunning = false;

        Board board;
        
        // This instance variable lets us scroll the board horizontally.
        int screenXOffset = 0;
        int scrollAmount = 5;

        // This is the name the gameboard is saved to when S is pressed.
        string pathToSavedGambeBoardConfigurationFile = @"MyLevel.txt";
        string pathToTextureCacheConfig = @"BoardTextureCache.txt";
        string pathToSpriteTextureCacheConfig = @"SpriteTextureCache.txt";

        KeyboardState oldKeyboardState;

        // Call setStateWhenUpdating on this instance variable to change to a different game state.
        public Game1 OurGame { get; set; }

        public AnimatedSprite Player { get; set; }

        public PlayGameState()
        {
            myEffectsManager = new cEffectManager();
        }

        public override void Initialize(Game1 ourGame)
        {
            this.OurGame = ourGame;
        }

        public override void LoadContent(ContentManager Content)
        {
            TextureCache.setupFileNamesAndcontent(pathToTextureCacheConfig, pathToSpriteTextureCacheConfig, Content);

            board = new Board(pathToSavedGambeBoardConfigurationFile);

            // TODO:  Create the "UserControlledSpriteConfig.txt" file or make the class create it if not found.
            Player = new UserControlledSprite("UserControlledSpriteConfig.txt");

            myEffectsManager.LoadContent(Content);
        }

        public override void UnloadContent()
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Player.Update(gameTime);

            // Move game board.
            KeyboardState keyState = Keyboard.GetState();

            if (keyboardDelayCounter > 0)
            {
                keyboardDelayCounter -= gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up) && !fireworksAreRunning)
                {
                    //myEffectsManager.AddEffect(eEffectType.explosion);
                    keyboardDelayCounter = 300;
                    fireworksAreRunning = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down) && !fireIsRunning)
                {
                    //myEffectsManager.AddEffect(eEffectType.fire);
                    keyboardDelayCounter = 300;
                    fireIsRunning = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left) && !snowIsFalling)
                {
                    //myEffectsManager.AddEffect(eEffectType.snow);
                    keyboardDelayCounter = 300;
                    snowIsFalling = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right) && !smokeIsRunning)
                {
                    //myEffectsManager.AddEffect(eEffectType.smoke);
                    keyboardDelayCounter = 300;
                    smokeIsRunning = false;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && !spiralIsRunning)
                {
                    //myEffectsManager.AddEffect(eEffectType.spiral);
                    keyboardDelayCounter = 300;
                    spiralIsRunning = true;
                }
            }

            myEffectsManager.Update(gameTime);


            if (!this.board.IsThereACollisionWith(Player, screenXOffset))
            {
                if (keyState.IsKeyDown(Keys.Right))
                {
                    screenXOffset -= scrollAmount;
                }

                if (keyState.IsKeyDown(Keys.Left))
                {
                    screenXOffset += scrollAmount;
                }
            }

            if (screenXOffset <= -this.board.BoardWidth)
            {
                screenXOffset = -this.board.BoardWidth;
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
                this.OurGame.SetStateWhenUpdating(this.OurGame.blankState, gameTime);
            }

            // Press E for editor state.
            if (newKeyboardState.IsKeyDown(Keys.E) && oldKeyboardState.IsKeyUp(Keys.E))
            {
                this.OurGame.SetStateWhenUpdating(this.OurGame.editBoardState, gameTime);
            }

            oldKeyboardState = newKeyboardState;  // set the new state as the old state for next time

        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.board.DrawBoard(spriteBatch, screenXOffset, false);  // screenXOffset scrolls the board left and right!
            Player.Draw(spriteBatch);
            myEffectsManager.Draw(spriteBatch);
        }
    }
}
