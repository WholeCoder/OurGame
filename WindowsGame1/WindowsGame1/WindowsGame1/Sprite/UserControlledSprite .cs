using System;
using System.IO;
using System.Text;

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

        public override string NameOfThisSubclassForWritingToConfigFile()
        {
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
