using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Google.Cloud.Bigtable.V2;
using Google.Cloud.Bigtable.Admin.V2;
using Google.Protobuf.WellKnownTypes;

namespace GoogleCloudSamples.Bigtable
{
    class QuickStart
    {
        public static void Main(string[] args)
        {
            // Your Google Cloud Platform project ID
            string projectId = "grass-clump-479";
            // You Google Cloud Bigtable instance ID
            string instanceId = "alex-ssd-prod";
            // The name of a table.
            string tableId = "Hello-Bigtable";
            // The name of a column family.
            string columnFamily = "cf";

            // Instantiates a BigtableTableAdminclient used to create a table.
            BigtableTableAdminClient bigtableTableAdminClient = BigtableTableAdminClient.Create();

            // Create a table with a single complex column family.
            Console.WriteLine(
                $"Create new table: {tableId} with complex column family: {columnFamily}, Instance: {instanceId}");

            bigtableTableAdminClient.CreateTable(
                new InstanceName(projectId, instanceId),
                tableId,
                new Table
                {
                    Granularity = Table.Types.TimestampGranularity.Millis,
                    ColumnFamilies =
                    {
                        {
                            columnFamily, new ColumnFamily
                            {
                                GcRule = new GcRule
                                {
                                    Union = new GcRule.Types.Union
                                    {
                                        Rules =
                                        {
                                            // Rule one {MaxNumVersions = 10}
                                            new GcRule{MaxNumVersions = 10},
                                            // Rule two {Intersection}
                                            new GcRule{Intersection = new GcRule.Types.Intersection
                                            {
                                                Rules =
                                                {
                                                    new GcRule {MaxNumVersions = 2},
                                                    new GcRule {MaxAge = Duration.FromTimeSpan(TimeSpan.FromDays(30.0))}
                                                }
                                            }}
                                        }
                                    }
                                }
                            }
                        }
                    }
                });

            // Verify the comlex GcRule was created
            Table t = bigtableTableAdminClient.GetTable(
                new GetTableRequest
                {
                    TableName = new Google.Cloud.Bigtable.Admin.V2.TableName(projectId, instanceId, tableId)
                });
            // Print the GcRule
            Console.WriteLine(t.ColumnFamilies[columnFamily].GcRule);
            // This prints 
            // { "union": { "rules": [ { "maxNumVersions": 10 }, { "intersection": { "rules": [ { "maxNumVersions": 2 }, { "maxAge": "2592000s" } ] } } ] } }
            // confirming that complex GcRule was created successfully

            // Clean up
            bigtableTableAdminClient.DeleteTable(
                new DeleteTableRequest
                {
                    TableName = new Google.Cloud.Bigtable.Admin.V2.TableName(projectId, instanceId, tableId)
                });
        }
    }
}




