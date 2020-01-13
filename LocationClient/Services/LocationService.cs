using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using BlazorState;
using MediatR;
using System;

namespace Celin
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
    public class LocationService
    {
        IMediator Mediator { get; }
        IJSRuntime JS { get; }
        string BingKey { get; }
        public static object Lock = new object();
        public DotNetObjectReference<LocationService> Ref { get; }
        [JSInvokable]
        public void Location(Address address)
        {
            Mediator.Send(new AppState.AddressAction { Address = address });
        }
        [JSInvokable]
        public void Error(string error)
        {
            Mediator.Send(new AppState.ErrorAction { ErrorMessage = error });
        }
        public void LoadMap()
        {
            Mediator.Send(new AppState.AddressAction());
            JS.InvokeAsync<object>("LoadMap", BingKey, Ref);
        }
        public LocationService(IMediator mediator, IJSRuntime jsRuntime)
        {
            Mediator = mediator;
            BingKey = "AmvRew_WUlWMYhAxgFXNTg9htdkTNc4N_reyiMtIZSZgOTAjuKWKzhrV-H6rjOgw";
            JS = jsRuntime;
            Ref = DotNetObjectReference.Create(this);
        }
    }
}
