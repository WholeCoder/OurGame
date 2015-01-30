using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using OurGame.GameStates;
using OurGame.OurGameLibrary;

namespace OurGame.Sprites
{
    internal static class SimpleAnimatedSpriteFactory
    {
        public static AnimatedSprite CreateAnimatedSprite(String filepath, Board board, State pState)
        {
            Debug.Assert(filepath != null && !filepath.Equals(""), "filepath must not be null or empty!");
            Debug.Assert(board != null, "board can not be null!");
            Debug.Assert(pState != null, "pState can not be null!");

            var configStringSplitRay = File.ReadAllLines(filepath);

            var typeOfAnimatedSprite = configStringSplitRay[0];

            AnimatedSprite spriteWeAreLoading = null;
            if (typeOfAnimatedSprite.Equals("AutomatedSprite"))
            {
                spriteWeAreLoading = new AutomatedSprite(filepath, board, pState);
            }
            else if (typeOfAnimatedSprite.Equals("UserControlledSprite"))
            {
                spriteWeAreLoading = new UserControlledSprite(filepath, board, pState);
            }

            return spriteWeAreLoading;
        }

        private static void AddText(FileStream fs, string value)
        {
            Debug.Assert(fs != null, "FileStream fs can't be null!");
            Debug.Assert(value != null, "value can't be null!");
            Debug.Assert(fs.CanWrite, "FileStream fs must be open for writing!");

            var info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        } // end method
    } // end class
} // end using