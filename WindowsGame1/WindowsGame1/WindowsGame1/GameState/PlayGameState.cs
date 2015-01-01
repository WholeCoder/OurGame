using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using Sprite;
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
        string pathToTextureCacheConfig = @"BoardTextureCache.txt";
        string pathToSpriteTextureCacheConfig = @"SpriteTextureCache.txt";

        KeyboardState oldKeyboardState;

        // Call setStateWhenUpdating on this instance variable to change to a different game state.
        public Game1 OurGame { get; set; }

        public UserControlledSprite Player { get; set; }

        public PlayGameState()
        {
        }

        public override void Initialize(Game1 ourGame)
        {
            this.OurGame = ourGame;
        }

        public override void LoadContent(ContentManager Content)
        {
            tCache = new TextureCache(pathToTextureCacheConfig, pathToSpriteTextureCacheConfig,Content);
            board = new Board(pathToSavedGambeBoardConfigurationFile, tCache); // MUST have tCache created before calling this!

            // old UserControlledSprite/*new Microsoft.Xna.Framework.Point(20, 20), new Microsoft.Xna.Framework.Point(2, 0), "Images/spritesheets/manspritesheet", tCache, 100, new Vector2(100, 100)*/
            // TODO:  Create the "UserControlledSpriteConfig.txt" file or make the class create it if not found.
            Player = new UserControlledSprite(tCache, "UserControlledSpriteConfig.txt");
        }

        public override void UnloadContent()
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Player.Update(gameTime);

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
            this.board.DrawBoard(spriteBatch, screenXOffset, false);  // screenXOffset scrolls the board left and right!
            Player.Draw(gameTime, spriteBatch);
        }
    }
}
