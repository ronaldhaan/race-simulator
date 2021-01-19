using RaceSimulator.Library.Controller;
using RaceSimulator.Library.Core;
using RaceSimulator.Library.Core.Events;
using RaceSimulator.Library.Core.Interfaces;
using RaceSimulator.Library.Core.Templates;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RaceSimulator.View.ConsoleApp
{
    class Program
    {
        private static List<string> tracknames;

        static void Main(string[] args = null)
        {
            tracknames = new List<string>();
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
                DrawTable();
                Console.WriteLine("Races are over! Start over? y/n");
                if(Console.ReadLine().ToUpper() == "Y")
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
                tracknames.Add(track.Name);
                Console.WriteLine($"Hello World! Welcome to: '{track.Name}'");
                //ConsoleRaceBuilder.DrawTrack(track);

                Data.CurrentRace.ParticipantsMoved += ConsoleRaceBuilder.RedrawTrack;
            }
           
        }

        private static void RaceFinished(object sender, RaceFinishedEventArgs e)
        {
            Start(e.Track);
        }

        public static void DrawTable()
        {
            ConsoleTable table = new ConsoleTable(tracknames);
            table.Draw();            
        }
    }
}
