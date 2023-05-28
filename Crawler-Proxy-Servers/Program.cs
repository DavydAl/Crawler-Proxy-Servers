using MySql.Data.MySqlClient;

namespace Crawler_Proxy_Servers
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = "server=127.0.0.1;port=3306;database=crawler;uid=Admin;password=Admin;";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                int maxThreads = 3;
                await WebCrawler.StartAsync(connection);
                connection.Close();
            }
        }
    }
}