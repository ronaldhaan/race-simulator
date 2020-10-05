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
            Competition com = Data.Initialize();
            Data.NextRace();
            Track track = Data.Competition.NextTrack();
            Console.WriteLine($"Hello World! Welcome to: '{track.Name}'");
            RaceBuilder.DrawTrack(track);

            for (; ; )
            {
                Thread.Sleep(100);
            }
        }
    }
}
