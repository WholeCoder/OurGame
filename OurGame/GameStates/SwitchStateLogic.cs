using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// Our usings.
using OurGame;
using OurGame.WindowsGame1;

namespace OurGame.GameStates
{
    // This class abstracts out the keyboard keys that are common to all the game's State objects.
    public static class SwitchStateLogic
    {
        public static void DoChangeGameStateFromKeyboardLogic(KeyboardState newKeyboardState, KeyboardState oldKeyboardState, Game1 ourGame, GameTime gameTime)
        {
            Debug.Assert(newKeyboardState != null, "newKeyboardState can not be null!");
            Debug.Assert(oldKeyboardState != null, "oldKeyBoardState can not be null!");
            Debug.Assert(ourGame != null, "ourGame can not be null!");
            Debug.Assert(gameTime != null, "gameTime can not be null!");

            // Press E for edit board state.
            if (newKeyboardState.IsKeyDown(Keys.E) && oldKeyboardState.IsKeyUp(Keys.E))
            {
                if (ourGame.CurrentState != ourGame.EditBoardState)
                {

                    ourGame.SetStateWhenUpdating(ourGame.EditBoardState, gameTime);
                }
            }

            // Press P for play game state.
            if (newKeyboardState.IsKeyDown(Keys.P) && oldKeyboardState.IsKeyUp(Keys.P))
            {
                if (ourGame.CurrentState != ourGame.PlayGameState)
                {
                    ((EditBoardState)ourGame.EditBoardState).SaveBoardToDiskAndLoadItIntoPlayGameState(gameTime);
                    ourGame.SetStateWhenUpdating(ourGame.PlayGameState, gameTime);
                }
            }
            if (newKeyboardState.IsKeyDown(Keys.Q) && oldKeyboardState.IsKeyUp(Keys.Q))
            {
                ourGame.Exit();
            }

            // Press B for the blank state.  Just for testing.
            if (newKeyboardState.IsKeyDown(Keys.B) && oldKeyboardState.IsKeyUp(Keys.B))
            {
                if (ourGame.CurrentState != ourGame.BlankState)
                {

                    ourGame.SetStateWhenUpdating(ourGame.BlankState, gameTime);
                }
            }

            if (newKeyboardState.IsKeyDown(Keys.H) && oldKeyboardState.IsKeyUp(Keys.H))
            {
                if (ourGame.CurrentState != ourGame.HelpMenuState)
                {

                    ourGame.SetStateWhenUpdating(ourGame.HelpMenuState, gameTime);
                }
            }

            if (newKeyboardState.IsKeyDown(Keys.Q) && oldKeyboardState.IsKeyUp(Keys.Q))
            {
                ourGame.Exit();
            }
        } // end method
    } // end class
} // end using
