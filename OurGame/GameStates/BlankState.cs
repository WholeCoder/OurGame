using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// My usings.
using OurGame.WindowsGame1;

namespace OurGame.GameStates
{
    // This class doesn't do anything. It is just used to demonstrate setStateWhenUpdating() and setStateWhenInitializing().
    public class BlankState : State
    {
        private KeyboardState _oldKeyboardState;

        private SpriteFont _helpFont;

        // Call setStateWhenUpdating on this instance variable to change to a different game state.
        private Game1 OurGame { get; set; }

        public BlankState()
        {

        }

        public override void Initialize(Game1 ourGame)
        {
            Debug.Assert(ourGame != null, "ourGame can not be null!");

            this.OurGame = ourGame;
        }

        protected override void LoadStatesContent(ContentManager Content)
        {

            this._helpFont = Content.Load<SpriteFont>(@"fonts\helpfont");

        }

        public override void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can not be equal to null!");

            KeyboardState newKeyboardState = Keyboard.GetState();  // get the newest state

            SwitchStateLogic.DoChangeGameStateFromKeyboardLogic(newKeyboardState, _oldKeyboardState, this.OurGame, gameTime);

            _oldKeyboardState = newKeyboardState;  // set the new state as the old state for next time
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(this._helpFont, "Blank Mode",
                                    new Vector2(10, 10), Color.Black, 0, Vector2.Zero,
                                    1, SpriteEffects.None, 1);
        }
    }
}