using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OurGame.OurGameLibrary
{
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

