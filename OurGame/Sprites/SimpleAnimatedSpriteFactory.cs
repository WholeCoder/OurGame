using System;
using System.IO;
using System.Text;
using System.Diagnostics;

// My usings
using OurGame.GameStates;
using OurGame.OurGameLibrary;

namespace OurGame.Sprites
{
    class SimpleAnimatedSpriteFactory
    {
        public static AnimatedSprite CreateAnimatedSprite(String filepath, Board board, PlayGameState pState)
        {
            Debug.Assert( filepath != null && !filepath.Equals(""),"filepath must not be null or empty!");

            string[] configStringSplitRay = File.ReadAllLines(filepath);

            string typeOfAnimatedSprite = configStringSplitRay[0];

            AnimatedSprite spriteWeAreLoading = null;
            if (typeOfAnimatedSprite.Equals("AutomatedSprite"))
            {
                spriteWeAreLoading = new AutomatedSprite(filepath, board, pState);
            } else if (typeOfAnimatedSprite.Equals("UserControlledSprite"))
            {
                spriteWeAreLoading = new UserControlledSprite(filepath, board, pState);
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
