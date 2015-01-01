using System;
using System.IO;
using System.Text;

using WindowsGameLibrary1;

namespace Sprite
{
    public class UserControlledSprite : AnimatedSprite
    {
        public UserControlledSprite(TextureCache tCache,string pathWithFile)
            : base(tCache)
        {
            base.textureFilename = pathWithFile; // From superclass
            this.Load(pathWithFile);
        }

        // This method comes from the AnimatedSprite abstract super-class.  Must call the supercalsses version
        public override void Load(string filepath)
        {
            if (!File.Exists(filepath))
            {
                using (FileStream fs = File.Create(filepath))
                {
                    // Set defaults

                    // Write "AutomatedSprite" to file and a \n.
                    AddText(fs, "UserControlledSprite");
                    AddText(fs, "\n");

                    // Write superclasses properties.
                    base.WriteToDisk(fs);

                    // Write defaults for this subclss.
                    this.WriteToDisk(fs);
                }
            }
            else
            {

                /*
                UserControlledSprite
                0,0
                0,0
                2,1
                100
                Images/spritesheets/manspritesheet

                 */
                String configurationString = "";  // Holds the entire configuration file.

                // Open the stream and read it back. 
                using (FileStream fs = File.OpenRead(filepath))
                {
                    byte[] b = new byte[1024];
                    UTF8Encoding temp = new UTF8Encoding(true);
                    while (fs.Read(b, 0, b.Length) > 0)
                    {
                        configurationString += temp.GetString(b);
                    }
                }
                string[] configStringSplitRay = configurationString.Split('\n');

                // This next call reads in the properties of the super-class.
                int newOffsetToStartReadingFrom = base.Load(configStringSplitRay, 1);

                // Next read in the instance members of this subclass.
                // we have access to base.tCache if needed!

            } // end else
        } // end method

        // Must call superclasses as well!
        public override void WriteToDisk(FileStream fs)
        {

        }
    } // end class
} // end using
