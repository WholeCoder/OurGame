using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WindowsGameLibrary1;

namespace Sprite
{
    public class AutomatedSprite : AnimatedSprite
    {
        public AutomatedSprite(Point frameSize, Point sheetSize, string textureFilename, TextureCache tCache, int TimeBetweenFrames, Vector2 CurrentPosition)
            : base(frameSize, sheetSize, textureFilename, tCache, TimeBetweenFrames, CurrentPosition)
        {

        }
    }
}
