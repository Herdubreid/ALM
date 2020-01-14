using MediatR;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Celin
{
    public class Coordinate
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
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
        HttpClient Http { get; }
        IMediator Mediator { get; }
        IJSRuntime JS { get; }
        string BingKey { get; set; }
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
            Mediator.Send(new AppState.ErrorAction { LocationErrorMessage = error });
        }
        [JSInvokable]
        public async Task GeoLocation(Coordinate coordinate)
        {
            var uri = new UriBuilder("https://celinalm.azurewebsites.net/api/GetAddress");
            var collection = System.Web.HttpUtility.ParseQueryString(string.Empty);
            collection.Add("code", "4SH1LqpnD15PSYTPkWse2/SeUAKcod2IHgngZiBVmGQbbglcgqh3fA==");
            collection.Add("latitude", coordinate.latitude.ToString());
            collection.Add("longitude", coordinate.longitude.ToString());
            uri.Query = collection.ToString();
            var response = await Http.GetAsync(uri.Uri);
            if (response.IsSuccessStatusCode)
            {
                var address = JsonSerializer.Deserialize<Address>(response.Content.ReadAsStringAsync().Result);
                address.latitude = coordinate.latitude;
                address.longitude = coordinate.longitude;
                await Mediator.Send(new AppState.AddressAction { Address = address });
            }
            else
            {
                await Mediator.Send(new AppState.ErrorAction { LocationErrorMessage = response.Content.ReadAsStringAsync().Result });
            }
        }
        public void LoadCoords()
        {
            Mediator.Send(new AppState.AddressAction());
            JS.InvokeAsync<object>("GeoLocation", Ref);
        }
        public void LoadMap()
        {
            Mediator.Send(new AppState.AddressAction());
            JS.InvokeAsync<object>("LoadMap", BingKey, Ref);
        }
        public LocationService(IMediator mediator, IJSRuntime jsRuntime, HttpClient http)
        {
            Http = http;
            Mediator = mediator;
            BingKey = "AmvRew_WUlWMYhAxgFXNTg9htdkTNc4N_reyiMtIZSZgOTAjuKWKzhrV-H6rjOgw";
            JS = jsRuntime;
            Ref = DotNetObjectReference.Create(this);
        }
    }
}
