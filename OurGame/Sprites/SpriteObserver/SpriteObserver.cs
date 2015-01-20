using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OurGame.Sprites.Observer
{
    public interface SpriteObserver
    {
        void update(int life);
    }
}
