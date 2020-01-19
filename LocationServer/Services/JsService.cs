using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using System;

namespace Celin
{
    public class Coordinate
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
    public class Marker
    {
        public int numb { get; set; }
        public bool visible { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string title { get; set; }
        public string address { get; set; }
    }
    public class JsService
    {
        IMediator Mediator { get; }
        IJSRuntime JS { get; }
        string MapToken { get; set; }
        public DotNetObjectReference<JsService> Ref { get; }
        [JSInvokable]
        public void GeoLocation(Coordinate coordinate)
        {
        }
        public void Zoom(int zoom, double[] center)
        {
            JS.InvokeVoidAsync("window.map.zoom", zoom, center);
        }
        public void MoveMarker(int numb, double lat, double lon)
        {
            JS.InvokeVoidAsync("window.map.move", numb, lat, lon);
        }
        public void AddMarker(Marker marker)
        {
            JS.InvokeVoidAsync("window.map.addMarker", marker);
        }
        public void LoadMap(string id)
        {
            JS.InvokeVoidAsync("window.map.init", id, MapToken);
        }
        public JsService(IConfiguration config, IMediator mediator, IJSRuntime jsRuntime)
        {
            Mediator = mediator;
            MapToken = config["mapToken"];
            JS = jsRuntime;
            Ref = DotNetObjectReference.Create(this);
        }
    }
}
