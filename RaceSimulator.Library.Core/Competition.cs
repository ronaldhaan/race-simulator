using RaceSimulator.Library.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.Library.Core
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; }

        public Queue<Track> Tracks { get; set; }

        public Competition() { }

        public Competition(List<IParticipant> participants, Queue<Track> tracks)
        {
            Participants = participants;
            Tracks = tracks;
        }

        public Track NextTrack()
        {
            Track track = null;

            if(Tracks != null && Tracks.Count > 0)
            {
                track = Tracks.Peek();
            }

            return track;
        }
    }
}
