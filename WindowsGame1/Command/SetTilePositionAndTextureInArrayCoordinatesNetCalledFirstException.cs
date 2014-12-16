using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Command
{
    class SetTilePositionAndTextureInArrayCoordinatesNetCalledFirstException : Exception
    {
        public SetTilePositionAndTextureInArrayCoordinatesNetCalledFirstException()
        {
        }

        public SetTilePositionAndTextureInArrayCoordinatesNetCalledFirstException(string message)
            : base(message)
        {
        }

        public SetTilePositionAndTextureInArrayCoordinatesNetCalledFirstException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
