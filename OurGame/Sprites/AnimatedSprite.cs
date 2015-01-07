using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.WindowsGameLibrary1;

namespace OurGame.Sprites
{
    public abstract class AnimatedSprite
    {
        public Vector2 CurrentPosition;
        private Vector2 _InitialPosition { get; set; }

        private int _ScaleUpThisSpriteFactor = 10;

        private Point _leftFrameSize;
        private Point _rightFrameSize;
        private Point _atRestFrameSize;

        private Point _leftSheetSize;
        private Point _rightSheetSize;
        private Point _atRestSheetSize;

        private string _leftTextureFilename;
        private string _rightTextureFilename;
        private string _atRestTextureFilename;


        private Point _CurrentFrame;
        private String _CurrentTextureFilename; // The textures are received from the TextureCache.
        private Point _CurrentSheetSize;
        private Point _CurrentFrameSize;

        private int _ElapsedGameTime;  // Used to slow down the animaiton of this AnimatedSprite.
        private int _TimeBetweenFrames;

        public Rectangle BoundingRectangle; // For collision detection.

        public AnimatedSprite(string configFilePathAndName)
        {
            Debug.Assert(!configFilePathAndName.Equals("") && configFilePathAndName != null, "configFilePathAndName can't be null or blank!");

            this.Load(configFilePathAndName);
            //this.SwitchToAtRestTexture();

            this._CurrentFrame = new Point(0, 0);
            this._CurrentTextureFilename = this._atRestTextureFilename;
            this._CurrentSheetSize = this._atRestSheetSize;
            this._CurrentFrameSize = this._atRestFrameSize;
            this.CurrentPosition.X = this._InitialPosition.X;
            this.CurrentPosition.Y = this._InitialPosition.Y;

            // _scaleUpThisSpriteFactor is the scall factor used in Draw.  Change this to be an instance member!
            this.BoundingRectangle = new Rectangle((int)this.CurrentPosition.X, (int)this.CurrentPosition.Y,
                                                         this._CurrentFrameSize.X * this._ScaleUpThisSpriteFactor, this._CurrentFrameSize.Y * this._ScaleUpThisSpriteFactor);
        }

        private void NextFrame(GameTime gameTime)
        {
            this._ElapsedGameTime += gameTime.ElapsedGameTime.Milliseconds;

            if (this._ElapsedGameTime >= this._TimeBetweenFrames) // 100 milliseconds
            {
                this._ElapsedGameTime = 0;

                ++this._CurrentFrame.X;

                if (this._CurrentFrame.X >= this._CurrentSheetSize.X)
                {
                    this._CurrentFrame.X = 0;
                    ++this._CurrentFrame.Y;
                    if (this._CurrentFrame.Y >= this._CurrentSheetSize.Y)
                        this._CurrentFrame.Y = 0;
                }
            } // end if
        }

        // this is used in the SwitchToGo...Texture() method to see if we need to switch our texture and attributes
        //    For example, switching to going right texture or at rest.
        public bool TestIfSameFrameMetrics(string textureFilename, Point sheetSize, Point frameSize)
        {
            Debug.Assert(!textureFilename.Equals("") && textureFilename != null, "textureFilename can't be null or blank!");
            Debug.Assert(sheetSize != null, "sheetSize can't be null!");
            Debug.Assert(frameSize != null, "frameSize can't be null!");

            return this._CurrentTextureFilename == textureFilename &&
                                        this._CurrentSheetSize == sheetSize &&
                                        this._CurrentFrameSize == frameSize;
        }

        public void SwitchToGoRightTexture()
        {
            bool allreadyUsingTexture = this.TestIfSameFrameMetrics(this._rightTextureFilename, this._rightSheetSize, this._rightFrameSize);
            
            if (allreadyUsingTexture)
            {
                return;
            }

            this._CurrentFrame = new Point(0, 0);
            this._CurrentTextureFilename = this._rightTextureFilename;
            this._CurrentSheetSize = this._rightSheetSize;
            this._CurrentFrameSize = this._rightFrameSize;

            this._ElapsedGameTime = 0;
        }

        public void SwitchToGoLeftTexture()
        {
            bool allreadyUsingTexture = this.TestIfSameFrameMetrics(this._leftTextureFilename, this._leftSheetSize, this._leftFrameSize);
            
            if (allreadyUsingTexture)
            {
                return;
            }

            this._CurrentFrame = new Point(0, 0);
            this._CurrentTextureFilename = this._leftTextureFilename;
            this._CurrentSheetSize = this._leftSheetSize;
            this._CurrentFrameSize = this._leftFrameSize;

            this._ElapsedGameTime = 0; 
        }

        public void SwitchToAtRestTexture()
        {
            bool allreadyUsingTexture = this.TestIfSameFrameMetrics(this._atRestTextureFilename, this._atRestSheetSize, this._atRestFrameSize);
            
            if (allreadyUsingTexture)
            {
                return;
            }

            this._CurrentFrame = new Point(0, 0);
            this._CurrentTextureFilename = this._atRestTextureFilename;
            this._CurrentSheetSize = this._atRestSheetSize;
            this._CurrentFrameSize = this._atRestFrameSize;

            this._ElapsedGameTime = 0; 
        }



