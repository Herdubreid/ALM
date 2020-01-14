using BlazorState;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Celin
{
    public partial class AppState
    {
        static readonly string NAME = "io-celin-client-location";
        public class AddTextHandler : ActionHandler<AddTextAction>
        {
            HttpClient Http { get; }
            AppState State => Store.GetState<AppState>();
            public override async Task<Unit> Handle(AddTextAction aAction, CancellationToken aCancellationToken)
            {
                var uri = new UriBuilder("https", "celinalm.azurewebsites.net");
                var row = State.EQList.First(r => r.z_NUMB_377.Equals(aAction.NUMB));
                var collection = System.Web.HttpUtility.ParseQueryString(string.Empty);
                collection.Add("numb", aAction.NUMB.ToString());
                collection.Add("text", JsonSerializer.Serialize(State.Address));
                if (row.LocationTextSeq == null)
                {
                    uri.Path = "api/AddTextEQ";
                    collection.Add("code", "N9WHu80FoeHVWGe0QDUXsh2ko1yreNoFqfnIgi5lY7WnZ2OenYDN1Q==");
                    collection.Add("name", NAME);
                    uri.Query = collection.ToString();
                }
                else
                {
                    uri.Path = "api/EditTextEQ";
                    collection.Add("code", "KATvnwjVhNiF9awyo2tiMqUmb4yoczR1mskiEEVC7DagtgaBBktxeA==");
                    collection.Add("seq", row.LocationTextSeq.ToString());
                    uri.Query = collection.ToString();
                }
                var response = await Http.GetAsync(uri.Uri);

                var handler = State.Changed;
                handler?.Invoke(State, null);

                return Unit.Value;
            }
            public AddTextHandler(IStore store, HttpClient http) : base(store)
            {
                Http = http;
            }
        }
        public class ErrorHandler : ActionHandler<ErrorAction>
        {
            AppState State => Store.GetState<AppState>();
            public override Task<Unit> Handle(ErrorAction aAction, CancellationToken aCancellationToken)
            {
                State.AzureErrorMessage = aAction.AzureErrorMessage;
                State.LocationErrorMessage = aAction.LocationErrorMessage;

                var handler = State.Changed;
                handler?.Invoke(State, null);

                return Unit.Task;
            }
            public ErrorHandler(IStore store) : base(store) { }
        }
        public class AddressHandler : ActionHandler<AddressAction>
        {
            AppState State => Store.GetState<AppState>();
            public override Task<Unit> Handle(AddressAction aAction, CancellationToken aCancellationToken)
            {
                State.Address = aAction.Address;
                State.Address.timestamp = DateTime.Now;
                State.LocationErrorMessage = string.Empty;

                var handler = State.Changed;
                handler?.Invoke(this, null);

                return Unit.Task;
            }
            public AddressHandler(IStore store) : base(store) { }
        }
        public class GetTextHandler : ActionHandler<GetTextAction>
        {
            class Response
            {
                public int sequence { get; set; }
                public IEnumerable<Address> adds { get; set; }
            }

            HttpClient Http { get; }
            AppState State => Store.GetState<AppState>();
            public override async Task<Unit> Handle(GetTextAction aAction, CancellationToken aCancellationToken)
            {
                var uri = new UriBuilder("https://celinalm.azurewebsites.net/api/GetTextEQ");
                var collection = System.Web.HttpUtility.ParseQueryString(string.Empty);
                collection.Add("code", "x8MfDXaLAsUE/GUs9kF2qOEJ60WaR2tMCBkD9XX7R5hgfmuoL9Oiag==");
                collection.Add("numb", aAction.NUMB);
                uri.Query = collection.ToString();
                var response = await Http.GetAsync(uri.Uri);
                if (response.IsSuccessStatusCode)
                {
                    var row = State.EQList.First(r => r.z_NUMB_377.ToString().Equals(aAction.NUMB));
                    try
                    {
                        var des = JsonSerializer.Deserialize<Response>(response.Content.ReadAsStringAsync().Result);
                        row.LocationTextSeq = des.sequence;
                        row.Locations = des.adds;
                    }
                    catch (Exception)
                    {
                        row.Locations = Enumerable.Empty<Address>();
                    }
                }

                return Unit.Value;
            }
            public GetTextHandler(IStore store, HttpClient http) : base(store)
            {
                Http = http;
            }
        }
        public class EQListHandler : ActionHandler<EQListAction>
        {
            HttpClient Http { get; }
            AppState State => Store.GetState<AppState>();
            public override async Task<Unit> Handle(EQListAction aAction, CancellationToken aCancellationToken)
            {
                var uri = new UriBuilder("https://celinalm.azurewebsites.net/api/EQListing");
                var collection = System.Web.HttpUtility.ParseQueryString(string.Empty);
                collection.Add("code", "3Uys2020MB0Y2QncCaXtscAecTSgZ6IjVci/0WHsVCYCOG/Maaebaw==");
                uri.Query = collection.ToString();
                var response = await Http.GetAsync(uri.Uri);
                if (response.IsSuccessStatusCode)
                {
                    State.EQList = JsonSerializer.Deserialize<IEnumerable<Celin.W1701A.Row>>(response.Content.ReadAsStringAsync().Result);
                }

                return Unit.Value;
            }
            public EQListHandler(IStore store, HttpClient http) : base(store)
            {
                Http = http;
            }
        }
    }
}
