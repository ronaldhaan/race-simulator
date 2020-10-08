using RaceSimulator.Library.Controller;
using RaceSimulator.Library.Core;

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
            
            Console.WriteLine($"{string.Join("", args)}Hello World! Welcome to: '{track.Name}'");
            Data.CurrentRace.ParticipantsChanged += ConsoleRaceBuilder.RedrawTrack;
            ConsoleRaceBuilder.DrawTrack(track);

            var i = Console.ReadKey();

            Data.CurrentRace.Start();

            for(; ; ) { }
        }

    }
}
