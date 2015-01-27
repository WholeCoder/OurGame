using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.GameStates;

namespace OurGame.WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        SpriteBatch _spriteBatch;

        public Game1()
        {
            new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        // These next methods and properties let us use the State pattern to switch between Game states - i.e. editor, play game, game over.
        public State CurrentState { get; set; }

        // Create one instance variable for each oft the different States that this game will have!
        // Create them in the Initialize method as well.
        public State EditBoardState { get; set; }
        public State BlankState { get; set; }
        public State PlayGameState { get; set; }
        public State HelpMenuState { get; set; }

        /***************************************************************************************************************/
        /*  *note*  These are used by the State subclasses to change state of the game to another. ex) Game Over state.*/

        // This version is called when we change state in an updateObserver() call.
        public void SetStateWhenUpdating(State state, GameTime gameTime)
        {
            Debug.Assert(state != null, "state can't be null!");
            Debug.Assert(gameTime != null, "gameTime can't be null!");

            this.CurrentState = state;
            this.CurrentState.Update(gameTime);
        }

        // This version is called in the Game1.Initilize() method.
        private void SetStateWhenInitializing(State state)
        {
            Debug.Assert(state != null, "state can't be null!");

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
            this.EditBoardState = new EditBoardState();
            this.EditBoardState.Initialize(this);

            this.SetStateWhenInitializing(this.EditBoardState);

            this.BlankState = new BlankState();
            this.BlankState.Initialize(this);

            this.PlayGameState = new PlayGameState();
            this.PlayGameState.Initialize(this);

            this.HelpMenuState = new HelpMenuState();
            this.HelpMenuState.Initialize(this);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            this.EditBoardState.LoadContent(Content);
            this.BlankState.LoadContent(Content);
            this.PlayGameState.LoadContent(Content);
            this.HelpMenuState.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Unload any non ContentManager content here

            // Call UnloadContent for every State you create!
            this.EditBoardState.UnloadContent();
            this.BlankState.UnloadContent();
            this.PlayGameState.UnloadContent();
            this.HelpMenuState.UnloadContent();
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

            _spriteBatch.Begin();

            this.CurrentState.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
