using System;
using System.Runtime.Serialization;

namespace OurGame.OurGameLibrary
{
    // This exception is thrown when you fail to call TextureCache.SetContent(...) with valid parameters before calling Texture.getInstance();
    // or call SoundSystem.getInstance() without calling SoundSystem.SetContent(...) method.
    internal class MustCallSetContentMethodFirstException : Exception
    {
        public MustCallSetContentMethodFirstException()
            : base()
        {
        }

        public MustCallSetContentMethodFirstException(string message)
            : base(message)
        {
        }

        public MustCallSetContentMethodFirstException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected MustCallSetContentMethodFirstException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}