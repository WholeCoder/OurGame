using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.WindowsGame1;
using OurGame.OurGameLibrary;

namespace OurGame.GameStates
{
    public abstract class State
    {
        abstract public void Initialize(Game1 ourGame);

        // Don't override this to load the content!  Use the method below!
        public void LoadContent(ContentManager Content)
        {
            // This is the reason that this method was implemented - to call TextureCache.SetContent(Content).
            TextureCache.SetContent(Content); // Must be called before TextureCache.getInstance() - a Singleton.

            this.LoadStatesContent(Content);
        }

        // Use this next method to load the state's Content.
        abstract protected void LoadStatesContent(ContentManager Content);
        
        abstract public void UnloadContent();
        abstract public void Update(GameTime gameTime);  // NOTE:  This method is called in the Board.setState(...) method!
        abstract public void Draw(GameTime gameTime, SpriteBatch spriteBatch); // NOTE:  spriteBatch.Begin()/End() are already called before and after this method!

    }
}
