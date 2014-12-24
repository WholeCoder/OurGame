using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameState
{
    // This interface is used to avoid a circular reference between the Game1 class and State classes.  It is applied to the Game1 class.
    public interface GameCircularDependencyFixInterface // for Game1 class!
    {
        // These next methods are used by the State classes to switch between States.
        void setStateWhenUpdating(State state, GameTime gameTime);
        void setStateWhenInitializing(State state);

        // Add a new one of these for each new state in the Game1 class.
        State getBlankState();
        State getEditBoardState();

        void Exit2(); // Alias for the Exit() method that quits the application altogether.
    }
}
