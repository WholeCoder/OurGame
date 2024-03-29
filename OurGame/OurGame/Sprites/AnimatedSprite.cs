﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OurGame.Commands;
using OurGame.Commands.ReverseTimeCommands;
using OurGame.GameStates;
using OurGame.OurGameLibrary;

namespace OurGame.Sprites
{
    public abstract class AnimatedSprite
    {
        private const SpriteEffects RightCurrentEffect = SpriteEffects.None;
        private const SpriteEffects LeftCurrentEffect = SpriteEffects.FlipHorizontally;
        private const SpriteEffects AtRestCurrentEffect = SpriteEffects.None;
        private Point _atRestFrameSize;
        private Point _atRestSheetSize;
        private string _atRestTextureFilename;
        protected Point CurrentFrame;
        protected Point CurrentFrameSize;
        private Point _currentSheetSize;
        protected SpriteEffects CurrentSpriteEffect;
        protected String CurrentTextureFilename; // The textures are received from the TextureCache.
        private int _elapsedGameTime; // Used to slow down the animaiton of this AnimatedSprite.
        private bool _isAtRest;
        private bool _isGoingLeft;
        private bool _isGoingRight;
        private Point _leftFrameSize;
        private Point _leftSheetSize;
        private string _leftTextureFilename;
        private Point _rightFrameSize;
        private Point _rightSheetSize;
        private string _rightTextureFilename;
        // This scales our characters and enemies up or down.
        protected int ScaleUpThisSpriteFactor;
        private int _timeBetweenFrames;
        public Vector2 CurrentPosition;
        public Vector2 InitialPosition;
        // ReSharper disable once InconsistentNaming
        public const int GRAVITY_DOWNWARD = 5;
        // This makes sure there is always downward "pressure" to keep the sprit on the ground;

        public Rectangle BoundingRectangle; // For collision detection.

        private readonly Stack<ICommand> _reversePositionAndScreenOffsetStackOfCommands;

        private int _lastY;
        private int _lastX;

        private int _lastScreenXOffset;

        public bool IsJumping;

        public override string ToString()
        {
            return "AnimatedSprite";
        }

        protected AnimatedSprite(string configFilePathAndName)
        {
            Debug.Assert(!configFilePathAndName.Equals("") && configFilePathAndName != null,
                "configFilePathAndName can't be null or blank!");

            Load(configFilePathAndName);

            CurrentFrame = new Point(0, 0);
            CurrentTextureFilename = _atRestTextureFilename;
            _currentSheetSize = _atRestSheetSize;
            CurrentFrameSize = _atRestFrameSize;
            CurrentPosition.X = InitialPosition.X;
            CurrentPosition.Y = InitialPosition.Y;
            CurrentSpriteEffect = AtRestCurrentEffect;
            _isAtRest = true;
            _isGoingLeft = false;
            _isGoingRight = false;

            // _scaleUpThisSpriteFactor is the scale factor used in Draw.
            BoundingRectangle = new Rectangle((int) CurrentPosition.X, (int) CurrentPosition.Y,
                CurrentFrameSize.X*ScaleUpThisSpriteFactor, CurrentFrameSize.Y*ScaleUpThisSpriteFactor);

            _reversePositionAndScreenOffsetStackOfCommands = new Stack<ICommand>();

            _lastX = -1;
            _lastY = -1;
            IsJumping = false;
            _lastScreenXOffset = 0;
        }

        public void SetLastXAndY(int x, int y, int screenXOffset)
        {
            _lastX = x;
            _lastY = y;
            _lastScreenXOffset = screenXOffset;
        }

        public int GetLastX()
        {
            return _lastX;
        }

        public int GetLastY()
        {
            return _lastY;
        }

        public int GetLastScreenXOffset()
        {
            return _lastScreenXOffset;
        }

        public void IncrementScaleFactor()
        {
            ScaleUpThisSpriteFactor++;
            BoundingRectangle.Width = ScaleUpThisSpriteFactor*CurrentFrameSize.X;
            BoundingRectangle.Height = ScaleUpThisSpriteFactor*CurrentFrameSize.Y;
        }

        public void DecrementScaleFactor()
        {
            ScaleUpThisSpriteFactor--;
            if (ScaleUpThisSpriteFactor <= 0)
            {
                ScaleUpThisSpriteFactor = 1;
            }
            BoundingRectangle.Width = ScaleUpThisSpriteFactor*CurrentFrameSize.X;
            BoundingRectangle.Height = ScaleUpThisSpriteFactor*CurrentFrameSize.Y;
        }

