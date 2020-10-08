using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.Library.Core
{
    public class ParticipantsChangedEventArgs : EventArgs
    {
        public Track Track { get; set; }    
    }
}
