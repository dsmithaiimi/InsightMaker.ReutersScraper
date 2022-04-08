using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace InsightMaker.ReutersScraper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                {
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine(@$"Params: {args[0]} and {args[1]}");
                    var sc = new ScrapeJob();
                    sc.RunScraper(args[0], args[1]);
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }







        //private static readonly HttpClient client = new HttpClient();
        //static async Task Main(string[] args)
        //{
        //    string channel = args[0];
        //    string outputpath = args[1];            

        //    var conf = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        //    var user = conf.GetSection("Reuters")["user"];
        //    var password = conf.GetSection("Reuters")["password"];

        //    var auth = await GetAccessToken(user,password);
        //    await ProcessRepositories(outputpath, channel, auth);
        //}
        //private static async Task<string> GetAccessToken(string user, string password)
        //{
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
        //    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
        //    client.DefaultRequestHeaders
        //        .Accept
        //        .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
        //    var stringTask = client.GetStringAsync($@"http://commerce.reuters.com/rmd/rest/xml/login?username={user}&password={password}");
        //    var msg = await stringTask;
        //    var jsonText = JsonConvert.DeserializeObject<ReutersAuthToken.Rootobject>(msg);
        //    return jsonText.authToken.authToken.ToString();
        //}

        //private static async Task ProcessRepositories(string outputpath, string channel, string authToken)
        //{
            
        //    var stringTask = client.GetStringAsync($"http://rmb.reuters.com/rmd/rest/xml/items?channel={channel}&token={authToken}");

        //    var msg = await stringTask;

        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(msg);
        //    string jsonText = JsonConvert.SerializeXmlNode(doc);

        //    var reuters = JsonConvert.DeserializeObject<ReutersResponse.Rootobject>(jsonText);
        //    var heads = "channel,dateCreated,geography,guid,headline,id,language,mediaType,priority,slug,source,version";

        //    List<string> csv = new List<string>();
        //    csv.Add(heads);
        //    foreach (var itm in reuters.results.result)
        //    {
        //        var geography = (itm.geography == null) ? "" : itm.geography.ToString().Replace(",", "|");
        //        var outstring = $@"{itm.channel},{itm.dateCreated.ToString("yyyy-MM-ddThh:mm:ss.SSSZ")},{geography},{itm.guid},{itm.headline},{itm.id},{itm.language},{itm.mediaType},{itm.priority},{itm.slug},{itm.source},{itm.version}";
        //        Console.WriteLine($@"[{DateTime.Now.ToString()}] {outstring}");
        //        csv.Add(outstring);
        //    }
        //    System.IO.File.WriteAllLines(@$"{outputpath}\Reuters-{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv", csv);
        //}
    }
}
