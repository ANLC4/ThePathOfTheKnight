using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePathofKnight
{
    public class KnightPathResponse
    {
        public string Starting { get; set; }
        public string Ending { get; set; }
        public string ShortestPath { get; set; }
        public int? NumberOfMoves { get; set; }        
        public string OperationId { get; set; }

        public KnightPathResponse()
        {
            
        }

        public KnightPathResponse(TheKnightPathRecord record)
        {
            Starting = record.Starting;
            Ending = record.Ending;
            ShortestPath = record.ShortestPath;
            NumberOfMoves = record.NumberOfMoves;
            OperationId = record.OperationId;
        }        
    }
}
