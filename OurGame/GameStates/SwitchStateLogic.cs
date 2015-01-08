using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using OurGame.WindowsGame1;

namespace OurGame.GameStates
{
    public class SwitchStateLogic
    {
        public static bool flag = true;

        public static void DoChangeGameStateFromKeyboardLogic(KeyboardState newKeyboardState, KeyboardState oldKeyboardState, Game1 OurGame, GameTime gameTime)
        {
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
