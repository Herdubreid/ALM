using BlazorState;
using System;
using System.Collections.Generic;

namespace Celin
{
    public partial class ALMState : State<ALMState>
    {
        public event EventHandler Changed;
        public string ErrorMessage { get; set; }
        public IEnumerable<AIS.AttachmentResponse> TextAttachments { get; set; }
        public W1701A.Response EQList { get; set; }
        public override void Initialize() { }
    }
}
