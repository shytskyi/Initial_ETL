using System.Data;
using System.Data.SqlClient;

namespace Initial_ETL
{
    internal class Program
    {
        // Connection strings to the source and target databases
        private const string SourceConnectionString =
            "Server=IS;Database=OpenDataBase;TrustServerCertificate=True;Trusted_Connection=True";
        private const string DestinationConnectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=OpenDataBase;TrustServerCertificate=True;Trusted_Connection=True";

        // Source and target table name
        private const string SourceTable = "dbo.OpenTable";
        private const string DestinationTable = "dbo.OpenTable";

        static async Task Main(string[] args)
        {

            try
            {
                Console.WriteLine("Start copy...");

                // Open a connection to the source
                using var sourceConn = new SqlConnection(SourceConnectionString);
                await sourceConn.OpenAsync();

                // Command to read all data
                using var cmd = new SqlCommand(
                    $"SELECT " +
                        $"Name COLLATE Latin1_General_CI_AS AS Name, " +
                        $"Description COLLATE Latin1_General_CI_AS AS Description, " +
                        $"CreatedDate, " +
                        $"ModifiedDate, " +
                        $"IsActive " +
                     $"FROM {SourceTable}", sourceConn);
                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                // Open a connection to the destination
                using var destConn = new SqlConnection(DestinationConnectionString);
                await destConn.OpenAsync();

                // Configuring SqlBulkCopy
                using var bulkCopy = new SqlBulkCopy(destConn)
                {
                    DestinationTableName = DestinationTable,
                    BatchSize = 5000,      // number of lines per package
                    BulkCopyTimeout = 600  // timeout in seconds
                };

                // If the column names are different, add a mapping:
                bulkCopy.ColumnMappings.Add("Name", "Name");
                bulkCopy.ColumnMappings.Add("Description", "Description");
                bulkCopy.ColumnMappings.Add("CreatedDate", "CreatedDate");
                bulkCopy.ColumnMappings.Add("ModifiedDate", "ModifiedDate");
                bulkCopy.ColumnMappings.Add("IsActive", "IsActive");

                // We are copying
                await bulkCopy.WriteToServerAsync(reader);

                Console.WriteLine("Success.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Erorr in proces: " + ex.Message);
            }        
        }
    }
}
