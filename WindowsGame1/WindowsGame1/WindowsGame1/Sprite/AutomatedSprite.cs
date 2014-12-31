using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGameLibrary1;

namespace Sprite
{
    public class AutomatedSprite : AnimatedSprite
    {
        private Point point;
        private Point point_2;
        private string p;
        private int p_2;
        private Vector2 vector2;

        public AutomatedSprite(Point frameSize, Point sheetSize, string textureFilename, TextureCache tCache, int TimeBetweenFrames, Vector2 CurrentPosition)
            : base(frameSize, sheetSize, textureFilename, tCache, TimeBetweenFrames, CurrentPosition)
        {

        }

        // This method comes from the AnimatedSprite abstract super-class.
        public override AnimatedSprite Load(string filepath)
        {
            throw new NotImplementedException();
            return null;
        }
    }
}
