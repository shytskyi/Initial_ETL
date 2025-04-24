namespace Initial_ETL
{
    public class DatabaseSettings
    {
        public string SourceConnectionString { get; set; } = null!;
        public string DestinationConnectionString { get; set; } = null!;
        public string SourceTable { get; set; } = null!;
        public string DestinationTable { get; set; } = null!;
        public int BatchSize { get; set; }
        public int BulkCopyTimeout { get; set; }
    }
}
