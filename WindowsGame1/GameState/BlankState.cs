using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameState
{
    // This class doesn't do anything. It is just used to demonstrate setStateWhenUpdating() and setStateWhenInitializing().
    public class BlankState : State
    {
        public BlankState()
        {
            
        }

        public override void Initialize(Microsoft.Xna.Framework.Game ourGame)
        {

        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {

        }

        public override void UnloadContent()
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {

        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {

        }
    }
}
