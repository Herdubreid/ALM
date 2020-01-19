using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace LocationServer
{
    class E1Service : Celin.AIS.Server
    {
        public E1Service(IConfiguration config, ILogger<E1Service> logger, IHttpClientFactory httpFactory)
            : base(config["e1-baseUrl"], logger, httpFactory.CreateClient())
        {
            AuthRequest.username = config["e1-username"];
            AuthRequest.password = config["e1-password"];
        }
    }
}
