using RaceSimulator.Library.Core.Interfaces;
using RaceSimulator.Library.Core.Templates;

using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.Library.Core.Events
{
    public class RaceFinishedEventArgs : TrackEventArgs
    {
        public List<IParticipant> RanglistFinishedRace { get; set; }

        public Dictionary<IParticipant, int> TimesCatchedUp { get; set; }

        public List<ParticipantTimeData> ParticipantTimeDatas { get; set; }


        public RaceFinishedEventArgs(List<IParticipant> ranglist, List<ParticipantTimeData> time, Dictionary<IParticipant, int> timesCatchedUp, Track t) : base(t)
        {
            RanglistFinishedRace = ranglist;
            ParticipantTimeDatas = time;
            TimesCatchedUp = timesCatchedUp;
        }
    }
}
