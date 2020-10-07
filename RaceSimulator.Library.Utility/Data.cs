using System.Collections.Generic;

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
                new Driver(
                    name: "Speedster",
                    points: 0,
                    equipment: new Car(10,10, 10, false),
                    teamColor: TeamColor.Blue
                ),
                new Driver(
                    name: "El Diablo", 
                    points: 0, 
                    equipment: new Car(20, 15, 5, false), 
                    teamColor: TeamColor.Red
                )
            });
        }

        public static void AddTracks()
        {   
            List<Track> trackList = new List<Track>()
            {
                new Track("zigZag", new SectionTypes[]
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
                }),
                new Track("Race of the living", new SectionTypes[]
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
                }),
            };

            foreach (Track track in trackList)
            {
                Competition.Tracks.Enqueue(track);
            }
        }

        public static Track NextRace()
        {
            Track track = Competition.NextTrack();

            if(track != null)
            {
                CurrentRace = new Race(track, Competition.Participants);
            }

            return track;
        }
    }
}
