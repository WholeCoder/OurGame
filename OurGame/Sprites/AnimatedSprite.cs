using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.OurGameLibrary;

namespace OurGame.Sprites
{
    public abstract class AnimatedSprite
    {
        public Vector2 CurrentPosition;
        public Vector2 _InitialPosition { get; set; }

        private int _ScaleUpThisSpriteFactor = 10;

        private Point _LeftFrameSize;
        private Point _RightFrameSize;
        private Point _AtRestFrameSize;

        private Point _LeftSheetSize;
        private Point _RightSheetSize;
        private Point _AtRestSheetSize;

        private string _LeftTextureFilename;
        private string _RightTextureFilename;
        private string _AtRestTextureFilename;

        private SpriteEffects _RightCurrentEffect = SpriteEffects.None;
        private SpriteEffects _LeftCurrentEffect = SpriteEffects.FlipHorizontally;
        private SpriteEffects _AtRestCurrentEffect = SpriteEffects.None;

        private bool _IsGoingLeft = false;
        private bool _IsGoingRight = false;
        private bool _IsAtRest = false;

        private Point _CurrentFrame;
        private String _CurrentTextureFilename; // The textures are received from the TextureCache.
        private Point _CurrentSheetSize;
        private Point _CurrentFrameSize;
        private SpriteEffects _CurrentSpriteEffect;

        private int _ElapsedGameTime;  // Used to slow down the animaiton of this AnimatedSprite.
        private int _TimeBetweenFrames;

        public Rectangle BoundingRectangle; // For collision detection.

        public AnimatedSprite(string configFilePathAndName)
        {
            Debug.Assert(!configFilePathAndName.Equals("") && configFilePathAndName != null, "configFilePathAndName can't be null or blank!");

            this.Load(configFilePathAndName);
            //this.SwitchToAtRestTexture();

            this._CurrentFrame = new Point(0, 0);
            this._CurrentTextureFilename = this._AtRestTextureFilename;
            this._CurrentSheetSize = this._AtRestSheetSize;
            this._CurrentFrameSize = this._AtRestFrameSize;
            this.CurrentPosition.X = this._InitialPosition.X;
            this.CurrentPosition.Y = this._InitialPosition.Y;
            this._CurrentSpriteEffect = this._AtRestCurrentEffect;
            this._IsAtRest = true;
            this._IsGoingLeft = false;
            this._IsGoingRight = false;

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

        public void SwitchToGoRightTexture()
        {
            // Don't reset everything if we are already going in that direction.
            if (this._IsGoingRight)
            {
                return;
            }
            else
            {
                this._IsGoingRight = true;
                this._IsGoingLeft = false;
                this._IsAtRest = false;
            }
            
            this._CurrentFrame = new Point(0, 0);
            this._CurrentTextureFilename = this._RightTextureFilename;
            this._CurrentSheetSize = this._RightSheetSize;
            this._CurrentFrameSize = this._RightFrameSize;
            this._CurrentSpriteEffect = this._RightCurrentEffect;

            this._ElapsedGameTime = 0;
        }

        public void SwitchToGoLeftTexture()
        {
            // Don't reset everything if we are already going in that direction.
            if (this._IsGoingLeft)
            {
                return;
            }
            else
            {
                this._IsGoingLeft = true;
                this._IsGoingRight = false;
                this._IsAtRest = false;
            }

            this._CurrentFrame = new Point(0, 0);
            this._CurrentTextureFilename = this._LeftTextureFilename;
            this._CurrentSheetSize = this._LeftSheetSize;
            this._CurrentFrameSize = this._LeftFrameSize;
            this._CurrentSpriteEffect = this._LeftCurrentEffect;

            this._ElapsedGameTime = 0; 
        }

        public void SwitchToAtRestTexture()
        {
            // Don't reset everything if we are already at rest.
            if (this._IsAtRest)
            {
                return;
            }
            else
            {
                this._IsAtRest = true;
                this._IsGoingRight = false;
                this._IsGoingLeft = false;
            }

            this._CurrentFrame = new Point(0, 0);
            this._CurrentTextureFilename = this._AtRestTextureFilename;
            this._CurrentSheetSize = this._AtRestSheetSize;
            this._CurrentFrameSize = this._AtRestFrameSize;
            this._CurrentSpriteEffect = this._AtRestCurrentEffect;

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

        protected abstract void UpdateAfterNextFrame(GameTime gameTime);


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
                              this._CurrentSpriteEffect,
                              0);
        } // end Draw method

