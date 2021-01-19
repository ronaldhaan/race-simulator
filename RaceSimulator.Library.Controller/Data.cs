using System;
using System.Collections.Generic;

using RaceSimulator.Library.Core;
using RaceSimulator.Library.Core.Enumerations;
using RaceSimulator.Library.Core.Events;
using RaceSimulator.Library.Core.Interfaces;

namespace RaceSimulator.Library.Controller
{
    public static class Data
    {
        public static Competition Competition { get; private set; }

        public static Race CurrentRace { get; set; }

        public static event EventHandler<RaceFinishedEventArgs> CurrentRaceFinised;

        public static void Initialize()
        {
            Competition = new Competition(new List<IParticipant>(), new Queue<Track>());
            AddParticipants();
            AddTracks();
        }

        public static void AddParticipants()
        {
            Competition.Participants.Clear();
            Competition.Participants.AddRange(new List<IParticipant>
            {
                new Driver(
                    name: "Speedster",
                    points: 0,
                    equipment: new Car(5,10, 0, false),
                    teamColor: TeamColor.Blue
                ),
                new Driver(
                    name: "El Diablo", 
                    points: 0, 
                    equipment: new Car(5, 10, 0, false), 
                    teamColor: TeamColor.Red
                ),
                new Driver(
                    name: "Test",
                    points: 0,
                    equipment: new Car(5, 10, 0, false),
                    teamColor: TeamColor.Green
                ),
                new Driver(
                    name: "Test2",
                    points: 0,
                    equipment: new Car(5, 10, 0, false),
                    teamColor: TeamColor.Green
                ),
            });
        }

        public static void AddTracks()
        {   
            List<Track> trackList = new List<Track>()
            {
                new Track("Race of the living", new SectionTypes[]
                {
                    // ==== First section row =====
                    SectionTypes.RightCorner,
                    SectionTypes.Straight,
                    SectionTypes.Finish,
                    SectionTypes.Straight,
                    SectionTypes.StartGrid,
                    SectionTypes.Straight,
                    SectionTypes.RightCorner,
                    SectionTypes.Straight,
                    SectionTypes.RightCorner,
                    SectionTypes.LeftCorner,
                    SectionTypes.RightCorner,
                    SectionTypes.Straight,
                    SectionTypes.Straight,
                    SectionTypes.Straight,
                    SectionTypes.RightCorner,
                    SectionTypes.LeftCorner,
                    SectionTypes.RightCorner,
                    SectionTypes.Straight,
                }),
               //new Track("zigZag", new SectionTypes[]
               //{
               //    SectionTypes.RightCorner,
               //    SectionTypes.RightCorner,
               //    SectionTypes.Straight,
               //    SectionTypes.LeftCorner,
               //    SectionTypes.LeftCorner,
               //    SectionTypes.Straight,
               //    SectionTypes.RightCorner,
               //    SectionTypes.RightCorner,
               //    SectionTypes.Straight,
               //    SectionTypes.LeftCorner,
               //    SectionTypes.LeftCorner,
               //    SectionTypes.Straight,
               //    SectionTypes.RightCorner,
               //    SectionTypes.RightCorner,
               //    SectionTypes.Straight,
               //    SectionTypes.Straight,
               //    SectionTypes.Straight,
               //    SectionTypes.Straight,
               //    SectionTypes.Straight,
               //    SectionTypes.RightCorner,
               //    SectionTypes.RightCorner,
               //    SectionTypes.Straight,
               //    SectionTypes.LeftCorner,
               //    SectionTypes.LeftCorner,
               //    SectionTypes.Straight,
               //    SectionTypes.RightCorner,
               //    SectionTypes.RightCorner,
               //    SectionTypes.Straight,
               //    SectionTypes.LeftCorner,
               //    SectionTypes.LeftCorner,
               //    SectionTypes.Straight,
               //    SectionTypes.RightCorner,
               //    SectionTypes.RightCorner,
               //    SectionTypes.Straight,
               //    SectionTypes.Finish,
               //    SectionTypes.Straight,
               //    SectionTypes.StartGrid,
               //    SectionTypes.Straight,
               //}),
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
                if (CurrentRace != null)
                {
                    CurrentRace.RaceFinished -= RaceFinished;
                    CurrentRace = null;
                }

                CurrentRace = new Race(track, Competition.Participants);
                CurrentRace.RaceFinished += RaceFinished;
            }

            return track;
        }

        private static void RaceFinished(object sender, RaceFinishedEventArgs e)
        {
            AddDataToStorage(e);
            e.Track = NextRace();
            CurrentRaceFinised?.Invoke(sender, e);
            CurrentRace.Start();
        }

        private static void AddDataToStorage(RaceFinishedEventArgs e)
        {
            Competition.AddData(e);
        }
    }
}
