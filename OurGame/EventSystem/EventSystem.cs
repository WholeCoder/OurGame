using System;
using System.Collections;
using System.Diagnostics.Eventing.Reader;
using OurGame.EventSystem.Events;

namespace OurGame.EventSystem
{
    // Can't use event in lowercase as a varibalbe name!
    public class EventSystem
    {
        private EventSystem _eventSystem;

        private readonly Hashtable _ehTable ;

        public EventSystem GetInstance()
        {
            if (_eventSystem == null)
            {
                _eventSystem = new EventSystem();
            }

            return _eventSystem;

        }

        private EventSystem()
        {
            _ehTable = new Hashtable();
        }

        public void RegisterEvent(String eventMessage, Event et)
        {
            _ehTable[eventMessage] = et;
        }
    }
}