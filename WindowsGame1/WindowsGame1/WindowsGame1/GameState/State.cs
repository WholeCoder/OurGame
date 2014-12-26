using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using WindowsGame1;

namespace GameState
{
    public abstract class State
    {
        abstract public void Initialize(Game1 ourGame);
        abstract public void LoadContent(ContentManager Content);
        abstract public void UnloadContent();
        abstract public void Update(GameTime gameTime);  // NOTE:  This method is called in the Board.setState(...) method!
        abstract public void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
