using NUnit.Framework;

using RaceSimulator.Library.Controller;
using RaceSimulator.Library.Core.Enumerations;
using RaceSimulator.Library.Core.Interfaces;

using System.Collections.Generic;

namespace RaceSimulator.Library.Core.Test
{
    class Model_Race_GetSectionDataShould
    {
        private Race _race;
        private Track _track;

        [SetUp]
        public void SetUp()
        {
            _track = new Track("hoi", new SectionTypes[] { SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.RightCorner });
            _race = new Race(_track, new List<IParticipant>());
        }

        [Test]
        public void GetSectionData_Section_Null()
        { 
            SectionData result = _race.GetSectionData(null);

            Assert.IsNull(result);
        }

        [Test]
        public void GetSectionData_NewSection_ReturnNewSectionData()
        {
            SectionData expected = new SectionData(null, 0, null, 0);
            Section section = new Section(SectionTypes.LeftCorner);

            SectionData result = _race.GetSectionData(section);

            Assert.AreEqual(expected.Left, result.Left);
            Assert.AreEqual(expected.Right, result.Right);
        }
    }
}
