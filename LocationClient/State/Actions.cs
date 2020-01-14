using BlazorState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Celin
{
    public partial class AppState
    {
        public class AddTextAction : IAction
        {
            public int NUMB { get; set; }
        }
        public class ErrorAction : IAction
        {
            public string AzureErrorMessage { get; set; }
            public string LocationErrorMessage { get; set; }
        }
        public class AddressAction : IAction
        {
            public Address Address { get; set; }
        }
        public class GetTextAction : IAction
        {
            public string NUMB { get; set; }
        }
        public class EQListAction : IAction { }
    }
}
