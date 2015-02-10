using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OurGame.GameStates
{
    // This class doesn't do anything. It is just used to demonstrate setStateWhenUpdating() and setStateWhenInitializing().
    public class BlankState : State
    {
        private SpriteFont _helpFont;
        private KeyboardState _oldKeyboardState;
        // Call setStateWhenUpdating on this instance variable to change to a different game state.
        private Game1 OurGame { get; set; }
        
        public override string ToString()
        {
            return "BlankState";
        }

        public override void Initialize(Game1 ourGame)
        {
            Debug.Assert(ourGame != null, "ourGame can not be null!");

            OurGame = ourGame;
        }

        protected override void LoadStatesContent(ContentManager Content)
        {
            Debug.Assert(Content != null, "Content can not be null!");

            _helpFont = Content.Load<SpriteFont>(@"fonts\helpfont");
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can not be equal to null!");

            var newKeyboardState = Keyboard.GetState(); // get the newest state

            SwitchStateLogic.DoChangeGameStateFromKeyboardLogic(newKeyboardState, _oldKeyboardState, OurGame, gameTime);

            _oldKeyboardState = newKeyboardState; // set the new state as the old state for next time
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");
            Debug.Assert(spriteBatch != null, "spriteBatch can not be null!");

            spriteBatch.DrawString(_helpFont, "Blank Mode",
                new Vector2(10, 10), Color.Black, 0, Vector2.Zero,
                1, SpriteEffects.None, 1);
        }
    }
}