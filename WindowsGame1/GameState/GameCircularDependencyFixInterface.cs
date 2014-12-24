using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameState
{
    // This interface is used to avoid a circular reference.  It is applied to the Game1 class.
    public interface GameCircularDependencyFixInterface
    {
        void setStateWhenUpdating(State state, GameTime gameTime);
        void setStateWhenInitializing(State state);

        State getBlankState();
        State getEditBoardState();

        void Exit2();
    }
}
