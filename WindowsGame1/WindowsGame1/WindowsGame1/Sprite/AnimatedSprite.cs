using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using WindowsGameLibrary1;

namespace Sprite
{
    public class AnimatedSprite
    {
        public Point FrameSize { get; set; }
        public Point CurrentFrame { get; set; }
        public Point SheetSize { get; set; }

        private string textureFilename;
        private TextureCache tCache { get; set; }

        public AnimatedSprite(Point frameSize, Point sheetSize, string textureFilename, TextureCache tCache)
        {
            this.FrameSize = frameSize;// new Point(75, 75);
            this.CurrentFrame = new Point(0, 0);
            this.SheetSize = sheetSize;// new Point(6, 8);

            this.textureFilename = textureFilename;
            this.tCache = tCache;
        }

        protected void Initialize()
        {
        }

        protected void LoadContent(ContentManager Content)
        {

        }
        
        protected void UnloadContent()
        {

        }

        protected void Update(GameTime gameTime)
        {
            ++this.CurrentFrame.X;
            if (this.CurrentFrame.X >= this.SheetSize.X)
            {
                this.CurrentFrame.X = 0;
                ++this.CurrentFrame.Y;
                if (this.CurrentFrame.Y >= this.SheetSize.Y)
                    this.CurrentFrame.Y = 0;
            }
        }

        protected override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Might need to call following too!:
            /*
             spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
             */
            spriteBatch.Draw(tCache.GetTexture2DFromString(textureFilename), 
                              Vector2.Zero,
                              new Rectangle(CurrentFrame.X * FrameSize.X,
                              CurrentFrame.Y * FrameSize.Y,
                              FrameSize.X,
                              FrameSize.Y),
                              Color.White, 
                              0, 
                              Vector2.Zero,
                              1, 
                              SpriteEffects.None, 
                              0);
            /*
             // Might need to call the following too!
             spriteBatch.End();
             */
        }
    }
}
