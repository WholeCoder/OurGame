using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OurGame.GameStates;

namespace OurGame.WindowsGame1
{
    /// <summary>
    ///     This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private SpriteBatch _spriteBatch;

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
        public State EditSpritesState { get; set; }
        /***************************************************************************************************************/
        /*  *note*  These are used by the State subclasses to change state of the game to another. ex) Game Over state.*/

        // This version is called when we change state in an updateObserver() call.
        public void SetStateWhenUpdating(State state, GameTime gameTime)
        {
            Debug.Assert(state != null, "state can't be null!");
            Debug.Assert(gameTime != null, "gameTime can't be null!");

            CurrentState = state;
            CurrentState.Update(gameTime);
        }

        // This version is called in the Game1.Initilize() method.
        private void SetStateWhenInitializing(State state)
        {
            Debug.Assert(state != null, "state can't be null!");

            CurrentState = state;
        }

        /***************************************************************************************************************/


        /// <summary>
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it can query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;

            // Create all states that this game will go through here! And initialize them!
            EditBoardState = new EditBoardState();
            EditBoardState.Initialize(this);

            BlankState = new BlankState();
            BlankState.Initialize(this);

            PlayGameState = new PlayGameState();
            PlayGameState.Initialize(this);

            HelpMenuState = new HelpMenuState();
            HelpMenuState.Initialize(this);

            EditSpritesState = new EditSpritesState();
            EditSpritesState.Initialize(this);

            SetStateWhenInitializing(EditSpritesState);

            base.Initialize();
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            EditBoardState.LoadContent(Content);
            BlankState.LoadContent(Content);
            PlayGameState.LoadContent(Content);
            HelpMenuState.LoadContent(Content);
            EditSpritesState.LoadContent(Content);
        }

        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Unload any non ContentManager content here

            // Call UnloadContent for every State you create!
            EditBoardState.UnloadContent();
            BlankState.UnloadContent();
            PlayGameState.UnloadContent();
            HelpMenuState.UnloadContent();
            EditSpritesState.UnloadContent();
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            CurrentState.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            CurrentState.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}