using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameState
{
    public abstract class State
    {
        abstract public void Initialize();
        abstract public void LoadContent();
        abstract public void UnloadContent();
        abstract public void Update(GameTime gameTime);
        abstract public void Draw(GameTime gameTime);
    }
}
