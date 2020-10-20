using RaceSimulator.Library.Controller;
using RaceSimulator.Library.Core;
using RaceSimulator.Library.Core.Events;
using RaceSimulator.Library.Core.Templates;

using System;
using System.Linq;

namespace RaceSimulator.View.ConsoleApp
{
    class Program
    {
        static void Main(string[] args = null)
        {
            Data.Initialize();
            Track track = Data.NextRace();
            Data.CurrentRaceFinised += RaceFinished;
            Start(track);

            while(true) { }
        }

        private static void Start(Track track)
        {
            Console.Clear();
            if(track == null)
            {
                DisplayData();
                Console.WriteLine("Races are over! Start over? y/n");
                string s = Console.ReadLine();
                if(s.ToUpper() == "Y")
                {
                    Main();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            else 
            {
                Console.WriteLine($"Hello World! Welcome to: '{track.Name}'");
                //ConsoleRaceBuilder.DrawTrack(track);

                Data.CurrentRace.ParticipantsMoved += ConsoleRaceBuilder.RedrawTrack;
            }
           
        }

        private static void DisplayData()
        {
            string[] columns = { "Name", "Points", "Finished", "Times catched up" };
            Console.WriteLine("Participant Ranglist:");

            ConsoleTable table = new ConsoleTable();
            table.PrintLine();
            table.PrintRow(columns);

            foreach (var data in Data.Competition.ParticipantPoints.Data)
            {
                ParticipantTimeData timeData = Data.Competition.ParticipantTimes.FindByName(data.Name);
                ParticipantTimesCatchedUp catchedUpData = Data.Competition.ParticipantTimesCatchedUp.FindByName(data.Name);
                int index = Data.Competition.Participants.FindIndex(p => p.Name == data.Name) + 1;
                table.PrintRow(new string[] { $"{index}: {data.Name}", data.Points.ToString(), $"{timeData.TimeSpan:hh\\:mm\\:ss\\.f}ms", catchedUpData.TimesCatchedUp.ToString() });
            }

            table.PrintLine();
        }

        private static void RaceFinished(object sender, RaceFinishedEventArgs e)
        {
            Start(e.Track);
        }
    }
}
