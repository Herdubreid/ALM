namespace Celin
{
    public class GT1701<T>
        where T : AIS.MoRequest, new()
    {
        public T Request { get; }
        public GT1701(string NUMB, int sequence, string text, bool append = true)
            : this(NUMB)
        {
            Request.sequence = sequence;
            Request.inputText = text;
            Request.appendText = append;
        }
        public GT1701(string NUMB, string name, string text) : this(NUMB)
        {
            Request.itemName = name;
            Request.inputText = text;
        }
        public GT1701(string NUMB, bool mmode = false) : this()
        {
            Request.multipleMode = mmode;
            Request.moKey = new string[] { NUMB };
        }
        public GT1701()
        {
            Request = new T()
            {
                formName = "P1701_W1701A",
                moStructure = "GT1701",
            };
        }
    }
}
