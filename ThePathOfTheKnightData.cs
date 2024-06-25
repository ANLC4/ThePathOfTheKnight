using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePathofKnight
{
    public class ThePathOfTheKnightData
    {
        private CosmosClient _client;
        private Database _db;
        private Container _ct;

        public ThePathOfTheKnightData()
        {
            _client = new CosmosClient(connectionString: Environment.GetEnvironmentVariable($"CosmosConnStr"));
            _db = _client.GetDatabase(Environment.GetEnvironmentVariable($"CosmosDatabase"));
            _ct = _db.GetContainer(Environment.GetEnvironmentVariable($"CosmosContainer"));
        }

        public async Task<TheKnightPathRecord> AddRecord(TheKnightPathRecord record)
        {           
            TheKnightPathRecord pr = await _ct.UpsertItemAsync<TheKnightPathRecord>(item: record);
            return pr;
        }

        public async Task<TheKnightPathRecord> GetRecordByCoordenates((int,int) Start, (int,int) End)
        {
            using FeedIterator<TheKnightPathRecord> feed = _ct.GetItemQueryIterator<TheKnightPathRecord>(
                queryText: $"SELECT * FROM {Environment.GetEnvironmentVariable($"CosmosContainer")} p WHERE p.Starting='{Start.ToString()}' and p.Ending='{End.ToString()}' and p.IsDeleted=false"
            );

            // Iterate query result pages
            while (feed.HasMoreResults)
            {
                FeedResponse<TheKnightPathRecord> response = await feed.ReadNextAsync();

                // Iterate query results
                foreach (TheKnightPathRecord item in response)
                {
                        return item; 
                }
            }

            return null;
        }

        public async Task<TheKnightPathRecord> GetRecordByID(string Id)
        {
            using FeedIterator<TheKnightPathRecord> feed = _ct.GetItemQueryIterator<TheKnightPathRecord>(
                queryText: $"SELECT * FROM theknight1 p WHERE p.id='{Id}' and p.IsDeleted=false"
            );

            // Iterate query result pages
            while (feed.HasMoreResults)
            {
                FeedResponse<TheKnightPathRecord> response = await feed.ReadNextAsync();

                // Iterate query results
                foreach (TheKnightPathRecord item in response)
                {
                    return item;
                }
            }

            return null;
        }

        public async Task<bool> DeleteRecord(string Id)
        {
            TheKnightPathRecord record = await GetRecordByID(Id);
            if (record == null)
                throw new ArgumentException("Couldn't find the record.");

            record.IsDeleted = true;
            record.DeletedOn = DateTime.Now;

            TheKnightPathRecord replacedItem = await _ct.ReplaceItemAsync<TheKnightPathRecord>(
                item: record,
                id: $"{Id}"                
            );

            return false;
        }

    }
}
