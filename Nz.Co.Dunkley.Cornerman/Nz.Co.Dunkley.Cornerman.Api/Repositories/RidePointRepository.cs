namespace Nz.Co.Dunkley.Cornerman.Api.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage.Table;
    using Models;

    public class RidePointRepository : IRidePointRepository
    {
        private readonly CloudTable _table;

        public RidePointRepository()
        {
            _table = AzureStorageHelper.CreateTable("RiderPoints");
        }

        public async Task CreateOrUpdate(
            string riderId, 
            DateTime pointDateTime, 
            double altitude, 
            double longitude, 
            double latitude)
        {
            var entity = new RidePointEntity()
            {
                PartitionKey = riderId,
                RowKey = pointDateTime.ToString("u"),
                Altitude = altitude,
                Latitude = latitude,
                Longitude = longitude,
                PointDateTime = pointDateTime,
            };

            var operation = TableOperation.InsertOrMerge(entity);

            await _table.ExecuteAsync(operation);
        }

        public async Task<RidePoint> Retrieve(string riderId, DateTime pointDateTime)
        {
            var operation = TableOperation.Retrieve<RidePointEntity>(
                riderId,
                pointDateTime.ToString("u")
                );

            var tableResult = await _table.ExecuteAsync(operation);

            var ridePointEntity = tableResult.Result as RidePointEntity;
            if (ridePointEntity == null)
                return null;

            var ridePoint = new RidePoint()
            {
                Altitude = ridePointEntity.Altitude,
                Longitude = ridePointEntity.Longitude,
                Latitude = ridePointEntity.Latitude,
                PointDateTime = ridePointEntity.PointDateTime,
            };

            return ridePoint;
        }

        public async Task<List<RidePoint>> RetrieveForRiderId(string riderId)
        {
            var filter = TableQuery.GenerateFilterCondition(
                "PartitionKey",
                QueryComparisons.Equal,
                riderId
                );

            var query = new TableQuery<RidePointEntity>().Where(filter);

            TableContinuationToken continuationToken = null;
            List<RidePoint> results = new List<RidePoint>();

            do
            {
                TableQuerySegment<RidePointEntity> querySegment =
                    await _table.ExecuteQuerySegmentedAsync(query, continuationToken);

                if (querySegment.Results != null)
                {
                    results.AddRange(
                        querySegment.Results.Select(
                            e => new RidePoint()
                            {
                                Altitude = e.Altitude,
                                Latitude = e.Latitude,
                                Longitude = e.Longitude,
                                PointDateTime = e.PointDateTime,
                            }
                        )
                    );
                }

                // keep looping until the ContinuousToken is null
                continuationToken = querySegment.ContinuationToken;
            }
            while (continuationToken != null);

            return results;
        }

        public async Task Delete(string riderId, DateTime pointDateTime)
        {
            var entity = new RidePointEntity()
            {
                PartitionKey = riderId,
                RowKey = pointDateTime.ToString("u"),
                ETag = "*",
            };
            var operation = TableOperation.Delete(entity);
            await _table.ExecuteAsync(operation);
        }
    }
}