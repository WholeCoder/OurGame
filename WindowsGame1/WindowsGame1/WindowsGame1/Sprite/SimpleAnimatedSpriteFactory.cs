using System;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;

// My usings
using WindowsGameLibrary1;

namespace Sprite
{
    class SimpleAnimatedSpriteFactory
    {
        public static AnimatedSprite createAnimatedSprite(String filepath, TextureCache tCache)
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

            string typeOfAnimatedSprite = configStringSplitRay[0];

            AnimatedSprite spriteWeAreLoading = null;
            if (typeOfAnimatedSprite.Equals("AutomatedSprite"))
            {
                // TODO: Remove all arguments to constructor and create the implementation of the load method.
                spriteWeAreLoading = new AutomatedSprite(new Microsoft.Xna.Framework.Point(20, 20), new Microsoft.Xna.Framework.Point(2, 0), "Images/spritesheets/manspritesheet", tCache, 100, new Vector2(100, 100));
                spriteWeAreLoading.Load(filepath);
            } else if (typeOfAnimatedSprite.Equals("UserControlledSprite"))
            {
                // TODO: Remove all arguments to constructor and create the implementation of the load method.
                spriteWeAreLoading = new UserControlledSprite(new Microsoft.Xna.Framework.Point(20, 20), new Microsoft.Xna.Framework.Point(2, 0), "Images/spritesheets/manspritesheet", tCache, 100, new Vector2(100, 100));
                spriteWeAreLoading.Load(filepath);
            }

            return spriteWeAreLoading;
        }

        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        } // end method
    } // end class
} // end using
