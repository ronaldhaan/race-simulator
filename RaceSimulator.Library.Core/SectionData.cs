using RaceSimulator.Library.Core.Interfaces;

namespace RaceSimulator.Library.Core
{
    public class SectionData
    {
        public IParticipant Left { get; set; }

        public int DistanceLeft { get; set; }
       

        public IParticipant Right { get; set; }


        public int DistanceRight { get; set; }

        public SectionData() : this(null, 0, null, 0) { }

        public SectionData(IParticipant left, int distanceLeft, IParticipant right, int distanceRight)
        {
            Left = left;
            DistanceLeft = distanceLeft;
            Right = right;
            DistanceRight = distanceRight;
        }

    }
}
