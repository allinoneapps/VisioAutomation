namespace VisioAutomation.ShapeSheet.Streams
{
    public class FixedSidSrcStreamBuilder : FixedStreamBuilderBase<SidSrc>
    {
        public FixedSidSrcStreamBuilder(int capacity) : base(capacity, StreamType.SidSrc)
        {

        }

        protected override void _fill_segment_with_item(Utilities.ArraySegment<short> seg, SidSrc item)
        {
            seg[0] = item.ShapeID;
            seg[1] = item.Src.Section;
            seg[2] = item.Src.Row;
            seg[3] = item.Src.Cell;
        }
    }
}