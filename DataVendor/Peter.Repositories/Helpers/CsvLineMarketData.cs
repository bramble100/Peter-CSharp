namespace Peter.Repositories.Helpers
{
    public static class CsvLineMarketData
    {
        public static string[] Header => new string[]
        {
            "Name",
            "ISIN",
            "Closing Price",
            "DateTime",
            "Volumen",
            "Previous Day Closing Price",
            "Stock Exchange"
        };
    }
}
