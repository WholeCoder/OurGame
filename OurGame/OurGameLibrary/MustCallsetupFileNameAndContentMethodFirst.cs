using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OurGame.OurGameLibrary
{

    // This exception is thrown when you fail to call TextureCache.setupFileNamesAndcontent(...) with valid parameters.
    class MustCallsetupFileNameAndContentMethodFirst : Exception
    {
        public MustCallsetupFileNameAndContentMethodFirst()
        {
        }

        public MustCallsetupFileNameAndContentMethodFirst(string message)
            : base(message)
        {
        }

        public MustCallsetupFileNameAndContentMethodFirst(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

