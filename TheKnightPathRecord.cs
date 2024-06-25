using Newtonsoft.Json;
using System;

namespace ThePathofKnight
{
    public class TheKnightPathRecord
    {
        public string Starting { get; set; }
        public string Ending { get; set; }
        public string ShortestPath { get; set; }
        public int? NumberOfMoves { get; set; }
        public Boolean IsFound { get; set; }
        public string OperationId { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public DateTime? MinimumRetentionDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string CosmosDbResourceId { get; set; }
        public string ComsosDbSelfReference { get; set; }
        public string CosmosDbEtag { get; set; }
        public string CosmosDbAttachmentsReference { get; set; }
        public int? CosmosDbTimestamp { get; set; }
    }
}
