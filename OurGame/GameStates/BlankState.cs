using Microsoft.Xna.Framework.Input;

// My usings.
using OurGame.WindowsGame1;
using OurGame.OurGameLibrary;

namespace OurGame.GameStates
{
    // This class doesn't do anything. It is just used to demonstrate setStateWhenUpdating() and setStateWhenInitializing().
    public class BlankState : State
    {
        private KeyboardState _oldKeyboardState;

        // Call setStateWhenUpdating on this instance variable to change to a different game state.
        public Game1 OurGame { get; set; }

        public BlankState()
        {

        }

        public override void Initialize(Game1 ourGame)
        {
            this.OurGame = ourGame;
        }

        protected override void LoadStatesContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {

        }

        public override void UnloadContent()
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            KeyboardState newKeyboardState = Keyboard.GetState();  // get the newest state

            SwitchStateLogic.DoChangeGameStateFromKeyboardLogic(newKeyboardState, _oldKeyboardState, this.OurGame, gameTime);

            _oldKeyboardState = newKeyboardState;  // set the new state as the old state for next time
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {

        }
    }
}