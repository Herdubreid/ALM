using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Celin
{
    public static class EQListing
    {
        [FunctionName("EQListing")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("EQ Listing.");
            string baseUrl = Environment.GetEnvironmentVariable("e1-baseUrl");
            var e1 = new AIS.Server(baseUrl, log);
            e1.AuthRequest.username = Environment.GetEnvironmentVariable("e1-username");
            e1.AuthRequest.password = Environment.GetEnvironmentVariable("e1-password");

            try
            {
                var list = await e1.RequestAsync<W1701A.Response>(new W1701A.Request());
                return new OkObjectResult(list.fs_P1701_W1701A.data.gridData.rowset);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
