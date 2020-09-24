using RaceSimulator.Library.Controller;

using System;
using System.Threading;

namespace RaceSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Data.Initialize();
            Data.NextRace();

            Console.WriteLine($"Current Track: {Data.CurrentRace.Track.Name}");

            for (; ; )
            {
                Thread.Sleep(100);
            }
        }
    }
}
