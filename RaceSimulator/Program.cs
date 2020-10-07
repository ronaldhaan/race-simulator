using RaceSimulator.Library.Controller;
using RaceSimulator.Library.Core;
using RaceSimulator.View;

using System;
using System.Threading;

namespace RaceSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Competition competition = Data.Initialize();
            Track track = Data.NextRace();
            
            Console.WriteLine($"{string.Join("", args)}Hello World! Welcome to: '{track.Name}'");
            Data.CurrentRace.PositionChanged += ConsoleRaceBuilder.RedrawTrack;
            ConsoleRaceBuilder.DrawTrack(track);

            Start();
        }

        private static void Start()
        {
            Console.ReadKey();

            while(true)
            {
                Thread.Sleep(100);
                Data.CurrentRace.Move();
            }

            //Start();
        }
    }
}
