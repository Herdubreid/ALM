using BlazorState;

namespace Celin
{
    public partial class ALMState
    {
        public class RefreshLocationAction : IAction
        {
            public int NUMB { get; set; }
        }
        public class TextAttachmentAction : IAction
        {
            public enum Types
            {
                GET, ADD, EDIT
            }
            public Types Type { get; set; }
            public int Sequence { get; set; }
            public int NUMB { get; set; }
            public string Name { get; set; }
            public string Text { get; set; }
        }
        public class EQListAction : IAction { }
    }
}
