using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.Library.Core.Events
{
    public class ParticipantsChangedEventArgs : TrackEventArgs
    {

        public ParticipantsChangedEventArgs(Track t) : base(t) { }
    }
}
