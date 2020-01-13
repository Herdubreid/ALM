using BlazorState;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Celin
{
    public partial class ALMState
    {
        public class TextAttachmentHandler : ActionHandler<TextAttachmentAction>
        {
            AIS.Server E1 { get; }
            ALMState State => Store.GetState<ALMState>();
            public override async Task<Unit> Handle(TextAttachmentAction aAction, CancellationToken aCancellationToken)
            {
                try
                {
                    switch (aAction.Type)
                    {
                        case TextAttachmentAction.Types.GET:
                            var get = new GT1701<AIS.MoGetText>(aAction.NUMB.ToString(), true);
                            var ats = await E1.RequestAsync(get.Request);
                            State.TextAttachments = ats.textAttachments;
                            State.ErrorMessage = ats.error;
                            break;
                        case TextAttachmentAction.Types.ADD:
                            var add = new GT1701<AIS.MoAddText>(aAction.NUMB.ToString(), aAction.Name, aAction.Text);
                            var addRs = await E1.RequestAsync(add.Request);
                            State.ErrorMessage = addRs.error;
                            break;
                        case TextAttachmentAction.Types.EDIT:
                            var edit = new GT1701<AIS.MoUpdateText>(aAction.NUMB.ToString(), aAction.Sequence, aAction.Text);
                            var editRs = await E1.RequestAsync(edit.Request);
                            State.ErrorMessage = editRs.error;
                            break;
                    }
                }
                catch (AIS.HttpWebException e)
                {
                    State.ErrorMessage = e.ErrorResponse.message ?? e.Message;
                }
                catch (Exception e)
                {
                    State.ErrorMessage = e.Message;
                }

                var handler = State.Changed;
                handler?.Invoke(State, null);

                return Unit.Value;
            }
            public TextAttachmentHandler(IStore store, AIS.Server e1) : base(store)
            {
                E1 = e1;
            }
        }
        public class EQListHandler : ActionHandler<EQListAction>
        {
            AIS.Server E1 { get; }
            ALMState State => Store.GetState<ALMState>();
            public override async Task<Unit> Handle(EQListAction aAction, CancellationToken aCancellationToken)
            {
                State.ErrorMessage = string.Empty;
                try
                {
                    var rs = await E1.RequestAsync<W1701A.Response>(new W1701A.Request());
                    State.EQList = rs;
                }
                catch (AIS.HttpWebException e)
                {
                    State.ErrorMessage = e.ErrorResponse.message ?? e.Message;
                }
                catch (Exception e)
                {
                    State.ErrorMessage = e.Message;
                }

                var handler = State.Changed;
                handler?.Invoke(State, null);

                return Unit.Value;
            }
            public EQListHandler(IStore store, AIS.Server e1) : base(store)
            {
                E1 = e1;
            }
        }
    }
}
