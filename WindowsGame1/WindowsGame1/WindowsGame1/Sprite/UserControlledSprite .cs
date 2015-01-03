using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// My using statements.
using WindowsGameLibrary1;

namespace Sprite
{
    public class UserControlledSprite : AnimatedSprite
    {
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
            if (keyState.IsKeyDown(Keys.Right))
            {
                this.CurrentPosition.X = this.CurrentPosition.X + 5;
            }

            if (keyState.IsKeyDown(Keys.Left))
            {
                this.CurrentPosition.X = this.CurrentPosition.X - 5;
            }
        }

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
