using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using WindowsGameLibrary1;

namespace Sprite
{
    public abstract class AnimatedSprite
    {
        public Vector2 CurrentPosition { get; set; }

        protected Point FrameSize;
        protected Point CurrentFrame;
        protected Point SheetSize;

        protected string textureFilename;
        protected Texture2D theTexture;
        protected TextureCache tCache { get; set; }

        private int ElapsedGameTime;  // Used to slow down the animaiton of this AnimatedSprite.
        private int TimeBetweenFrames;

        public AnimatedSprite(Point frameSize, Point sheetSize, string textureFilename, TextureCache tCache, int TimeBetweenFrames, Vector2 CurrentPosition)
        {
            this.FrameSize = frameSize;
            this.CurrentFrame = new Point(0, 0);
            this.SheetSize = sheetSize;
            this.ElapsedGameTime = 0;
            this.TimeBetweenFrames = TimeBetweenFrames;
            this.CurrentFrame = CurrentFrame;

            this.textureFilename = textureFilename;
            this.theTexture = tCache.GetTexture2DFromStringSpriteArray(textureFilename);
            this.tCache = tCache;
        }

        public void Update(GameTime gameTime)
        {
            this.ElapsedGameTime += gameTime.ElapsedGameTime.Milliseconds;

            if (this.ElapsedGameTime >= this.TimeBetweenFrames) // 100 milliseconds
            {
                this.ElapsedGameTime = 0;

                ++this.CurrentFrame.X;
                if (this.CurrentFrame.X >= this.SheetSize.X)
                {
                    this.CurrentFrame.X = 0;
                    ++this.CurrentFrame.Y;
                    if (this.CurrentFrame.Y >= this.SheetSize.Y)
                        this.CurrentFrame.Y = 0;
                }
            } // end if
        } // end method

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tCache.GetTexture2DFromStringSpriteArray(textureFilename),
                              this.CurrentPosition,
                              new Rectangle(CurrentFrame.X * FrameSize.X + CurrentFrame.X + 1,// CurrentFrame.X+1 is an offset for pixel boundaries in image
                              CurrentFrame.Y * FrameSize.Y,
                              FrameSize.X,
                              FrameSize.Y),
                              Color.White,
                              0,
                              Vector2.Zero,
                              10, // scale
                              SpriteEffects.None,
                              0);
        } // end Draw method

        public abstract AnimatedSprite Load(string filepath);
    } // end class
} // end namespace
