using System;
using System.IO;
using System.Text;
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

        protected int ElapsedGameTime;  // Used to slow down the animaiton of this AnimatedSprite.
        protected int TimeBetweenFrames;

        public AnimatedSprite(TextureCache tCache)
        {
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

        // This will start at the startOffset and read out it's attributes.
        protected int Load(string[] configArray, int startOffset)
        {
            /*
                UserControlledSprite - ALREADY READ IN
                0,0 - startOffset is set here 
                0,0
                2,1
                100
                Images/spritesheets/manspritesheet

                 */
            // Convert.ToInt32(configStringSplitRay[0].Split(':')[1]);
            
            // read in properties for the AnimatedSprite starting at startOffset.
            this.FrameSize = new Point(Convert.ToInt32(configArray[startOffset].Split(',')[0]),
                                       Convert.ToInt32(configArray[startOffset].Split(',')[1]));

            this.CurrentFrame = new Point(Convert.ToInt32(configArray[startOffset+1].Split(',')[0]),
                                          Convert.ToInt32(configArray[startOffset+1].Split(',')[0]));

            this.SheetSize = new Point(Convert.ToInt32(configArray[startOffset+2].Split(',')[0]),
                                          Convert.ToInt32(configArray[startOffset+2].Split(',')[0]));

            this.TimeBetweenFrames = Convert.ToInt32(configArray[startOffset+3]);

            this.textureFilename = configArray[startOffset+4];
            this.theTexture = tCache.GetTexture2DFromStringSpriteArray(this.textureFilename);

            Console.WriteLine("this.textureFilename == " + this.textureFilename);

            return startOffset+5;
        }


        public abstract void Load(string filepath);

        // Must wrap these in a filestream.  This will just append the attributes to the file
        public virtual void WriteToDisk(FileStream fs)
        {
            this.FrameSize = new Point(20, 20) ;
            AddText(fs,this.FrameSize.X+","+this.FrameSize.Y);
            AddText(fs, "\n");

            this.CurrentFrame = new Point(0, 0);
            AddText(fs,this.CurrentFrame.X+","+this.CurrentFrame.Y);
            AddText(fs, "\n");

            this.SheetSize = new Point(2,0);
            AddText(fs,this.SheetSize.X+","+this.SheetSize.Y);
            AddText(fs, "\n");

            this.TimeBetweenFrames = 100;
            AddText(fs, this.TimeBetweenFrames+"");
            AddText(fs, "\n");

            this.textureFilename = "Images/spritesheets/manspritesheet";
            AddText(fs,this.textureFilename);
            AddText(fs, "\n");

            this.ElapsedGameTime = 0; // SKIPPTED!!!!!!!!!!!!!!!!!!!! NOT WRITTEN TO DISK
            this.theTexture = tCache.GetTexture2DFromStringSpriteArray(textureFilename);
        }

        // Useful in sub-classes.
        protected static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        } // end method
    } // end class
} // end namespace
