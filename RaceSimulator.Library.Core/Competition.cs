﻿using RaceSimulator.Library.Core.Events;
using RaceSimulator.Library.Core.Interfaces;
using RaceSimulator.Library.Core.Templates;

using System.Collections.Generic;

namespace RaceSimulator.Library.Core
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; }

        public Queue<Track> Tracks { get; set; }

        public RaceData<ParticipantPointsData> ParticipantPoints { get; set; }

        public RaceData<ParticipantTimeData> ParticipantTimes { get; set; }

        public RaceData<ParticipantTimesCatchedUp> ParticipantTimesCatchedUp { get; set; }

        public Competition(List<IParticipant> participants, Queue<Track> tracks)
        {
            Participants = participants;
            Tracks = tracks;
            ParticipantPoints = new RaceData<ParticipantPointsData>();
            ParticipantTimes = new RaceData<ParticipantTimeData>();
            ParticipantTimesCatchedUp = new RaceData<ParticipantTimesCatchedUp>();
        }

        public Track NextTrack()
        {
            Track track = null;

            if(Tracks.Count > 0)
            {
                track = Tracks?.Dequeue();
            }

            return track;
        }

        public void AddPoints(List<IParticipant> ranglist)
        {
            int points = 3;

            foreach (IParticipant p in ranglist)
            {       
                ParticipantPointsData data = ParticipantPoints.FindByName(p.Name);
                if (data != null)
                {
                    if (points > 0)
                    {
                        p.Points += points;
                        data.AddPoints(points);
                    }
                }
                else
                {
                    if (points > 0)
                    {
                        p.Points += points;
                    }

                    ParticipantPoints.Add(new ParticipantPointsData(p));
                }

                points--;
            }

            ParticipantPoints.Data.Sort(delegate (ParticipantPointsData p1, ParticipantPointsData p2)
            {
                return p2.Points.CompareTo(p1.Points);
            });
        }

        public void AddFinishedTimes(List<ParticipantTimeData> ptdatas)
        {
            foreach(ParticipantTimeData ptdata in ptdatas)
                ParticipantTimes.Add(ptdata);
        }

        public void AddTimesCatchedUp(Dictionary<IParticipant, int> dic)
        {
            List<IParticipant> participants = new List<IParticipant>(dic.Keys);

            foreach(var p in participants)
            {
                if(dic.TryGetValue(p, out int times))
                {
                    ParticipantTimesCatchedUp.Add(new ParticipantTimesCatchedUp(p.Name, times));
                }
            }
        }
    }
} 
