using RaceSimulator.Library.Core.Enumerations;

using System.Collections.Generic;

namespace RaceSimulator.Library.Core
{
    public class Track
    {
        public string Name { get; set; }

        public LinkedList<Section> Sections { get; set; }

        public Track() { }

        public Track(string name, SectionTypes[] sections)
        {
            Name = name;
        }
        
    }
}