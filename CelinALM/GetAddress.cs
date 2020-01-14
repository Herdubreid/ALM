using BingMapsRESTToolkit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Celin
{
    public static class GetAddress
    {
        [FunctionName("GetAddress")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetAddress");

            string slat = req.Query["latitude"];
            string slon = req.Query["longitude"];
            if (double.TryParse(slat, out var lat) && double.TryParse(slon, out var lon))
            {
                ReverseGeocodeRequest rq = new ReverseGeocodeRequest();
                rq.BingMapsKey = "AmvRew_WUlWMYhAxgFXNTg9htdkTNc4N_reyiMtIZSZgOTAjuKWKzhrV-H6rjOgw";
                rq.Point = new Coordinate(lat, lon);
                var response = await rq.Execute();
                if (response.StatusCode == 200)
                {
                    var loc = response.ResourceSets[0].Resources[0] as Location;
                    return new OkObjectResult(loc.Address);
                }
                else
                {
                    return new BadRequestObjectResult(response.StatusDescription);
                }
            }

            return new BadRequestObjectResult($"Invalid coordinates {slat}, {slon}!");
        }
    }
}