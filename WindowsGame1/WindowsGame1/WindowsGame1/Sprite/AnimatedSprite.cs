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
        private Point FrameSize;
        private Point CurrentFrame;
        private Point SheetSize;

        private string textureFilename;
        private Texture2D theTexture;
        private TextureCache tCache { get; set; }

        public AnimatedSprite(Point frameSize, Point sheetSize, string textureFilename, TextureCache tCache)
        {
            this.FrameSize = frameSize;// new Point(75, 75);
            this.CurrentFrame = new Point(0, 0);
            this.SheetSize = sheetSize;// new Point(6, 8);

            this.textureFilename = textureFilename;
            this.theTexture = tCache.GetTexture2DFromStringSpriteArray(textureFilename);
            this.tCache = tCache;
        }

        public void Update(GameTime gameTime)
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

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tCache.GetTexture2DFromStringSpriteArray(textureFilename), 
                              Vector2.Zero,
                              new Rectangle(CurrentFrame.X * FrameSize.X + CurrentFrame.X + 1,// CurrentFrame.X+1 is an offset for pixel boundaries in image
                              CurrentFrame.Y * FrameSize.Y,
                              FrameSize.X,
                              FrameSize.Y),
                              Color.White, 
                              0, 
                              Vector2.Zero,
                              1, 
                              SpriteEffects.None, 
                              0);
        } // end Draw method
    } // end class
} // end namespace
