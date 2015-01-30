using System;

namespace OurGame.OurGameLibrary
{
    // This exception is thrown when you fail to call TextureCache.SetContent(...) with valid parameters before calling Texture.getInstance();
    internal class MustCallSetContentMethodFirst : Exception
    {
        public MustCallSetContentMethodFirst()
        {
        }

        public MustCallSetContentMethodFirst(string message)
            : base(message)
        {
        }

        public MustCallSetContentMethodFirst(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}