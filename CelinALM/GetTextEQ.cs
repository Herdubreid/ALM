using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Celin
{
    public static class GetTextEQ
    {
        static readonly string NAME = "io-celin-client-location";

        [FunctionName("GetTextEQ")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetTextEQ");
            string baseUrl = Environment.GetEnvironmentVariable("e1-baseUrl");
            var e1 = new AIS.Server(baseUrl, log);
            e1.AuthRequest.username = Environment.GetEnvironmentVariable("e1-username");
            e1.AuthRequest.password = Environment.GetEnvironmentVariable("e1-password");

            string numb = req.Query["numb"];
            var mo = new GT1701<AIS.MoGetText>(numb, true);
            try
            {
                var rs = await e1.RequestAsync(mo.Request);
                var t = rs.textAttachments.FirstOrDefault(a => a.itemName.Equals(NAME));
                if (t != null)
                {
                    try
                    {
                        var adds = System.Text.RegularExpressions.Regex.Unescape(t.text).Split('\n')
                            .Select(s => JsonSerializer.Deserialize<Address>(s))
                            .ToList();
                        Address last = null;
                        foreach (var l in adds)
                        {
                            if (last != null)
                            {
                                l.distance = calculate(last.latitude, last.longitude, l.latitude, l.longitude, 'K');
                            }
                            last = l;
                        }
                        return new OkObjectResult(new { t.sequence, adds });
                    }
                    catch (Exception) { }
                }
                return new OkObjectResult(null);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
        static double calculate(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            if ((lat1 == lat2) && (lon1 == lon2))
            {
                return 0;
            }
            else
            {
                double theta = lon1 - lon2;
                double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
                dist = Math.Acos(dist);
                dist = rad2deg(dist);
                dist = dist * 60 * 1.1515;
                if (unit == 'K')
                {
                    dist = dist * 1.609344;
                }
                else if (unit == 'N')
                {
                    dist = dist * 0.8684;
                }
                return (dist);
            }
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private static double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}
