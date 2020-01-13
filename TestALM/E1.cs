using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Test
{
    class E1 : Celin.AIS.Server
    {
        public E1(IConfiguration config, ILogger logger, HttpClient http)
            : base(config["baseUrl"], logger, http)
        {
            AuthRequest.username = config["username"];
            AuthRequest.password = config["password"];
        }
    }
}
