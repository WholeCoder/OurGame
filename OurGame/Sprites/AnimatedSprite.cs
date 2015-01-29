using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.Commands;
using OurGame.GameStates;
using OurGame.OurGameLibrary;
using OurGame.Commands.ReverseTimeCommands;

namespace OurGame.Sprites
{
    public abstract class AnimatedSprite
    {
        public Vector2 CurrentPosition;
        public Vector2 InitialPosition;

        // This scales our characters and enemies up or down.
        private int _scaleUpThisSpriteFactor;

        private Point _leftFrameSize;
        private Point _rightFrameSize;
        private Point _atRestFrameSize;

        private Point _leftSheetSize;
        private Point _rightSheetSize;
        private Point _atRestSheetSize;

        private string _leftTextureFilename;
        private string _rightTextureFilename;
        private string _atRestTextureFilename;

        private const SpriteEffects RightCurrentEffect = SpriteEffects.None;
        private const SpriteEffects LeftCurrentEffect = SpriteEffects.FlipHorizontally;
        private const SpriteEffects AtRestCurrentEffect = SpriteEffects.None;

        private bool _isGoingLeft = false;
        private bool _isGoingRight = false;
        private bool _isAtRest = false;

        private Point _currentFrame;
        private String _currentTextureFilename; // The textures are received from the TextureCache.
        private Point _currentSheetSize;
        private Point _currentFrameSize;
        private SpriteEffects _currentSpriteEffect;

        private int _elapsedGameTime;  // Used to slow down the animaiton of this AnimatedSprite.
        private int _timeBetweenFrames;

        // ReSharper disable once InconsistentNaming
        protected const int GRAVITY_DOWNWARD = 5; // This makes sure there is always downward "pressure" to keep the sprit on the ground;

        public Rectangle BoundingRectangle; // For collision detection.

        private readonly Stack<ICommand> _ReversePositionAndScreenOffsetStackOfCommands;

        protected AnimatedSprite(string configFilePathAndName)
        {
            Debug.Assert(!configFilePathAndName.Equals("") && configFilePathAndName != null, "configFilePathAndName can't be null or blank!");

            this.Load(configFilePathAndName);

            this._currentFrame = new Point(0, 0);
            this._currentTextureFilename = this._atRestTextureFilename;
            this._currentSheetSize = this._atRestSheetSize;
            this._currentFrameSize = this._atRestFrameSize;
            this.CurrentPosition.X = this.InitialPosition.X;
            this.CurrentPosition.Y = this.InitialPosition.Y;
            this._currentSpriteEffect = AtRestCurrentEffect;
            this._isAtRest = true;
            this._isGoingLeft = false;
            this._isGoingRight = false;

            // _scaleUpThisSpriteFactor is the scale factor used in Draw.
            this.BoundingRectangle = new Rectangle((int)this.CurrentPosition.X, (int)this.CurrentPosition.Y,
                                                         this._currentFrameSize.X * _scaleUpThisSpriteFactor, this._currentFrameSize.Y * _scaleUpThisSpriteFactor);

            this._ReversePositionAndScreenOffsetStackOfCommands = new Stack<ICommand>();
        }

        public void IncrementScaleFactor()
        {
            this._scaleUpThisSpriteFactor++;
            this.BoundingRectangle.Width = this._scaleUpThisSpriteFactor * this._currentFrameSize.X;
            this.BoundingRectangle.Height = this._scaleUpThisSpriteFactor * this._currentFrameSize.Y;
        }

        public void DecrementScaleFactor()
        {
            this._scaleUpThisSpriteFactor--;
            if (this._scaleUpThisSpriteFactor <= 0)
            {
                this._scaleUpThisSpriteFactor = 1;
            }
            this.BoundingRectangle.Width = this._scaleUpThisSpriteFactor * this._currentFrameSize.X;
            this.BoundingRectangle.Height= this._scaleUpThisSpriteFactor * this._currentFrameSize.Y;
        }

