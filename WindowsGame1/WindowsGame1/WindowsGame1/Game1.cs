using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.GameStates;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        // These next methods and properties let us use the State pattern to switch between Game states - i.e. editor, play game, game over.
        public State CurrentState { get; set; }

        // Create one instance variable for each oft the different States that this game will have!
        // Create them in the Initialize method as well.
        public State editBoardState { get; set; }
        public State blankState { get; set; }
        public State playGameState { get; set; }

        /***************************************************************************************************************/
        /*  *note*  These are used by the State subclasses to change state of the game to another. ex) Game Over state.*/

        // This version is called when we change state in an update() call.
        public void SetStateWhenUpdating(State state, GameTime gameTime)
        {
            this.CurrentState = state;
            this.CurrentState.Update(gameTime);
        }

        // This version is called in the Game1.Initilize() method.
        public void SetStateWhenInitializing(State state)
        {
            this.CurrentState = state;
        }

        /***************************************************************************************************************/


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;

            // Create all states that this game will go through here! And initialize them!
            this.editBoardState = new EditBoardState();
            this.editBoardState.Initialize(this);

            this.blankState = new BlankState();
            this.blankState.Initialize(this);

            this.SetStateWhenInitializing(this.editBoardState);

            this.playGameState= new PlayGameState();
            this.playGameState.Initialize(this);

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

            this.editBoardState.LoadContent(Content);
            this.blankState.LoadContent(Content);
            this.playGameState.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Unload any non ContentManager content here

            // Call UnloadContent for every State you create!
            this.editBoardState.UnloadContent();
            this.blankState.UnloadContent();
            this.playGameState.UnloadContent();
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

            this.CurrentState.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            this.CurrentState.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
