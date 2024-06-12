namespace MauiApp3
{
    internal static class Extensions
    {
        public static void UpdateSegments(this SevenSegmentIndicator sevenSegmentIndicator, BoxView topSegment, BoxView leftTopSegment,
            BoxView rightTopSegment, BoxView middleSegment, BoxView leftBottomSegment, BoxView rightBottomSegment, BoxView bottomSegment)
        {
            topSegment.IsVisible = sevenSegmentIndicator.TopSegment;
            leftTopSegment.IsVisible = sevenSegmentIndicator.LeftTopSegment;
            rightTopSegment.IsVisible = sevenSegmentIndicator.RightTopSegment;
            middleSegment.IsVisible = sevenSegmentIndicator.MiddleSegment;
            leftBottomSegment.IsVisible = sevenSegmentIndicator.LeftBottomSegment;
            rightBottomSegment.IsVisible = sevenSegmentIndicator.RightBottomSegment;
            bottomSegment.IsVisible = sevenSegmentIndicator.BottomSegment;
        }
    }
}
