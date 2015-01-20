using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OurGame.Sprites.Observer
{
    public interface SpriteSubject
    {
        void registerObserver(SpriteObserver sObserver);
        void removeObserver(SpriteObserver aObserver);
        void notifyObservers();
    }
}
