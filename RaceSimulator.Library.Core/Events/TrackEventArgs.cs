using System;

namespace RaceSimulator.Library.Core.Events
{
    public class TrackEventArgs : EventArgs
    {
        public Track Track { get; set; }
    }
}