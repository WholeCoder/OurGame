using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D aTile;

        // Level editor instance variables
        int tileWidth = 20;
        int tileHeight = 20;

        //Default width X height it starts in a 800x480 resolution.
        int screenWidth;
        int screenHeight;

        int numberOfHorizontalTiles;
        int numberofVerticalTiles;

        // This is the position of the mouse "locked" onto a grid position.
        Vector2 mouseCursorLockedToNearestGridPositionVector;

        TextureCache tCache;

        MouseState lastMouseState;
        MouseState currentMouseState;

        bool rightClickOccurred = true;
        bool leftClickOccurred = false;

        Texture2D[,] gameBoard;

        // This instance variable lets us scroll the board horizontally.
        int screenXOffset = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;

            screenHeight = Window.ClientBounds.Height;  // defaults to 480
            screenWidth  = Window.ClientBounds.Width;   // defaults to 800

            numberofVerticalTiles   = screenHeight / tileHeight;
            numberOfHorizontalTiles = screenWidth / tileWidth;

            lastMouseState = Mouse.GetState();

            gameBoard = new Texture2D[numberofVerticalTiles, numberOfHorizontalTiles];
            for (int i = 0; i < gameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < gameBoard.GetLength(1); j++)
                {
                    gameBoard[i, j] = null;
                }
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            aTile = Content.Load<Texture2D>(@"Images/tile");

            tCache = new TextureCache(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            MouseState ms = Mouse.GetState();


            // The active state from the last frame is now old
            lastMouseState = currentMouseState;

            // Get the mouse state relevant for this frame
            currentMouseState = Mouse.GetState();

            // Recognize a single click of the right mouse button
            if (lastMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed)
            {
                // React to the click
                // ...
                rightClickOccurred = true;
            }

            if (rightClickOccurred)
            {
                // Flip to the next texture under the mouse pointer.
                this.tCache.NextTexture();
                rightClickOccurred = false;
            }

            // Recognize a single click of the leftmouse button
            if (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
            {
                // React to the click
                // ...
                leftClickOccurred = true;
            }

            if (leftClickOccurred)
            {
                int putInGameArrayY = ms.Y / tileHeight;
                int putInGameArrayX = ms.X / tileWidth;

                if (putInGameArrayY < this.gameBoard.GetLength(0) && putInGameArrayX < this.gameBoard.GetLength(1))
                {
                    this.gameBoard[putInGameArrayY, putInGameArrayX] = this.tCache.GetCurrentTexture();
                }
                leftClickOccurred = false;
            }

            int putX = (ms.X / tileWidth) * tileWidth + screenXOffset % tileWidth;
            int putY = (ms.Y / tileHeight) * tileHeight;

            mouseCursorLockedToNearestGridPositionVector = new Vector2(putX, putY);

            // Move game board.
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Right))
            {
                screenXOffset -= 1;
            }

            if (keyState.IsKeyDown(Keys.Left))
            {
                screenXOffset += 1;
            }

            if (screenXOffset <= -screenWidth)
            {
                screenXOffset = -screenWidth;
            }

            if (screenXOffset >= 0)
            {
                screenXOffset = 0;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            for (int y = 0; y < screenHeight; y += tileHeight)
            {
                C3.XNA.Primitives2D.DrawLine(spriteBatch, new Vector2(0.0f + screenXOffset, y), new Vector2(screenWidth + screenXOffset, y), Color.White);
            }

            for (int x = 0; x < screenWidth+1; x += tileWidth)
            {
                C3.XNA.Primitives2D.DrawLine(spriteBatch, new Vector2(x + screenXOffset, 0.0f), new Vector2(x + screenXOffset, screenHeight), Color.White);
            }

            for (int i = 0; i < gameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < gameBoard.GetLength(1); j++)
                {
                    if (gameBoard[i, j] != null)
                    {
                        Vector2 tilePosition = new Vector2(j * tileWidth + screenXOffset, i * tileHeight);
                        spriteBatch.Draw(gameBoard[i, j], tilePosition, Color.White);
                    }
                }
            }

            spriteBatch.Draw(tCache.GetCurrentTexture(), mouseCursorLockedToNearestGridPositionVector, Color.White);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
