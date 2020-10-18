using RaceSimulator.Library.Controller;
using RaceSimulator.Library.Core;
using RaceSimulator.Library.Core.Events;

using System;
using System.Threading;

namespace RaceSimulator.View.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Data.Initialize();
            Track track = Data.NextRace();
            Data.CurrentRaceEnded += Data_CurrentRaceEnded;
            Start(track);

            while(true) { }
        }

        private static void Start(Track track)
        {
            Console.Clear();
            if(track == null)
            {
                Console.WriteLine("Races are over! Start over? y/n");
                string s = Console.ReadLine();
                if(s.ToUpper() == "Y")
                {
                    Main(new string[0]);
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

                Data.CurrentRace.ParticipantsChanged += ConsoleRaceBuilder.RedrawTrack;
            }
           
        }

        private static void Data_CurrentRaceEnded(object sender, RaceEndedEventArgs e)
        {
            Start(e.Track);
        }
    }
}
