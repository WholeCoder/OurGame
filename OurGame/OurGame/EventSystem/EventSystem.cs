﻿using System;
using System.Collections;
using OurGame.EventSystem.Events;

namespace OurGame.EventSystem
{
    // Can't use event in lowercase as a varibalbe name!
    public class EventSystem
    {
        private readonly Hashtable _ehTable = new Hashtable();
        private EventSystem _eventSystem;

        private EventSystem()
        {
        }

        public EventSystem GetInstance()
        {
            if (_eventSystem == null)
            {
                _eventSystem = new EventSystem();
            }

            return _eventSystem;
        }

        public void RegisterEvent(String eventMessage, Event et)
        {
            _ehTable[eventMessage] = et;
        }
    }
}