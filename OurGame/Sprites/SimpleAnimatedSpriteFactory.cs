using System;
using System.IO;
using System.Text;
using System.Diagnostics;

// My usings
using OurGame.WindowsGameLibrary1;

namespace OurGame.Sprites
{
    class SimpleAnimatedSpriteFactory
    {
        public static AnimatedSprite CreateAnimatedSprite(String filepath, TextureCache tCache)
        {
            Debug.Assert( filepath != null && !filepath.Equals(""),"filepath must not be null or empty!");
            Debug.Assert(tCache != null, "TextureCache tCache can not be null!");

            string[] configStringSplitRay = File.ReadAllLines(filepath);

            string typeOfAnimatedSprite = configStringSplitRay[0];

            AnimatedSprite spriteWeAreLoading = null;
            if (typeOfAnimatedSprite.Equals("AutomatedSprite"))
            {
                // TODO: Remove all arguments to constructor and create the implementation of the load method.
                spriteWeAreLoading = new AutomatedSprite(tCache,filepath/*new Microsoft.Xna.Framework.Point(20, 20), new Microsoft.Xna.Framework.Point(2, 0), "Images/spritesheets/manspritesheet", tCache, 100, new Vector2(100, 100)*/);
                
                // TODO: possibly call this later to "Load(Content)"
                //spriteWeAreLoading.Load(filepath);
            } else if (typeOfAnimatedSprite.Equals("UserControlledSprite"))
            {
                // TODO: Remove all arguments to constructor and create the implementation of the load method.
                spriteWeAreLoading = new UserControlledSprite(filepath/*new Microsoft.Xna.Framework.Point(20, 20), new Microsoft.Xna.Framework.Point(2, 0), "Images/spritesheets/manspritesheet", tCache, 100, new Vector2(100, 100)*/);

                // TODO: possibly call this later to "Load(Content)"
                //spriteWeAreLoading.Load(filepath);
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
