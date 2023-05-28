using Crawler_Proxy_Servers.Classes;
using HtmlAgilityPack;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace Crawler_Proxy_Servers
{
    internal class WebCrawler
    {


        private const string BaseUrl = "https://proxyservers.pro/proxy/list/order/port/order_dir/asc";
        private const int MaxConcurrentTasks = 3;

        public static async Task StartAsync(MySqlConnection connection)
        {
            var httpClient = new HttpClient();
            var htmlDocument = new HtmlDocument();
            var proxies = new List<ProxyServer>();
            var totalPages = 0;

            DateTime dtInicio = DateTime.Now;

            // Criar a pasta com a data e hora atual
            string folderPath = CreateFolderPath();
            Console.WriteLine("Processo iniciado, aguarde");

            var tasks = new List<Task>();

            while (true)
            {
                if (tasks.Count >= MaxConcurrentTasks)
                {
                    var completedTask = await Task.WhenAny(tasks);
                    tasks.Remove(completedTask);
                }

                var pageUrl = $"{BaseUrl}/page/{totalPages + 1}";

                var html = await httpClient.GetStringAsync(pageUrl);
                htmlDocument.LoadHtml(html);

                HtmlNodeCollection rows = htmlDocument.DocumentNode.SelectNodes("//table[@class='table table-hover']/tbody/tr");

                if (rows == null || rows.Count == 0)
                    break;

                tasks.Add(Task.Run(() =>
                {
                    var pageProxies = new List<ProxyServer>();

                    foreach (var tr in rows)
                    {
                        var cells = tr.Descendants("td").ToList();

                        var portNode = cells[2].SelectSingleNode(".//span[@class='port']");

                        //O html não esta vindo com o valor que fica dentro do span que corresponde ao valor da porta exibida na tela, sendo assim

                        var port = portNode?.GetAttributeValue("data-port", "");

                        var proxy = new ProxyServer
                        {
                            IPAdress = cells[1].InnerText.Trim(),
                            Port = port,
                            Country = cells[3].InnerText.Trim(),
                            Protocol = cells[6].InnerText.Trim()
                        };

                        pageProxies.Add(proxy);
                    }

                    lock (proxies)
                    {
                        proxies.AddRange(pageProxies);
                    }
                }));

                totalPages++;
            }

            await Task.WhenAll(tasks);

            int idCrawler = SaveCrawlerInfo(connection, dtInicio, totalPages, proxies.Count);
            SaveProxiesToDatabase(proxies, connection, idCrawler);
            SaveProxiesToJson(proxies, folderPath);
            SaveHtmlPages(htmlDocument, folderPath, totalPages);
        }

        private static void SaveProxiesToDatabase(List<ProxyServer> proxies, MySqlConnection connection, int idCrawler)
        {
            string sqlInsertProxy = "INSERT INTO proxyserver (idCrawler, IPAdress, Port, Country, Protocol) VALUES (@idCrawler, @IPAdress, @Port, @Country, @Protocol)";

            using (var command = new MySqlCommand(sqlInsertProxy, connection))
            {
                foreach (var proxy in proxies)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@idCrawler", idCrawler);
                    command.Parameters.AddWithValue("@IPAdress", proxy.IPAdress);
                    command.Parameters.AddWithValue("@Port", proxy.Port);
                    command.Parameters.AddWithValue("@Country", proxy.Country);
                    command.Parameters.AddWithValue("@Protocol", proxy.Protocol);

                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Número de proxies salvos no banco de dados: " + proxies.Count);
        }

        private static int SaveCrawlerInfo(MySqlConnection connection, DateTime dtInicio, int totalPages, int totalProxies)
        {
            DateTime dtTermino = DateTime.Now;

            string sqlInsertCrawler = "INSERT INTO crawler (dtInicio, dtTermino, qtdPaginas, qtdLinhas) VALUES (@dtInicio, @dtTermino, @qtdPaginas, @qtdLinhas); SELECT LAST_INSERT_ID()";

            using (var command = new MySqlCommand(sqlInsertCrawler, connection))
            {
                command.Parameters.AddWithValue("@dtInicio", dtInicio);
                command.Parameters.AddWithValue("@dtTermino", dtTermino);
                command.Parameters.AddWithValue("@qtdPaginas", totalPages);
                command.Parameters.AddWithValue("@qtdLinhas", totalProxies);

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        private static void SaveProxiesToJson(List<ProxyServer> proxies, string folderPath)
        {
            var proxyList = new List<ProxyServer>();

            foreach (var proxy in proxies)
            {
                var proxyWithoutId = new ProxyServer
                {
                    IPAdress = proxy.IPAdress,
                    Port = proxy.Port,
                    Country = proxy.Country,
                    Protocol = proxy.Protocol
                };

                proxyList.Add(proxyWithoutId);
            }

            var json = JsonConvert.SerializeObject(proxyList, Formatting.Indented);
            var filePath = Path.Combine(folderPath, "proxies.json");
            File.WriteAllText(filePath, json);

            Console.WriteLine("Número de proxies salvos no arquivo JSON: " + proxies.Count);
            Console.WriteLine("Arquivo JSON salvo em: " + Path.GetFullPath(filePath));
        }

        private static string CreateFolderPath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string downloadsDirectory = Path.Combine(baseDirectory, "..", "..", "..", "Downloads");
            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string folderPath = Path.Combine(downloadsDirectory, currentDateTime);
            Directory.CreateDirectory(folderPath);
            return folderPath;
        }

        private static void SaveHtmlPages(HtmlDocument htmlDocument, string folderPath, int totalPages)
        {

            for (int i = 1; i <= totalPages; i++)
            {
                var pageUrl = $"{BaseUrl}/page/{i}";
                var html = htmlDocument.DocumentNode.OuterHtml;
                var filePath = Path.Combine(folderPath, $"page_{i}.html");
                File.WriteAllText(filePath, html);
            }

                Console.WriteLine("Arquivos HTML salvo em: " + Path.GetFullPath(folderPath));
        }
    }

}
