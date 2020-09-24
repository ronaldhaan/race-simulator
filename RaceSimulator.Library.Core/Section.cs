using RaceSimulator.Library.Core.Enumerations;

namespace RaceSimulator.Library.Core
{
    public class Section
    {
        public SectionTypes SectionType { get; set; }

        public Section() { }

        public Section(SectionTypes sectionTypes)
        {
            SectionType = sectionTypes;
        }

    }
}