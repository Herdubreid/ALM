using BlazorState;
using Celin;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Test
{
    public class Address
    {
        public string addressLine { get; set; }
        public string adminDistrict { get; set; }
        public string countryRegion { get; set; }
        public string formattedAddress { get; set; }
        public string locality { get; set; }
        public string postalCode { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public DateTime? timestamp { get; set; }
        public double? distance { get; set; }
    }
    class Attachment
    {
        public int sequence { get; set; }
        public string name { get; set; }
        public string text { get; set; }
    }
    [Subcommand(typeof(AzureTextAttachmentCmd))]
    [Subcommand(typeof(TextAttachmentCmd))]
    [Subcommand(typeof(EQListCmd))]
    class Cmd
    {
        [Command("azta", Description = "Azure Text Attachments")]
        [Subcommand(typeof(Add))]
        [Subcommand(typeof(Get))]
        class AzureTextAttachmentCmd
        {
            [Command("add", Description = "Add Text")]
            class Add : BaseTextAttachmentCmd
            {
                [Argument(1, Description = "Text")]
                [Required]
                string Text { get; set; }
                HttpClient Http { get; }
                async Task OnExecuteAsync()
                {
                    var uri = new UriBuilder("https", "celinalm.azurewebsites.net");
                    var collection = System.Web.HttpUtility.ParseQueryString(string.Empty);
                    collection.Add("numb", NUMB.ToString());
                    collection.Add("text", Text);
                    uri.Path = "api/AddTextEQ";
                    collection.Add("code", "N9WHu80FoeHVWGe0QDUXsh2ko1yreNoFqfnIgi5lY7WnZ2OenYDN1Q==");
                    uri.Query = collection.ToString();
                    var response = await Http.GetAsync(uri.Uri);
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                }
                public Add(IStore store, IMediator mediator, Celin.AIS.Server server, HttpClient http)
                    : base(store, mediator, server)
                {
                    Http = http;
                }
            }
            [Command("get", Description = "Get Text")]
            class Get : BaseTextAttachmentCmd
            {
                class Response
                {
                    public int sequence { get; set; }
                    public IEnumerable<Address> adds { get; set; }
                }
                HttpClient Http { get; }
                async Task OnExecuteAsync()
                {
                    var uri = new UriBuilder("https://celinalm.azurewebsites.net/api/GetTextEQ");
                    var collection = System.Web.HttpUtility.ParseQueryString(string.Empty);
                    collection.Add("code", "x8MfDXaLAsUE/GUs9kF2qOEJ60WaR2tMCBkD9XX7R5hgfmuoL9Oiag==");
                    collection.Add("numb", NUMB.ToString());
                    uri.Query = collection.ToString();
                    var response = await Http.GetAsync(uri.Uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var res = response.Content.ReadAsStringAsync().Result;
                        try
                        {
                            var des = JsonSerializer.Deserialize<Response>(res);
                            Console.WriteLine("Sequence {0}", des.sequence);
                            var addresses = des.adds ?? Enumerable.Empty<Address>();
                            DateTime? last = null;
                            foreach (var a in des.adds)
                            {
                                TimeSpan? duration = last == null
                                    ? null
                                    : a.timestamp - last;
                                Console.WriteLine("{0} ({1}) {2} {3}", a.formattedAddress, duration?.ToString(@"hh\:mm"), a.timestamp, a.distance);
                                last = a.timestamp;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
                public Get(IStore store, IMediator mediator, Celin.AIS.Server e1, HttpClient http)
                    : base(store, mediator, e1)
                {
                    Http = http;
                }
            }
        }
        [Command("ta", Description = "Text Attachment")]
        [Subcommand(typeof(Edit))]
        [Subcommand(typeof(Add))]
        [Subcommand(typeof(Get))]
        class TextAttachmentCmd
        {
            [Command("get", Description = "Get")]
            class Get : BaseTextAttachmentCmd
            {
                async Task OnExecuteAsync()
                {
                    await Mediator.Send(new ALMState.TextAttachmentAction
                    {
                        Type = ALMState.TextAttachmentAction.Types.GET,
                        NUMB = NUMB
                    });

                    Dump();
                }
                public Get(IStore store, IMediator mediator, Celin.AIS.Server e1)
                    : base(store, mediator, e1) { }
            }
            [Command("add", Description = "Add")]
            class Add : BaseTextAttachmentCmd
            {
                [Argument(1, Description = "Text")]
                [Required]
                string Text { get; set; }
                [Argument(2, Description = "Name (optional)")]
                string Name { get; set; }
                async Task OnExecuteAsync()
                {
                    await Mediator.Send(new ALMState.TextAttachmentAction
                    {
                        Type = ALMState.TextAttachmentAction.Types.ADD,
                        NUMB = NUMB,
                        Name = Name,
                        Text = Text
                    });

                    Dump();
                }
                public Add(IStore store, IMediator mediator, Celin.AIS.Server e1)
                    : base(store, mediator, e1) { }
            }
            [Command("edit", Description = "Edit")]
            class Edit : BaseTextAttachmentCmd
            {
                [Argument(1, Description = "Sequence")]
                [Required]
                int Sequence { get; set; }
                [Argument(2, Description = "Text")]
                [Required]
                string Text { get; set; }
                async Task OnExecuteAsync()
                {
                    await Mediator.Send(new ALMState.TextAttachmentAction
                    {
                        Type = ALMState.TextAttachmentAction.Types.EDIT,
                        NUMB = NUMB,
                        Sequence = Sequence,
                        Text = Text
                    });

                    Dump();
                }
                public Edit(IStore store, IMediator mediator, Celin.AIS.Server e1)
                    : base(store, mediator, e1) { }
            }
        }
        [Command("list", Description = "List Equipments")]
        class EQListCmd : BaseCmd
        {
            async Task OnExecuteAsync()
            {
                await Mediator.Send(new ALMState.EQListAction());

                Dump();

            }
            public EQListCmd(IStore store, IMediator mediator, Celin.AIS.Server e1)
                : base(store, mediator, e1) { }
        }
    }
}
