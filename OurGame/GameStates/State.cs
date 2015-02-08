using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OurGame.OurGameLibrary;

namespace OurGame.GameStates
{
    public abstract class State
    {
        // Scroll amount lef tand right.0.
        public const int SCROLL_AMOUNT = 5;
        // This instance variable lets us scroll the board horizontally.

        protected State()
        {
            ScreenXOffset = 0;
        }

        public int ScreenXOffset { get; set; }

        public abstract void Initialize(Game1 ourGame);
        // Don't override this to load the content!  Use the method below!
        public void LoadContent(ContentManager Content)
        {
            Debug.Assert(Content != null, "Content can not be null!");

            // This is the reason that this method was implemented - to call TextureCache.SetContent(Content).
            TextureCache.SetContent(Content); // Must be called before TextureCache.getInstance() - a Singleton.
            SoundSystem.SetContent(Content);

            LoadStatesContent(Content);
        }

        // Use this next method to load the state's Content.
        protected abstract void LoadStatesContent(ContentManager Content);
        public abstract void UnloadContent();
        public abstract void Update(GameTime gameTime);
        // NOTE:  This method is called in the Board.setState(...) method!
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}