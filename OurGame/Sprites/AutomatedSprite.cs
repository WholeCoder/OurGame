using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

using OurGame.WindowsGameLibrary1;

namespace OurGame.Sprites
{
    public class AutomatedSprite : AnimatedSprite
    {
        public AutomatedSprite(string configFilePathAndName)
            : base(configFilePathAndName)
        {
            //            this.Load(pathWithFile);  // Will write defaults to disk if the file isn't found.
        }

        // This will start at the startOffset and read out it's attributes.
        public sealed override void Load(string[] configArray, int startOffset)
        {
            Debug.Assert(configArray != null, "configArray can't be null!");
            Debug.Assert(startOffset >= 0, "startOffset must be >= 0!");

            // Nothing to Load yet!
            // TODO: Read properties starting at startOffset.
        }


        public override void UpdateAfterNextFrame(GameTime gameTime)
        {

        }
        
        public override string NameOfThisSubclassForWritingToConfigFile()
        {
            // This is written out to the config file and used as a "constant" in the SimpleAnimatedSpriteFactory.
            return "AutomatedSprite";
        }

        // In this method we use fs to write out the subclasses properties.
        public sealed override void Write(FileStream fs)
        {
            Debug.Assert(fs.CanWrite, "FileStream fs must be open for writing!");

            // Nothing to write yet!
            // TODO: Write out attributes if they exist for UserCotnrolledSprite

        } // end method

    } // end class
}
