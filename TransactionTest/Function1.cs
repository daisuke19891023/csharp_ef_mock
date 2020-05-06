using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace TransactionTest
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string id = req.Query["id"];
            string name = req.Query["name"];
            string age = req.Query["age"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //id = id ?? data?.id;

            //if (id != null)
            //{
                var context = new UserDbContext();
                var dataLayer = new DataLayer(context);
                dataLayer.UsingUpsertData(int.Parse(id), name, int.Parse(age));
                var items = dataLayer.GetData(int.Parse(id));
                var res = JsonConvert.SerializeObject(items);
                return new OkObjectResult(res);
            //}
            //else
            //{
            //    new BadRequestObjectResult("Please pass a name on the query string or in the request body");
            //}
     }
    }
}
