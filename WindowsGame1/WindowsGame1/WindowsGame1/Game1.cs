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
        Texture2D texture;
        string str1 = @"images\head";
        string str2 = "images\\head";
        int i = 0;
        int delta = 1;

        int stardelta1;
        int star1y = 0;

        int stardelta2;
        int star2y = 0;

        Texture2D textureTransparent;
        Texture2D aTile;

        Random rnd = new Random();

        // Level editor instance variables
        int tileWidth = 20;
        int tileHeight = 20;

        //Default width X height it starts in a 800x600 resolution.
        int width = 0;
        int height = 0;

        int numberOfHorizontalTiles;
        int numberofVerticalTiles;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public int getRandomDelta()
        {
            return rnd.Next(1, 5); 
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
            stardelta1 = getRandomDelta();// creates a number between 1 and 12
            stardelta2 = getRandomDelta(); // creates a number between 1 and 12

            this.IsMouseVisible = true;

            height = Window.ClientBounds.Height;
            width = Window.ClientBounds.Width;

            numberofVerticalTiles = height / tileHeight;
            numberOfHorizontalTiles = width / tileWidth;

            Console.WriteLine("height = " + height);
            Console.WriteLine("width = " + width);

            Console.WriteLine("numberofVerticalTiles = " + numberofVerticalTiles);
            Console.WriteLine("numberOfHorizontalTiles = " + numberOfHorizontalTiles);

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
            texture = Content.Load<Texture2D>(@"Images/head");

            textureTransparent = Content.Load<Texture2D>(@"Images/transparent_star");
            aTile = Content.Load<Texture2D>(@"Images/tile");
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
            i += delta;

            if ((delta > 0 && i >= 100)  || (delta < 0 && i <= 0))
            {
                delta *= -1;
            }

            // Window.ClientBounds.Height

            // Make stars fall like snowflakes
            star1y += stardelta1;
            if (star1y > 100)
            {
                star1y = 0;
                stardelta1 = getRandomDelta();
            }

            star2y += stardelta2;
            if (star2y > 100)
            {
                star2y = 0;
                stardelta2 = getRandomDelta();
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
            MouseState ms = Mouse.GetState();

            /*int tileWidth = 20;
            int tileHeight = 20;
            */
            int putX = (ms.X / tileWidth) * tileWidth;
            int putY = (ms.Y / tileHeight) * tileHeight;

            Vector2 vec3 = new Vector2(putX, putY);//star2y

            spriteBatch.Begin();

            spriteBatch.Draw(aTile, vec3, Color.White);
            
            /*            Vector2 vec = new Vector2(0.0f + i, 0.0f);
                        Vector2 vec2 = new Vector2(0.0f, star1y); 
                        Vector2 vec3 = new Vector2(ms.X, ms.Y);//star2y

                        //Vector2.Zero
                        spriteBatch.Begin();
                        spriteBatch.Draw(texture, vec, Color.White);
                        spriteBatch.Draw(textureTransparent, vec2, Color.White);
                        C3.XNA.Primitives2D.FillRectangle(spriteBatch, new Rectangle(100, 100, 10, 10), Color.Red);
            */
            for (int y = 0; y < height; y += tileHeight)
            {
                C3.XNA.Primitives2D.DrawLine(spriteBatch, new Vector2(0.0f, y), new Vector2(width, y), Color.White);
            }

            for (int x = 0; x < width; x += tileWidth)
            {
                C3.XNA.Primitives2D.DrawLine(spriteBatch, new Vector2(x, 0.0f), new Vector2(x, height), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
