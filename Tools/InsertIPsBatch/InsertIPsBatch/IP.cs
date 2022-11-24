namespace InsertIPsBatch
{
    public class IP
    {
        private string _value;
        private long _valueInt;
        public IP(string value)
        {
            Value = value;
        }
        public string Value { get { return _value; } set { _value = value; ValueInt = Helper.ConvertIPToNumber(_value); } }
        public long ValueInt { get; set; }

        public override string ToString()
        {
            return string.Format("{0}-{1}", Value, ValueInt);
        }
    }
}