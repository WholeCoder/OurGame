using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// My using statements.
using OurGame.WindowsGameLibrary1;

namespace OurGame.Sprites
{
    public class UserControlledSprite : AnimatedSprite
    {
        bool _pressedRight = false;

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
    } // end class
} // end using
