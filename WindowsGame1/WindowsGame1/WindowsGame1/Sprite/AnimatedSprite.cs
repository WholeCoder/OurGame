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
        public Vector2 InitialPosition { get; set; }

        private Point leftFrameSize;
        private Point rightFrameSize;
        private Point atRestFrameSize;

        private Point leftSheetSize;
        private Point rightSheetSize;
        private Point atRestSheetSize;

        private string leftTextureFilename;
        private string rightTextureFilename;
        private string atRestTextureFilename;



        private Point CurrentFrame;
        private String CurrentTextureFilename; // The textures are received from the TextureCache.
        private Point CurrentSheetSize;
        private Point CurrentFrameSize;

        private TextureCache tCache { get; set; }

        private int ElapsedGameTime;  // Used to slow down the animaiton of this AnimatedSprite.
        private int TimeBetweenFrames;

        public AnimatedSprite(TextureCache tCache, string configFilePathAndName)
        {
            this.tCache = tCache;
            this.Load(configFilePathAndName);
            this.SwitchToGoRightTexture();
        }

        private void NextFrame(GameTime gameTime)
        {
            this.ElapsedGameTime += gameTime.ElapsedGameTime.Milliseconds;

            if (this.ElapsedGameTime >= this.TimeBetweenFrames) // 100 milliseconds
            {
                this.ElapsedGameTime = 0;

                ++this.CurrentFrame.X;
                //Console.WriteLine("this.CurrentFrame.X == " + this.CurrentFrame.X);
                if (this.CurrentFrame.X >= this.CurrentSheetSize.X)
                {
                    this.CurrentFrame.X = 0;
                    ++this.CurrentFrame.Y;
                    if (this.CurrentFrame.Y >= this.CurrentSheetSize.Y)
                        this.CurrentFrame.Y = 0;
                }
            } // end if
        }

        public void SwitchToGoRightTexture()
        {
            this.CurrentFrame = new Point(0, 0);
            this.CurrentTextureFilename = this.rightTextureFilename;
            this.CurrentSheetSize = this.rightSheetSize;
            this.CurrentFrameSize = this.rightFrameSize;
        }

        public void SwitchToGoLeftTexture()
        {
            this.CurrentFrame = new Point(0, 0);
            this.CurrentTextureFilename = this.leftTextureFilename;
            this.CurrentSheetSize = this.leftSheetSize;
            this.CurrentFrameSize = this.leftFrameSize;
        }

        public void SwitchToAtRestTexture()
        {
            this.CurrentFrame = new Point(0, 0);
            this.CurrentTextureFilename = this.atRestTextureFilename;
            this.CurrentSheetSize = this.atRestSheetSize;
            this.CurrentFrameSize = this.atRestFrameSize;
        }



        // A template method.
        public virtual void Update(GameTime gameTime)
        {
            this.NextFrame(gameTime);

            this.UpdateInAfterNextFrame(gameTime);
        } // end method

        public abstract void UpdateAfterNextFrame(GameTime gameTime);


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tCache.GetTexture2DFromStringSpriteArray(CurrentTextureFilename),
                              this.CurrentPosition,
                              new Rectangle(CurrentFrame.X * CurrentFrameSize.X + CurrentFrame.X + 1,// CurrentFrame.X+1 is an offset for pixel boundaries in image
                              CurrentFrame.Y * CurrentFrameSize.Y,
                              CurrentFrameSize.X,
                              CurrentFrameSize.Y),
                              Color.White,
                              0,
                              Vector2.Zero,
                              10, // scale
                              SpriteEffects.None,
                              0);
        } // end Draw method

        // This will start at the startOffset and read out it's attributes.
        public abstract void Load(string[] configArray, int startOffset);
        public abstract string NameOfThisSubclassForWritingToConfigFile();

        // In this method we use fs to write out the subclasses properties.
        public abstract void Write(FileStream fs);


        // This is the template method pattern.
        public void Load(string filepath)
        {
            if (!File.Exists(filepath))
            {
                using (FileStream fs = File.Create(filepath))
                {
                    // Set defaults

                    // Write "AutomatedSprite" to file and a \n.
                    AddText(fs, this.NameOfThisSubclassForWritingToConfigFile()); // ex) "UserControlledSprite"
                    AddText(fs, "\n");

                    this.InitialPosition = new Vector2(100, 100);
                    AddText(fs, this.InitialPosition.X + "," + this.InitialPosition.Y);
                    AddText(fs, "\n");


                    this.leftFrameSize = new Point(20, 20);
                    AddText(fs, this.leftFrameSize.X + "," + this.leftFrameSize.Y);
                    AddText(fs, "\n");

                    this.rightFrameSize = new Point(20, 20);
                    AddText(fs, this.rightFrameSize.X + "," + this.rightFrameSize.Y);
                    AddText(fs, "\n");

                    this.atRestFrameSize = new Point(20, 20);
                    AddText(fs, this.atRestFrameSize.X + "," + this.atRestFrameSize.Y);
                    AddText(fs, "\n");


                    this.leftSheetSize = new Point(2, 0);
                    AddText(fs, this.leftSheetSize.X + "," + this.leftSheetSize.Y);
                    AddText(fs, "\n");

                    this.rightSheetSize = new Point(2, 0);
                    AddText(fs, this.rightSheetSize.X + "," + this.rightSheetSize.Y);
                    AddText(fs, "\n");

                    this.atRestSheetSize = new Point(1, 0);
                    AddText(fs, this.atRestSheetSize.X + "," + this.atRestSheetSize.Y);
                    AddText(fs, "\n");


                    this.leftTextureFilename = "Images/spritesheets/manspritesheet";
                    AddText(fs, this.leftTextureFilename);
                    AddText(fs, "\n");

                    this.rightTextureFilename = "Images/spritesheets/manspritesheet";
                    AddText(fs, this.rightTextureFilename);
                    AddText(fs, "\n");

                    this.atRestTextureFilename = "Images/spritesheets/manspritesheet";
                    AddText(fs, this.atRestTextureFilename);
                    AddText(fs, "\n");

                    
                    this.TimeBetweenFrames = 100;
                    AddText(fs, this.TimeBetweenFrames + "");
                    AddText(fs, "\n");

                    // Write out the subclass's properties.
                    this.Write(fs);
                }
            }
            else
            {
                /*
                    UserControlledSprite
                    100,100
                    20,20
                    20,20
                    20,20
                    2,0
                    2,0
                    1,0
                    Images/spritesheets/manspritesheet
                    Images/spritesheets/manspritesheet
                    Images/spritesheets/manspritesheet
                    100
                */
                String configurationString = "";  // Holds the entire configuration file.

                // Open the stream and read it back. 
                using (FileStream fs = File.OpenRead(filepath))
                {
                    byte[] b = new byte[1024];
                    UTF8Encoding temp = new UTF8Encoding(true);
                    while (fs.Read(b, 0, b.Length) > 0)
                    {
                        configurationString += temp.GetString(b);
                    }
                }
                string[] configStringSplitRay = configurationString.Split('\n');

                this.InitialPosition = new Vector2(Convert.ToInt32(configStringSplitRay[1].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[1].Split(',')[1]));


                this.leftFrameSize = new Point(Convert.ToInt32(configStringSplitRay[2].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[2].Split(',')[1]));

                this.rightFrameSize = new Point(Convert.ToInt32(configStringSplitRay[3].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[3].Split(',')[1]));


                this.atRestFrameSize = new Point(Convert.ToInt32(configStringSplitRay[4].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[4].Split(',')[1]));

                this.leftSheetSize = new Point(Convert.ToInt32(configStringSplitRay[5].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[5].Split(',')[1]));

                this.rightSheetSize = new Point(Convert.ToInt32(configStringSplitRay[6].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[6].Split(',')[1]));

                this.atRestSheetSize = new Point(Convert.ToInt32(configStringSplitRay[7].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[7].Split(',')[1]));

                this.leftTextureFilename = configStringSplitRay[8];
                this.rightTextureFilename = configStringSplitRay[9];
                this.atRestTextureFilename = configStringSplitRay[10];

                this.TimeBetweenFrames = Convert.ToInt32(configStringSplitRay[11]);


                // This next call reads in the properties of the sub-class. (template method)
                this.Load(configStringSplitRay, 12);

            } // end else

            this.ElapsedGameTime = 0; // SKIPPTED!!!!!!!!!!!!!!!!!!!! NOT WRITTEN TO DISK
            this.CurrentTextureFilename = this.leftTextureFilename;
            this.CurrentPosition = this.InitialPosition;
        }

        // Useful in sub-classes.
        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        } // end method
    } // end class
} // end namespace
