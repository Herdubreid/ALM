using BlazorState;
using System;
using System.Collections.Generic;

namespace Celin
{
    public partial class AppState : State<AppState>
    {
        public event EventHandler Changed;
        public string AzureErrorMessage { get; set; }
        public string LocationErrorMessage { get; set; }
        public Address Address { get; set; }
        public IEnumerable<W1701A.Row> EQList { get; set; }
        public override void Initialize() { }
    }
}
