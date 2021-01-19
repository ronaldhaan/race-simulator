using RaceSimulator.Library.Core.Interfaces;
using RaceSimulator.Library.Core.Templates;

using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.Library.Core.Events
{
    public class RaceFinishedEventArgs : TrackEventArgs
    {
        public RaceData<ParticipantPointsData> RanglistFinishedRace { get; set; }

        public RaceData<ParticipantTimesCatchedUp> TimesCatchedUp { get; set; }

        public RaceData<ParticipantTimePerSectionData> ParticipantTimePerSectionDatas { get; set; }
        public RaceData<ParticipantTimeData> ParticpantTimeData { get; set; }

        public RaceFinishedEventArgs(RaceData<ParticipantTimeData> ptimeData, RaceData<ParticipantPointsData> ranglist, RaceData<ParticipantTimePerSectionData> time, RaceData<ParticipantTimesCatchedUp> timesCatchedUp, Track t) : base(t)
        {
            ParticpantTimeData = ptimeData;
            RanglistFinishedRace = ranglist;
            ParticipantTimePerSectionDatas = time;
            TimesCatchedUp = timesCatchedUp;
        }
    }
}
