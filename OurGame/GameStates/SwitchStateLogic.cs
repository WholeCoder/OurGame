using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using OurGame.WindowsGame1;

namespace OurGame.GameStates
{
    // This class abstracts out the keyboard keys that are common to all the game's State objects.
    public class SwitchStateLogic
    {
        public static bool flag = true;

        public static void DoChangeGameStateFromKeyboardLogic(KeyboardState newKeyboardState, KeyboardState oldKeyboardState, Game1 OurGame, GameTime gameTime)
        {
            // Press E for edit board state.
            if (newKeyboardState.IsKeyDown(Keys.E) && oldKeyboardState.IsKeyUp(Keys.E))
            {
                if (OurGame.CurrentState != OurGame.editBoardState)
                {

                    OurGame.SetStateWhenUpdating(OurGame.editBoardState, gameTime);
                }
            }

            // Press P for play game state.
            if (newKeyboardState.IsKeyDown(Keys.P) && oldKeyboardState.IsKeyUp(Keys.P))
            {
                if (OurGame.CurrentState != OurGame.playGameState)
                {
                    ((EditBoardState)OurGame.editBoardState).SaveBoardToDiskAndReloadPlayGameState(gameTime);
                    OurGame.SetStateWhenUpdating(OurGame.playGameState, gameTime);
                }
            }
            if (newKeyboardState.IsKeyDown(Keys.Q) && oldKeyboardState.IsKeyUp(Keys.Q))
            {
                OurGame.Exit();
            }

            // Press B for the blank state.  Just for testing.
            if (newKeyboardState.IsKeyDown(Keys.B) && oldKeyboardState.IsKeyUp(Keys.B))
            {
                if (OurGame.CurrentState != OurGame.blankState)
                {

                    OurGame.SetStateWhenUpdating(OurGame.blankState, gameTime);
                }
            }

            if (newKeyboardState.IsKeyDown(Keys.H) && oldKeyboardState.IsKeyUp(Keys.H))
            {
                if (OurGame.CurrentState != OurGame.helpMenuState)
                {

                    OurGame.SetStateWhenUpdating(OurGame.helpMenuState, gameTime);
                }
            }

            if (newKeyboardState.IsKeyDown(Keys.Q) && oldKeyboardState.IsKeyUp(Keys.Q))
            {
                OurGame.Exit();
            }
        } // end method
    } // end class
} // end using
