namespace MauiApp3
{
    internal class SevenSegmentIndicator
    {
        public bool TopSegment { get; private set; }
        public bool LeftTopSegment { get; private set; }
        public bool RightTopSegment { get; private set; }
        public bool MiddleSegment { get; private set; }
        public bool LeftBottomSegment { get; private set; }
        public bool RightBottomSegment { get; private set; }
        public bool BottomSegment { get; private set; }

        public void ChangeSegmentsVisibilityByNumber(int digit)
        {
            if (digit < 0 || digit > 9)
                throw new ArgumentOutOfRangeException($"SevenSegmentIndicator.ChangeSegmentsVisibilityByNumber({digit})");

            switch (digit)
            {
                default:
                    break;
                case 0:
                    TopSegment = true;
                    LeftTopSegment = true;
                    RightTopSegment = true;
                    MiddleSegment = false;
                    LeftBottomSegment = true;
                    RightBottomSegment = true;
                    BottomSegment = true;
                    break;
                case 1:
                    TopSegment = false;
                    LeftTopSegment = false;
                    RightTopSegment = true;
                    MiddleSegment = false;
                    LeftBottomSegment = false;
                    RightBottomSegment = true;
                    BottomSegment = false;
                    break;
                case 2:
                    TopSegment = true;
                    LeftTopSegment = false;
                    RightTopSegment = true;
                    MiddleSegment = true;
                    LeftBottomSegment = true;
                    RightBottomSegment = false;
                    BottomSegment = true;
                    break;
                case 3:
                    TopSegment = true;
                    LeftTopSegment = false;
                    RightTopSegment = true;
                    MiddleSegment = true;
                    LeftBottomSegment = false;
                    RightBottomSegment = true;
                    BottomSegment = true;
                    break;
                case 4:
                    TopSegment = false;
                    LeftTopSegment = true;
                    RightTopSegment = true;
                    MiddleSegment = true;
                    LeftBottomSegment = false;
                    RightBottomSegment = true;
                    BottomSegment = false;
                    break;
                case 5:
                    TopSegment = true;
                    LeftTopSegment = true;
                    RightTopSegment = false;
                    MiddleSegment = true;
                    LeftBottomSegment = false;
                    RightBottomSegment = true;
                    BottomSegment = true;
                    break;
                case 6:
                    TopSegment = true;
                    LeftTopSegment = true;
                    RightTopSegment = false;
                    MiddleSegment = true;
                    LeftBottomSegment = true;
                    RightBottomSegment = true;
                    BottomSegment = true;
                    break;
                case 7:
                    TopSegment = true;
                    LeftTopSegment = false;
                    RightTopSegment = true;
                    MiddleSegment = false;
                    LeftBottomSegment = false;
                    RightBottomSegment = true;
                    BottomSegment = false;
                    break;
                case 8:
                    TopSegment = true;
                    LeftTopSegment = true;
                    RightTopSegment = true;
                    MiddleSegment = true;
                    LeftBottomSegment = true;
                    RightBottomSegment = true;
                    BottomSegment = true;
                    break;
                case 9:
                    TopSegment = true;
                    LeftTopSegment = true;
                    RightTopSegment = true;
                    MiddleSegment = true;
                    LeftBottomSegment = false;
                    RightBottomSegment = true;
                    BottomSegment = true;
                    break;
            }
        }
    }
}