        public void SavePositionToReverseTimeStack(PlayGameState pState)
        {
            ICommand reverseTimeCommand = new SetGameMetricsToPreviousValuesCommand(pState,pState.ScreenXOffset,this);
            this._ReversePositionAndScreenOffsetStackOfCommands.Push(reverseTimeCommand);
        }

        public void ReverseTimeForThisSprite()
        {
            if (this._ReversePositionAndScreenOffsetStackOfCommands.Count > 0)
            {
                ICommand reverseTimeCommand = this._ReversePositionAndScreenOffsetStackOfCommands.Pop();
                reverseTimeCommand.Execute();
            }
        }

        public void ApplyDownwardGravity()
        {
            this.CurrentPosition.Y += GRAVITY_DOWNWARD;
            this.BoundingRectangle.Y = (int)this.CurrentPosition.Y;
        }

        private void NextFrame(GameTime gameTime)
        {
            this._elapsedGameTime += gameTime.ElapsedGameTime.Milliseconds;

            if (this._elapsedGameTime >= this._timeBetweenFrames) // 100 milliseconds
            {
                this._elapsedGameTime = 0;

                ++this._currentFrame.X;

                if (this._currentFrame.X >= this._currentSheetSize.X)
                {
                    this._currentFrame.X = 0;
                    ++this._currentFrame.Y;
                    if (this._currentFrame.Y >= this._currentSheetSize.Y)
                        this._currentFrame.Y = 0;
                }
            } // end if
        }

        public void SwitchToGoRightTexture()
        {
            // Don't reset everything if we are already going in that direction.
            if (this._isGoingRight)
            {
                return;
            }
            else
            {
                this._isGoingRight = true;
                this._isGoingLeft = false;
                this._isAtRest = false;
            }
            
            this._currentFrame = new Point(0, 0);
            this._currentTextureFilename = this._rightTextureFilename;
            this._currentSheetSize = this._rightSheetSize;
            this._currentFrameSize = this._rightFrameSize;
            this._currentSpriteEffect = RightCurrentEffect;

            this._elapsedGameTime = 0;
        }

        public void SwitchToGoLeftTexture()
        {
            // Don't reset everything if we are already going in that direction.
            if (this._isGoingLeft)
            {
                return;
            }
            else
            {
                this._isGoingLeft = true;
                this._isGoingRight = false;
                this._isAtRest = false;
            }

            this._currentFrame = new Point(0, 0);
            this._currentTextureFilename = this._leftTextureFilename;
            this._currentSheetSize = this._leftSheetSize;
            this._currentFrameSize = this._leftFrameSize;
            this._currentSpriteEffect = LeftCurrentEffect;

            this._elapsedGameTime = 0; 
        }

        public void SwitchToAtRestTexture()
        {
            // Don't reset everything if we are already at rest.
            if (this._isAtRest)
            {
                return;
            }
            else
            {
                this._isAtRest = true;
                this._isGoingRight = false;
                this._isGoingLeft = false;
            }

            this._currentFrame = new Point(0, 0);
            this._currentTextureFilename = this._atRestTextureFilename;
            this._currentSheetSize = this._atRestSheetSize;
            this._currentFrameSize = this._atRestFrameSize;
            this._currentSpriteEffect = AtRestCurrentEffect;

            this._elapsedGameTime = 0; 
        }



        // A template method.
        public virtual void Update(GameTime gameTime)
        {
            this.NextFrame(gameTime);

            //******************************** DO NOT CHANGE THE ORDER OF THIS CODE!!!!!!!!!!!!!!!
                // MUST KEEP THIS ORDER SO USERCONTROLLED SPRITE CAN JUMP - THIS MUST BE AFTER THE this.UpdateAfterNextFrame(gameTime)!!!!!
                this.UpdateAfterNextFrame(gameTime);

                // Update the bounding rectangle of this sprite
                this.BoundingRectangle = new Rectangle((int)this.CurrentPosition.X, (int)this.CurrentPosition.Y,
                                                 this._currentFrameSize.X * _scaleUpThisSpriteFactor, this._currentFrameSize.Y * _scaleUpThisSpriteFactor);
            //******************************** DO NOT CHANGE THE ORDER OF THIS CODE!!!!!!!!!!!!!!!
        } // end method

