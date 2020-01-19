using BlazorState;
using MediatR;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Celin
{
    public partial class ALMState
    {
        protected static readonly string NAME = "io-celin-client-location";
        public class RefreshAddressHandler : ActionHandler<RefreshLocationAction>
        {
            AIS.Server E1 { get; }
            ALMState State => Store.GetState<ALMState>();
            public override async Task<Unit> Handle(RefreshLocationAction aAction, CancellationToken aCancellationToken)
            {
                State.ErrorMessage = string.Empty;
                try
                {
                    var mo = new GT1701<AIS.MoGetText>(aAction.NUMB.ToString(), true);
                    var mots = await E1.RequestAsync(mo.Request);
                    var t = mots.textAttachments.FirstOrDefault(a => a.itemName.Equals(NAME));
                    if (t != null)
                    {
                        var row = State.EQList.First(r => r.z_NUMB_377.Equals(aAction.NUMB));
                        row.Locations = Regex.Unescape(t.text).Split('\n')
                            .Select(s => JsonSerializer.Deserialize<Address>(s))
                            .ToList();
                    }
                }
                catch (Exception) { }

                return Unit.Value;
            }
            public RefreshAddressHandler(IStore store, AIS.Server e1) : base(store)
            {
                E1 = e1;
            }
        }
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
                    State.EQList = rs.fs_P1701_W1701A.data.gridData.rowset;
                    foreach (var r in State.EQList)
                    {
                        var mo = new GT1701<AIS.MoGetText>(r.z_NUMB_377.ToString(), true);
                        var mots = await E1.RequestAsync(mo.Request);
                        var t = mots.textAttachments.FirstOrDefault(a => a.itemName.Equals(NAME));
                        if (t != null)
                        {
                            try
                            {
                                r.Locations = Regex.Unescape(t.text).Split('\n')
                                    .Select(s => JsonSerializer.Deserialize<Address>(s))
                                    .ToList();
                            }
                            catch (Exception) { }
                        }
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
            public EQListHandler(IStore store, AIS.Server e1) : base(store)
            {
                E1 = e1;
            }
        }
    }
}
