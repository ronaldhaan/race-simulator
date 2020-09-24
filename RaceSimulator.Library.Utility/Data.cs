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
            List<IParticipant> participants = new List<IParticipant>
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
            };

            Competition.Participants.AddRange(participants);
        }

        public static void AddTracks()
        {
            List<Section> sections = new List<Section>() 
            { 
                new Section()
                {
                    SectionType = SectionTypes.StartGrid,
                },
                new Section()
                {
                    SectionType = SectionTypes.LeftCorner,
                },
                new Section()
                {
                    SectionType = SectionTypes.RightCorner,
                },
                new Section()
                {
                    SectionType = SectionTypes.Straight,
                },
            };

            List<Track> trackList = new List<Track>()
            {
                new Track()
                {
                    Name = "Finish",
                    Sections = new LinkedList<Section>(sections),
                }
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