        public void SavePositionToReverseTimeStack(PlayGameState pState)
        {
            ICommand reverseTimeCommand = new SetGameMetricsToPreviousValuesCommand(pState, pState.ScreenXOffset, this);
            _reversePositionAndScreenOffsetStackOfCommands.Push(reverseTimeCommand);
        }

        public void ReverseTimeForThisSprite()
        {
            if (_reversePositionAndScreenOffsetStackOfCommands.Count > 0)
            {
                var reverseTimeCommand = _reversePositionAndScreenOffsetStackOfCommands.Pop();
                reverseTimeCommand.Execute();
            }
        }

        public virtual void ApplyDownwardGravity(Board theBoard, State state)
        {
/*
            Rectangle possiblePosition = BoundingRectangle;
            possiblePosition.Y += GRAVITY_DOWNWARD;

            var tilesBelowSprite = new List<Tile>();
            for (int i = 0; i < theBoard.TheBoard.GetLength(0); i++)
            {
                for (int j = 0; j < theBoard.TheBoard.GetLength(1); j++)
                {
                    if (possiblePosition.Intersects(theBoard.TheBoard[i, j].BoundingRectangle))
                    {
                        if (theBoard.TheBoard[i, j].BoundingRectangle.Y > possiblePosition.Y)
                        {
                            tilesBelowSprite.Add(theBoard.TheBoard[i, j]);
                            Console.Write("Found -"+(new Random()).Next());
                            break;
                        }
                    }
                }
            }

            if (!tilesBelowSprite.Any())
            {
                CurrentPosition.Y += GRAVITY_DOWNWARD;
                BoundingRectangle.Y = (int) CurrentPosition.Y;

            }
*/
            CurrentPosition.Y += GRAVITY_DOWNWARD;
            BoundingRectangle.Y = (int)CurrentPosition.Y;

        }

        private void NextFrame(GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");

            _elapsedGameTime += gameTime.ElapsedGameTime.Milliseconds;

            if (_elapsedGameTime >= _timeBetweenFrames) // 100 milliseconds
            {
                _elapsedGameTime = 0;

                ++CurrentFrame.X;

                if (CurrentFrame.X >= _currentSheetSize.X)
                {
                    CurrentFrame.X = 0;
                    ++CurrentFrame.Y;
                    if (CurrentFrame.Y >= _currentSheetSize.Y)
                        CurrentFrame.Y = 0;
                }
            } // end if
        }

        public void SwitchToGoRightTexture()
        {
            // Don't reset everything if we are already going in that direction.
            if (_isGoingRight)
            {
                return;
            }
            _isGoingRight = true;
            _isGoingLeft = false;
            _isAtRest = false;

            CurrentFrame = new Point(0, 0);
            CurrentTextureFilename = _rightTextureFilename;
            _currentSheetSize = _rightSheetSize;
            CurrentFrameSize = _rightFrameSize;
            CurrentSpriteEffect = RightCurrentEffect;

            _elapsedGameTime = 0;
        }

        public void SwitchToGoLeftTexture()
        {
            // Don't reset everything if we are already going in that direction.
            if (_isGoingLeft)
            {
                return;
            }
            _isGoingLeft = true;
            _isGoingRight = false;
            _isAtRest = false;

            CurrentFrame = new Point(0, 0);
            CurrentTextureFilename = _leftTextureFilename;
            _currentSheetSize = _leftSheetSize;
            CurrentFrameSize = _leftFrameSize;
            CurrentSpriteEffect = LeftCurrentEffect;

            _elapsedGameTime = 0;
        }

        public void SwitchToAtRestTexture()
        {
            // Don't reset everything if we are already at rest.
            if (_isAtRest)
            {
                return;
            }
            _isAtRest = true;
            _isGoingRight = false;
            _isGoingLeft = false;

            CurrentFrame = new Point(0, 0);
            CurrentTextureFilename = _atRestTextureFilename;
            _currentSheetSize = _atRestSheetSize;
            CurrentFrameSize = _atRestFrameSize;
            CurrentSpriteEffect = AtRestCurrentEffect;

            _elapsedGameTime = 0;
        }


