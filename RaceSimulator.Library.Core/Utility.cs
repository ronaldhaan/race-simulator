using RaceSimulator.Library.Core.Interfaces;
using RaceSimulator.Library.Core.Templates;

using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulator.Library.Core
{
    public static class Utility
    {
        public static List<IParticipant> SortParticipants(List<IParticipant> participants)
        {
            List<IParticipant> sorted = new List<IParticipant>(participants);

            sorted.Sort(new ParticipantComparer());

            return sorted;
        }

        public static int GetTimesCatchedUp(IParticipant p, RaceData<ParticipantTimesCatchedUp> participantTimesCatchedUp)
        {
            var pData = participantTimesCatchedUp.FindByName(p.Name);

            int times = 0;
            if (pData != null)
            {
                times = pData.TimesCatchedUp;
            }
            return times;
        }

        public static TimeSpan GetFinished(IParticipant p, RaceData<ParticipantTimePerSectionData> participantTimePerSectionData)
        {
            var pData = participantTimePerSectionData.FindByName(p.Name);


            TimeSpan time = TimeSpan.Zero;

            if (pData != null)
            {
                time = pData.TotalTime;
            }

            return time;
        }

        public static int GetPoints(IParticipant p, RaceData<ParticipantPointsData> participantPointsData)
        {
            var pData = participantPointsData.FindByName(p.Name);

            int points = 0;

            if (pData != null)
            {
                points = pData.Points;
            }

            return points;
        }
    }
}
