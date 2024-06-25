using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace ThePathofKnight
{
    public class FindThePath
    {
        private readonly ILogger<FindThePath> _logger;

        private readonly IThePathOfTheKnight _path;

        public FindThePath(ILogger<FindThePath> log, IThePathOfTheKnight path)
        {
            _logger = log;
            _path = path;
        }

        [FunctionName("FindThePath")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiParameter(name: "Start", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The Knight Path **Start** position")]
        [OpenApiParameter(name: "End", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The Knight Path **End** position")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The Path Id")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation($"FindThePath started with the params: Start:{req.Query["Start"]}/End: {req.Query["end"]}.");

            string start = req.Query["Start"];
            string end = req.Query["End"];            

            TheKnightPathRecord result = new TheKnightPathRecord();

            if (start == null || end == null) { return new BadRequestObjectResult("BadRequest: Enter Start and End values"); }

            ThePathOfTheKnightData data = new ThePathOfTheKnightData();

            try
            {
                var st1 = Convert.ToInt32(req.Query["Start"].ToString().Split(',').ToList()[0]);
                var st2 = Convert.ToInt32(req.Query["Start"].ToString().Split(',').ToList()[1]);
                var end1 = Convert.ToInt32(req.Query["End"].ToString().Split(',').ToList()[0]);
                var end2 = Convert.ToInt32(req.Query["End"].ToString().Split(',').ToList()[1]);

                var findResult = await data.GetRecordByCoordenates((st1, st2), (end1, end2));
                if (findResult != null)
                {
                    result = findResult;
                }
                else
                {
                    findResult = await _path.FindShortest((st1, st2), (end1, end2));
                    result = await data.AddRecord(findResult);
                }                
            }
            catch(IndexOutOfRangeException ex)
            {      
                _logger.LogError(ex.Message);
                return new BadRequestObjectResult($"BadRequest: [Start:{start}] Value is out of the Chessboard");
            }
            catch(CosmosException ex)
            {
                _logger.LogError(ex.Message);
                return new BadRequestObjectResult($"BadRequest: Couldn't save to the Database, contact support.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.IsFound = false;                
            }           

            if (!result.IsFound) 
            {
                _logger.LogError($"Path of the Knight not found [Start:{start}] and [End:{end}]");
                return new BadRequestObjectResult($"BadRequest: Entered values [Start:{start}] and [End:{end}] must be on the Chessboard and must be in this format Start=1,2&End=6,7");
            }                       

            return new OkObjectResult($"Please use this Id to search for the results: {result.Id}");
        }
    }
}