        // A template method.
        public virtual void Update(GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");

            NextFrame(gameTime);

            //******************************** DO NOT CHANGE THE ORDER OF THIS CODE!!!!!!!!!!!!!!!
            // MUST KEEP THIS ORDER SO USERCONTROLLED SPRITE CAN JUMP - THIS MUST BE AFTER THE this.UpdateAfterNextFrame(gameTime)!!!!!
            UpdateAfterNextFrame(gameTime);

            // Update the bounding rectangle of this sprite
            BoundingRectangle = new Rectangle((int) CurrentPosition.X, (int) CurrentPosition.Y,
                CurrentFrameSize.X*ScaleUpThisSpriteFactor, CurrentFrameSize.Y*ScaleUpThisSpriteFactor);
            //******************************** DO NOT CHANGE THE ORDER OF THIS CODE!!!!!!!!!!!!!!!
        } // end method

        protected abstract void UpdateAfterNextFrame(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 mouseCursorUpperLeftCorner)
        {
            Debug.Assert(spriteBatch != null, "spriteBatch can not be null!");
            Debug.Assert(mouseCursorUpperLeftCorner != null, "mouseCursorUpperLeftCorner can not be null!");

            spriteBatch.Draw(TextureCache.getInstance().GetTexture2DFromStringSpriteArray(CurrentTextureFilename),
                mouseCursorUpperLeftCorner,
                new Rectangle(CurrentFrame.X*CurrentFrameSize.X + CurrentFrame.X + 1,
                    // CurrentFrame.X+1 is an offset for pixel boundaries in image
                    CurrentFrame.Y*CurrentFrameSize.Y,
                    CurrentFrameSize.X,
                    CurrentFrameSize.Y),
                Color.White,
                0,
                Vector2.Zero,
                ScaleUpThisSpriteFactor, // scale
                CurrentSpriteEffect,
                0);
        }

        // Used when drawing a sprite through the SpriteManager.
        public virtual void DrawSubclassName(SpriteBatch spriteBatch, EditSpritesState pState)
        {
            Debug.Assert(spriteBatch != null, "spriteBatch can not be null!");
            Debug.Assert(pState != null, "pState can not be null!");

            var subClassName = NameOfThisSubclassForWritingToConfigFile;
            var posAboveSprite = CurrentPosition.Y - pState.HelpFont.MeasureString(subClassName).Y;

            //Vector2 posAboveSprite = new Vector2(this.CurrentPosition.X, this.CurrentPosition.Y-Boa);
            spriteBatch.DrawString(pState.HelpFont, NameOfThisSubclassForWritingToConfigFile,
                new Vector2(CurrentPosition.X+pState.ScreenXOffset, posAboveSprite), Color.Black, 0, Vector2.Zero,
                1, SpriteEffects.None, 1);
        }

        // Called when must draw on mouse cursor position.
        public void DrawSubclassName(SpriteBatch spriteBatch, Vector2 mouseCursorUpperLeftCorner,
            EditSpritesState pState)
        {
            Debug.Assert(spriteBatch != null, "spriteBatch can not be null!");
            Debug.Assert(mouseCursorUpperLeftCorner != null, "mouseCursorUpperLeftCorner can not be null!");
            Debug.Assert(pState != null, "pState can not be null!");

            var subClassName = NameOfThisSubclassForWritingToConfigFile;
            var posAboveSprite = mouseCursorUpperLeftCorner.Y - pState.HelpFont.MeasureString(subClassName).Y;

            spriteBatch.DrawString(pState.HelpFont, NameOfThisSubclassForWritingToConfigFile,
                new Vector2(mouseCursorUpperLeftCorner.X, posAboveSprite), Color.Black, 0, Vector2.Zero,
                1, SpriteEffects.None, 1);
        }

        public int GetSpriteScaleFactor()
        {
            return ScaleUpThisSpriteFactor;
        }

        public void SetSpriteScaleFactor(int sf)
        {
            if (sf <= 0)
            {
                sf = 1;
            }

            ScaleUpThisSpriteFactor = sf;
            BoundingRectangle.Width = ScaleUpThisSpriteFactor*CurrentFrameSize.X;
            BoundingRectangle.Height = ScaleUpThisSpriteFactor*CurrentFrameSize.Y;
        }

        // This will start at the startOffset and read out it's attributes.
        protected abstract void Load(string[] configArray, int startOffset);
        public abstract string NameOfThisSubclassForWritingToConfigFile { get; }


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
                Debug.Assert(!NameOfThisSubclassForWritingToConfigFile.Equals(""),
                    "AnimatedSprite.NameOfThisSubclassForWritingToConfigFile() must return the name of the subclass that is being loaded!");

                InitialPosition = new Vector2(200, 200);


                _leftFrameSize = new Point(20, 20);
                _rightFrameSize = new Point(20, 20);
                _atRestFrameSize = new Point(20, 20);


                _leftSheetSize = new Point(2, 0);

                _rightSheetSize = new Point(2, 0);

