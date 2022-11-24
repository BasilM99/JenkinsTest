namespace InsertIPsBatch
{
    public class IPRange
    {
        public long? StartRange { get; set; }
        public long? EndRange { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return string.Format("{0}-{1}", StartRange, EndRange);
        }
    }
}