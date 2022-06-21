using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AddProdcttoDb
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post",
            Route = null)] HttpRequest req,
            ILogger log,
            [Sql("Products",ConnectionStringSetting ="SqlConnString")]
                out Product newProduct)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            newProduct = JsonConvert.DeserializeObject<Product>(requestBody);
            return new OkObjectResult(newProduct);
        }
    }
}