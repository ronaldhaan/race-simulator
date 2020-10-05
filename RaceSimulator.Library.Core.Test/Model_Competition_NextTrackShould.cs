using NUnit.Framework;

using RaceSimulator.Library.Core.Enumerations;

using System.Collections.Generic;

namespace RaceSimulator.Library.Core.Test
{
    [TestFixture]
    public class Model_Competition_NextTrackShould
    {
        private Competition competition;

        [SetUp]
        public void Setup()
        {
            competition = new Competition(new List<Interfaces.IParticipant>(), new Queue<Track>());
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            Track result = competition.NextTrack();

            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            Track track = new Track("TestTrack", new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.Straight });

            competition.Tracks.Enqueue(track);

            Assert.AreEqual(track, competition.NextTrack());
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            Track track = new Track("TestTrack", new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.Straight });

            competition.Tracks.Enqueue(track);

            Track result = competition.NextTrack();
            result = competition.NextTrack();

            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            Track testTrack = new Track("TestTrack2", new SectionTypes[] { SectionTypes.LeftCorner, SectionTypes.RightCorner });
            Track track = new Track("TestTrack", new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.Straight });

            competition.Tracks.Enqueue(testTrack);
            competition.Tracks.Enqueue(track);

            Assert.AreEqual(testTrack, competition.NextTrack());
        }
    }
}