        protected abstract void UpdateAfterNextFrame(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureCache.getInstance().GetTexture2DFromStringSpriteArray(_currentTextureFilename),
                              this.CurrentPosition,
                              new Rectangle(_currentFrame.X * _currentFrameSize.X + _currentFrame.X + 1,// CurrentFrame.X+1 is an offset for pixel boundaries in image
                              _currentFrame.Y * _currentFrameSize.Y,
                              _currentFrameSize.X,
                              _currentFrameSize.Y),
                              Color.White,
                              0,
                              Vector2.Zero,
                              _scaleUpThisSpriteFactor, // scale
                              this._currentSpriteEffect,
                              0);
        } // end Draw method

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 mouseCursorUpperLeftCorner)
        {

            spriteBatch.Draw(TextureCache.getInstance().GetTexture2DFromStringSpriteArray(_currentTextureFilename),
                               mouseCursorUpperLeftCorner,
                               new Rectangle(_currentFrame.X * _currentFrameSize.X + _currentFrame.X + 1,// CurrentFrame.X+1 is an offset for pixel boundaries in image
                               _currentFrame.Y * _currentFrameSize.Y,
                               _currentFrameSize.X,
                               _currentFrameSize.Y),
                               Color.White,
                               0,
                               Vector2.Zero,
                               _scaleUpThisSpriteFactor, // scale
                               this._currentSpriteEffect,
                               0);

        }

        // Used when drawing a sprite through the SpriteManager.
        public void DrawSubclassName(SpriteBatch spriteBatch, EditSpritesState pState)
        {
            string subClassName = this.NameOfThisSubclassForWritingToConfigFile();
            float posAboveSprite = this.CurrentPosition.Y - pState._helpFont.MeasureString(subClassName).Y;

            //Vector2 posAboveSprite = new Vector2(this.CurrentPosition.X, this.CurrentPosition.Y-Boa);
            spriteBatch.DrawString(pState._helpFont, this.NameOfThisSubclassForWritingToConfigFile(),
                new Vector2(this.CurrentPosition.X, posAboveSprite), Color.Black, 0, Vector2.Zero,
                1, SpriteEffects.None, 1);

        }

        // Called when must draw on mouse cursor position.
        public void DrawSubclassName(SpriteBatch spriteBatch, Vector2 mouseCursorUpperLeftCorner, EditSpritesState pState)
        {
            string subClassName = this.NameOfThisSubclassForWritingToConfigFile();
            float posAboveSprite = mouseCursorUpperLeftCorner.Y - pState._helpFont.MeasureString(subClassName).Y;

            spriteBatch.DrawString(pState._helpFont, this.NameOfThisSubclassForWritingToConfigFile(),
                new Vector2( mouseCursorUpperLeftCorner.X, posAboveSprite), Color.Black, 0, Vector2.Zero,
                1, SpriteEffects.None, 1);

        }

        public int GetSpriteScaleFactor()
        {
            return this._scaleUpThisSpriteFactor;
        }

        public void SetSpriteScaleFactor(int sf)
        {
            this._scaleUpThisSpriteFactor = sf;
            this.BoundingRectangle.Width = this._scaleUpThisSpriteFactor * this._currentFrameSize.X;
            this.BoundingRectangle.Height = this._scaleUpThisSpriteFactor * this._currentFrameSize.Y;
        }

        // This will start at the startOffset and read out it's attributes.
        protected abstract void Load(string[] configArray, int startOffset);
        public abstract string NameOfThisSubclassForWritingToConfigFile();

        // In this method we use fs to write out the subclasses properties.
        protected abstract void Write(FileStream fs);


        // This is the template method pattern.
        public void Load(string filepath)
        {
            Debug.Assert(!filepath.Equals("") && filepath != null, "filepath can't be null or blank!");

            if (!File.Exists(filepath))
            {
                // Set defaults

                // Write "AutomatedSprite" to file and a \n.
                Debug.Assert(!this.NameOfThisSubclassForWritingToConfigFile().Equals(""), "AnimatedSprite.NameOfThisSubclassForWritingToConfigFile() must return the name of the subclass that is being loaded!");

                this.InitialPosition = new Vector2(200, 200);


                this._leftFrameSize = new Point(20, 20);
                this._rightFrameSize = new Point(20, 20);
                this._atRestFrameSize = new Point(20, 20);


                this._leftSheetSize = new Point(2, 0);

                this._rightSheetSize = new Point(2, 0);

                this._atRestSheetSize = new Point(1, 0);


                this._leftTextureFilename = "Images/spritesheets/manspritesheet";

                this._rightTextureFilename = "Images/spritesheets/manspritesheet";

                this._atRestTextureFilename = "Images/spritesheets/manspritesheet";


                this._timeBetweenFrames = 100;

                this._scaleUpThisSpriteFactor = 2;

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
                    2
                */

                string[] configStringSplitRay = File.ReadAllLines(filepath);

                this.InitialPosition = new Vector2(Convert.ToInt32(configStringSplitRay[1].Split(',')[0]),
                                                   Convert.ToInt32(configStringSplitRay[1].Split(',')[1]));

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

                this._timeBetweenFrames = Convert.ToInt32(configStringSplitRay[11]);

                this._scaleUpThisSpriteFactor= Convert.ToInt32(configStringSplitRay[12]);


                // This next call reads in the properties of the sub-class. (template method)
                this.Load(configStringSplitRay, 13);

            } // end else

            this._elapsedGameTime = 0; 
            this._currentTextureFilename = this._atRestTextureFilename;
            this.CurrentPosition = this.InitialPosition;
        }

        public void WritePropertiesToFile(string filepath)
        {
            using (FileStream fs = File.Create(filepath))
            {
                AddText(fs, this.NameOfThisSubclassForWritingToConfigFile()); // ex) "UserControlledSprite"
                AddText(fs, "\n");

                AddText(fs, this.InitialPosition.X + "," + this.InitialPosition.Y);
                AddText(fs, "\n");

                AddText(fs, this._leftFrameSize.X + "," + this._leftFrameSize.Y);
                AddText(fs, "\n");

                AddText(fs, this._rightFrameSize.X + "," + this._rightFrameSize.Y);
                AddText(fs, "\n");

                AddText(fs, this._atRestFrameSize.X + "," + this._atRestFrameSize.Y);
                AddText(fs, "\n");

                AddText(fs, this._leftSheetSize.X + "," + this._leftSheetSize.Y);
                AddText(fs, "\n");

                AddText(fs, this._rightSheetSize.X + "," + this._rightSheetSize.Y);
                AddText(fs, "\n");

                AddText(fs, this._atRestSheetSize.X + "," + this._atRestSheetSize.Y);
                AddText(fs, "\n");

                AddText(fs, this._leftTextureFilename);
                AddText(fs, "\n");

                AddText(fs, this._rightTextureFilename);
                AddText(fs, "\n");

                AddText(fs, this._atRestTextureFilename);
                AddText(fs, "\n");

                AddText(fs, this._timeBetweenFrames + "");
                AddText(fs, "\n");

                AddText(fs, this._scaleUpThisSpriteFactor + "");
                AddText(fs, "\n");

                // Write out the subclass's properties.
                this.Write(fs);
            }
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
