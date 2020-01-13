using BlazorState;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Test
{
    abstract class BaseTextAttachmentCmd : BaseCmd
    {
        [Argument(0, Description = "Equipment Number")]
        [Required]
        protected int NUMB { get; set; }
        public BaseTextAttachmentCmd(IStore store, IMediator mediator, Celin.AIS.Server e1)
            : base(store, mediator, e1) { }
    }
}
