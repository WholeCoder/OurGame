using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGameLibrary1;

namespace Sprite
{
    public class UserControlledSprite : AnimatedSprite
    {
        public UserControlledSprite(Point frameSize, Point sheetSize, string textureFilename, TextureCache tCache, int TimeBetweenFrames, Vector2 CurrentPosition)
            : base(frameSize, sheetSize, textureFilename, tCache, TimeBetweenFrames, CurrentPosition)
        {
        }

        // This method comes from the AnimatedSprite abstract super-class.
        public override AnimatedSprite Load(string filepath)
        {
            throw new NotImplementedException();
            return null;
        } // end method
    } // end class
} // end using
