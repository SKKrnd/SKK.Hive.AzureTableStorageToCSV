using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace SKK.Hive.AzureTableStorageToCSV
{
    public class StorageTableReader
    {
        private readonly CloudStorageAccount cloudStorageAccount;
        private CloudTableClient tableClient;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="cloudStorageAccount">Azure Storage.</param>
        public StorageTableReader(CloudStorageAccount cloudStorageAccount)
        {
            this.cloudStorageAccount = cloudStorageAccount;
            tableClient = cloudStorageAccount.CreateCloudTableClient();
        }

        /// <summary>
        /// Get Tables from Azure Storage.
        /// </summary>
        /// <returns>List of Cloud Tables.</returns>
        public IList<CloudTable> GetTables()
        {
            var tables = new List<CloudTable>();
            TableContinuationToken token = null;
            //
            do
            {
                var result = tableClient.ListTablesSegmentedAsync(token).Result;
                token = result.ContinuationToken;
                tables.AddRange(result.Results);
            }
            while (token != null);
            //
            return tables;
        }

        /// <summary>
        /// Get data from selected table.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <param name="date">Export data date.</param>
        /// <returns>List of table rows.</returns>
        public List<Dictionary<string, string>> GetTableData(string tableName, DateTime date)
        {
            var cloudTable = tableClient.GetTableReference(tableName);
            var tableQuery = new TableQuery().Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThan, date.AddDays(-1).ToString()), TableOperators.And, TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, date.AddDays(1).ToString())));

            TableContinuationToken token = null;
            var results = new List<DynamicTableEntity>();
            var readings = new List<Dictionary<string, string>>();
            //
            do
            {
                var result = cloudTable.ExecuteQuerySegmentedAsync(tableQuery, token).Result;
                token = result.ContinuationToken;
                results.AddRange(result.Results);
                Console.WriteLine($"{results.Count} records read.");
            }
            while (token != null);
            //
            foreach (var item in results)
            {
                var reading = new Dictionary<string, string>();
                reading.Add(nameof(item.PartitionKey), item.PartitionKey);
                reading.Add(nameof(item.RowKey), item.RowKey);
                reading.Add(nameof(item.Timestamp), item.Timestamp.ToString());
                //
                foreach (var property in item.Properties)
                {
                    reading.Add(property.Key, property.Value.PropertyAsObject.ToString());
                }
                readings.Add(reading);
            }
            //
            return readings;
        }
    }
}
