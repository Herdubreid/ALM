using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Celin
{
    public static class EditTextEQ
    {
        [FunctionName("EditTextEQ")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("EditTextEQ");
            string baseUrl = Environment.GetEnvironmentVariable("e1-baseUrl");
            var e1 = new AIS.Server(baseUrl, log);
            e1.AuthRequest.username = Environment.GetEnvironmentVariable("e1-username");
            e1.AuthRequest.password = Environment.GetEnvironmentVariable("e1-password");

            string numb = req.Query["numb"];
            int seq = int.Parse(req.Query["seq"]);
            string text = req.Query["text"];
            var mo = new GT1701<AIS.MoUpdateText>(numb, seq, text);
            try
            {
                var rs = await e1.RequestAsync(mo.Request);
                return new OkObjectResult(rs);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