        // This will start at the startOffset and read out it's attributes.
        protected abstract void Load(string[] configArray, int startOffset);
        protected abstract string NameOfThisSubclassForWritingToConfigFile();

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


                    this._LeftFrameSize = new Point(20, 20);
                    AddText(fs, this._LeftFrameSize.X + "," + this._LeftFrameSize.Y);
                    AddText(fs, "\n");

                    this._RightFrameSize = new Point(20, 20);
                    AddText(fs, this._RightFrameSize.X + "," + this._RightFrameSize.Y);
                    AddText(fs, "\n");

                    this._AtRestFrameSize = new Point(20, 20);
                    AddText(fs, this._AtRestFrameSize.X + "," + this._AtRestFrameSize.Y);
                    AddText(fs, "\n");


                    this._LeftSheetSize = new Point(2, 0);
                    AddText(fs, this._LeftSheetSize.X + "," + this._LeftSheetSize.Y);
                    AddText(fs, "\n");

                    this._RightSheetSize = new Point(2, 0);
                    AddText(fs, this._RightSheetSize.X + "," + this._RightSheetSize.Y);
                    AddText(fs, "\n");

                    this._AtRestSheetSize = new Point(1, 0);
                    AddText(fs, this._AtRestSheetSize.X + "," + this._AtRestSheetSize.Y);
                    AddText(fs, "\n");


                    this._LeftTextureFilename = "Images/spritesheets/manspritesheet";
                    AddText(fs, this._LeftTextureFilename);
                    AddText(fs, "\n");

                    this._RightTextureFilename = "Images/spritesheets/manspritesheet";
                    AddText(fs, this._RightTextureFilename);
                    AddText(fs, "\n");

                    this._AtRestTextureFilename = "Images/spritesheets/manspritesheet";
                    AddText(fs, this._AtRestTextureFilename);
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

                this._LeftFrameSize = new Point(Convert.ToInt32(configStringSplitRay[2].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[2].Split(',')[1]));

                this._RightFrameSize = new Point(Convert.ToInt32(configStringSplitRay[3].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[3].Split(',')[1]));


                this._AtRestFrameSize = new Point(Convert.ToInt32(configStringSplitRay[4].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[4].Split(',')[1]));

                this._LeftSheetSize = new Point(Convert.ToInt32(configStringSplitRay[5].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[5].Split(',')[1]));

                this._RightSheetSize = new Point(Convert.ToInt32(configStringSplitRay[6].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[6].Split(',')[1]));

                this._AtRestSheetSize = new Point(Convert.ToInt32(configStringSplitRay[7].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[7].Split(',')[1]));

                this._LeftTextureFilename = configStringSplitRay[8];
                this._RightTextureFilename = configStringSplitRay[9];
                this._AtRestTextureFilename = configStringSplitRay[10];

                this._TimeBetweenFrames = Convert.ToInt32(configStringSplitRay[11]);


                // This next call reads in the properties of the sub-class. (template method)
                this.Load(configStringSplitRay, 12);

            } // end else

            this._ElapsedGameTime = 0; 
            this._CurrentTextureFilename = this._AtRestTextureFilename;
            this.CurrentPosition = this._InitialPosition;
        }

        // Useful in sub-classes.
        public static void AddText(FileStream fs, string value)
        {
            Debug.Assert(fs != null, "FileStream fs can't be null!");
            Debug.Assert(value != null, "value can't be null!");
            Debug.Assert(fs.CanWrite, "FileStream fs must be open for writing!");

            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        } // end method
    } // end class
} // end namespace