        // A template method.
        public virtual void Update(GameTime gameTime)
        {
            this.NextFrame(gameTime);

            this.UpdateAfterNextFrame(gameTime);

            // Update the bounding rectangle of this sprite
            this.BoundingRectangle = new Rectangle((int)this.CurrentPosition.X, (int)this.CurrentPosition.Y,
                                             this._CurrentFrameSize.X * this._ScaleUpThisSpriteFactor, this._CurrentFrameSize.Y * this._ScaleUpThisSpriteFactor);

        } // end method

        public abstract void UpdateAfterNextFrame(GameTime gameTime);


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureCache.getInstance().GetTexture2DFromStringSpriteArray(_CurrentTextureFilename),
                              this.CurrentPosition,
                              new Rectangle(_CurrentFrame.X * _CurrentFrameSize.X + _CurrentFrame.X + 1,// CurrentFrame.X+1 is an offset for pixel boundaries in image
                              _CurrentFrame.Y * _CurrentFrameSize.Y,
                              _CurrentFrameSize.X,
                              _CurrentFrameSize.Y),
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
            Debug.Assert(!filepath.Equals("") && filepath != null, "filepath can't be null or blank!");

            if (!File.Exists(filepath))
            {
                using (FileStream fs = File.Create(filepath))
                {
                    // Set defaults

                    // Write "AutomatedSprite" to file and a \n.
                    Debug.Assert(!this.NameOfThisSubclassForWritingToConfigFile().Equals(""), "AnimatedSprite.NameOfThisSubclassForWritingToConfigFile() must return the name of the subclass that is being loaded!");
                    AddText(fs, this.NameOfThisSubclassForWritingToConfigFile()); // ex) "UserControlledSprite"
                    AddText(fs, "\n");

                    this._InitialPosition = new Vector2(200, 200);
                    AddText(fs, this._InitialPosition.X + "," + this._InitialPosition.Y);
                    AddText(fs, "\n");


                    this._leftFrameSize = new Point(20, 20);
                    AddText(fs, this._leftFrameSize.X + "," + this._leftFrameSize.Y);
                    AddText(fs, "\n");

                    this._rightFrameSize = new Point(20, 20);
                    AddText(fs, this._rightFrameSize.X + "," + this._rightFrameSize.Y);
                    AddText(fs, "\n");

                    this._atRestFrameSize = new Point(20, 20);
                    AddText(fs, this._atRestFrameSize.X + "," + this._atRestFrameSize.Y);
                    AddText(fs, "\n");


                    this._leftSheetSize = new Point(2, 0);
                    AddText(fs, this._leftSheetSize.X + "," + this._leftSheetSize.Y);
                    AddText(fs, "\n");

                    this._rightSheetSize = new Point(2, 0);
                    AddText(fs, this._rightSheetSize.X + "," + this._rightSheetSize.Y);
                    AddText(fs, "\n");

                    this._atRestSheetSize = new Point(1, 0);
                    AddText(fs, this._atRestSheetSize.X + "," + this._atRestSheetSize.Y);
                    AddText(fs, "\n");


                    this._leftTextureFilename = "Images/spritesheets/manspritesheet";
                    AddText(fs, this._leftTextureFilename);
                    AddText(fs, "\n");

                    this._rightTextureFilename = "Images/spritesheets/manspritesheet";
                    AddText(fs, this._rightTextureFilename);
                    AddText(fs, "\n");

                    this._atRestTextureFilename = "Images/spritesheets/manspritesheet";
                    AddText(fs, this._atRestTextureFilename);
                    AddText(fs, "\n");

                    
                    this._TimeBetweenFrames = 100;
                    AddText(fs, this._TimeBetweenFrames + "");
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

                string[] configStringSplitRay = File.ReadAllLines(filepath);

                this._InitialPosition = new Vector2(Convert.ToInt32(configStringSplitRay[1].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[1].Split(',')[1]));
                Console.Write("----------- InitialPosition == " + this._InitialPosition.X + "," + this._InitialPosition.Y);

                this._leftFrameSize = new Point(Convert.ToInt32(configStringSplitRay[2].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[2].Split(',')[1]));

                this._rightFrameSize = new Point(Convert.ToInt32(configStringSplitRay[3].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[3].Split(',')[1]));


                this._atRestFrameSize = new Point(Convert.ToInt32(configStringSplitRay[4].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[4].Split(',')[1]));

                this._leftSheetSize = new Point(Convert.ToInt32(configStringSplitRay[5].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[5].Split(',')[1]));

                this._rightSheetSize = new Point(Convert.ToInt32(configStringSplitRay[6].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[6].Split(',')[1]));

                this._atRestSheetSize = new Point(Convert.ToInt32(configStringSplitRay[7].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[7].Split(',')[1]));

                this._leftTextureFilename = configStringSplitRay[8];
                this._rightTextureFilename = configStringSplitRay[9];
                this._atRestTextureFilename = configStringSplitRay[10];

                this._TimeBetweenFrames = Convert.ToInt32(configStringSplitRay[11]);


                // This next call reads in the properties of the sub-class. (template method)
                this.Load(configStringSplitRay, 12);

            } // end else

            this._ElapsedGameTime = 0; 
            this._CurrentTextureFilename = this._atRestTextureFilename;
            this.CurrentPosition = this._InitialPosition;
        }

        // Useful in sub-classes.
        private static void AddText(FileStream fs, string value)
        {
            Debug.Assert(fs != null, "FileStream fs can't be null!");
            Debug.Assert(value != null, "value can't be null!");
            Debug.Assert(fs.CanWrite, "FileStream fs must be open for writing!");

            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        } // end method
    } // end class
} // end namespace
