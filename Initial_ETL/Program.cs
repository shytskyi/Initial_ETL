using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Initial_ETL
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var dbSettings = configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();

                Console.WriteLine("Start copy...");

                // Open a connection to the source
                using var sourceConn = new SqlConnection(dbSettings.SourceConnectionString);
                await sourceConn.OpenAsync();

                // Command to read all data
                using var cmd = new SqlCommand(
                    $"SELECT " +
                        $"CAST(Name AS NVARCHAR(255)) COLLATE Latin1_General_CI_AS AS Name, " +
                        $"Description COLLATE Latin1_General_CI_AS AS Description, " +
                        $"CreatedDate, " +
                        $"ModifiedDate, " +
                        $"IsActive " +
                     $"FROM {dbSettings!.SourceTable}", sourceConn);
                using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                // Open a connection to the destination
                using var destConn = new SqlConnection(dbSettings!.DestinationConnectionString);
                await destConn.OpenAsync();

                // Configuring SqlBulkCopy
                using var bulkCopy = new SqlBulkCopy(destConn)
                {
                    DestinationTableName = dbSettings!.DestinationTable,
                    BatchSize = 5000,      // number of lines per package
                    BulkCopyTimeout = 600  // timeout in seconds
                };

                // If the column names are different, add a mapping:
                bulkCopy.ColumnMappings.Add("Name", "Name2");
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
