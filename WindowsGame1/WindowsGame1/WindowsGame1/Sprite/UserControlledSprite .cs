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
    }
}
