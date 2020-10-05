using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

using RaceSimulator.Library.Core;
using RaceSimulator.Library.Core.Enumerations;
using RaceSimulator.Library.Core.Interfaces;

namespace RaceSimulator.Library.Controller
{
    public static class Data
    {
        public static Competition Competition { get; private set; }

        public static Race CurrentRace { get; set; }

        public static Competition Initialize()
        {
            Competition = new Competition(new List<IParticipant>(), new Queue<Track>());
            AddParticipants();
            AddTracks();
            return Competition;
        }

        public static void AddParticipants()
        {
            Competition.Participants.AddRange(new List<IParticipant>
            {
                new Driver()
                {
                    Name = "Speedster",
                    Equipment = new Car()
                    {
                        Quality = 10,
                        IsBroken = false,
                        Performance = 10,
                        Speed = 10
                    },
                    Points = 0,
                    TeamColor = TeamColor.Blue
                },
                new Driver()
                {
                    Name = "El Diablo",
                    Equipment = new Car()
                    {
                        Quality = 15,
                        IsBroken = false,
                        Performance = 20,
                        Speed = 5
                    },
                    Points = 0,
                    TeamColor = TeamColor.Red
                }
            });
        }

        public static void AddTracks()
        {
            List<SectionTypes[]> st = new List<SectionTypes[]>
            {
                new SectionTypes[]
                {
                    // ==== First section row =====
                    SectionTypes.RightCorner,
                    SectionTypes.Straight,
                    SectionTypes.StartGrid,
                    SectionTypes.Straight,
                    SectionTypes.RightCorner,
                    SectionTypes.Straight,
                    SectionTypes.RightCorner,
                    SectionTypes.LeftCorner,
                    SectionTypes.RightCorner,
                    SectionTypes.Straight,
                    SectionTypes.RightCorner,
                    SectionTypes.LeftCorner,
                    SectionTypes.RightCorner,
                    SectionTypes.Straight,
                },
                new SectionTypes[]
                {
                    SectionTypes.RightCorner,
                    SectionTypes.RightCorner,
                    SectionTypes.Straight,
                    SectionTypes.LeftCorner,
                    SectionTypes.LeftCorner,
                    SectionTypes.Straight,
                    SectionTypes.RightCorner,
                    SectionTypes.RightCorner,
                    SectionTypes.Straight,
                    SectionTypes.LeftCorner,
                    SectionTypes.LeftCorner,
                    SectionTypes.Straight,
                    SectionTypes.RightCorner,
                    SectionTypes.RightCorner,
                    SectionTypes.Straight,
                    SectionTypes.Straight,
                    SectionTypes.Straight,
                    SectionTypes.Straight,
                    SectionTypes.RightCorner,
                    SectionTypes.RightCorner,
                    SectionTypes.Straight,
                    SectionTypes.LeftCorner,
                    SectionTypes.LeftCorner,
                    SectionTypes.Straight,
                    SectionTypes.RightCorner,
                    SectionTypes.RightCorner,
                    SectionTypes.Straight,
                    SectionTypes.LeftCorner,
                    SectionTypes.LeftCorner,
                    SectionTypes.Straight,
                    SectionTypes.RightCorner,
                    SectionTypes.RightCorner,
                    SectionTypes.Straight,
                    SectionTypes.Finish,
                    SectionTypes.StartGrid,
                    SectionTypes.Straight,
                }
            };     

            List<Track> trackList = new List<Track>()
            {
                new Track("Race of the living", st[0]),
                new Track("zigZag", st[1])
            };

            foreach (Track track in trackList)
            {
                Competition.Tracks.Enqueue(track);
            }
        }

        public static void NextRace()
        {
            Track track = Competition.NextTrack();

            if(track != null)
            {
                CurrentRace = new Race(track, Competition.Participants);
            }
        }
    }
}