                _atRestSheetSize = new Point(1, 0);


                _leftTextureFilename = "Images/spritesheets/manspritesheet";

                _rightTextureFilename = "Images/spritesheets/manspritesheet";

                _atRestTextureFilename = "Images/spritesheets/manspritesheet";


                _timeBetweenFrames = 100;

                ScaleUpThisSpriteFactor = 2;
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

                var configStringSplitRay = File.ReadAllLines(filepath);

                InitialPosition = new Vector2(Convert.ToInt32(configStringSplitRay[1].Split(',')[0]),
                    Convert.ToInt32(configStringSplitRay[1].Split(',')[1]));

                _leftFrameSize = new Point(Convert.ToInt32(configStringSplitRay[2].Split(',')[0]),
                    Convert.ToInt32(configStringSplitRay[2].Split(',')[1]));

                _rightFrameSize = new Point(Convert.ToInt32(configStringSplitRay[3].Split(',')[0]),
                    Convert.ToInt32(configStringSplitRay[3].Split(',')[1]));


                _atRestFrameSize = new Point(Convert.ToInt32(configStringSplitRay[4].Split(',')[0]),
                    Convert.ToInt32(configStringSplitRay[4].Split(',')[1]));

                _leftSheetSize = new Point(Convert.ToInt32(configStringSplitRay[5].Split(',')[0]),
                    Convert.ToInt32(configStringSplitRay[5].Split(',')[1]));

                _rightSheetSize = new Point(Convert.ToInt32(configStringSplitRay[6].Split(',')[0]),
                    Convert.ToInt32(configStringSplitRay[6].Split(',')[1]));

                _atRestSheetSize = new Point(Convert.ToInt32(configStringSplitRay[7].Split(',')[0]),
                    Convert.ToInt32(configStringSplitRay[7].Split(',')[1]));

                _leftTextureFilename = configStringSplitRay[8];
                _rightTextureFilename = configStringSplitRay[9];
                _atRestTextureFilename = configStringSplitRay[10];

                _timeBetweenFrames = Convert.ToInt32(configStringSplitRay[11]);

                ScaleUpThisSpriteFactor = Convert.ToInt32(configStringSplitRay[12]);


                // This next call reads in the properties of the sub-class. (template method)
                Load(configStringSplitRay, 13);
            } // end else

            _elapsedGameTime = 0;
            CurrentTextureFilename = _atRestTextureFilename;
            CurrentPosition = InitialPosition;
        }

        public void WritePropertiesToFile(string filepath)
        {
            Debug.Assert(filepath != null && !filepath.Equals(""),
                "filepath can not be null and can not be the empty string!");

            using (var fs = File.Create(filepath))
            {
                Utilities.AddText(fs, NameOfThisSubclassForWritingToConfigFile); // ex) "UserControlledSprite"
                Utilities.AddText(fs, "\n");

                Utilities.AddText(fs, InitialPosition.X + "," + InitialPosition.Y);
                Utilities.AddText(fs, "\n");

                Utilities.AddText(fs, _leftFrameSize.X + "," + _leftFrameSize.Y);
                Utilities.AddText(fs, "\n");

                Utilities.AddText(fs, _rightFrameSize.X + "," + _rightFrameSize.Y);
                Utilities.AddText(fs, "\n");

                Utilities.AddText(fs, _atRestFrameSize.X + "," + _atRestFrameSize.Y);
                Utilities.AddText(fs, "\n");

                Utilities.AddText(fs, _leftSheetSize.X + "," + _leftSheetSize.Y);
                Utilities.AddText(fs, "\n");

                Utilities.AddText(fs, _rightSheetSize.X + "," + _rightSheetSize.Y);
                Utilities.AddText(fs, "\n");

                Utilities.AddText(fs, _atRestSheetSize.X + "," + _atRestSheetSize.Y);
                Utilities.AddText(fs, "\n");

                Utilities.AddText(fs, _leftTextureFilename);
                Utilities.AddText(fs, "\n");

                Utilities.AddText(fs, _rightTextureFilename);
                Utilities.AddText(fs, "\n");

                Utilities.AddText(fs, _atRestTextureFilename);
                Utilities.AddText(fs, "\n");

                Utilities.AddText(fs, _timeBetweenFrames + "");
                Utilities.AddText(fs, "\n");

                Utilities.AddText(fs, ScaleUpThisSpriteFactor + "");
                Utilities.AddText(fs, "\n");

                // Write out the subclass's properties.
                Write(fs);
            }
        } // end method
    } // end class
} // end namespace