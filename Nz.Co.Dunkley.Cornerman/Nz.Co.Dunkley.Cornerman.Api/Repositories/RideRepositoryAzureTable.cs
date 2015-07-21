namespace Nz.Co.Dunkley.Cornerman.Api.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage.Table;
    using Models;
    using Newtonsoft.Json;

    public class RideRepositoryAzureTable: IRideRepository
    {
        private readonly AzureStorageContext _azureStorageContext;

        public RideRepositoryAzureTable(AzureStorageContext azureStorageContext = null)
        {
            _azureStorageContext = azureStorageContext ?? new AzureStorageContext();
        }

        public string Create(Ride ride)
        {
            InsertOrMergeRideEntity(
                ride,
                "Ride",
                new
                {
                    RideId = ride.RideId,
                    Title = ride.Title,
                    StartDateTime = ride.StartDateTime,
                    CompletedDateTime = ride.CompletedDateTime
                });

            InsertOrMergeRideEntity(
                ride,
                "Lead:" + ride.Lead.MembershipId,
                ride.Lead);

                InsertOrMergeRideEntity(
                    ride,
                    "Tailgunner:" + ride.Tailgunner.MembershipId,
                    ride.Tailgunner);

            ride.Wingmen.Select(
                wingman => await InsertOrMergeRideEntity(
                    ride,
                    "Wingman:" + wingman.MembershipId,
                    wingman);

                ride.Riders.Select(
                    rider => await InsertOrMergeRideEntity(
                        ride,
                        "Rider:" + rider.MembershipId,
                        rider)));

            return ride.RideId;
        }

        public Task<string> CreateAsync(Ride ride)
        {
            var taskList = new List<Task>
            {
                InsertOrMergeRideEntity(
                    ride,
                    "Ride",
                    new
                    {
                        RideId = ride.RideId,
                        Title = ride.Title,
                        StartDateTime = ride.StartDateTime,
                        CompletedDateTime = ride.CompletedDateTime
                    }),
                InsertOrMergeRideEntity(
                    ride,
                    "Lead:" + ride.Lead.MembershipId,
                    ride.Lead),
                InsertOrMergeRideEntity(
                    ride,
                    "Tailgunner:" + ride.Tailgunner.MembershipId,
                    ride.Tailgunner)
            };
            
            taskList.AddRange(
                ride.Wingmen.Select(
                    wingman => InsertOrMergeRideEntity(
                        ride,
                        "Wingman:" + wingman.MembershipId,
                        wingman)));

            taskList.AddRange(
                ride.Riders.Select(
                    rider => InsertOrMergeRideEntity(
                        ride,
                        "Rider:" + rider.MembershipId,
                        rider)));

            Task.WaitAll(taskList.ToArray());

            return Task.Run(() => ride.RideId);
        }

        private void InsertOrMergeRideEntity(Ride ride, string rowKey, object data)
        {
            var rideEntity = new RideEntity()
            {
                PartitionKey = ride.RideId,
                RowKey = rowKey,
                JsonData = JsonConvert.SerializeObject(data),
            };

            var operation = TableOperation.InsertOrMerge(rideEntity);
            await _azureStorageContext.RidesTable.ExecuteAsync(operation);
        }

        private Task InsertOrMergeRideEntity(Ride ride, string rowKey, object data)
        {
            var rideEntity = new RideEntity()
            {
                PartitionKey = ride.RideId,
                RowKey = rowKey,
                JsonData = JsonConvert.SerializeObject(data),
            };

            var operation = TableOperation.InsertOrMerge(rideEntity);
            return _azureStorageContext.RidesTable.ExecuteAsync(operation);
        }

        public async Task<Ride> Retrieve(string rideId)
        {
            var filter = TableQuery.GenerateFilterCondition(
                "PartitionKey",
                QueryComparisons.Equal,
                rideId
                );

            var query = new TableQuery<RideEntity>().Where(filter);

            TableContinuationToken continuationToken = null;
            var ride = new Ride()
            {
                RideId = rideId,
            };

            do
            {
                TableQuerySegment<RideEntity> querySegment =
                    await _azureStorageContext.RidesTable.ExecuteQuerySegmentedAsync(query, continuationToken);

                if (querySegment.Results != null)
                {
                    foreach (var result in querySegment.Results)
                    {
                        if (result.RowKey == "Ride")
                        {
                            var rideRow = JsonConvert.DeserializeObject<Ride>(result.JsonData);
                            ride.CompletedDateTime = rideRow.CompletedDateTime;
                            ride.StartDateTime = rideRow.StartDateTime;
                            ride.Title = rideRow.Title;
                            continue;
                        }
                        if (result.RowKey.StartsWith("Lead:"))
                        {
                            var riderRow = JsonConvert.DeserializeObject<Rider>(result.JsonData);
                            ride.Lead = riderRow;
                            continue;
                        }
                        if (result.RowKey.StartsWith("Tailgunner:"))
                        {
                            var riderRow = JsonConvert.DeserializeObject<Rider>(result.JsonData);
                            ride.Tailgunner = riderRow;
                            continue;
                        }
                        if (result.RowKey.StartsWith("Wingman:"))
                        {
                            var riderRow = JsonConvert.DeserializeObject<Rider>(result.JsonData);
                            ride.Wingmen.Add(riderRow);
                            continue;
                        }
                        if (result.RowKey.StartsWith("Rider:"))
                        {
                            var riderRow = JsonConvert.DeserializeObject<Rider>(result.JsonData);
                            ride.Riders.Add(riderRow);
                            continue;
                        }
                    }
                }

                // keep looping until the ContinuousToken is null
                continuationToken = querySegment.ContinuationToken;
            }
            while (continuationToken != null);

            return ride;          
        }

        public Task<List<Ride>> RetrieveForMembershipId(string membershipId)
        {
            throw new NotImplementedException();
        }

        public Task<string> Update(Ride ride)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string rideId)
        {
            var filter = TableQuery.GenerateFilterCondition(
                "PartitionKey",
                QueryComparisons.Equal,
                rideId
                );

            var query = new TableQuery<RideEntity>().Where(filter);

            TableContinuationToken continuationToken = null;

            var rideEntities = new List<RideEntity>();

            do
            {
                TableQuerySegment<RideEntity> querySegment =
                    await _azureStorageContext.RidesTable.ExecuteQuerySegmentedAsync(query, continuationToken);

                if (querySegment.Results != null)
                {
                    rideEntities.AddRange(querySegment.Results.Select(operation => new RideEntity()
                    {
                        PartitionKey = operation.PartitionKey, 
                        RowKey = operation.RowKey, 
                        ETag = "*",
                    }));
                }

                // keep looping until the ContinuousToken is null
                continuationToken = querySegment.ContinuationToken;
            }
            while (continuationToken != null);

            foreach (var rideEntity in rideEntities)
            {
                var operation = TableOperation.Delete(rideEntity);
                await _azureStorageContext.RiderPointsTable.ExecuteAsync(operation);
            }
        }
    }
}