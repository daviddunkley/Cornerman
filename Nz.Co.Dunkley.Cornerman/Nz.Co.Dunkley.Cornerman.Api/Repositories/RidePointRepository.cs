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
        private readonly AzureStorageContext _azureStorageContext;

        public RidePointRepository(AzureStorageContext azureStorageContext = null)
        {
            _azureStorageContext = azureStorageContext ?? new AzureStorageContext();
        }

        public string CreateOrUpdate(
            string riderId, 
            DateTime pointDateTime, 
            double altitude, 
            double longitude, 
            double latitude)
        {
            var entity = new RidePointEntity()
            {
                PartitionKey = riderId,
                RowKey = pointDateTime.Ticks.ToString(),
                Altitude = altitude,
                Latitude = latitude,
                Longitude = longitude,
                PointDateTime = pointDateTime,
            };

            var operation = TableOperation.InsertOrMerge(entity);

            _azureStorageContext.RiderPointsTable.ExecuteAsync(operation).GetAwaiter().GetResult();

            return entity.RowKey;
        }

        public RidePoint Retrieve(string riderId, string ridePointId)
        {
            var operation = TableOperation.Retrieve<RidePointEntity>(
                riderId,
                ridePointId);

            var tableResult = _azureStorageContext.RiderPointsTable.ExecuteAsync(operation).Result;

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

        public List<RidePoint> RetrieveForRiderId(string riderId)
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
                    _azureStorageContext.RiderPointsTable.ExecuteQuerySegmentedAsync(query, continuationToken).Result;

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

        public void Delete(string riderId, string ridePointId)
        {
            var entity = new RidePointEntity()
            {
                PartitionKey = riderId,
                RowKey = ridePointId,
                ETag = "*",
            };

            var operation = TableOperation.Delete(entity);
            _azureStorageContext.RiderPointsTable.ExecuteAsync(operation).GetAwaiter().GetResult();
        }

        public void DeleteForRiderId(string riderId)
        {
            var ridePoints = RetrieveForRiderId(riderId);

            foreach (var ridePoint in ridePoints)
            {
                Delete(riderId, ridePoint.PointDateTime.Ticks.ToString());
            }
        }
    }
}