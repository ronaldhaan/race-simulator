using RaceSimulator.Library.Core.Enumerations;

using System.Collections.Generic;

namespace RaceSimulator.Library.Core
{
    public class Track
    {
        public string Name { get; set; }

        public LinkedList<Section> Sections { get; set; }

        public Track(string name, SectionTypes[] sectionTypes)
        {
            Name = name;
            Sections = CreateSectionsFromTypes(sectionTypes);
        }

        private LinkedList<Section> CreateSectionsFromTypes(SectionTypes[] sectionTypes)
        {
            LinkedList<Section> sections = new LinkedList<Section>();

            foreach(SectionTypes sectionType in sectionTypes)
            { 
                sections.AddLast(new Section(sectionType));
            }

            return sections;
        }
        
    }
}