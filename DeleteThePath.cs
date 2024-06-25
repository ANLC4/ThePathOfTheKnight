using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace ThePathofKnight
{
    public class DeleteThePath
    {
        private readonly ILogger<DeleteThePath> _logger;

        public DeleteThePath(ILogger<DeleteThePath> log)
        {
            _logger = log;
        }

        [FunctionName("DeleteThePath")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "Id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The Knight Path **Id** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            
            string id = req.Query["Id"];
            
            _logger.LogInformation($"Delete the Path of the Knight received. Id:{id}");
            

            TheKnightPathRecord result = new TheKnightPathRecord();

            if (id == null) {
                _logger.LogError($"Necessary parameter Id was not found");
                return new BadRequestObjectResult("BadRequest: Enter Id value."); 
            }

            ThePathOfTheKnightData data = new ThePathOfTheKnightData();

            try
            {
                var findResult = await data.DeleteRecord(id);
            }
            catch(ArgumentException ex)
            {
                _logger.LogError(ex.Message);
                return new NotFoundObjectResult($"Not Found: Couldn't find this Id:{id}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new BadRequestObjectResult($"Error deleting this Id:{id}. {Environment.NewLine} {ex.Message}");
            }

            return new OkObjectResult($"Record Id:{id} deleted.");
        }
    }
}

