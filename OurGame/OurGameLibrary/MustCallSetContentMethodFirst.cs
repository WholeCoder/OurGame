﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OurGame.OurGameLibrary
{

    // This exception is thrown when you fail to call TextureCache.setupFileNamesAndcontent(...) with valid parameters.
    class MustCallSetContentMethodFirst : Exception
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
