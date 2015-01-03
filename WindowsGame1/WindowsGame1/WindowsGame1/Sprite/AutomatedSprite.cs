using System;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;

using WindowsGameLibrary1;

namespace Sprite
{
    public class AutomatedSprite : AnimatedSprite
    {
        public AutomatedSprite(TextureCache tCache, string configFilePathAndName)
            : base(tCache, configFilePathAndName)
        {
            //            this.Load(pathWithFile);  // Will write defaults to disk if the file isn't found.
        }

        // This will start at the startOffset and read out it's attributes.
        public override void Load(string[] configArray, int startOffset)
        {
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
        public override void Write(FileStream fs)
        {
            // Nothing to write yet!
            // TODO: Write out attributes if they exist for UserCotnrolledSprite

        } // end method

    } // end class
}
