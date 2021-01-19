using RaceSimulator.Library.Core.Events;
using RaceSimulator.Library.Core.Interfaces;
using RaceSimulator.Library.Core.Templates;

using System;
using System.Collections.Generic;
using System.Linq;

namespace RaceSimulator.Library.Core
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; }

        public Queue<Track> Tracks { get; set; }

        public List<TrackRaceData> RaceDataPerTrack { get; set; }

        public Competition(List<IParticipant> participants, Queue<Track> tracks)
        {
            Participants = participants;
            Tracks = tracks;
            RaceDataPerTrack = new List<TrackRaceData>();
        }

        public Track NextTrack()
        {
            Track track = null;

            if (Tracks.Count > 0)
            {
                track = Tracks?.Dequeue();
            }

            return track;

        }

        public void AddData(RaceFinishedEventArgs e)
        {
            var raceData = new TrackRaceData
            {
                TrackName = e.Track.Name,
                ParticipantPointsData = e.RanglistFinishedRace,
                ParticipantTimePerSectionData = e.ParticipantTimePerSectionDatas,
                ParticipantTimesCatchedUp = e.TimesCatchedUp,
                ParticipantTimeData = e.ParticpantTimeData
            };


            SetPoints(ref raceData);

            if (!RaceDataPerTrack.Contains(raceData))
            {
                RaceDataPerTrack.Add(raceData);
            }
        }

        public void SetPoints(ref TrackRaceData raceData)
        {
            var participants = new List<IParticipant>(Participants);
            participants.Sort(new ParticipantComparer(raceData));

            int points = 2;

            foreach(var p in participants)
            {
                var pData = raceData.ParticipantPointsData.FindByName(p.Name);

                for(int i = 0; i < points; i++)
                    raceData.ParticipantPointsData.Add(pData);

                points--;
            }
        }
    }
} 
