﻿using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// Our usings.
using OurGame;
using OurGame.WindowsGame1;

namespace OurGame.GameStates
{
    // This class abstracts out the keyboard keys that are common to all the game's State objects.
    public class SwitchStateLogic
    {
        public static bool flag = true;

        public static void DoChangeGameStateFromKeyboardLogic(KeyboardState newKeyboardState, KeyboardState oldKeyboardState, Game1 ourGame, GameTime gameTime)
        {
            Debug.Assert(newKeyboardState != null, "newKeyboardState can not be null!");
            Debug.Assert(oldKeyboardState != null, "oldKeyBoardState can not be null!");
            Debug.Assert(ourGame != null, "ourGame can not be null!");
            Debug.Assert(gameTime != null, "gameTime can not be null!");

            // Press E for edit board state.
            if (newKeyboardState.IsKeyDown(Keys.E) && oldKeyboardState.IsKeyUp(Keys.E))
            {
                if (ourGame.CurrentState != ourGame.editBoardState)
                {

                    ourGame.SetStateWhenUpdating(ourGame.editBoardState, gameTime);
                }
            }

            // Press P for play game state.
            if (newKeyboardState.IsKeyDown(Keys.P) && oldKeyboardState.IsKeyUp(Keys.P))
            {
                if (ourGame.CurrentState != ourGame.playGameState)
                {
                    ((EditBoardState)ourGame.editBoardState).SaveBoardToDiskAndReloadPlayGameState(gameTime);
                    ourGame.SetStateWhenUpdating(ourGame.playGameState, gameTime);
                }
            }
            if (newKeyboardState.IsKeyDown(Keys.Q) && oldKeyboardState.IsKeyUp(Keys.Q))
            {
                ourGame.Exit();
            }

            // Press B for the blank state.  Just for testing.
            if (newKeyboardState.IsKeyDown(Keys.B) && oldKeyboardState.IsKeyUp(Keys.B))
            {
                if (ourGame.CurrentState != ourGame.blankState)
                {

                    ourGame.SetStateWhenUpdating(ourGame.blankState, gameTime);
                }
            }

            if (newKeyboardState.IsKeyDown(Keys.H) && oldKeyboardState.IsKeyUp(Keys.H))
            {
                if (ourGame.CurrentState != ourGame.helpMenuState)
                {

                    ourGame.SetStateWhenUpdating(ourGame.helpMenuState, gameTime);
                }
            }

            if (newKeyboardState.IsKeyDown(Keys.Q) && oldKeyboardState.IsKeyUp(Keys.Q))
            {
                ourGame.Exit();
            }
        } // end method
    } // end class
} // end using
