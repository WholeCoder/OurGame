using System;
using System.IO;
using System.Text;

using WindowsGameLibrary1;

namespace Sprite
{
    public class AutomatedSprite : AnimatedSprite
    {
        public AutomatedSprite(TextureCache tCache,string pathWithFile)
            : base(tCache)
        {
            base.textureFilename = pathWithFile;
            this.Load(pathWithFile);  // Will write defaults to disk if the file isn't found.
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
                    AddText(fs, "AutomatedSprite");
                    AddText(fs, "\n");

                    // Write superclasses properties.
                    base.WriteToDisk(fs);

                    // Write defaults for this subclss.
                    this.WriteToDisk(fs);
                }
            } else 
            {
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
    }
}
