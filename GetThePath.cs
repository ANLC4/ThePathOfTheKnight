using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace ThePathofKnight
{
    public class GetThePath
    {
        private readonly ILogger<GetThePath> _logger;

        public GetThePath(ILogger<GetThePath> log)
        {
            _logger = log;
        }

        [FunctionName("GetThePath")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]        
        [OpenApiParameter(name: "Id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The Knight Path **Id** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            string id = req.Query["Id"];
            _logger.LogInformation($"GetThePath started with the param: Id: {id}");            

            TheKnightPathRecord result = new TheKnightPathRecord();

            if (id == null) 
            {
                _logger.LogError($"Necessary parameter Id was not found");
                return new BadRequestObjectResult("BadRequest: Enter Id value."); 
            }

            ThePathOfTheKnightData data = new ThePathOfTheKnightData();

            try
            {
                var findResult = await data.GetRecordByID(id);
                if (findResult != null)
                {
                    result = findResult;
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
                return new NotFoundObjectResult($"Not Found: Couldn't find this Id:{id}.");
            }

            return new OkObjectResult(new KnightPathResponse(result));
        }
    }
}

