﻿// [START bigtable_quickstart]
using System;
// Imports the Google Cloud client library
using Google.Cloud.Bigtable.V2;

namespace GoogleCloudSamples.Bigtable
{
    class QuickStart
    {
        static int Main(string[] args)
        {
            // Your Google Cloud Platform project ID
            string projectId = "YOUR-PROJECT-ID";
            // [END bigtable_quickstart]
            if (projectId == "YOUR-PROJECT" + "-ID")
            {
                Console.WriteLine("Edit Program.cs and replace YOUR-PROJECT-ID with your project id.");
                return -1;
            }
            // [START bigtable_quickstart]
            // The name of the Cloud Bigtable instance
            const string instanceId = "my-bigtable-instance";
            // The name of the Cloud Bigtable table
            const string tableId = "my-table";
      
            try
            {
                // Creates a Bigtable client
                BigtableClient bigtableClient = BigtableClient.Create();

                // Read a row from my-table using a row key
                Row row = bigtableClient.ReadRow(new TableName(projectId, instanceId, tableId), "r1", RowFilters.CellsPerRowLimit(1));
                // Print the row key and data (column value, labels, timestamp)
                Console.WriteLine($"Row key: {row.Key.ToStringUtf8()}" +
                                  $"  Column Family: {row.Families[0].Name}" +
                                  $"    Column Qualifyer: {row.Families[0].Columns[0]}" +
                                  $"      Value: {row.Families[0].Columns[0].Cells[0].Value.ToStringUtf8()}" +
                                  $"      Labels: {row.Families[0].Columns[0].Cells[0].Labels}" +
                                  $"      Timestamp: {row.Families[0].Columns[0].Cells[0].TimestampMicros}");
            }
            catch (Exception ex)
            {
                // Handle error performing the read operation
                Console.WriteLine($"Error reading row r1: {ex.Message}");
            }
            return 0;
        }
    }
}
// [END bigtable_quickstart]

