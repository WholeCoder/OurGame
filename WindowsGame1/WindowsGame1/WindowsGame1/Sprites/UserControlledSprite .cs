using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// My using statements.
using OurGame.WindowsGameLibrary1;

namespace OurGame.Sprites
{
    public sealed class UserControlledSprite : AnimatedSprite
    {
        private int _StartyingYCoordinateForJumping;
        private int _JumpDelta = 0;
        private bool _CurrentlyJumpting = false;

        public UserControlledSprite(TextureCache tCache, string configFilePathAndName)
            : base(tCache, configFilePathAndName)
        {

        }

        // This will start at the startOffset and read out it's attributes.
        public override void Load(string[] configArray, int startOffset)
        {
            // Nothing to Load yet!
            // TODO: Read properties starting at startOffset.
        }

        public override void UpdateAfterNextFrame(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Right) && keyState.IsKeyUp(Keys.Left))
            {

                this.SwitchToGoRightTexture();
                this.CurrentPosition.X = this.CurrentPosition.X + 5;
            }
            else if (keyState.IsKeyDown(Keys.Left) && keyState.IsKeyUp(Keys.Right))
            {
                this.SwitchToGoLeftTexture();
                this.CurrentPosition.X = this.CurrentPosition.X - 5;
            } else if (keyState.IsKeyUp(Keys.Left) && keyState.IsKeyUp(Keys.Right))
            {
                this.SwitchToAtRestTexture();
            }

            if (keyState.IsKeyDown(Keys.Space) && !this._CurrentlyJumpting)
            {
                this._JumpDelta = -10;
                this._CurrentlyJumpting = true;
                this._StartyingYCoordinateForJumping = (int)this.CurrentPosition.Y;
            }

            if (this._CurrentlyJumpting && this.CurrentPosition.Y <= this._StartyingYCoordinateForJumping)
            {
                this.CurrentPosition.Y += this._JumpDelta;
                this._JumpDelta += 1;
            }

            if (this.CurrentPosition.Y > this._StartyingYCoordinateForJumping)
            {
                this._CurrentlyJumpting = false;
                this._JumpDelta = 0;
                this.CurrentPosition.Y = this._StartyingYCoordinateForJumping;
            }
        } // end method

        public override string NameOfThisSubclassForWritingToConfigFile()
        {
            // This is written out to the config file.
            return "UserControlledSprite";
        }

        // In this method we use fs to write out the subclasses properties.
        public override void Write(FileStream fs)
        {
            // Nothing to write yet!
            // TODO: Write out attributes if they exist for UserCotnrolledSprite

        } // end method

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            C3.XNA.Primitives2D.DrawRectangle(spriteBatch, this.BoundingRectangle, Color.Red);
        }
    } // end class
} // end using
