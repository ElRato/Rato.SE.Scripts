namespace IngameScript
{
    public class TestLowLevelStore: Program.ILowLevelStore
    {
        internal string Store;
        public string Read()
        {
            return Store;
        }

        public void Write(string value)
        {
            Store = value;
        }
    }
}