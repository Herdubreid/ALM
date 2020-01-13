using BlazorState;
using Celin;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using System;
using System.IO;
using System.Text.Json;

namespace Test
{
    abstract class BaseCmd
    {
        protected Celin.AIS.Server E1 { get; }
        protected IMediator Mediator { get; }
        protected IStore Store { get; }
        protected ALMState State => Store.GetState<ALMState>();

        [Option("-j|--json", CommandOptionType.SingleValue, Description = "Output result to Json File")]
        protected (bool HasValue, string Value) JsonFile { get; set; }
        protected void Dump()
        {
            var state = new
            {
                State.ErrorMessage,
                State.TextAttachments,
                State.EQList
            };
            var json = JsonSerializer.Serialize(state, new JsonSerializerOptions { IgnoreNullValues = true, WriteIndented = true });
            if (JsonFile.HasValue)
            {
                var fs = File.CreateText($"{JsonFile.Value}.json");
                fs.Write(json);
                fs.Close();
            }
            else
            {
                Console.WriteLine(json);
            }
        }
        public BaseCmd(IStore store, IMediator mediator, Celin.AIS.Server e1)
        {
            Store = store;
            Mediator = mediator;
            E1 = e1;
        }
    }
}
