using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace GameState
{
    // This class doesn't do anything. It is just used to demonstrate setStateWhenUpdating() and setStateWhenInitializing().
    public class BlankState : State
    {
        KeyboardState oldKeyboardState;

        // Call setStateWhenUpdating on this instance variable to change to a different game state.
        public GameCircularDependencyFixInterface OurGame { get; set; }

        public BlankState()
        {
            
        }

        public override void Initialize(GameCircularDependencyFixInterface ourGame)
        {
            this.OurGame = ourGame;
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {

        }

        public override void UnloadContent()
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            KeyboardState newKeyboardState = Keyboard.GetState();  // get the newest state

            if (newKeyboardState.IsKeyDown(Keys.E) && oldKeyboardState.IsKeyUp(Keys.E))
            {
                this.OurGame.setStateWhenUpdating(this.OurGame.getEditBoardState(), gameTime);
            }

            oldKeyboardState = newKeyboardState;  // set the new state as the old state for next time
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {

        }
    }
}
